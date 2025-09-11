using SourceCode.POM;
using SourceCode.LogUtilities;
using LogUtils.Diagnostics.Tests;

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
        private static ThisIsSoLogger coolLogger;

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            coolLogger = new ThisIsSoLogger();

            // set logger
            logger = base.Logger;

            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
            // Initialize the POM objects
            InitializePOM();

            //---------------------------------------------------------------------------+
            
            Slugcats.MarshawFeatures.Hooks();
            Slugcats.SluggFeatures.Hooks();

            On.RainWorldGame.Update += CoordOnClick;
            On.Player.Jump += Player_Jump;

            //---------------------------------------------------------------------------
        }

        private void Player_Jump(On.Player.orig_Jump orig, Player self)
        {
            coolLogger.Test();
            coolLogger.Results();

            orig(self);
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

            coolLogger = new LogUtilities.ThisIsSoLogger();

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
    }
}

