using SourceCode.Helpers;
using UnityEngine;
namespace SourceCode.Utilities
{
    public class ShaderList
    {
        public const string HoldButtonCircle = "HoldButtonCircle";
        public const string CustomDepth = "CustomDepth";

        #region custom / forks
        public const string slugg_CustomTextureDepth = "slugg.CustomTextureDepth";
        public const string Basic = "Basic";

        #endregion
    }
    public class SluggShaders
    {
        /// My Shaders
        private static LogUtils.Logger log => Plugin.log;
        private static Func<Color, string> f;

        public static void Hooks()
        {
            f = LogUtils.Console.AnsiColorConverter.AnsiToForeground;

            On.RainWorld.LoadResources += LoadShaders;
        }

        private static void LoadShaders(On.RainWorld.orig_LoadResources orig, RainWorld self)
        {
            orig(self);

            try
            {
                // Log the paths to make sure
                // C:/Program Files (x86)/Steam/steamapps/common/Rain World/RainWorld_Data/StreamingAssets\sluggshaders\testingshader
                var file = PathHelpers.GetModFolder() + "/sluggShaders/sluggshaders";
                // C:/Program Files (x86)/Steam/steamapps/common/Rain World/RainWorld_Data/StreamingAssets/sluggshaders/testingshader
                var replace = file.Replace('\\', '/');

                // log if its correct
                log.Log($"{f(Color.yellow)}Sucess!\n{file}\n{replace}\n");

                // ASSET BUNDLE MOMENTOS :)
                AssetBundle assetBundle = AssetBundle.LoadFromFile(replace);
                // this one its being null.
                self.Shaders.Add(ShaderList.slugg_CustomTextureDepth, FShader.CreateShader(ShaderList.slugg_CustomTextureDepth, assetBundle.LoadAsset<Shader>("Assets/Shaders/CustomTextureDepth.shader")));
            }
            catch (Exception ex)
            {
                log.Log($"{f(FunHelpers.RGB(200, 200, 0) )}NOT sucefful!\n{ex}");
            }

            // it output: Sucess!!
            // ..i forgot to log the path for make sure if its correct

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
