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
        }
    }
}
