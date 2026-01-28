using UnityEngine;
using SourceCode.RemixMenu;

namespace SourceCode.Slugcats
{
    public class Slugg
    {
        public static SlugcatStats.Name slugg { get => SourceCode.Plugin.slgSlugg; }
        private static LogUtils.Logger logger => Plugin.log;

        public static void Hooks()
        {
        }
    }
}
