using UnityEngine;
using BepInEx;
using BepInEx.Logging;

namespace SourceCode
{
    // good class template to the rain world modding.
    // this attribute needs to have the SAME "id", "name" and "version" from "modinfo.json" to work well
    [BepInPlugin(ID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // the fucking logger to use.
        public static ManualLogSource logger;

        // i like to store these consts and use them.
        // idk why but i am used to this
        private const string ID = "slugg.mod";
        private const string NAME = "Marshawwwwwwwwwwwww";
        private const string VERSION = "0.1.2";

        // the core of the mod. hooks will do everything work :>
        public void OnEnable()
        {
            // set logger
            logger = base.Logger;

            // Initialize logging. Useful for see if the mod is actually working.
            On.RainWorld.OnModsInit += Initialize;
        }

        private void Initialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            // Logs to the file
            logger.LogInfo("I am fucking alive!");

            // ALWAYS REMEMBER TO CALL THE ORIG AT SOMEWHERE
            orig(self);
        }
    }
}