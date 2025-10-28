using SourceCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Objects
{
    public class TestingObject : PlayerCarryableItem, IDrawable
    {
        #region Omega Fields
        public static Vector2 objPos;
        public static PlayerCarryableItem self;
        public static float rad;
        public Vector2? GetSetRotation() => setRotation;
        public void SetSetRotation(Vector2? value) => setRotation = value;
        public Vector2 GetLastRotation() => lastRotation;
        public void SetLastRotation(Vector2 value) => lastRotation = value;
        public Vector2 GetRotation() => rotation;
        public void SetRotation(Vector2 value) => rotation = value;
        public float LastDarkness { get; set; }
        public float Darkness { get; set; }
        public Vector2 position { get => objPos; set => objPos = value; }
        public float radius { get => rad; set => rad = value; }
        private static LogUtils.Logger logger { get => new LogUtils.Logger(Plugin.logger); }
        private Vector2 rotation;
        private Vector2 lastRotation;
        private Vector2? setRotation;
        #endregion
        private static LogUtils.Logger log { get => new LogUtils.Logger(Plugin.logger); }
        private static Func<Color, string> f = LogConsole.AnsiColorConverter.AnsiToForeground;

        public TestingObject(AbstractPhysicalObject abstractPhysicalObject, Vector2 lastRotation = default, Vector2 rotation = default, Vector2? setRotation = null) : base(abstractPhysicalObject)
        {
            bodyChunks = new BodyChunk[1];
            bodyChunks[0] = new BodyChunk(this, 0, new Vector2(0f, 0f), 8f, 0.2f);
            bodyChunkConnections = new BodyChunkConnection[0];
            airFriction = 0.999f;
            gravity = 0.50f;
            bounce = 0.50f;
            surfaceFriction = 0.4f;
            collisionLayer = 2;
            waterFriction = 0.98f;
            buoyancy = 0.4f;
            firstChunk.loudness = 7f;

            self = new TestingObject(abstractPhysicalObject);

            SetLastRotation(lastRotation);
            SetRotation(rotation);
            SetSetRotation(setRotation);
        }

        public override void PlaceInRoom(Room placeRoom)
        {
            base.PlaceInRoom(placeRoom);

            Vector2 center = placeRoom.MiddleOfTile(abstractPhysicalObject.pos);

            int i = 0;
            bodyChunks[i].HardSetPosition(new Vector2(1, 1) * 20f + center);
        }
        public override void Update(bool eu)
        {
            base.Update(eu);
        }
        
        public static void OnHooks()
        {
            log.Log($"{f(FunHelpers.RGB(148, 82, 41))}<TestingObject> was called! (a good thing?)");

            On.SandboxGameSession.SpawnItems += Spawn;
            On.Player.Grabability += OnGrabbed;
        }
        private static void Spawn(On.SandboxGameSession.orig_SpawnItems orig, SandboxGameSession self, IconSymbol.IconSymbolData data, WorldCoordinate pos, EntityID entityID)
        {
            try
            {
                // gets the abstract object
                var abs = AbstractTestingObject.TestingObjectAOT;

                // if the item type is same as the abstract
                if (data.itemType == abs)
                {
                    // instantiate a new abstract fireball
                    var abstractFireball = new AbstractTestingObject(self.game.world, pos, entityID);
                    // adds the entity
                    self.game.world.GetAbstractRoom(0).AddEntity(abstractFireball);
                }
                orig(self, data, pos, entityID);
            }
            catch (Exception ex)
            {
                logger.LogError($"<TestingObject.cs/TestingObject/Spawn> failed, \"try\" again.\n{ex}");
            }
        }
        private static Player.ObjectGrabability OnGrabbed(On.Player.orig_Grabability orig, Player self, PhysicalObject obj)
        {
            try
            {
                // if the object is fireball
                if (obj is TestingObject)
                {
                    return Player.ObjectGrabability.OneHand;
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError($"<TestingObject.cs/TestingObject/OnGrabbed> failed, \"try\" again.\n{ex}");
            }
            return orig(self, obj);
        }

        //IDrawable methods
        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];
            sLeaser.sprites[0] = new FSprite("Futile_White", true);

            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            Vector2 vector = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, timeStacker);
            Vector2 v = Vector3.Slerp(GetLastRotation(), GetRotation(), timeStacker);

            int colorWiggle = -39;  // color lerp clock
            if (colorWiggle < 40)
            {
                colorWiggle++;
            }
            else
            {
                colorWiggle = -39;
            }

            Color c = Color.Lerp(Color.yellow, Color.yellow, Mathf.InverseLerp(0, 40, Mathf.Abs(colorWiggle)));

            color = c;

            LastDarkness = Darkness;
            Darkness = rCam.room.Darkness(vector) * (1f - rCam.room.LightSourceExposure(vector));

            var x = vector.x - camPos.x;
            var y = vector.y - camPos.y;

            if (Darkness != LastDarkness)
            {
                ApplyPalette(sLeaser, rCam, rCam.currentPalette);
            }

            for (int i = 0; i < Math.Min(4, sLeaser.sprites.Length); i++)
            {
                sLeaser.sprites[i].x = x;
                sLeaser.sprites[i].y = y;
                sLeaser.sprites[i].rotation = RWCustom.Custom.VecToDeg(v);
                sLeaser.sprites[i].shader = rCam.room.game.rainWorld.Shaders["VectorCircle"];
                sLeaser.sprites[i].scale = 2f;

                objPos = new Vector2(x, y);
                rad = sLeaser.sprites[i].width / 2;
            }

            if (blink > 0 && UnityEngine.Random.value < 0.5f)
            {
                sLeaser.sprites[0].color = blinkColor;
            }
            else
            {
                sLeaser.sprites[0].color = color;
            }

            if (slatedForDeletetion || room != rCam.room)
            {
                sLeaser.CleanSpritesAndRemove();
            }
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            color = UnityEngine.Color.Lerp(new Color(255, 255, 255), palette.blackColor, Darkness);

            sLeaser.sprites[0].color = color;
        }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            newContatiner ??= rCam.ReturnFContainer("Items");

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                newContatiner.AddChild(fsprite);
            }
        }
    }
    internal class AbstractTestingObject : AbstractPhysicalObject
    {
        public static readonly AbstractObjectType TestingObjectAOT = new("TestingObject", true);

        public AbstractTestingObject(World world, WorldCoordinate pos, EntityID ID) : base(world, TestingObjectAOT, null, pos, ID) { }
        public override void Realize()
        {
            // calls the original method
            base.Realize();
            // instantiate the fireball here
            realizedObject = new TestingObject(this);
        }
        public class AbstractTypeFireball
        {
            public static objType TestingObjectOT = new("TestingObject", true);
        }
    }
}