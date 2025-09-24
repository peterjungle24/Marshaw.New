using SourceCode.POM;
using SourceCode.UnitTests;
using LogUtils.Diagnostics.Tests;
using SourceCode.Helpers;
using SourceCode.Utilities;

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
        public static string modID { get => ID; }
        
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
        private Func<Color, string> f;
        private LogUtils.Logger log;
        private UnitUtilsTest test;

        // the remix menu instance
        private static RemixMenu.REMIX_menuses remix;

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            // set logger
            logger = base.Logger;

            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
            // Initialize the POM objects
            InitializePOM();

            //---------------------------------------------------------------------------+
            
            // Slugcats
            Slugcats.MarshawFeatures.Hooks();
            Slugcats.SluggFeatures.Hooks();

            SluggShaders.Hooks();

            On.RainWorldGame.Update += CoordOnClick;
            On.Player.Jump += Player_Jump;

            //---------------------------------------------------------------------------
        }

        private void Player_Jump(On.Player.orig_Jump orig, Player self)
        {
            test.Test();

            var report = test.CreateReport();
            log.LogDebug(report);

            orig(self);
        }
        private void CoordOnClick(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            if (Input.GetMouseButtonDown(1) )
            {
                logger.LogInfo($"[{Input.mousePosition.x}], [{Input.mousePosition.y}]");
                UnityEngine.Debug.Log($"[{Input.mousePosition.x}], [{Input.mousePosition.y}]");
            }
            orig(self);
        }
        private void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            try
            {
                log = new LogUtils.Logger(logger);
                f = LogConsole.AnsiColorConverter.AnsiToForeground;
                // Logs to the file
                log.LogInfo($"{f(new Color(255, 119, 0) ) }I am fucking alive!");

                test = new UnitUtilsTest("Cool name for this test in specific");

                // ascii my beloved
                var hidden = FunHelpers.BytesArrayAsString(new byte[] { 66, 114, 111, 32, 105, 32, 114, 101, 97, 108, 108, 121, 32, 108, 105, 107, 101, 32, 116, 104, 105, 115, 32, 119, 97, 121 });
                log.Log($"{f(Color.yellow)}{hidden}");

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
            catch (Exception ex)
            {
                logger.LogError($"<Plugin.Initialize()> looks like something went wrong.\n{ex}");
                Debugf.Log($"<Plugin.Initialize()> looks like something went wrong.\n{ex}");
            }
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
            RegisterManagedObject<GreenScreen, GreenScreen_Data, GreenScreen_REPR>("Green Screen", objects, false);
            RegisterManagedObject<IndividualRender, IndividualRender_Data, IndividualRender_REPR>("Individual Render", objects, false);

            RegisterManagedObject<ToolTip, ToolTip_Data, ToolTip_REPR>("Tool Tip", helpers, false);
            RegisterManagedObject<Texted, Texted_Data, Texted_REPR>("Text Object", helpers, false);
        }
    }
}

/* Getting my mod path

Ok so, theres 2 ways

var path0 = Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) );
var path1 = ModManager.ActiveMods.Find(x => x.id == ID).path;

but here is the thing:
- the path0 returns with \, it looks like a exclusive Windows one
- and path1 returns with /, and it sounds recommended
Rawra note: "when in doubt, try to work with "/" where possible. "\" is a windows exclusive meme and will cause issues most of the time on other systems."

*/
#region Credits

// FluffBall
// Alduris
// Traso (oricord bot dev) (and first outside of Rain World server)
// Rawra
// Glebi (some spriting stuff and more)
// ChefBananex (region stuff)
// RollinGP3 (region stuff)
// BensoneWhite
// slithersss
// slime_cubed (slugbase topics)
// Magica (one of the greatest modder artists i ever seen)
// Nuclear Pasta
// Thalber
// NaCio
// luna ☾fallen/endspeaker☽
// Pocky
// Elliot (Solace's creator)
// IWannaBread
// Rose
// doppelkeks
// Tat011
// Human Resource
// @verityoffaith
// dogcat
// hootis
// Tuko (teached me about my region in first time)
// Ethan Barron
// Bro
// Orinaari (kiki the Scugs) (i still remember their gift after all those times..)
// Nope
// AT
// GreatestGrasshopper
// StormTheCat (Slugpup Safari Dev)

#endregion