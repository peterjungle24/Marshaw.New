using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Misc
{
    public static class DeathSounds
    {
        public static SoundID snd_robloxOOF { get; private set; }
        public static SoundID snd_myLeg { get; private set; }
        public static SoundID snd_hl2Death { get; private set; }
        public static SoundID snd_tom_JerryScream { get; private set; }
        public static SoundID snd_pizzaIsHere { get; private set; }
        public static SoundID snd_connectionTerminated { get; private set; }
        public static SoundID snd_peloBoom { get; private set; }
        public static SoundID snd_penguinz0 { get; private set; }
        public static SoundID snd_battleCatsDeath { get; private set; }
        public static SoundID snd_thatsABug { get; private set; }
        public static SoundID snd_gtav { get; private set; }

        /// <summary>
        /// Initialize these fucking sounds in the [ RainWorld.OnModsInit ] hook.
        /// </summary>
        public static void Initialize()
        {
            snd_robloxOOF = new SoundID("death-robloxOof", true);
            snd_connectionTerminated = new SoundID("death-connectionTerminated", true);
            snd_myLeg = new SoundID("death-myLeg", true);
            snd_gtav = new SoundID("death-gtaV", true);
            snd_hl2Death = new SoundID("death-hl2", true);
            snd_penguinz0 = new SoundID("death-penguinz0", true);
            snd_peloBoom = new SoundID("death-peloBoom", true);
            snd_pizzaIsHere = new SoundID("death-pizzaIsHere", true);
            snd_thatsABug = new SoundID("death-thatsABug", true);
            snd_tom_JerryScream = new SoundID("death-tomJerryScream", true);
            snd_battleCatsDeath = new SoundID("death-battleCats", true);
        }
    }
    public static class CustomSFX
    {
        public static SoundID EFF_doubleJump { get; private set; }

        public static void Initialize()
        {
            EFF_doubleJump = new("double-jump", true);
        }
    }
}
