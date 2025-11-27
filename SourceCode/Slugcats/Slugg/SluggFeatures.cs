using UnityEngine;
using SourceCode.RemixMenu;

namespace SourceCode.Slugcats
{
    public class SluggFeatures
    {
        public static SlugcatStats.Name slugg { get => SourceCode.Plugin.slgSlugg; }
        private static LogUtils.Logger logger => Plugin.log;

        public static void Hooks()
        {
        }
    }
}
