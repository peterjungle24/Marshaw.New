using UnityEngine;
using BepInEx;
using LogUtils;
using LogUtils.Diagnostics.Tests;
using LogUtils.Enums;
using LogUtils.Helpers.FileHandling;
using LogUtils.Properties;
using SourceCode;
using SourceCode.Helpers;
using SourceCode.POM;
using SourceCode.Utilities;
using SourceCode.Creatures;
using BepInEx.Logging;
using Mono.Cecil;
using Mono.Cecil.Cil;
using LogUtils.Diagnostics;
using System.Diagnostics;
using Fisobs;
using Fisobs.Core;
using LogUtils.Timers;

namespace SourceCode
{
    // good class template to the rain world modding.
    // this attribute needs to have the SAME "id", "name" and "version" from "modinfo.json" to work well
    [BepInPlugin(ID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // the slugcats ids
        public static bool isMarshaw;
        public static bool isSlugg;
        public static readonly SlugcatStats.Name slgMarshaw = new SlugcatStats.Name("slugg.slugcat.marshaw");
        public static readonly SlugcatStats.Name slgSlugg = new SlugcatStats.Name("slugg.slugcat.slugg");
        public static LogUtils.Logger log;
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
        private LogID myLogID = new LogID("sluggLog", LogsFolder.CurrentPath, LogAccess.FullAccess);

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            log = new LogUtils.Logger(LogID.BepInEx, LogID.Unity, myLogID);

            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
            // Initialize the POM objects
            InitializePOM();
            // Calls (and registers) the both Fisobs and Critobs hooks
            //FisobsHooks();

            Content.Register(new FIsobs.TestObjectFisobs());

            //---------------------------------------------------------------------------+

            #region Slugcats

            // MARSHAW
            //Slugcats.Marshaw.Hooks();
            // SLUGG
            //Slugcats.Slugg.Hooks();
            //Slugcats.SluggGraphics.Hooks();

            #endregion
            #region Misc
            ////-Misc
            //SluggShaders.Hooks();
            #endregion

            On.Player.Update += SlugcatFlags;
        }

        private void SlugcatFlags(On.Player.orig_Update orig, Player self, bool eu)
        {
            if (self.slugcatStats.name == slgMarshaw) isMarshaw = true; else isMarshaw = false;
            if (self.slugcatStats.name == slgSlugg) isSlugg = true; else isSlugg = false;

            orig(self, eu);
        }
        private void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            // ALWAYS REMEMBER TO CALL THE ORIG AT SOMEWHERE
            orig(self);

            try
            {
                log.LogInfo($"{Color.yellow}Marshaw is being initialized.");

                InitializeDefault();
                //InitializeLogUtils();
            }
            catch (Exception ex) { throw ex; }
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
            //RegisterManagedObject<IndividualRender, IndividualRender_Data, IndividualRender_REPR>("Individual Render", objects, false);
            RegisterManagedObject<MyTrigger, MyTrigger_Data, MyTrigger_REPR>("My Trigger", objects, false);
        }
        private void FisobsHooks()
        {
            Creatures.Lizards.TestLizardHooks.OnHooks();
            Creatures.Lizards.LizoBloingHooks.OnHooks();

            RegisterFisobs();
        }
        private void RegisterFisobs()
        {
            Content.Register(new Creatures.Lizards.TestLizardCritob());
            Content.Register(new Creatures.Lizards.LizoBloingCritob());
        }

        private void InitializeDefault()
        {
            #region Font
            // initialize the field "font"
            font = Custom.GetDisplayFont();

            #endregion
            #region Menu Remix
            // initializes the "remix" field
            remix = new RemixMenu.REMIX_menuses();
            // set a registered HI (oi) to th machine connector
            MachineConnector.SetRegisteredOI(ID, remix);
            #endregion
        }
        private void InitializeLogUtils()
        {
            // creates a new group and i get a registered ID
            var logGroup = new LogUtils.LogGroupBuilder()
            {
                Name = "My news Group",
                Path = "_Path A",
                ModIDHint = ID,
            }
                .GetRegisteredID();

            var info = new PathInfo("_Path B");
            
            // i set the permissions here
            logGroup.Properties.FolderPermissions = LogUtils.Enums.FileSystem.FolderPermissions.Move;
            var groupLog = new LogUtils.Logger(new LogID(logGroup, "logFile_0", LogAccess.FullAccess), LogID.BepInEx);

            groupLog.LogInfo("\ti am fucking testing this.");
            groupLog.LogInfo($"\t GetRoot moment. \"{info.GetRoot()}\"");
            log.LogInfo($"\t\t{Color.green}meta(data):\n[{logGroup.Properties.ToData()}]");
        }

