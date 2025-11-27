using SourceCode.Helpers;

namespace SourceCode.Slugcats
{
    public class SluggGraphics
    {
        private static SlugcatStats.Name slugg { get => Plugin.slgSlugg; }
        private static LogUtils.Logger log => Plugin.log;
        private static TriangleMesh mesh;
        private static TriangleMesh.Triangle[] triangles;

        public static void Hooks()
        {
            On.RainWorld.OnModsInit += Initialize;
            On.PlayerGraphics.InitiateSprites += InitiateSprites;
            On.PlayerGraphics.DrawSprites += DrawSprites;
        }

        private static void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            try
            {
                triangles = new TriangleMesh.Triangle[] { new(0, 1, 2), new(0, 1, 3), };
                mesh = new TriangleMesh("Futile_White", triangles, true, false);
            }
            catch (Exception ex) { log.LogError($"{FunHelpers.RGB(255, 48, 48)}Thrown from <SourceCode.Slugcats/SluggGraphics/Initialize();>\n{ex}"); }
        }
        private static void InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            try { }
            catch (Exception ex) { log.LogError($"{FunHelpers.RGB(255, 48, 48)}Thrown from <SourceCode.Slugcats/SluggGraphics/InitiateSprites();>\n{ex}"); }
        }
        private static void DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            try
            {
                if (self.player != null && sLeaser.sprites != null && self.player.slugcatStats.name == slugg)
                    log.LogOnce($"<DrawSprites> lenght: {FunHelpers.RGB(207, 255, 48)}{sLeaser.sprites.Length}");
            }
            catch (Exception ex) { log.LogError($"{FunHelpers.RGB(255, 48, 48)}Thrown from <SourceCode.Slugcats/SluggGraphics/DrawSprites();>\n{ex}"); }
        }
    }
}

// if (self.player != null && sLeaser.sprites != null && self.player.slugcatStats.name == slugg)
//{
//    log.LogImportant($"<InitiateSprites> lenght: {f(FunHelpers.RGB(207, 255, 48))}{sLeaser.sprites.Length}");
//}