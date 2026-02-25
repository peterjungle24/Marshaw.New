using UnityEngine;
using SourceCode.Creatures.Lizards;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using SourceCode.Helpers;

namespace SourceCode.Creatures.Lizards
{
    public static class LizoBloingHooks
    {
        public static void OnHooks()
        {
            On.LizardBreeds.BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate += On_LizardBreeds_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate;
            On.LizardVoice.GetMyVoiceTrigger += On_LizardVoice_GetMyVoiceTrigger;
            On.MultiplayerUnlocks.UnlockedCritters += UnlockedCritters;
        }

        private static CreatureTemplate On_LizardBreeds_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate(On.LizardBreeds.orig_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate orig, CreatureTemplate.Type type, CreatureTemplate lizardAncestor, CreatureTemplate pinkTemplate, CreatureTemplate blueTemplate, CreatureTemplate greenTemplate)
        {
            // breed stuff, this determines your lizard's general stats.
            CreatureTemplate temp;
            LizardBreedParams breedParams;
            if (type == Enums.CreatureTemplateType.LizoBloing)
            {
                temp = orig(type, lizardAncestor, pinkTemplate, blueTemplate, greenTemplate);
                breedParams = (temp.breedParameters as LizardBreedParams)!;
                temp.type = type;
                temp.name = "LizoBloing";
                breedParams.template = type;
                breedParams.standardColor = FunHelpers.RGB(255, 255, 60);
                breedParams.baseSpeed = 4.2f;
                breedParams.terrainSpeeds[1] = new(1f, 1f, 1f, 1f);
                breedParams.terrainSpeeds[3] = new(1f, 1f, 1f, 1f);
                breedParams.terrainSpeeds[4] = new(1f, 1f, 1f, 1f);
                breedParams.terrainSpeeds[5] = new(.1f, 1f, 1f, 1f);
                breedParams.terrainSpeeds[6] = new(.1f, 1f, 1f, 1f);
                breedParams.biteDelay = 2;
                breedParams.biteInFront = 20f;
                breedParams.biteRadBonus = 20f;
                breedParams.biteHomingSpeed = 4.5f;
                breedParams.biteChance = 0.9f;
                breedParams.attemptBiteRadius = 120f;
                breedParams.getFreeBiteChance = 1f;
                breedParams.biteDamage = 1f;
                breedParams.biteDamageChance = 0.2f;
                breedParams.toughness = 2f;
                breedParams.stunToughness = 4f;
                breedParams.regainFootingCounter = 1;
                breedParams.bodyMass = 2f;
                breedParams.bodySizeFac = 0.25f;
                breedParams.floorLeverage = 8f;
                breedParams.maxMusclePower = 12f;
                breedParams.wiggleSpeed = 0.8f;
                breedParams.wiggleDelay = 20;
                breedParams.bodyStiffnes = 0.4f;
                breedParams.swimSpeed = 1f;
                breedParams.idleCounterSubtractWhenCloseToIdlePos = 10;
                breedParams.danger = 0.5f;
                breedParams.aggressionCurveExponent = 0.7f;
                breedParams.headShieldAngle = 160f;
                temp.visualRadius = 2300f;
                temp.waterVision = 0.7f;
                temp.throughSurfaceVision = 0.95f;
                breedParams.perfectVisionAngle = Mathf.Lerp(1f, -1f, 4f / 9f);
                breedParams.periferalVisionAngle = Mathf.Lerp(1f, -1f, 7f / 9f);
                breedParams.biteDominance = 1f;
                breedParams.limbSize = 0.50f;
                breedParams.stepLength = 1f;
                breedParams.liftFeet = 0.3f;
                breedParams.feetDown = 0.5f;
                breedParams.noGripSpeed = 0.25f;
                breedParams.limbSpeed = 6f;
                breedParams.limbQuickness = 0.6f;
                breedParams.limbGripDelay = 1;
                breedParams.smoothenLegMovement = true;
                breedParams.legPairDisplacement = .03f;
                breedParams.walkBob = 2.7f;
                breedParams.tailSegments = 16;
                breedParams.tailStiffness = 300f;
                breedParams.tailStiffnessDecline = 0.25f;
                breedParams.tailLengthFactor = 0.8f;
                breedParams.tailColorationStart = 0.9f;
                breedParams.tailColorationExponent = 8f;
                breedParams.headSize = 1f;
                breedParams.neckStiffness = 0.37f;
                breedParams.jawOpenAngle = 150f;
                breedParams.jawOpenLowerJawFac = 0.7666667f;
                breedParams.jawOpenMoveJawsApart = 25f;
                breedParams.headGraphics = new int[5];
                breedParams.framesBetweenLookFocusChange = 20;
                breedParams.tamingDifficulty = 3f;
                temp.movementBasedVision = 0.3f;
                temp.waterPathingResistance = 3f;
                temp.dangerousToPlayer = breedParams.danger;
                temp.doPreBakedPathing = false;
                temp.requireAImap = true;
                temp.preBakedPathingAncestor = pinkTemplate;
                temp.meatPoints = 7;
                temp.baseDamageResistance = breedParams.toughness * 2f;
                temp.baseStunResistance = breedParams.toughness;
                temp.damageRestistances[(int)Creature.DamageType.Bite, 0] = 2.5f;
                temp.damageRestistances[(int)Creature.DamageType.Bite, 1] = 3f;

                return temp;

                /*
                breeds.standardColor = FunHelpers.RGB(255, 255, 60);
                breeds.tailColorationStart = 1f;
                breeds.tailColorationExponent = 0.15f;
                breeds.tongueChance = 0.50f;
                breeds.canExitLounge = true;
                breeds.headGraphics = new int[] { 2, 2, 2, 2, 2 };
                breeds.limbSize = 0.50f;
                breeds.bodySizeFac = 0.25f;
                */
            }
            return orig(type, lizardAncestor, pinkTemplate, blueTemplate, greenTemplate);
        }
        private static SoundID On_LizardVoice_GetMyVoiceTrigger(On.LizardVoice.orig_GetMyVoiceTrigger orig, LizardVoice self)
        {
            var res = orig(self);
            List<SoundID> list;
            SoundID soundID;
            if (self.lizard is Lizard l)
            {
                if (l is TestLizard)
                {
                    // the voice here is green lizard, you can change that if needed
                    var array = new[]
                    {
                        SoundID.Lizard_Voice_Pink_A,
                        SoundID.Lizard_Voice_Green_A,
                    };
                    list = [];
                    for (var i = 0; i < array.Length; i++)
                    {
                        soundID = array[i];
                        if (soundID.Index != -1 && l.abstractPhysicalObject.world.game.soundLoader.workingTriggers[soundID.Index])
                            list.Add(soundID);
                    }
                    if (list.Count == 0)
                        res = SoundID.None;
                    else
                        res = list[Random.Range(0, list.Count)];
                }

            }
            return res;
        }
        private static List<CreatureTemplate.Type> UnlockedCritters(On.MultiplayerUnlocks.orig_UnlockedCritters orig, MultiplayerUnlocks.LevelUnlockID ID)
        {
            var creatureTemplateType = Enums.CreatureTemplateType.LizoBloing;

            List<CreatureTemplate.Type> list = orig(ID);

            if (ID == MultiplayerUnlocks.LevelUnlockID.Default)
                list.Add(Enums.CreatureTemplateType.LizoBloing);

            return list;
        }
    }
}