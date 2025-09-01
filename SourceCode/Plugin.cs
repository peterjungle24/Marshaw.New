using SourceCode.POM;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;

namespace SourceCode
{
    // good class template to the rain world modding.
    // this attribute needs to have the SAME "id", "name" and "version" from "modinfo.json" to work well
    [BepInPlugin(ID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // the Marshaw scug id
        public static readonly SlugcatStats.Name slgMarshaw = new SlugcatStats.Name("slugg.slugcat.marshaw");
        public static readonly SlugcatStats.Name slgSlugg = new SlugcatStats.Name("slugg.slugcat.slugg");

        // the features

        // the fucking logger to use.
        public static ManualLogSource logger;
        // cool font
        public static string font;

        // i like to store these consts and use them.
        // idk why but i am used to this
        private const string ID = "slugg.mod";
        private const string NAME = "Marshawwwwwwwwwwwww";
        private const string VERSION = "0.1.2";

        // the remix menu instance
        private static RemixMenu.REMIX_menuses remix;

        // some features

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            // set logger
            logger = base.Logger;

            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
            // Initialize the POM objects
            InitializePOM();

            // HOOKS - Slugcat.Marshaw
            Slugcats.MarshawFeatures.Hooks();
            // HOOKS - Slugcat.Slugg
            Slugcats.SluggFeatures.Hooks();

            On.RainWorldGame.Update += CoordOnClick;
            On.Player.Update += BlinkUpdate;
        }

        public enum PlayerBlink
        {
            closed,
            opened,
            died,
            stunned
        }
        private PlayerBlink blinks;
        private void BlinkUpdate(On.Player.orig_Update orig, Player self, bool eu)
        {
            self.Blink(0);

            orig(self, eu);
        }

        private void CoordOnClick(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            if (Input.GetMouseButtonDown(1) )
            {
                logger.LogInfo($"[{Input.mousePosition.x}], [{Input.mousePosition.y}]");
                Debug.Log($"[{Input.mousePosition.x}], [{Input.mousePosition.y}]");
            }
            orig(self);
        }

        private void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            // Logs to the file
            logger.LogInfo("I am fucking alive!");

            /****************************************************/

            /* FONT */
            font = Custom.GetDisplayFont();

            /* MENU REMIX */
            remix = new RemixMenu.REMIX_menuses();
            MachineConnector.SetRegisteredOI(ID, remix);

            /****************************************************/

            // ALWAYS REMEMBER TO CALL THE ORIG AT SOMEWHERE
            orig(self);
        }
        private void InitializePOM()
        {
            var objects = "Slugg objects";
            var helpers = "Slugg Helpers";

            // objects
            //RegisterManagedObject<Defualt, Defualt_Data, Defualt_REPR>("Defualt", objects, false); (template one)
            RegisterManagedObject<ClimbableSurface, ClimbableSurface_Data, ClimbableSurface_REPR>("Climbable Surface", objects, false);
            RegisterManagedObject<Trianglez, Trianglez_Data, Trianglez_REPR>("Triangles Object", objects, false);
            RegisterManagedObject<PaletteTrigger, PaletteTrigger_Data, PaletteTrigger_REPR>("Palette Trigger", objects, false);

            RegisterManagedObject<ToolTip, ToolTip_Data, ToolTip_REPR>("Tool Tip", helpers, false);
            RegisterManagedObject<Texted, Texted_Data, Texted_REPR>("Text Object", helpers, false);
        }

        private void GetValueFromMatrix(ref Matrix4x4 matrix, out float value)
        {
            value = 0;

            for (var row = 0; row < 4; row++)
                for (var column = 0; column < 4; column++)
                    value = matrix[row, column];
        }
    }
}