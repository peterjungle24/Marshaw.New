using UnityEngine;
using Fisobs;
using Fisobs.Core;
using SourceCode.Helpers;

namespace SourceCode.FIsobs
{
    internal class TestObject : Weapon
    {
        public TestObjectAbstract abstr { get; }
        
        public float rotVel;
        public float lastDarkness = -1f;
        public float darkness;
        public new float rotation;
        public new float lastRotation;
        private Color blackColor;
        private float randomValue { get => Randomf.value; }

        public TestObject(TestObjectAbstract abstr, Vector2 pos, Vector2 vel) : base(abstr, abstr.world)
        {
            // i think it initializes
            this.abstr = abstr;
            // creates a new BodyChunk
            bodyChunks = new[] { new BodyChunk(this, 0, pos + vel, 4 * (abstr.scaleX + abstr.scaleY), 0.35f) { goThroughFloors = false } };
            // sets the lastPosition to the position parameter
            bodyChunks[0].lastPos = bodyChunks[0].pos;
            // same for Velocity
            bodyChunks[0].vel = vel;

            // theres no connections, but maybe its still good to create it
            bodyChunkConnections = new BodyChunkConnection[0];
            airFriction = 0.999f;
            gravity = 0.9f;
            bounce = 0.6f;
            surfaceFriction = 0.45f;
            collisionLayer = 1;
            waterFriction = 0.92f;
            buoyancy = 0.75f;

            lastRotation = rotation;
        }
        public override void PlaceInRoom(Room placeRoom)
        {
            base.PlaceInRoom(placeRoom);

            Vector2 center = placeRoom.MiddleOfTile(abstractPhysicalObject.pos);
            bodyChunks[0].HardSetPosition(new Vector2(0, 0) * 20f + center);
        }
        public override void HitByWeapon(Weapon weapon)
        {
            base.HitByWeapon(weapon);

            if (grabbedBy.Count > 0)
            {
                Creature grabber = grabbedBy[0].grabber;
                Vector2 push = firstChunk.vel * firstChunk.mass / grabber.firstChunk.mass;
                grabber.firstChunk.vel += push;
            }

            firstChunk.vel = Vector2.zero;
        }
        public override void TerrainImpact(int chunk, IntVector2 direction, float speed, bool firstContact)
        {
            base.TerrainImpact(chunk, direction, speed, firstContact);

            if (speed > 10)
            {
                room.PlaySound(SoundID.Spear_Fragment_Bounce, firstChunk.pos, 0.35f, 2f);
            }
        }

        #region IDrawable
        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];
            sLeaser.sprites[0] = new FSprite("Circle20", true);
            AddToContainer(sLeaser, rCam, null);
        }
        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            Vector2 pos = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, timeStacker);
            float num = Mathf.InverseLerp(305f, 380f, timeStacker);
            pos.y -= 20f * Mathf.Pow(num, 3f);
            float num2 = Mathf.Pow(1f - num, 0.25f);
            lastDarkness = darkness;
            darkness = rCam.room.Darkness(pos);
            darkness *= 1f - 0.5f * rCam.room.LightSourceExposure(pos);

            sLeaser.sprites[0].x = pos.x - camPos.x;
            sLeaser.sprites[0].y = pos.y - camPos.y;
            sLeaser.sprites[0].rotation = Mathf.Lerp(lastRotation, rotation, timeStacker);
            sLeaser.sprites[0].scaleY = num2 * abstr.scaleY;
            sLeaser.sprites[0].scaleX = num2 * abstr.scaleX;
            sLeaser.sprites[0].color = blackColor;

            if (blink > 0 && randomValue < 0.5f)
                sLeaser.sprites[0].color = blinkColor;

            if (slatedForDeletetion || room != rCam.room)
                sLeaser.CleanSpritesAndRemove();
        }
        public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            blackColor = palette.blackColor;
        }
        public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
        {
            newContainer ??= rCam.ReturnFContainer("Items");

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fsprite.RemoveFromContainer();
                newContainer.AddChild(fsprite);
            }
        }
        #endregion
    }
}
