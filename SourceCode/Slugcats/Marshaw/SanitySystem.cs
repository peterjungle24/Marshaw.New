using SourceCode.POM;
using LogUtils.Diagnostics.Tests;
using SourceCode.Helpers;
using SourceCode.Utilities;
using SourceCode;
using LogUtils;
using Random = UnityEngine.Random;
using JetBrains.Annotations;

namespace SourceCode.Slugcats
{
    public class SanitySystem
    {
        /// <summary>The current alpha from the sprite</summary>
        public float spriteAlpha { get => SanityGraphics.sprite.alpha; }
        /// <summary>Dictionary that stores the amount of sanity each creature will drains from player,<br/>if the player is close enough.</summary>
        public static Dictionary<CreatureTemplate.Type, float> creatureDictionary = new Dictionary<CreatureTemplate.Type, float>();
        /// <summary>List that can regenerate a little bit of sanity, if the player is close enough.</summary>
        public static List<CreatureTemplate.Type> friendlyCreatureTypes = new List<CreatureTemplate.Type>();
        /// <summary>Nice logger for log</summary>
        private static LogUtils.Logger log { get => Plugin.log; }
        /// <summary>flag that checks if the current slugcat is Marshaw</summary>
        private static bool isMarshaw { get => Plugin.isMarshaw; }
        /// <summary>a check when the sprite alpha is below than 0.10f</summary>
        private static bool isCritical;
        private static FakeCreature fakceCrit;
        private static float lastThreat = 0f;

        /// <summary>
        /// Method that acts like a core.<br/>
        /// Connects each private and public method for execute.
        /// </summary>
        public static void OnHooks()
        {
            // SANITY
            On.RainWorld.OnModsInit += OnInitialize;
            On.Player.Update += Logic;

            // GRAPHICS
            On.RainWorld.OnModsInit += SanityGraphics.OnInitialize;
            On.RoomCamera.DrawUpdate += SanityGraphics.UI_Handle;
            On.RoomCamera.DrawUpdate += SanityGraphics.MovingUI;

            On.PlayerGraphics.DrawSprites += Nice;

            // FAKE CREATURE
            FakeCreatureHook.HookOn();
            FakeCreatureEntry.OnEnable();
            On.RainWorldGame.Update += FakeCreature_Update;
        }

        private static void FakeCreature_Update(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            fakceCrit.Update(self);

            orig(self);
        }

