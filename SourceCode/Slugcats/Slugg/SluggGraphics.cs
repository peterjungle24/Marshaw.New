using SourceCode.Helpers;

namespace SourceCode.Slugcats
{
    public class SluggGraphics
    {
        private static SlugcatStats.Name slugg { get => Plugin.slgSlugg; }
        private static LogUtils.Logger log => Plugin.log;
        private static TriangleMesh mesh;
        private static TriangleMesh.Triangle[] triangles;
        private static int newIndex;
        
        public static void Hooks()
        {
            On.RainWorld.OnModsInit += Initialize;
            On.PlayerGraphics.InitiateSprites += InitializeSprites;
            //On.PlayerGraphics.DrawSprites += DrawSprites;
            //On.PlayerGraphics.AddToContainer += AddToContainer;
        }

        private static void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            try
            {
                triangles = new TriangleMesh.Triangle[] { new(0, 1, 2), new(0, 1, 3), };
                mesh = new TriangleMesh("Futile_White", triangles, true, false);
            }
            catch (Exception ex) { log.Log($"{FunHelpers.RGB(255, 48, 48)}Thrown from <SourceCode.Slugcats/SluggGraphics/Initialize();>\n{ex}"); throw; }
        }
        private static void InitializeSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            newIndex = sLeaser.sprites.Length + 1;
            log.LogInfo($"InitializeSprites has been called.\no={sLeaser.sprites.Length}\nc={sLeaser.sprites.Length + 1}\n");
            
            if (self.player != null && sLeaser.sprites != null && self.player.slugcatStats.name == slugg)
            {
                // its 13
                // so adding more one,i its 14.
                Array.Resize(ref sLeaser.sprites, sLeaser.sprites.Length + 1);

                if (sLeaser.sprites[newIndex] != null)
                    sLeaser.sprites[newIndex] = mesh;
                else
                    log.Log($"{FunHelpers.RGB(255, 48, 48)}Something went wrong here!");
            }

            orig(self, sLeaser, rCam);
        }
        private static void AddToContainer(On.PlayerGraphics.orig_AddToContainer orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            orig(self, sLeaser, rCam, newContatiner);

            try
            {
                var targetContainer = newContatiner ?? rCam.ReturnFContainer("Midground");

                if (targetContainer != null)
                    targetContainer.AddChild(sLeaser.sprites[newIndex]);
            }
            catch (Exception ex) { log.Log($"{FunHelpers.RGB(255, 48, 48)}Thrown from <SourceCode.Slugcats/SluggGraphics/AddToContainer();>\n{ex}"); throw; }
        }
        private static void DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            try
            {
                var chunk = self.player.mainBodyChunk;

                if (self.player.slugcatStats.name == slugg)
                {
                    (sLeaser.sprites[newIndex] as TriangleMesh).MoveVertice(0, new Vector2(200, 200) );
                    (sLeaser.sprites[newIndex] as TriangleMesh).MoveVertice(1, new Vector2(500, 200) );
                    (sLeaser.sprites[newIndex] as TriangleMesh).MoveVertice(2, new Vector2(200, 500) );
                    (sLeaser.sprites[newIndex] as TriangleMesh).MoveVertice(3, new Vector2(500, 500) );
                }
            }
            catch (Exception ex) { log.Log($"{FunHelpers.RGB(255, 48, 48)}Thrown from <SourceCode.Slugcats/SluggGraphics/DrawSprites();>\n{ex}"); throw; }

            // i finally added this one.
            orig(self, sLeaser, rCam, timeStacker, camPos);
        }
        
    }
}

// if (self.player != null && sLeaser.sprites != null && self.player.slugcatStats.name == slugg)
//{
//    log.LogImportant($"<InitiateSprites> lenght: {f(FunHelpers.RGB(207, 255, 48))}{sLeaser.sprites.Length}");
//}