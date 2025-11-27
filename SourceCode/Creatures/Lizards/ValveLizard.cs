#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = LogUtils.Logger;
using SourceCode.Helpers;
using Mono;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Xml.XPath;
using LogUtils;
using LogUtils.Diagnostics;
using LogUtils.Diagnostics.Tests;

namespace SourceCode.Creatures.Lizards
{
    public static class ValveLizardEnums
    {
        public static readonly CreatureTemplate.Type creatureTemplateType = Enum_CreatureTemplateType.ValveLizard;
        public static readonly MultiplayerUnlocks.SandboxUnlockID sandboxUnlockID = Enum_SandboxUnlockID.ValveLizard;
    }
    public class ValveLizardTemplate : CreatureTemplate
    {
        private static LogUtils.Logger logger => Plugin.log;

        public ValveLizardTemplate(CreatureTemplate.Type type, CreatureTemplate ancestor, List<TileTypeResistance> tileResistances, List<TileConnectionResistance> connectionResistances, CreatureTemplate.Relationship defaultRelationship) : base(type, ancestor, tileResistances, connectionResistances, defaultRelationship)
        {
            /*
            // sets the class ancestor to the parameter ancestor
            base.ancestor = ancestor;
            // sets the breed parameters for this creature template.
            base.breedParameters = lizardParameters();
            // sets the base type to my CreatureTemplate.Type type (ValveLizard)
            base.type = type;
            */
        }
    }
    public static class ValveLizardHooks
    {
        private static LogUtils.Logger logger => Plugin.log;

        public static void Hooks()
        {
            // Mains
            On.RainWorld.Awake += Awake;
            On.MultiplayerUnlocks.UnlockedCritters += UnlockedCreatures;
            On.CreatureSymbol.SpriteNameOfCreature += UnlockIcon;
            On.CreatureSymbol.ColorOfCreature += ColorIcon;
            On.MultiplayerUnlocks.SandboxUnlockID.Init += SandboxUnlockID_Init;
            On.MultiplayerUnlocks.SandboxItemUnlocked += MultiplayerUnlocks_SandboxItemUnlocked;
            On.LizardBreeds.BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate += BreedTemplates;
            On.StaticWorld.InitCustomTemplates += InitCustomTemplates;

            // Graphics

            // IL
            IL.LizardBreeds.BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate += IL_BreedTtemplates;
        }

        /*
        - Match place after CreatureTemplate is created.
        - Define a label and store it.
        - Emit delegate that checks for your lizard type
        - Emit branch that targets your label when it is not your lizard type
        - Emit instructions that add all necessary values to create your CreatureTemplate. Optionally you can provide your own type here instead of the one on the stack.
        - Emit delegate accepting all of these values that creates your instance and returns it on the stack
        - You might need to emit an OpCode to store it on the stack, but you might not need to do this
        */