        /// <summary>
        /// Executes the sanity logic on the Player update.<br/>
        /// For now, it only works if the current slugcat is Marshaw.
        /// </summary>
        private static void Logic(On.Player.orig_Update orig, Player self, bool eu)
        {
            if (isMarshaw)
            {
                // the acumulative
                float accumulative = 0f;
                // the Player current room
                Room room = self.room;
                // current position
                Vector2 position = self.mainBodyChunk.pos;

                foreach (var obj in room.FindObjectsNearby<Creature>(position, 120f))
                {
                    if (obj != self && obj is Creature creature)
                    {
                        //template.
                        var template = creature.Template.type;
                        //ancestor
                        var ancestor = creature.Template.ancestor;
                        //Calculates the distance_checker between a creature and the _player
                        var dist = (creature.mainBodyChunk.pos - self.mainBodyChunk.pos).magnitude;

                        //if is NOT dead
                        if (creature.dead == false)
                        {
                            //if the distance_checker its below than 120f
                            if (dist <= 120f)
                                accumulative += CalculateDictionary(creature.Template); //Get the sanity value for this creature
                        }
                        else
                            continue;   //otherwise, does nothing
                    }
                }

                //if creature its in the distance_checker, will have the [ threat ] flag
                bool threat = accumulative > 0;
                // accumulative value for regen
                float regen_factoir = 0.0001f;
                // a timer
                float timer = 100f;

                // subtract alpha with accumulative
                SanityGraphics.sprite.alpha -= accumulative;

                // if its not threat and its not dead or not conscious
                if (!threat && (!self.dead || !self.Consious))
                {
                    if (lastThreat >= timer)
                        SanityGraphics.sprite.alpha += regen_factoir;

                    lastThreat += 1f;
                }
                else
                    lastThreat = 0f;

                // if the alpha is less than 0.10f
                if (SanityGraphics.sprite.alpha <= 0.10f)
                { OnCritical(self); isCritical = true; }
                else isCritical = false;
            }

            orig(self, eu);
        }
        /// <summary>Method that initialize values in OnModsInit period.</summary>
        private static void OnInitialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            try
            { CollectionsSetup(); fakceCrit = new(); }
            catch (Exception ex)
            { log.LogError($"<SanitySystem.OnInitialize()> something went wrong.\n{ex}"); }
        }
        /// <summary>Method tha calculates the sanity value, using dictionary and list as base.</summary>
        /// <param name="crit">creature used to be calculated</param>
        private static float CalculateDictionary(CreatureTemplate crit)
        {
            //Constants variables CAN'T BE CHANGED.
            const float def = 0.0005f;  //Default Svalue.
            const float friendly_regen = 0.0010f;   //Sanity change may be positive or negative

            if (crit == null)
                return def;

            //....i think i already defined
            var crit_type = crit.type;          //type
            var crit_ancestor = crit.ancestor;  //ancestors
            float value;                        //Svalue

            //check the crit_type
            if (creatureDictionary.TryGetValue(crit_type, out value))
                return value;

            //check the List
            if (friendlyCreatureTypes.Contains(crit_type))
                return -friendly_regen; //creature restores sanity of _player when it is nearby

            //if ancestor its NOT null
            if (crit_ancestor != null && crit_ancestor.type != null)
            {
                //check the ancestor
                if (creatureDictionary.TryGetValue(crit_ancestor.type, out value))
                    return value;
                if (friendlyCreatureTypes.Contains(crit_ancestor.type))
                    return -friendly_regen; //creature restores sanity of _player when it is nearby
            }

            return def;
        }
        /// <summary>
        /// Method that initializes all the dictionary and list values.<br/>
        /// Better if called in <see cref="OnInitialize"></see>.
        /// </summary>
        private static void CollectionsSetup()
        {
            // - check the dictionary to get a valid value from it.
            // - otherwise you check if its in the friendly list.
            // - any other situation, you apply the default

            // Dict
            creatureDictionary[CreatureTemplate.Type.Scavenger] = 0.0035f;
            creatureDictionary[CreatureTemplate.Type.LizardTemplate] = 0.0022f;
            creatureDictionary[CreatureTemplate.Type.Vulture] = 0.0025f;
            creatureDictionary[CreatureTemplate.Type.Centipede] = 0.0040f;
            creatureDictionary[CreatureTemplate.Type.PoleMimic] = 0.0015f;
            creatureDictionary[CreatureTemplate.Type.Centiwing] = 0.0045f;
            creatureDictionary[CreatureTemplate.Type.SmallCentipede] = 0.0020f;
            creatureDictionary[CreatureTemplate.Type.RedCentipede] = 0.0050f;

            // List
            friendlyCreatureTypes.Add(CreatureTemplate.Type.Fly);
            friendlyCreatureTypes.Add(CreatureTemplate.Type.CicadaA);
            friendlyCreatureTypes.Add(CreatureTemplate.Type.CicadaB);
        }
        /// <summary>
        /// Executes when the sanity is on their critical levels (alpha less than 0.10f)
        /// </summary>
        private static void OnCritical(Player player)
        {
            // gets RCAM from player
            var rcam = player.room.game.cameras[0];

            // eyes closed
            player.Blink(5);
            // makes the screen shake
            rcam.screenShake = 0.05f;
        }
        private static void Nice(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            if (isCritical == true)
                SetAlpha(0.25f);
            else
                SetAlpha(1f);

            orig(self, sLeaser, rCam, timeStacker, camPos);

            void SetAlpha(float alpha)
            {
                for (var i = 0; i < sLeaser.sprites.Length; i++)
                    sLeaser.sprites[i].alpha = alpha;
            }
        }

        /// <summary>Class just made for handle the HUD, UI, idk</summary>
        private static class SanityGraphics
        {
            /// <summary>That ominous circle on the top-right of your screen.</summary>
            public static FSprite sprite;

