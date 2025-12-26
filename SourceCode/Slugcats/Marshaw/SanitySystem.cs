using SourceCode.POM;
using LogUtils.Diagnostics.Tests;
using SourceCode.Helpers;
using SourceCode.Utilities;
using SourceCode;
using LogUtils;

namespace SourceCode.Slugcats.Marshaw
{
    public class SanitySystem
    {
        /// <summary>Dictionary that stores the amount of sanity each creature will drains from player,<br/>if the player is close enough.</summary>
        public static Dictionary<CreatureTemplate.Type, float> creatureDictionary = new Dictionary<CreatureTemplate.Type, float>();
        /// <summary>List that can regenerate a little bit of sanity, if the player is close enough.</summary>
        public static List<CreatureTemplate.Type> friendlyCreatureTypes = new List<CreatureTemplate.Type>();
        /// <summary>Nice logger for log</summary>
        private static LogUtils.Logger log { get => Plugin.log; }
        /// <summary>flag that checks if the current slugcat is Marshaw</summary>
        private static bool isMarshaw { get => Plugin.isMarshaw; }
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
                    OnCritical(self);
            }

            orig(self, eu);
        }
        /// <summary>Method that initialize values in OnModsInit period.</summary>
        private static void OnInitialize(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            try
                { CollectionsSetup(); }
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
            if (friendlyCreatureTypes.Contains(crit_type) )
                return -friendly_regen; //creature restores sanity of _player when it is nearby

            //if ancestor its NOT null
            if (crit_ancestor != null && crit_ancestor.type != null)
            {
                //check the ancestor
                if (creatureDictionary.TryGetValue(crit_ancestor.type, out value))
                    return value;
                if (friendlyCreatureTypes.Contains(crit_ancestor.type) )
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
            // eyes closed
            player.Blink(5);
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
    }
}