        /*
        private void LogsFolder_OnMoveComplete(LogUtils.Events.PathChangeEventArgs e)
        {
            using (var schedule = LogUtils.UtilityCore.Scheduler)
            {
                schedule.Schedule(frameInterval: 40, invokeLimit: -1, syncToRainWorld: true, action: () =>
                {
                    log.LogInfo($"\t{Color.yellow}<Complete>\"myLogID.currentFolderPath\": [{myLogID.Properties.CurrentFolderPath}].");
                    log.LogInfo($"\t{Color.yellow}<Complete>\"e.NewPath\": [{e.NewPath}].");
                    log.LogInfo($"\t{Color.yellow}<Complete>\"LogsFolder.CurrentPath\": [{LogsFolder.CurrentPath}].");
                });
            }

            if (myLogID != null)
            {
                myLogID.Properties.FileLock.Release();
                // idk if its needed but i will add it anyway
                myLogID.Properties.NotifyPathChanged();
                // i think it updates it.
                // this is just a theorical path. i dont think i actually have a place to it.
                myLogID.Properties.ChangePath(e.NewPath);
            }
            else log.LogError($"\t<OnMoveComplete> myLogID is null!");
        }
        private void LogsFolder_OnMoveAborted(LogUtils.Events.PathChangeEventArgs e)
        {
            using (var schedule = LogUtils.UtilityCore.Scheduler)
            {
                schedule.Schedule(frameInterval: 40, invokeLimit: -1, syncToRainWorld: true, action: () =>
                {
                    log.LogInfo($"\t{Color.yellow}<Aborted>\"myLogID.currentFolderPath\": [{myLogID.Properties.CurrentFolderPath}].");
                    log.LogInfo($"\t{Color.yellow}<Aborted>\"e.NewPath\": [{e.NewPath}].");
                    log.LogInfo($"\t{Color.yellow}<Aborted>\"LogsFolder.CurrentPath\": [{LogsFolder.CurrentPath}].");
                });
            }

            if (myLogID != null)
                myLogID.Properties.FileLock.Release();
            else
                log.LogError($"\t<OnMoveAborted> myLogID is null!");
        }
        private void OnPathChangePendingEvent(LogUtils.Events.PathChangeEventArgs e)
        {
            log.LogInfo($"\t{Color.cyan}<Pending>\"myLogID.currentFolderPath\": [{myLogID.Properties.CurrentFolderPath}].");

            if (myLogID != null)
            {
                myLogID.Properties.FileLock.Acquire();

                if (LogsFolder.ContainsPath(myLogID.Properties.CurrentFolderPath) == true)
                {
                    LogsFolder.OnPathChange.CompletedEvent += LogsFolder_OnMoveComplete;
                    LogsFolder.OnPathChange.AbortedEvent += LogsFolder_OnMoveAborted;
                }
            }
            else
                log.LogError($"\t<OnMovePending> myLogID is null!");
        }
        */
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

// oh and having unregistered log files in Logs folder IS NOT REALLY SUPPORTED by LogUtils
// if Fluffball refers to the "log_utils config", then its refering to the file "LogUtils.cfg" with "BepInEx.cfg"
/* About changing properties of LogID

- Register a LogID to create property entries in `logs.txt`
- Once entries are written to file, changing them through a code change will not change behavior by default.

Two ways to change behavior after property entries are written to file.
I. Increase the Version property for your LogID.
II. Set ReadOnly property to false. This will temporarily disable ReadOnly status allowing you to make changes.

Note: If you need to change the path after entries are written to file, DO NOT change through the constructor.The path is connected to theentries written to file, and changing it makes LogUtils believe it is a new log file with new entries to be written to file.
My suggestion to change the path is you can use `Properties.SetPath()` to set a temporary path for your file, or bumping the Version, and then changing the `OriginalFolderPath` field directly.

Example:
--------------------------------------
LogID myLogID = new LogID("example.log", "SomePath", LogAccess.FullAccess, true);

const int LAST_KNOWN_REVISION = 6;
if (myLogID.Properties.Version.Minor < LAST_KNOWN_REVISION)
{
  //Increase properties version
myLogID.Properties.Version.Bump(VersionCode.Minor);
}
myLogID.Properties.OriginalFolderPath = "MyNewPath";
--------------------------------------

This advice only is recommended for code after it has been released. At the development stage, you can change these values through the properties file, and as long as you keep the values in sync, there wont be an issue. No version bump required.

*/