            /// <summary>Method that initialize things</summary>
            public static void OnInitialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
            {
                orig(self);

                try
                {
                    // initializes the sprite field
                    sprite = new FSprite("Futile_White", true);

                    // sets some values
                    sprite.scaleX = 6f;                                         // scale of the sprite.
                    sprite.scaleY = 6f;
                    sprite.x = 1215.5f;                                         // the PRECISE coordinates of position.
                    sprite.y = 705.25f;
                    sprite.color = Color.white;                                 // sets their initial color.
                    sprite.shader = self.Shaders[ShaderList.HoldButtonCircle];  // shader responsable for make it a circle
                    sprite.alpha = 1f;                                          // alpha behaves if its full or not. 1f is full.
                }
                catch (Exception ex)
                { log.LogError($"<SanitySystem.SanityGraphics.OnInitialize()> something went wrong.\n{ex}"); }
            }
            /// <summary>Handles some UI things here</summary>
            public static void UI_Handle(On.RoomCamera.orig_DrawUpdate orig, RoomCamera self, float timeStacker, float timeSpeed)
            {
                // if its on the story session
                if (self.game.IsStorySession == true)
                {
                    // if its marshaw
                    if (isMarshaw == true)
                        // adds the sprite
                        self.ReturnFContainer("HUD").AddChild(sprite);
                    else
                        // otherwise, remove it.
                        self.ReturnFContainer("HUD").RemoveChild(sprite);

                    // runs the lerping effect
                    LerpEffect(self);
                }

                orig(self, timeStacker, timeSpeed);
            }
            /// <summary>
            /// Its a testing method.<br/>
            /// It allows me to move UI <i>(if i coded)</i>, or just for increase/decrease alpha.
            /// </summary>
            public static void MovingUI(On.RoomCamera.orig_DrawUpdate orig, RoomCamera self, float timeStacker, float timeSpeed)
            {
                orig(self, timeStacker, timeSpeed);
                // variable. avoiding magical numbers
                var speed = 0.02f;

                // decreases alpha by speed
                if (Input.GetKey(KeyCode.Keypad1)) sprite.alpha -= speed;
                // increases alpha by speed
                if (Input.GetKey(KeyCode.Keypad3)) sprite.alpha += speed;
            }
            /// <summary>Method responsable for making the LERP effect on the room, and in itself.</summary>
            public static void LerpEffect(RoomCamera rcam)
            {
                // lerps the dessaturation with the alpha (credits for Alduris)
                rcam.effect_desaturation = Mathf.InverseLerp(1f, 0.10f, sprite.alpha);
                // lerps the darkness with a little bit of the alpha (credits for Alduris)
                rcam.effect_darkness = (1f - sprite.alpha) * 0.25f;
                // when more less alpha the sprite has, more red it will be.
                sprite.color = Color.Lerp(Color.red, Color.white, sprite.alpha);
            }
        }
        private class FakeCreature
        {
            private int waitCounter = 0;

            public void Update(RainWorldGame game)
            {
                waitCounter++;
                foreach (var player in game.Players)
                {
                    if (player.realizedCreature?.room != null && player.realizedCreature?.room == game.cameras[0].room && !game.cameras[0].room.abstractRoom.gate)
                    {
                        foreach (var shortCut in player.realizedCreature.room.shortcuts.Where(i => i.shortCutType == ShortcutData.Type.RoomExit && ((Custom.Dist(i.StartTile.ToVector2() * 20f, player.realizedCreature.DangerPos) > 200 && Custom.Dist(i.StartTile.ToVector2() * 20f, player.realizedCreature.DangerPos) < 600) || Random.value > 0.5f)))
                        {
                            if (Random.value < Custom.LerpMap(
                                    Custom.Dist(shortCut.StartTile.ToVector2() * 20f - new Vector2(10, 10), 
                                    player.realizedCreature.DangerPos), 60, 300, 0.06f, 0.02f, 0.4f) 
                                / 20f * 0.6f *
                                Mathf.Clamp01(waitCounter - 80) *
                                Custom.LerpMap(waitCounter, 80, 120, 0.1f, 1f) *
                                Custom.LerpMap(waitCounter, 300, 500, 1f, 2f) * 0.5f)
                            {

                                AbstractCreature acreature = new AbstractCreature(player.world, FakeCreatureEntry.templates[Random.Range(0, FakeCreatureEntry.templates.Length)], null, player.pos, game.GetNewID());
                                acreature.Realize();
                                var creature = acreature.realizedCreature;
                                creature.inShortcut = true;

                                if (Random.value > 0.025f)
                                {
                                    var module = new FakeCreatureModule(creature);
                                    creature.CollideWithObjects = false;
                                    FakeCreatureHook.modules.Add(creature, module);
                                }
                                else
                                    player.Room.AddEntity(creature.abstractCreature);

                                waitCounter = 0;
                                game.shortcuts.CreatureEnterFromAbstractRoom(creature, player.world.GetAbstractRoom(shortCut.destinationCoord.room), shortCut.destNode);
                            }
                        }
                    }
                }
            }
        }
        private class FakeCreatureEntry
        {
            public static CreatureTemplate[] templates;
            public static Shader displacementShader;

