using SourceCode.POM;
using LogUtils.Diagnostics.Tests;
using SourceCode.Helpers;
using SourceCode.Utilities;
using SourceCode;
using Fisobs.Core;
using SourceCode.Objects;
using LogUtils;
using LogUtils.Enums;
using LogUtils.Properties;

namespace SourceCode
{
    // good class template to the rain world modding.
    // this attribute needs to have the SAME "id", "name" and "version" from "modinfo.json" to work well
    [BepInPlugin(ID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // the slugcats ids
        public static readonly SlugcatStats.Name slgMarshaw = new SlugcatStats.Name("slugg.slugcat.marshaw");
        public static readonly SlugcatStats.Name slgSlugg = new SlugcatStats.Name("slugg.slugcat.slugg");
        // i fucking like this one, it stores my mod ID globally
        public static string modID { get => ID; }
        // the fucking logger to use.
        public static ManualLogSource logger;
        // cool font
        public static string font;

        // i like to store these consts and use them.
        // idk why but i am used to this
        private const string ID = "slugg.mod";
        private const string NAME = "Marshawwwwwwwwwwwww";
        private const string VERSION = "0.1.2";
        private LogUtils.Logger log;

        // the remix menu instance
        private static RemixMenu.REMIX_menuses remix;

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            // set logger
            logger = base.Logger;

            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
            // Add room scripts
            AddRoomScripts();

            MyLogID();

            // Initialize the POM objects
            InitializePOM();
            
            //---------------------------------------------------------------------------+

            #region Slugcats
            Slugcats.MarshawFeatures.Hooks();

            Slugcats.SluggFeatures.Hooks();
            //Slugcats.SluggGraphics.Hooks();
            #endregion
            #region Creatures

            #region lizards
            // i moved to somewhere
            #endregion

            #endregion
            #region Objects
            Objects.TestingObject.OnHooks();
            #endregion
            #region Misc
            SluggShaders.Hooks();
            //MyTrigger_Hooks.OnHooks();
            #endregion

            //---------------------------------------------------------------------------
        }

        private void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            // ALWAYS REMEMBER TO CALL THE ORIG AT SOMEWHERE
            orig(self);

            try
            {
                // register Fisobs objects and creatures
                RegisterFisobs();

                log = new LogUtils.Logger(logger);

                // ascii my beloved
                var hidden = MyMessages.ChoseRandomMessages();
                log.Log($"{Color.yellow}{hidden}");

                /****************************************************/

                Creatures.Lizards.LizardTest_Hooks.OnHooks();

                /****************************************************/

                /* FONT */
                font = Custom.GetDisplayFont();

                /* MENU REMIX */
                remix = new RemixMenu.REMIX_menuses();
                MachineConnector.SetRegisteredOI(ID, remix);

                /****************************************************/
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
            
            // objects
            //RegisterManagedObject<Defualt, Defualt_Data, Defualt_REPR>("Defualt", objects, false); (template one)
            RegisterManagedObject<ClimbableSurface, ClimbableSurface_Data, ClimbableSurface_REPR>("Climbable Surface", objects, false);
            RegisterManagedObject<Trianglez, Trianglez_Data, Trianglez_REPR>("Triangles Object", objects, false);
            RegisterManagedObject<PaletteTrigger, PaletteTrigger_Data, PaletteTrigger_REPR>("Palette Trigger", objects, false);
            RegisterManagedObject<GreenScreen, GreenScreen_Data, GreenScreen_REPR>("Green Screen", objects, false);
            RegisterManagedObject<IndividualRender, IndividualRender_Data, IndividualRender_REPR>("Individual Render", objects, false);
            RegisterManagedObject<MyTrigger, MyTrigger_Data, MyTrigger_REPR>("My Trigger", objects, false);

        }
        private void RegisterFisobs()
        {
            CustomObject_Hooks.OnHooks();

            Content.Register(new CustomObjectFisob() );
        }
        private void AddRoomScripts()
        {
            On.RoomSpecificScript.AddRoomSpecificScript += (On.RoomSpecificScript.orig_AddRoomSpecificScript orig, Room room) =>
            {
                if (room.abstractRoom.name == "SU_A24")
                {
                    if (room.game.IsStorySession)
                        room.AddObject(new RoomScripts.RSCR_WhateverIsThis(room) );
                }
            };
        }

        protected private void MyLogID()
        {
            var myLog = new LogID("pedro.log", LogAccess.FullAccess, true);
            var properties = new LogProperties("pedro.log");
            var log = new LogUtils.Logger(myLog);

            log.LogInfo("\nWhat The Hell????\n");
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