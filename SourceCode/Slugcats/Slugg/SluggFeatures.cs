using UnityEngine;
using SourceCode.RemixMenu;

namespace SourceCode.Slugcats
{
    internal class SluggFeatures
    {
        public static SlugcatStats.Name slugg { get => SourceCode.Plugin.slgSlugg; }
        public static ManualLogSource Logger { get => SourceCode.Plugin.logger; }

        public static void Hooks()
        {
            // COSMETIC ONES
            On.Player.Die += RandomSoundOnDeath;
        }

        private static void RandomSoundOnDeath(On.Player.orig_Die orig, Player self)
        {
            // makes a random number between 0 and the lenght of random array
            //var random = Randomf.Range(0, Misc.DeathSounds.randomArray.Length);

            Debug.Log("no");
            // then play a random death sound.
            self.room.PlaySound(Misc.DeathSounds.snd_gtav, self.mainBodyChunk);

            orig(self);
        }
    }
}
