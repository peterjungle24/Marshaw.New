using SourceCode.Helpers;
using UnityEngine;
namespace SourceCode.Utilities
{
    public class ShaderList
    {
        public const string HoldButtonCircle = "HoldButtonCircle";
        public const string CustomDepth = "CustomDepth";
        public static string VectorCircle = "VectorCircle";
        public static string GhostDistortion = "GhostDisto";
        public const string Basic = "Basic";

        #region custom / forks
        public const string slugg_CustomTextureDepth = "slugg.CustomTextureDepth";
        #endregion
    }
    public class SluggShaders
    {
        /// My Shaders
        private static LogUtils.Logger log => Plugin.log;

        public static void Hooks()
        {
            On.RainWorld.LoadResources += LoadShaders;
        }

        private static void LoadShaders(On.RainWorld.orig_LoadResources orig, RainWorld self)
        {
            orig(self);

            // Log the paths to make sure
            // C:/Program Files (x86)/Steam/steamapps/common/Rain World/RainWorld_Data/StreamingAssets\sluggshaders\testingshader
            var file = PathHelpers.GetModFolder() + "/sluggShaders/sluggshaders";
            // C:/Program Files (x86)/Steam/steamapps/common/Rain World/RainWorld_Data/StreamingAssets/sluggshaders/testingshader
            var replace = file.Replace('\\', '/');

            // ASSET BUNDLE MOMENTOS :)
            AssetBundle assetBundle = AssetBundle.LoadFromFile(replace);
            if (assetBundle != null)
                self.Shaders.Add(ShaderList.slugg_CustomTextureDepth, FShader.CreateShader(ShaderList.slugg_CustomTextureDepth, assetBundle.LoadAsset<Shader>("Assets/Shaders/CustomTextureDepth.shader")));
            else
                throw new NullReferenceException($"<ShaderList.LoadShaders()> it seems like the \"assetBundle\" is null");
            /*
            try
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(AssetManager.ResolveFilePath(PathHelpers.GetModFolder() + $"/sluggShaders"));
                // Add Shaders
                self.Shaders.Add("slugg.TestingShader", FShader.CreateShader("slugg.TestingShader", assetBundle.LoadAsset<Shader>("Assets/Shaders/TestingShader.shader")));

                log.LogInfo($"{f(FunHelpers.RGB(74, 82, 237) ) }<Shaders.cs/LoadShaders().try>{f(Color.white) } called after!\n");
            }
            catch (Exception ex)
            {
                log.LogError(ex);
            }
            */
        }
    }
}
