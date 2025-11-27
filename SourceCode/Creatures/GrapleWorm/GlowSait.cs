using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUtils;
using Logger = LogUtils.Logger;
using SourceCode.Helpers;

namespace SourceCode.Creatures.GrapleWorm
{
    public static class GlowSaitEnums
    {
        //If you use the same string name, you can use it like ID in World File [ world_PD.txt ]
        public static readonly CreatureTemplate.Type creatureTemplateType = new(nameof(GlowSait), true);
        public static readonly MultiplayerUnlocks.SandboxUnlockID sandboxUnlockID = new(nameof(GlowSait), true);
    }
    public class GlowSaitTemplate : CreatureTemplate
    {
        private static LogUtils.Logger logger => Plugin.log;

        public GlowSaitTemplate(CreatureTemplate.Type sait_type, CreatureTemplate ancestor, List<TileTypeResistance> tileResistances, List<TileConnectionResistance> connectionResistances, CreatureTemplate.Relationship defaultRelationship) : base(sait_type, ancestor, tileResistances, connectionResistances, defaultRelationship)
        {
            this.name = "Glowing Sait";
            this.AI = true;
            this.canSwim = false;
            this.grasps = 1;

            base.shortcutColor = Color.green;
            base.smallCreature = true;
            base.type = type;
        }
    }
    public class GlowSait : TubeWorm
    {
        private static LogUtils.Logger logger => Plugin.log;

        public GlowSait(AbstractCreature abstractCreature, World world) : base(abstractCreature, world)
        {
        }

        public override void InitiateGraphicsModule()
        {
            if (base.graphicsModule == null)
            {
                base.graphicsModule = new TubeWormGraphics(this);
            }
        }

    }
    public static class GlowSaitHooks
    {
        private static LogUtils.Logger logger => Plugin.log;

        public static void Hooks()
        {
            // Mains
            On.RainWorld.Awake += Awake;
            On.MultiplayerUnlocks.UnlockedCritters += UnlockedCreatures;
            On.StaticWorld.InitStaticWorld += InitializeStaticWorld;
            On.CreatureSymbol.SpriteNameOfCreature += UnlockIcon;
            On.MultiplayerUnlocks.SandboxUnlockID.Init += SandboxUnlockID_Init;
            On.MultiplayerUnlocks.SandboxItemUnlocked += MultiplayerUnlocks_SandboxItemUnlocked;

            // Graphics
            On.TubeWormGraphics.ApplyPalette += ApplyPalette;
            On.TubeWormGraphics.InitiateSprites += InitiateSprites;
            On.TubeWormGraphics.DrawSprites += DrawSprites;
        }

