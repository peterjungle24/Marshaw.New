using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SourceCode.Objects
{
    public class Fireball : PlayerCarryableItem, IDrawable
    {
        //These are necessary variables that will be useful to us later
        public Vector2 rotation;
        public Vector2 lastRotation;
        public float darkness;
        public float lastDarkness;

        public Fireball(AbstractPhysicalObject absObj) : base(absObj)
        {
            this.bodyChunks = new BodyChunk[] { new BodyChunk(this, 0, Vector2.zero, 2, 3) };
            this.bodyChunkConnections = new BodyChunkConnection[0];
            this.collisionLayer = 2;
            this.gravity = 0.50f;
            this.airFriction = 0.999f;
            this.waterFriction = 0.90f;
            this.surfaceFriction = 0.5f;
            this.bounce = 0f;
        }

        public override void Update(bool eu)
        {
            base.Update(eu);

            this.lastRotation = this.rotation;
            // if some creature grabs me
            if (this.grabbedBy.Count > 0)
            {
                // this shit is a template LOL (idk what it does)
                this.rotation = Custom.PerpendicularVector(Custom.DirVec(this.firstChunk.pos, this.grabbedBy[0].grabber.mainBodyChunk.pos));
                // doesnt allow you to grab the object when upside down
                this.rotation.y = Mathf.Abs(this.rotation.y);
            }
            // if i touch the ground (grass)
            if (this.firstChunk.contactPoint.y < 0)
            {
                // uhh
                this.rotation = (this.rotation - Custom.PerpendicularVector(this.rotation) * 0.1f * this.firstChunk.vel.x).normalized;
                //Just reducing the speed of the object: the smaller the multiplier, the sooner it will stop
                firstChunk.vel.x *= 0.8f;
            }
        }
        public override void PlaceInRoom(Room placeRoom)
        {
            base.PlaceInRoom(placeRoom);

            this.firstChunk.HardSetPosition(placeRoom.MiddleOfTile(abstractPhysicalObject.pos.Tile));
            //Custom.RNV() just sets a random direction
            this.rotation = Custom.RNV();
            this.lastRotation = rotation;
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[0];
            sLeaser.sprites[0] = new FSprite("Circle20", true) { scale = 0.5f };

            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            var pos = Vector2.Lerp(this.firstChunk.lastPos, this.firstChunk.pos, timeStacker);
            var rotation = Vector2.Lerp(this.lastRotation, this.rotation, timeStacker);
            this.lastDarkness = darkness;

            this.darkness = rCam.room.Darkness(pos) * (1f - rCam.room.LightSourceExposure(pos));
            if (darkness != lastDarkness) ApplyPalette(sLeaser, rCam, rCam.currentPalette);

            foreach (FSprite sprite in sLeaser.sprites)
            {
                sprite.x = pos.x - camPos.x;
                sprite.y = pos.y - camPos.y;
                sprite.rotation = Custom.VecToDeg(rotation);
            }

            //If your object is PlayerCarryableItem, then when approaching an object, it can "flash" in one frame, hinting that it can be grabbed
            if (blink > 0 && UnityEngine.Random.value < 0.5f)
                sLeaser.sprites[0].color = blinkColor;
            else sLeaser.sprites[0].color = color;
            if (slatedForDeletetion || rCam.room != room)
                sLeaser.CleanSpritesAndRemove();
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            this.color = new Color(255f, 165f, 0f);
            this.color = Color.Lerp(color, palette.blackColor, darkness);
            sLeaser.sprites[0].color = this.color;
        }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            newContatiner = newContatiner ?? rCam.ReturnFContainer("Items");
            foreach (FSprite sprite in sLeaser.sprites)
            {
                sprite.RemoveFromContainer();
                newContatiner.AddChild(sprite);
            }
        }
    }
    public class AbstractFireball : AbstractPhysicalObject
    {
        public AbstractFireball(World world, WorldCoordinate pos, EntityID id) : base(world, ObjectRegister.fireball, null, pos, id)
        {
            base.ID = id;
            base.pos = pos;
            base.world = world;
        }
    }
    public static class FireballHooks
    {
        private static LogUtils.Logger log => Plugin.log;

        public static void Hooks()
        {
            // register and unregister -> initialize
            On.RainWorld.OnModsEnabled += Initialize;
            On.RainWorld.OnModsDisabled += Unitialize;
            // realize -> when its spawned, i guess idk
            On.AbstractPhysicalObject.Realize += Realize;
            // grabability -> hook that can make some dependencies to hold
            On.Player.Grabability += GrabAbility;
            // itemicon -> sets the icon
            On.ItemSymbol.SpriteNameForItem += SetSpriteIcon;
            // itemcolor -> sets the color of the icon
            On.ItemSymbol.ColorForItem += SetColorIcon;
        }

        private static Color SetColorIcon(On.ItemSymbol.orig_ColorForItem orig, AbstrObjType itemType, int intData)
        {
            if (itemType == ObjectRegister.fireball) return Color.yellow;

            return orig(itemType, intData);
        }
        private static string SetSpriteIcon(On.ItemSymbol.orig_SpriteNameForItem orig, AbstrObjType itemType, int intData)
        {
            if (itemType == ObjectRegister.fireball) return "Futile_White";

            return orig(itemType, intData);
        }
        private static void Initialize(On.RainWorld.orig_OnModsEnabled orig, RainWorld self, ModManager.Mod[] newlyEnabledMods)
        {
            orig(self, newlyEnabledMods);
            try
            { ObjectRegister.RegisterValues(); }
            catch (Exception ex)
            {
                log.LogError($"Error in \"Objects.FireballHooks.Initialize()\"\n{ex}");
                throw ex;
            }
        }
        private static void Unitialize(On.RainWorld.orig_OnModsDisabled orig, RainWorld self, ModManager.Mod[] newlyDisabledMods)
        {
            orig(self, newlyDisabledMods);

            try
            {
                foreach (ModManager.Mod mod in newlyDisabledMods)
                    if (mod.id == Plugin.modID) ObjectRegister.UnregisterValues();
            }
            catch (Exception ex)
            {
                log.LogError($"Error in \"Objects.FireballHooks.Unitialize()\"\n{ex}");
                throw ex;
            }
        }
        private static void Realize(On.AbstractPhysicalObject.orig_Realize orig, AbstrPhyObject self)
        {
            orig(self);

            if (self.type == ObjectRegister.fireball) self.realizedObject = new Fireball(self);
        }
        private static Player.ObjectGrabability GrabAbility(On.Player.orig_Grabability orig, Player self, PhysicalObject obj)
        {
            if (obj is Fireball)
                return Player.ObjectGrabability.OneHand;

            return orig(self, obj);
        }
    }
}