        #region Mains
        private static CreatureTemplate.Type originalType;
        private static CreatureTemplate BreedTemplates(On.LizardBreeds.orig_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate orig, CreatureTemplate.Type type, CreatureTemplate lizardAncestor, CreatureTemplate pinkTemplate, CreatureTemplate blueTemplate, CreatureTemplate greenTemplate)
        {
            // set the original type to this type
            originalType = type;
            // create this new proxy type to the type
            CreatureTemplate.Type proxyType = type;
            // if the proxy type is the same as the creatureTemplateType
            if (proxyType == ValveLizardEnums.creatureTemplateType)
                // then sets to the White Lizarrd one.
                proxyType = CreatureTemplate.Type.YellowLizard;

            return orig(proxyType, lizardAncestor, pinkTemplate, blueTemplate, greenTemplate);
        }
        private static void Awake(On.RainWorld.orig_Awake orig, RainWorld self)
        {
            orig(self);
            try
            {
                var unlockID = ValveLizardEnums.sandboxUnlockID;
                MultiplayerUnlocks.CreatureUnlockList.Add(unlockID);
            }
            catch (Exception ex)
            {
                logger.LogError($"<ValveLizardHooks.Awake()> {ex}");
            }
        }
        private static List<CreatureTemplate.Type> UnlockedCreatures(On.MultiplayerUnlocks.orig_UnlockedCritters orig, MultiplayerUnlocks.LevelUnlockID ID)
        {
            var creatureTemplateType = ValveLizardEnums.creatureTemplateType;

            List<CreatureTemplate.Type> list = orig(ID);

            if (ID == MultiplayerUnlocks.LevelUnlockID.Default)
            {
                list.Add(ValveLizardEnums.creatureTemplateType);
            }

            return list;
        }
        private static Color ColorIcon(On.CreatureSymbol.orig_ColorOfCreature orig, IconSymbol.IconSymbolData iconData)
        {
            if (iconData.critType == ValveLizardEnums.creatureTemplateType)
                return Color.yellow;

            return orig(iconData);
        }
        private static string UnlockIcon(On.CreatureSymbol.orig_SpriteNameOfCreature orig, IconSymbol.IconSymbolData iconData)
        {
            //if the creature type is the same as my creature_template_type
            if (iconData.critType == ValveLizardEnums.creatureTemplateType)
                // return this icon name
                return "Kill_White_Lizard";

            return orig(iconData);
        }
        private static bool MultiplayerUnlocks_SandboxItemUnlocked(On.MultiplayerUnlocks.orig_SandboxItemUnlocked orig, MultiplayerUnlocks self, MultiplayerUnlocks.SandboxUnlockID unlockID)
        {
            // returns true if the unlockID is 
            if (unlockID == ValveLizardEnums.sandboxUnlockID) return true;

            return orig(self, unlockID);
        }
        private static void SandboxUnlockID_Init(On.MultiplayerUnlocks.SandboxUnlockID.orig_Init orig)
        {
            orig();

            // adds a entry of my sandbox unlock id
            ExtEnum<MultiplayerUnlocks.SandboxUnlockID>.values.AddEntry(ValveLizardEnums.sandboxUnlockID.value);
        }
        private static void InitCustomTemplates(On.StaticWorld.orig_InitCustomTemplates orig)
        {
            // gets the three creature templates for feed the Templates parameters
            // for the "BreedTemplates" overload
            var pinkLizor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.PinkLizard);
            var blueLizor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.BlueLizard);
            var greenLizor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.GreenLizard);

            // gets the lizard ancestor
            var ancestor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LizardTemplate);
            // set relationship of the creature
            var relation = new CreatureTemplate.Relationship(CreatureTemplate.Relationship.Type.Ignores, 0f);
            // sets a tile type resistance
            var tileResistance = new List<TileTypeResistance>();
            // sets a tile connect resistance
            var tileConnectionResistance = new List<TileConnectionResistance>();
            // creates a new creature template from my lizard
            var creatureTemplate = LizardBreeds.BreedTemplate
            (
                // my creature template type enum       // the lizor ancestor
                ValveLizardEnums.creatureTemplateType, ancestor,
                // all the three templates
                pinkLizor, blueLizor, greenLizor
            );

            // set the creature name
            creatureTemplate.name = "Valve Lizard";
            // looks like some creatures have the pre baked pathing
            // but since this ones doesnt have one, it wont work. so a "false" fits here
            creatureTemplate.doPreBakedPathing = false;
            // the pre baking pathing from the ancestor
            // since i have none, then i need to set to my ancestor
            creatureTemplate.preBakedPathingAncestor = ancestor;

            #region Setting Values from the LizardBreedParams
            // just creates a variable that holds the breed parameters
            var breeds = creatureTemplate.breedParameters as LizardBreedParams;

            breeds.standardColor = FunHelpers.RGB(255, 255, 60);
            breeds.tailColorationStart = 1f;
            breeds.tailColorationExponent = 0.15f;
            breeds.tongueChance = 0.50f;
            breeds.canExitLounge = true;
            breeds.headGraphics = new int[] { 2, 2, 2, 2, 2 };
            breeds.limbSize = 0.50f;
            breeds.bodySizeFac = 0.25f;
            #endregion

            // sets my index to my creature template
            StaticWorld.creatureTemplates[ValveLizardEnums.creatureTemplateType.Index] = creatureTemplate;

            orig();
        }
        #endregion
        #region Graphics

        #endregion
        #region IL
        private static void IL_BreedTtemplates(ILContext il)
        {
            // creates a cursor
            var ponteiro = new ILCursor(il);
            int stlocValue = 0;

            logger.LogInfo("\nBEFORE the pointer creation\n");

            // a GotoNext for matches?
            ponteiro.GotoNext
            (
                // After something, i presume FORWARD
                MoveType.After,
                // matches with stdloc.s
                Z => Z.MatchNewobj<CreatureTemplate>(),
                // and maybe go to next to the "stLoc" index
                Z => Z.MatchStloc(out stlocValue)
            );

            // defines a lable?
            var lable0 = ponteiro.DefineLabel();

            // emit the delegate for check my lizard type?
            ponteiro.EmitDelegate(() => originalType == ValveLizardEnums.creatureTemplateType);
            // i guess it, emits a "if" that targets the thing that, uses a "if" check
            ponteiro.Emit(OpCodes.Brfalse, lable0);

            // emits the second parameter, i hope
            // lizardAncestor
            ponteiro.Emit(OpCodes.Ldarg_1);
            // gets the first local variable...
            // TileType resistances
            ponteiro.Emit(OpCodes.Ldloc_0);
            // gets the second local variable
            // TileConnection resistances
            ponteiro.Emit(OpCodes.Ldloc_1);

            // replace existing template with a new one
            ponteiro.EmitDelegate((CreatureTemplate lizardAncestor, List<TileTypeResistance> tileResist, List<TileConnectionResistance> tileConnect) =>
            {
                if (lizardAncestor == null) logger.LogError("<ValveLizard.cs/ValveLizardHooks.IL_BreedParameters()> \"lizardAncestor\" was null.");
                if (tileResist == null) logger.LogError("<ValveLizard.cs/ValveLizardHooks.IL_BreedParameters()> \"tileResist\" was null.");
                if (tileConnect == null) logger.LogError("<ValveLizard.cs/ValveLizardHooks.IL_BreedParameters()> \"tileConnect\" was null.");

                // emit delegate for accept breed params??
                // for me it only returns this object.
                // no idea if IL will find the right line and do things
                // or it will create one on the start of the hook
                return new ValveLizardTemplate(originalType, lizardAncestor, tileResist, tileConnect, new CreatureTemplate.Relationship(CreatureTemplate.Relationship.Type.Ignores, 0.4f));
            });
            // emits the "stLoc" with the index
            ponteiro.Emit(OpCodes.Stloc, stlocValue);

            // marks after EVERY emit
            ponteiro.MarkLabel(lable0);
        }

        #endregion
        #region methods
        private static LizardBreedParams lizardParameters()
        {
            // the "TileTypeResistance" and "TileConnectionResistance" its kinda similar to me from DNSpy
            // literally
            // --
            // for me it seems this list handles how the AI would move around of the room
            List<TileTypeResistance> list = new List<TileTypeResistance>();
            // For me it seems this list handles how the connections of a movement would work on the room geometry
            // not THAT sure if its correct, but unlike TileTypeResistance, here i use "MovementConnection" instead of "AI_Tile"
            List<TileConnectionResistance> list2 = new List<TileConnectionResistance>();
            // creates a new parameters of LizardBreeds, but, like, not using a template one
            // literally creating one
            LizardBreedParams lizardBreedParams = new LizardBreedParams(CreatureTemplate.Type.BlueLizard);
            // this is kinda confusing to me, looks like its speed of terrain on a geometry, where the AI_Tile can see as accessible.
            lizardBreedParams.terrainSpeeds = new LizardBreedParams.SpeedMultiplier[Enum.GetNames(typeof(AItile.Accessibility)).Length];
            // looks like it means "Body Radius Factor".. i guess
            lizardBreedParams.bodyRadFac = 1f;
            // maybe it means "Pull Down Factor" (ok it looks more clearer)
            lizardBreedParams.pullDownFac = 1f;
            // a factor of the body lenght.
            lizardBreedParams.bodyLengthFac = 1f;

            lizardBreedParams.terrainSpeeds[1] = new LizardBreedParams.SpeedMultiplier(1f, 1f, 0.8f, 1.7f);
            list.Add(new TileTypeResistance(AItile.Accessibility.Floor, 1f, PathCost.Legality.Allowed));
            lizardBreedParams.terrainSpeeds[3] = new LizardBreedParams.SpeedMultiplier(0.8f, 1f, 0.8f, 1.7f);
            list.Add(new TileTypeResistance(AItile.Accessibility.Corridor, 2f, PathCost.Legality.Allowed));
            list2.Add(new TileConnectionResistance(MovementConnection.MovementType.DropToFloor, 40f, PathCost.Legality.Allowed));
            list2.Add(new TileConnectionResistance(MovementConnection.MovementType.LizardTurn, 60f, PathCost.Legality.Allowed));
            lizardBreedParams.biteDelay = 20;
            lizardBreedParams.biteInFront = 40f;
            lizardBreedParams.biteHomingSpeed = 0.7f;
            lizardBreedParams.biteChance = 0.33333334f;
            lizardBreedParams.attemptBiteRadius = 100f;
            lizardBreedParams.getFreeBiteChance = 0.45f;
            lizardBreedParams.biteDamage = 2f;
            lizardBreedParams.biteDamageChance = 0.5f;
            lizardBreedParams.toughness = 2.5f;
            lizardBreedParams.stunToughness = 2.5f;
            lizardBreedParams.regainFootingCounter = 10;
            lizardBreedParams.baseSpeed = 6.7f;
            lizardBreedParams.bodyMass = 7.5f;
            lizardBreedParams.bodySizeFac = 1.2f;
            lizardBreedParams.floorLeverage = 7f;
            lizardBreedParams.maxMusclePower = 16f;
            lizardBreedParams.danger = 0.45f;
            lizardBreedParams.aggressionCurveExponent = 0.95f;
            lizardBreedParams.wiggleSpeed = 0f;
            lizardBreedParams.wiggleDelay = 55;
            lizardBreedParams.bodyStiffnes = 0.5f;
            lizardBreedParams.swimSpeed = 0.6f;
            lizardBreedParams.idleCounterSubtractWhenCloseToIdlePos = 0;
            lizardBreedParams.biteDominance = 0.8f;
            lizardBreedParams.headShieldAngle = 100f;
            lizardBreedParams.canExitLounge = false;
            lizardBreedParams.canExitLoungeWarmUp = false;
            lizardBreedParams.findLoungeDirection = 0f;
            lizardBreedParams.loungeDistance = 170f;
            lizardBreedParams.preLoungeCrouch = 50;
            lizardBreedParams.preLoungeCrouchMovement = -0.3f;
            lizardBreedParams.loungeSpeed = 1f;
            lizardBreedParams.loungeMaximumFrames = 40;
            lizardBreedParams.loungePropulsionFrames = 40;
            lizardBreedParams.loungeJumpyness = 0f;
            lizardBreedParams.loungeDelay = 150;
            lizardBreedParams.riskOfDoubleLoungeDelay = 0.3f;
            lizardBreedParams.postLoungeStun = 40;
            lizardBreedParams.loungeTendensy = 1f;
            lizardBreedParams.perfectVisionAngle = Mathf.Lerp(1f, -1f, 0f);
            lizardBreedParams.periferalVisionAngle = Mathf.Lerp(1f, -1f, 0.2777778f);
            lizardBreedParams.limbSize = 1.4f;
            lizardBreedParams.limbThickness = 1f;
            lizardBreedParams.stepLength = 0.9f;
            lizardBreedParams.liftFeet = 0.5f;
            lizardBreedParams.feetDown = 1f;
            lizardBreedParams.noGripSpeed = 0.05f;
            lizardBreedParams.limbSpeed = 3f;
            lizardBreedParams.limbQuickness = 0.3f;
            lizardBreedParams.limbGripDelay = 1;
            lizardBreedParams.smoothenLegMovement = false;
            lizardBreedParams.legPairDisplacement = 1f;
            lizardBreedParams.standardColor = FunHelpers.RGB(255, 255, 50);
            lizardBreedParams.walkBob = 2f;
            lizardBreedParams.tailSegments = 7;
            lizardBreedParams.tailStiffness = 300f;
            lizardBreedParams.tailStiffnessDecline = 0.6f;
            lizardBreedParams.tailLengthFactor = 0.9f;
            lizardBreedParams.tailColorationStart = 0.05f;
            lizardBreedParams.tailColorationExponent = 4f;
            // the head sizee of the lizard. "1f" its the default value.
            lizardBreedParams.headSize = 1f;
            lizardBreedParams.neckStiffness = 1f;
            lizardBreedParams.jawOpenAngle = 50f;
            lizardBreedParams.jawOpenLowerJawFac = 0.5f;
            lizardBreedParams.jawOpenMoveJawsApart = 14f;
            lizardBreedParams.headGraphics = new int[] { 1, 1, 1, 1, 1 };
            lizardBreedParams.framesBetweenLookFocusChange = 160;
            lizardBreedParams.tamingDifficulty = 0.8f;

            return lizardBreedParams;
        }
        #endregion
    }
}