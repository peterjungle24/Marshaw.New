using UnityEngine;
using BepInEx;
using LogUtils;
using LogUtils.Diagnostics.Tests;
using LogUtils.Enums;
using LogUtils.Properties;
using SourceCode;
using SourceCode.Helpers;
using SourceCode.POM;
using SourceCode.Utilities;
using BepInEx.Logging;
using Mono.Cecil;
using Mono.Cecil.Cil;
using LogUtils.Diagnostics;

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
        public static readonly LogUtils.Logger log = new LogUtils.Logger(LogID.BepInEx, LogID.Unity);

        // i fucking like this one, it stores my mod ID globally
        public static string modID { get => ID; }
        // cool font
        public static string font;

        // i like to store these consts and use them.
        // idk why but i am used to this
        private const string ID = "slugg.mod";
        private const string NAME = "Marshawwwwwwwwwwwww";
        private const string VERSION = "0.1.2";

        // the remix menu instance
        private static RemixMenu.REMIX_menuses remix;

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
            // Add room scripts
            AddRoomScripts();
            // just a method that tests and creates LogIds (logUtils)
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

            #region Tube Worms
            Creatures.GrapleWorm.GlowSaitHooks.Hooks();
            #endregion
            #region lizards
            Creatures.Lizards.ValveLizardHooks.Hooks();
            #endregion

            #endregion
            #region Misc
            SluggShaders.Hooks();
            #endregion
        }

        private void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            // ALWAYS REMEMBER TO CALL THE ORIG AT SOMEWHERE
            orig(self);

            try
            {
                // ascii my beloved
                var hidden = MyMessages.ChoseRandomMessages();
                log.Log($"{Color.yellow}{hidden}");

                /* FONT */
                font = Custom.GetDisplayFont();

                /* MENU REMIX */
                remix = new RemixMenu.REMIX_menuses();
                MachineConnector.SetRegisteredOI(ID, remix);
                /****************************************************/
            }
            catch (Exception ex)
            {
                log.LogError($"<Plugin.Initialize()> looks like something went wrong.\n{ex}");
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
        private void MyLogID()
        {
            var myLog = new LogID("pedro.log", LogAccess.FullAccess, true);
            var properties = new LogProperties("pedro.log");
            var log = new LogUtils.Logger(myLog);

            log.LogInfo("\nWhat The Hell????\n");

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

/* IL (being beguinner and having a giant text of things that i dont even know)

Ldarg_0 refers the the instance itself, unless it is static, and then it refers to the first parameter.

ld - short for load. This puts a reference to something on the stack
st - short for store. This takes something on the stack and stores it in preallocated memory (a reference to something)
fld - short for field. I guess its a field, literally
loc - short for local. It seems to be a local variable. Maybe needs to be emmited
elem - short for element. This is going to be used to store values in a position inside an array

OpCodes.Dup duplicates a value on the stack
OpCodes.pop removes a value from the stack

brtrue - IF STATMENT when true
brfalse, brnull - IF STATMENT when false

null, or 0 is treated as false

That's the purpose of the labels. They specify a instruction in the stack to jump to
It's how loops work, jump statement work, if statement work
For loops it jumps to an earlier place in the stack


You know (no i dont) that marking the label sets the label to target the instruction at the cursor's current position.
And you are using the label to tell the branch instruction where to jump to
*/