        #region Mains
        private static void Awake(On.RainWorld.orig_Awake orig, RainWorld self)
        {
            orig(self);
            try
            {
                //UNLOCK IDS NEED TO BE SAME CREATURETEMPLATE.TYPE IN STRINGS (i guess)
                var unlockID = GlowSaitEnums.sandboxUnlockID;
                // adds to the list of Unlocks
                MultiplayerUnlocks.CreatureUnlockList.Add(unlockID);
            }
            catch (Exception ex)
            {
                logger.LogError($"<GlowSaitHooks.Awake()> {ex}");
            }
        }
        private static List<CreatureTemplate.Type> UnlockedCreatures(On.MultiplayerUnlocks.orig_UnlockedCritters orig, MultiplayerUnlocks.LevelUnlockID ID)
        {
            var creatureTemplateType = GlowSaitEnums.creatureTemplateType;

            //Get list returned by orig containing unlocked CreatureTemplate.Types for a particular unlock group identified by ID
            List<CreatureTemplate.Type> list = orig(ID);

            //Check if the ID is the same as the level of default
            if (ID == MultiplayerUnlocks.LevelUnlockID.Default)
            {
                //Add my custom TubeWorm template to the list
                list.Add(GlowSaitEnums.creatureTemplateType);
            }

            return list;
        }
        private static void InitializeStaticWorld(On.StaticWorld.orig_InitStaticWorld orig)
        {
            orig();
            try
            {
                // gets the tubeworm ancestor
                var ancestor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.TubeWorm);
                // set relationship of the creature
                var relation = new CreatureTemplate.Relationship(CreatureTemplate.Relationship.Type.Ignores, 0f);
                // sets a tile type resistance
                var tileResistance = new List<TileTypeResistance>();
                // sets a tile connect resistance
                var tileConnectionResistance = new List<TileConnectionResistance>();
                // creates a new creature template from my sait
                var creatureTemplate = new GlowSaitTemplate(GlowSaitEnums.creatureTemplateType, ancestor, tileResistance, tileConnectionResistance, relation);

                // sets my index to my creature template
                StaticWorld.creatureTemplates[GlowSaitEnums.creatureTemplateType.Index] = creatureTemplate;
            }
            catch (Exception me)
            {
                logger.LogError($"<GlowSaitHooks.InitializeStaticWorld> Something went wrong\n{me}");
            }
        }
        private static string UnlockIcon(On.CreatureSymbol.orig_SpriteNameOfCreature orig, IconSymbol.IconSymbolData iconData)
        {
            //if the creature type is the same as my creature_template_type
            if (iconData.critType == GlowSaitEnums.creatureTemplateType)
                // return this icon name
                return "Kill_Tubeworm";

            return orig(iconData);
        }
        private static bool MultiplayerUnlocks_SandboxItemUnlocked(On.MultiplayerUnlocks.orig_SandboxItemUnlocked orig, MultiplayerUnlocks self, MultiplayerUnlocks.SandboxUnlockID unlockID)
        {
            // returns true if the unlockID is 
            if (unlockID == GlowSaitEnums.sandboxUnlockID) return true;

            return orig(self, unlockID);
        }
        private static void SandboxUnlockID_Init(On.MultiplayerUnlocks.SandboxUnlockID.orig_Init orig)
        {
            orig();

            // adds a entry of my sandbox unlock id
            ExtEnum<MultiplayerUnlocks.SandboxUnlockID>.values.AddEntry(GlowSaitEnums.sandboxUnlockID.value);
        }
        #endregion
        #region Graphics
        private static void ApplyPalette(On.TubeWormGraphics.orig_ApplyPalette orig, TubeWormGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            //call the orig
            orig(self, sLeaser, rCam, palette);

            if (self.worm.Template.type == GlowSaitEnums.creatureTemplateType)
            {
                //change colors
                sLeaser.sprites[0].color = new Color(0.6666667f, 0.94509804f, 0.3372549f);
            }
        }
        private static void InitiateSprites(On.TubeWormGraphics.orig_InitiateSprites orig, TubeWormGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            orig(self, sLeaser, rCam);

            // if the creature_template_type is the same as my glow sait
            if (self.worm.Template.type == GlowSaitEnums.creatureTemplateType)
            {
                // resizes the array to have more space
                Array.Resize<FSprite>(ref sLeaser.sprites, sLeaser.sprites.Length + 1);
                // set the new sprite
                sLeaser.sprites[sLeaser.sprites.Length - 1] = new FSprite("FaceB0", true);
                // set the color
                sLeaser.sprites[sLeaser.sprites.Length - 1].color = new Color(160, 2, 2);
                // set the scale
                sLeaser.sprites[sLeaser.sprites.Length - 1].scale = 1.3f;
                // set the shader
                sLeaser.sprites[0].shader = FShader.defaultShader;
                // return the FContainer and add child
                rCam.ReturnFContainer("Midground").AddChild(sLeaser.sprites[sLeaser.sprites.Length - 1]);
            }
        }
        private static void DrawSprites(On.TubeWormGraphics.orig_DrawSprites orig, TubeWormGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            orig(self, sLeaser, rCam, timeStacker, camPos);

            // if the creature_template_type is the same as my glow sait
            if (self.worm.Template.type == GlowSaitEnums.creatureTemplateType)
            {
                // set the alpha to 1
                sLeaser.sprites[0].alpha = 1f;
                // set position of the face to the.. i dont remember
                sLeaser.sprites[sLeaser.sprites.Length - 1].SetPosition((Vector2.Lerp(self.bodyParts[2].lastPos, self.bodyParts[2].pos, timeStacker) + Vector2.Lerp(self.bodyParts[1].lastPos, self.bodyParts[1].pos, timeStacker)) / 2f - camPos);
                // move the face to front
                sLeaser.sprites[sLeaser.sprites.Length - 1].MoveToFront();
            }
        }
        #endregion
    }
}
