using SourceCode.POM;

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

        // the fucking logger to use.
        public static ManualLogSource logger;

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
        }

        private void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            // Logs to the file
            logger.LogInfo("I am fucking alive!");

            /****************************************************/

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

            // objects
            //RegisterManagedObject<Defualt, Defualt_Data, Defualt_REPR>("Defualt", objects, false); (template one)
            RegisterManagedObject<ClimbableSurface, ClimbableSurface_Data, ClimbableSurface_REPR>("Climbable Surface", objects, false);
        }  
    }
}