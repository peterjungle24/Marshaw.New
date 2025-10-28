using SourceCode.Helpers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Creatures.Lizards
{
    public class LizardTest_Enums
    {
        public static readonly CreatureTemplate.Type CTTlizardTest = new CreatureTemplate.Type("lizardTest", true);
        public static readonly MultiplayerUnlocks.SandboxUnlockID MUlizardTest = new MultiplayerUnlocks.SandboxUnlockID("lizardTest", true);
    }
    public class LizardTestTemplate : CreatureTemplate
    {
        public LizardTestTemplate(CreatureTemplate.Type sait_type, CreatureTemplate ancestor, List<TileTypeResistance> tileResistances, List<TileConnectionResistance> connectionResistances, CreatureTemplate.Relationship defaultRelationship) : base(sait_type, ancestor, tileResistances, connectionResistances, defaultRelationship)
        {
            this.name = "Testing Lizard";
            this.AI = true;
            this.canFly = false;
            this.canSwim = true;

            base.shortcutColor = Color.yellow;
            base.smallCreature = false;
            base.type = type;
        }
    }
    public class LizardTest : Lizard
    {
        public LizardTest(AbstractCreature abstractCreature, World world) : base(abstractCreature, world)
        {
        }

        public override void InitiateGraphicsModule()
        {
            if (base.graphicsModule == null)
            {
                base.graphicsModule = new LizardGraphics(this);
            }
        }
    }
    public class LizardTest_Hooks
    {
        private static LogUtils.Logger log { get => new LogUtils.Logger(Plugin.logger); }
        private static Func<Color, string> f = LogConsole.AnsiColorConverter.AnsiToForeground;

        public static void OnHooks()
        {
            log.Log($"{f(FunHelpers.RGB(148, 82, 41) ) }<LizardTestHooks> was called! (a good thing?)");

            // functional
            On.RainWorld.Awake += AddMultiplayerUnlocks;
            On.StaticWorld.InitCustomTemplates += InitializeCustomTemplate;
            On.StaticWorld.InitStaticWorld += InitializeStaticWorld;
            On.MultiplayerUnlocks.UnlockedCritters += UnlockCreature;
            On.CreatureSymbol.SpriteNameOfCreature += IconUnlockData;
            On.CreatureSymbol.ColorOfCreature += ColorUnlockData;

            LizardTestGraphics.GraphicHooks();
        }

        #region AddMultiplayerUnlocks
        private static void AddMultiplayerUnlocks(On.RainWorld.orig_Awake orig, RainWorld self)
        {
            try
            {
                orig(self);

                // gets a enum of MultiplayerUnlocks for my TestLizard
                var multiUnlock = LizardTest_Enums.MUlizardTest;
                // add the multiplayer unlock to the list of unlocks
                MultiplayerUnlocks.CreatureUnlockList.Add(multiUnlock); 
            }
            catch (Exception ex)
            {
                log.LogError(@"<LizardTest.cs/LizardTest_Hooks/Awake> exception from Catch: " + ex);
            }
        }

        #endregion
        #region IconUnlockData
        private static string IconUnlockData(On.CreatureSymbol.orig_SpriteNameOfCreature orig, IconSymbol.IconSymbolData iconData)
        {
            // gets a creature template.type enum
            var creatureTemplateType = LizardTest_Enums.CTTlizardTest;

            //if the creature type of icon data is the same as my CreatureTemplate
            if (iconData.critType == creatureTemplateType)
                // the icon name. i can use reference from RegionKit "shortcutIcon" object
                return "Kill_Slugcat";

            return orig(iconData);
        }

        #endregion
        #region ColorUnlockData

        private static Color ColorUnlockData(On.CreatureSymbol.orig_ColorOfCreature orig, IconSymbol.IconSymbolData iconData)
        {
            // gets a creature template.type enum
            var creatureTemplateType = LizardTest_Enums.CTTlizardTest;

            //if the creature type of icon data is the same as my CreatureTemplate
            if (iconData.critType == creatureTemplateType)
                // the color of the icon
                return Color.blue;

            return orig(iconData);
        }
        #endregion
        #region InitializeCustomTemplate
        private static void InitializeCustomTemplate(On.StaticWorld.orig_InitCustomTemplates orig)
        {
            try
            {
                // creature template.type
                var creatureTemplateType = LizardTest_Enums.CTTlizardTest;

                // calls orig
                orig();

                // vet a creature template, and assigning to a variable
                var ancestor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LizardTemplate);
                // instance a new relationship of the lizard
                var relationaship = new CreatureTemplate.Relationship(CreatureTemplate.Relationship.Type.Ignores, 1f);
                // i am not sure, maybe a type of resistance of tile?
                var tileTypeResistance = new List<TileTypeResistance>();
                // i am also not sure, maybe a connection resistance of tile
                var tileConnectionResistance = new List<TileConnectionResistance>();
                // instance the CreatureTemplate
                var lizardTemplate = new LizardTestTemplate(creatureTemplateType, ancestor, tileTypeResistance, tileConnectionResistance, relationaship);

                // add my lizard to the list of creature templates
                StaticWorld.creatureTemplates[creatureTemplateType.Index] = lizardTemplate;
            }
            catch (Exception me)
            {
                log.LogError($"<LizardTest.cs/LizardTest_Hooks/InitializeCustomTemplate> TryCatch was sucefully FAILED\n{me}");
            }
        }

        #endregion
        #region InitializeStaticWorld
        private static void InitializeStaticWorld(On.StaticWorld.orig_InitStaticWorld orig)
        {
            try
            {
                // i guess i just, initializes it, not sure
                var creatureTemplateType = LizardTest_Enums.CTTlizardTest;
                orig();
            }
            catch (Exception ex)
            {
                log.LogError($"<LizardTest.cs/LizardTest_Hooks/InitializeStaticWorld> TryCatch was sucefully FAILED\n{ex}");
            }
        }

        #endregion
        #region UnlockCreature
        private static List<CreatureTemplate.Type> UnlockCreature(On.MultiplayerUnlocks.orig_UnlockedCritters orig, MultiplayerUnlocks.LevelUnlockID ID)
        {
            // gets the CreatureTemplate.type here
            var creatureTemplateType = LizardTest_Enums.CTTlizardTest;
            // just logs here for precaution
            log.LogInfo($"<LizardTest.cs/LizardTest_Hooks/UnlockCreature> {f(Color.green) }\"creatureTemplateType\" index before: {creatureTemplateType.Index}");

            // creats a list of creature type to the orig, containing ID
            List<CreatureTemplate.Type> list = orig(ID);

            // if the ID is the same as the default of LevelUnlockID
            if (ID == MultiplayerUnlocks.LevelUnlockID.SU)
            {
                // add my creature template.type to the list
                list.Add(creatureTemplateType);
            }

            // logs the index after all of the logic
            log.LogInfo($"<LizardTest.cs/LizardTest_Hooks/UnlockCreature> {f(Color.green)}\"creatureTemplateType\" index after: {creatureTemplateType.Index}");

            // returns the list (same as returning the ORIG i guess)
            return list;
        }

        #endregion

    }
    public class LizardTestGraphics
    {
        public static LogUtils.Logger log { get => new LogUtils.Logger(Plugin.logger); }
        private static Func<Color, string> f = LogConsole.AnsiColorConverter.AnsiToForeground;

        public static void GraphicHooks()
        {
            // all of this should run the code within inside of this condition:
            // if (self.lizard.Template.type == LizardTest_Enums.CTTlizardTest)
            On.LizardGraphics.InitiateSprites += InitiateSprites;
        }

        private static void InitiateSprites(On.LizardGraphics.orig_InitiateSprites orig, LizardGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            if (self.lizard.Template.type == LizardTest_Enums.CTTlizardTest)
            {
                sLeaser.sprites[0].color = Color.white;
            }

            orig(self, sLeaser, rCam);
        }
    }
}