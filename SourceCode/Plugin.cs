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
using SourceCode.Creatures;
using BepInEx.Logging;
using Mono.Cecil;
using Mono.Cecil.Cil;
using LogUtils.Diagnostics;
using System.Diagnostics;
using Fisobs;
using Fisobs.Core;
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

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
            // Initialize the POM objects
            //InitializePOM();
            // Some lizard hooks here
            // if theres more, i will create a new method.
            On.LizardBreeds.BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate += LizardHooks.On_LizardBreeds_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate;
            On.LizardVoice.GetMyVoiceTrigger += LizardHooks.On_LizardVoice_GetMyVoiceTrigger;
            // Initialize the creatures and objects from Fisobs
            RegisterFisobs();
            //---------------------------------------------------------------------------+

            ////-Slugcats
            // MARSHAW
            Slugcats.Marshaw.Hooks();
            // SLUGG
            //Slugcats.Slugg.Hooks();
            //Slugcats.SluggGraphics.Hooks();
            
            ////-Creatures
            // Tube Worms
            Creatures.GrapleWorm.GlowSaitHooks.Hooks();
            // lizards
            Creatures.Lizards.ValveLizardHooks.Hooks();

            ////-Objects
            Objects.FireballHooks.Hooks();

            ////-Misc
            SluggShaders.Hooks();

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
                /*
                // ascii my beloved
                // chooses a random message.. wow
                //var hidden = MyMessages.ChoseRandomMessages();
                // log it.
                //log.Log($"{Color.yellow}{hidden}");

                //- FONT -//
                // initialize the field "font"
                //font = Custom.GetDisplayFont();
                //log.LogInfo($"{Color.yellow}Marshaw is alive, i guess.");

                //- MENU REMIX -//
                // initializes the "remix" field
                //remix = new RemixMenu.REMIX_menuses();
                // set a registered HI (oi) to th machine connector
                //MachineConnector.SetRegisteredOI(ID, remix);
                */

                //var logId = new LogID(group.GetRegisteredID(), group.Name, LogAccess.FullAccess);
                log = new LogUtils.Logger(LogID.BepInEx, LogID.Unity);
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
            RegisterManagedObject<IndividualRender, IndividualRender_Data, IndividualRender_REPR>("Individual Render", objects, false);
            RegisterManagedObject<MyTrigger, MyTrigger_Data, MyTrigger_REPR>("My Trigger", objects, false);

        }
        private void RegisterFisobs()
        {
            Content.Register(new Creatures.Lizards.TestLizardCritob() );
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