            public static void OnEnable()
            {
                var idk = OnEnable;
                On.StaticWorld.InitStaticWorld += StaticWorld_InitStaticWorld;
            }

            private static void StaticWorld_InitStaticWorld(On.StaticWorld.orig_InitStaticWorld orig)
            {
                orig();
                PostModsInit();
                On.StaticWorld.InitStaticWorld -= StaticWorld_InitStaticWorld;
            }
            public static void PostModsInit()
            {
                templates = new[]
                {
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.RedCentipede),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.RedLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.RedCentipede),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.RedLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.RedCentipede),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.RedLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.CyanLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.YellowLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.BlueLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.GreenLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.PinkLizard),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.Spider),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.CicadaA),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.CicadaB),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.TubeWorm),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.Slugcat),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LanternMouse),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.Salamander),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.Snail),
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.BigNeedleWorm),
                };
            }
            public static void TryAddFakeCreatureModule(Creature source, Creature target)
            {
                if (FakeCreatureHook.modules == null)
                    return;
                if (FakeCreatureHook.modules.TryGetValue(source, out _))
                {
                    var module = new FakeCreatureModule(target);
                    target.CollideWithObjects = false;
                    FakeCreatureHook.modules.Add(target, module);
                }
            }
        }
        private class FakeCreatureHook
        {
            public static ConditionalWeakTable<Creature, FakeCreatureModule> modules = new ConditionalWeakTable<Creature, FakeCreatureModule>();

            public static void HookOn()
            {
                On.Creature.Update += Creature_Update;
                On.Creature.SpitOutOfShortCut += Creature_SpitOutOfShortCut;
                On.Creature.SuckedIntoShortCut += Creature_SuckedIntoShortCut;
                On.Lizard.Collide += Lizard_Collide;
                On.Centipede.Collide += Centipede_Collide;
                On.DaddyLongLegs.Collide += DaddyLongLegs_Collide;
                On.Lizard.AttemptBite += Lizard_AttemptBite;
                On.Lizard.Bite += Lizard_Bite;
                On.Creature.Die += Creature_Die;
                On.Lizard.Violence += Lizard_Violence;
                On.Creature.Violence += Creature_Violence;
            }

            private static void Creature_Die(On.Creature.orig_Die orig, Creature self)
            {
                if (modules.TryGetValue(self, out var module))
                {
                    self.killTag = null;
                    module.SuckIntoShortCut();
                    return;
                }

                orig(self);
            }
            private static void Creature_Violence(On.Creature.orig_Violence orig, Creature self, BodyChunk source, Vector2? directionAndMomentum, BodyChunk hitChunk, PhysicalObject.Appendage.Pos hitAppendage, Creature.DamageType type, float damage, float stunBonus)
            {
                if (source?.owner is Creature crit && modules.TryGetValue(crit, out _)) damage = 0;
                orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus);
            }
            private static void Lizard_Violence(On.Lizard.orig_Violence orig, Lizard self, BodyChunk source, Vector2? directionAndMomentum, BodyChunk hitChunk, PhysicalObject.Appendage.Pos onAppendagePos, Creature.DamageType type, float damage, float stunBonus)
            {
                if (source?.owner is Creature crit && modules.TryGetValue(crit, out _)) damage = 0;
                orig(self, source, directionAndMomentum, hitChunk, onAppendagePos, type, damage, stunBonus);
            }
            private static void Lizard_Bite(On.Lizard.orig_Bite orig, Lizard self, BodyChunk chunk)
            {
                if (modules.TryGetValue(self, out var module) && chunk.owner is Player)
                { module.SuckIntoShortCut(); return; }

                orig(self, chunk);
            }
            private static void Lizard_AttemptBite(On.Lizard.orig_AttemptBite orig, Lizard self, Creature creature)
            {
                if (modules.TryGetValue(self, out var module) && creature is Player)
                { module.SuckIntoShortCut(); return; }

                orig(self, creature);
            }
            private static void DaddyLongLegs_Collide(On.DaddyLongLegs.orig_Collide orig, DaddyLongLegs self, PhysicalObject otherObject, int myChunk, int otherChunk)
            {
                if (modules.TryGetValue(self, out var module) && otherObject is Player)
                { module.SuckIntoShortCut(); return; }

                orig(self, otherObject, myChunk, otherChunk);
            }
            private static void Centipede_Collide(On.Centipede.orig_Collide orig, Centipede self, PhysicalObject otherObject, int myChunk, int otherChunk)
            {
                if (modules.TryGetValue(self, out var module) && otherObject is Player)
                { module.SuckIntoShortCut(); return; }

                orig(self, otherObject, myChunk, otherChunk);
            }
            private static void Lizard_Collide(On.Lizard.orig_Collide orig, Lizard self, PhysicalObject otherObject, int myChunk, int otherChunk)
            {
                if (modules.TryGetValue(self, out var module) && otherObject is Player)
                { module.SuckIntoShortCut(); return; }

                orig(self, otherObject, myChunk, otherChunk);
            }
            private static void Creature_SuckedIntoShortCut(On.Creature.orig_SuckedIntoShortCut orig, Creature self, IntVector2 entrancePos, bool carriedByOther)
            {
                var type = self.room.shortcutData(entrancePos).shortCutType;
                if (modules.TryGetValue(self, out var module) && (type == ShortcutData.Type.RoomExit || type == ShortcutData.Type.CreatureHole))
                { module.SuckIntoShortCut(false); return; }

                orig(self, entrancePos, carriedByOther);
            }
            private static void Creature_SpitOutOfShortCut(On.Creature.orig_SpitOutOfShortCut orig, Creature self, IntVector2 pos, Room newRoom, bool spitOutAllSticks)
            {
                orig(self, pos, newRoom, spitOutAllSticks);

                if (modules.TryGetValue(self, out var module)) module.SpitOutShortCut();
            }
            private static void Creature_Update(On.Creature.orig_Update orig, Creature self, bool eu)
            {
                orig(self, eu);
                if (modules.TryGetValue(self, out var module)) module.Update();
            }
        }
        private class FakeCreatureModule
        {
            public readonly int maxCounter;
            private int counter = -1;
            private WeakReference<Creature> creatureRef;

            public FakeCreatureModule(Creature creature)
            {
                creatureRef = new WeakReference<Creature>(creature);
                maxCounter = Random.Range(200, 500);
            }

            public void Update()
            {
                if (!creatureRef.TryGetTarget(out var creature)) return;

                bool needBreak = false;
                if (creature.room != null)
                {
                    foreach (var ply in creature.room.PlayersInRoom)
                    {
                        if (needBreak) break;
                        if (creature.bodyChunks == null) continue;

                        foreach (var chunk in creature.bodyChunks)
                        {
                            if (chunk == null) continue;
                            if (ply.bodyChunks.Any(i => i != null && Custom.DistLess(i.pos, chunk.pos, (i.rad + chunk.rad))))
                            {
                                SuckIntoShortCut();
                                needBreak = true;
                                break;
                            }
                        }
                    }
                }
                if (needBreak) return;
                if (counter >= 0)
                {
                    if (creature.inShortcut) return;

                    counter++;
                    if (counter == maxCounter)
                    {
                        creature.room.AddObject(new KarmicShockwave(creature, creature.mainBodyChunk.pos, 10, 3f, 5f));
                        creature.Destroy();
                    }
                }
                if (creature.room == null) return;
            }
            public void SpitOutShortCut()
            {
                if (!creatureRef.TryGetTarget(out var creature)) return;
                counter = 0;
            }
            public void Destroy()
            {
                if (!creatureRef.TryGetTarget(out var creature)) return;

                creature.LoseAllGrasps();
                while (creature.grabbedBy.Any())
                {
                    var grasp = creature.grabbedBy.First();
                    grasp.grabber.ReleaseGrasp(grasp.grabber.grasps.IndexOf(grasp));
                }

                creature.Destroy();
            }
            public void SuckIntoShortCut(bool createEffect = true)
            {
                if (!creatureRef.TryGetTarget(out var creature)) return;

                if (creature.graphicsModule != null)
                {
                    creature.room.AddObject(new KarmicShockwave(creature, creature.mainBodyChunk.pos, 10, 3f, 5f));
                    if (createEffect) creature.room.PlaySound(SoundID.SB_A14, 0f, 0.76f, 1f);
                }

                Destroy();
            }
        }
    }
}