using Fisobs;
using Fisobs.Core;
using Fisobs.Items;
using Fisobs.Sandbox;
using SourceCode.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Objects
{
    internal class CustomObject : Weapon
    {
        public CustomObject(AbstractCustomObject abstr, Vector2 pos, Vector2 vel) :base(abstr, abstr.world)
        {
            // assign the abstract object field to the parameter one
            this._abstractObject = abstr;

            // creates body chunks
            this.bodyChunks = new BodyChunk[]
            {
                new BodyChunk(this, 0, pos + vel, 4 * (_abstractObject.scaleX + _abstractObject.scaleY), 0.35f)
            };
            this.bodyChunks[0].goThroughFloors = false;
            this.bodyChunks[0].lastPos = bodyChunks[0].pos;
            this.bodyChunks[0].vel = vel;
            //this.bodyChunkConnections = new BodyChunkConnection[0];
            this.airFriction = 0.999f;
            this.gravity = 0.9f;
            this.bounce = 0.6f;
            this.surfaceFriction = 0.45f;
            this.collisionLayer = 1;
            this.waterFriction = 0.92f;
            this.buoyancy = 0.75f;

            this.lastRotation = rotation;

            this.rotationOffset = 30 - 15;
        }

        public override void Update(bool eu)
        {
            // calls the Update base here
            base.Update(eu);
        }
        public override void HitByWeapon(Weapon weapon)
        {
            try
            {
                base.HitByWeapon(weapon);

                // checks if its something holding them and the weapon is a spear
                if (this.grabbedBy.Count > 0)
                {
                    if (weapon is Spear)
                    {
                        var crit = this.grabbedBy[0].grabber;
                        var explosion = new Explosion(this.room, this, this.bodyChunks[0].pos, 1, 20f, 20f, 1f, 1f, 2f, crit, 1f, 1f, 2f);

                        this.room.AddObject(explosion);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"<CustomObject.HitByWeapon()>Exception thrown!\n{ex}");
            }
        }
        public override void TerrainImpact(int chunk, IntVector2 direction, float speed, bool firstContact)
        {
            try
            {
                base.TerrainImpact(chunk, direction, speed, firstContact);

                this.room.PlaySound(SoundID.Centipede_Shock, this.bodyChunks[0].pos);
            }
            catch (Exception ex)
            {
                log.LogError($"<CustomObject.TerrainImpact()>Exception thrown!\n{ex}");
            }
        }

        #region IDrawable

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            try
            {
                sLeaser.sprites = new FSprite[]
                {
                    new FSprite("Futile_White", true)
                };

                //sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders[Shaders.HoldButtonCircle];
                sLeaser.sprites[0].color = _abstractObject.color;
            }
            catch (Exception ex)
            {
                log.LogError($"<CustomObject.InitiateSprites()>Exception thrown!\n{ex}");
            }
        }
        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            try
            {
                Vector2 pos = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, timeStacker);
                float num = Mathf.InverseLerp(305f, 380f, timeStacker);
                pos.y -= 20f * Mathf.Pow(num, 3f);
                float num2 = Mathf.Pow(1f - num, 0.25f);
                lastDarkness = darkness;
                darkness = rCam.room.Darkness(pos);
                darkness *= 1f - 0.5f * rCam.room.LightSourceExposure(pos);

                for (int i = 0; i < 2; i++)
                {
                    sLeaser.sprites[i].x = pos.x - camPos.x;
                    sLeaser.sprites[i].y = pos.y - camPos.y;
                    sLeaser.sprites[i].scaleY = num2 * _abstractObject.scaleY;
                    sLeaser.sprites[i].scaleX = num2 * _abstractObject.scaleX;
                }

                sLeaser.sprites[0].color = blackColor;
                sLeaser.sprites[0].scaleY *= 1.175f - _abstractObject.damage * 0.2f;
                sLeaser.sprites[0].scaleX *= 1.175f - _abstractObject.damage * 0.2f;

                sLeaser.sprites[1].color = Color.Lerp(Custom.HSL2RGB(_abstractObject.hue, _abstractObject.saturation, 0.55f), blackColor, darkness);

                if (blink > 0)
                {
                    sLeaser.sprites[0].color = blinkColor;
                }
                else if (num > 0.3f)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        sLeaser.sprites[j].color = Color.Lerp(sLeaser.sprites[j].color, earthColor, Mathf.Pow(Mathf.InverseLerp(0.3f, 1f, num), 1.6f));
                    }
                }

                if (slatedForDeletetion || room != rCam.room)
                {
                    sLeaser.CleanSpritesAndRemove();
                }
            }
            catch (Exception ex)
            {
                log.LogError($"<CustomObject.DrawSprites()>Exception thrown!\n{ex}");
            }
        }
        public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            try
            {
                blackColor = palette.blackColor;
                earthColor = Color.Lerp(palette.fogColor, palette.blackColor, 0.5f);
            }
            catch (Exception ex)
            {
                log.LogError($"<CustomObject.ApplyPalette()>Exception thrown!\n{ex}");
            }
        }
        public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
        {
            try
            {
                newContainer ??= rCam.ReturnFContainer(ContainerLayers.Items.ToString());

                foreach (FSprite fsprite in sLeaser.sprites)
                {
                    newContainer.AddChild(fsprite);
                }
            }
            catch (Exception ex)
            {
                log.LogError($"<CustomObject.AddToContainer()>Exception thrown!\n{ex}");
            }
        }

        #endregion

        public AbstractCustomObject abstractObject { get => _abstractObject; }
        public float rotVel;
        public float lastDarkness = -1f;
        public float darkness;
        private static LogUtils.Logger log { get => new LogUtils.Logger(Plugin.logger); }
        private Color blackColor;
        private Color earthColor;
        private float rotationOffset;
        private AbstractCustomObject _abstractObject;
    }
    internal class AbstractCustomObject : AbstractPhysicalObject
    {
        public AbstractCustomObject(World world, WorldCoordinate pos, EntityID ID) :base(world, CustomObjectFisob.customObjectType, null, pos, ID)
        {
            color = Color.yellow;
            scaleX = 1;
            scaleY = 1;
            saturation = 0.5f;
            hue = 1f;
        }
        public override void Realize()
        {
            base.Realize();
            if (realizedObject == null)
                realizedObject = new CustomObject(this, Room.realizedRoom.MiddleOfTile(pos.Tile), Vector2.zero);
        }

        public Color color;
        public float hue;
        public float saturation;
        public float scaleX;
        public float scaleY;
        public float damage;
    }
}
