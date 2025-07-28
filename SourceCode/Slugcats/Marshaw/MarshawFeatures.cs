// Please for better understanding, just minimizes all #region's
// Its better not only for me, but maybe for you as well
// Because my coding skills sucks

namespace SourceCode.Slugcats
{
    public class MarshawFeatures
    {
        public static SlugcatStats.Name marshaw { get => SourceCode.Plugin.slgMarshaw; }    //name of my slugcat
        public static ManualLogSource Logger { get => SourceCode.Plugin.logger; }

        public static void Hooks()
        {
            // DOUBLE SPEAR HOLD -------------------------------------
            // makes Marshaw hold 2 spears with 2 hands
            On.Player.Grabability += SpearDealer;

            // CRAFTING          -------------------------------------
            // The results of Crafting.
            On.Player.CraftingResults += CraftingResultsManager;
            // The hand managing
            On.Player.GraspsCanBeCrafted += Grasps;
            // After the mods initialize.
            On.RainWorld.PostModsInit += PostMotsInitialize;

            // PUPIFY            -------------------------------------
            // you are pup.
            On.Player.ctor += Pupify;
        }

        #region DOUBLE SPEAR HOLD

        public static Player.ObjectGrabability SpearDealer(On.Player.orig_Grabability orig, Player self, PhysicalObject obj)
        {
            // checks if the scug is Marshaw
            if (self.SlugCatClass == marshaw)
            {
                // if object is Spear
                if (obj is Spear)
                {
                    // return the grab ability with 2 hands (1)
                    return (Player.ObjectGrabability)1;
                }
            }
            // call the orig as return
            return orig(self, obj);
        }

        #endregion
        #region CRAFTING

        // <summary>
        /// create the hooks for let the craft works
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        public static void PostMotsInitialize(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            try
            {
                //Create the hook for [ GourmandCombos.CraftingResults ]
                On.MoreSlugcats.GourmandCombos.CraftingResults += GourmanCombos;

                //Call the orig
                orig(self);
            }
            catch (Exception ex)    //if gets the error instead
            {
                //log the error. Shrimple
                Logger.LogError(ex);
            }
        }
        /// <summary>
        /// Manages some grasps for the specific slugcat (marshaw)
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool Grasps(On.Player.orig_GraspsCanBeCrafted orig, Player self)
        {
            //if the player its Marshaw
            if (self.SlugCatClass == marshaw)
            {
                //return this input for up and method is not null
                return self.input[0].y == 1 && self.CraftingResults() != null;
            }
            //return (call) the orig
            return orig(self);
        }
        public static objType CraftingResultsManager(On.Player.orig_CraftingResults orig, Player self)
        {
            //if the grasps length is less than 2 and the class is not Marshaw
            if (self.grasps.Length < 2 || self.SlugCatClass != marshaw)
            {
                //call orig
                return orig(self);
            }

            //craft results for the hands and the craft results and i dont remember anymore sorry
            //variable for the hands full already for craft
            var craftingResult = Craft(self, self.grasps[0], self.grasps[1]);

            return craftingResult?.type;
        }
        /// <summary> The combos manager, where it will use the "Craft" method for craft things </summary>
        public static objPhy GourmanCombos(On.MoreSlugcats.GourmandCombos.orig_CraftingResults orig, PhysicalObject crafter, Creature.Grasp graspA, Creature.Grasp graspB)
        {
            //If the player is Marshaw
            if ( (crafter as Player).SlugCatClass == marshaw)
            {
                // effects

                //crafter.room.PlaySound(SoundID.SS_AI_Give_The_Mark_Boom, crafter.firstChunk.pos);
                //crafter.room.AddObject(new TemplarCircle(crafter, crafter.firstChunk.pos, 10, 2f, 0f, 10, true) );

                // ~effects

                //return the method that allows you to make the spears, and more
                return Craft(crafter as Player, graspA, graspB);
            }

            //return (call) orig
            return orig(crafter, graspA, graspB);
        }
        /// <summary>
        /// The recipes of craftings are stored here, allowing any ingrendient
        /// </summary>
        /// <returns>a result of the craft from 2 specific items listed here</returns>
        public static objPhy Craft(Player player, Creature.Grasp graspA, Creature.Grasp graspB)
        {
            var spear = new AbstractSpear(player.room.world, null, player.abstractCreature.pos, player.room.game.GetNewID(), false);            //normal spear
            var explosiveSpear = new AbstractSpear(player.room.world, null, player.abstractCreature.pos, player.room.game.GetNewID(), true);          //explosive spear
            var electricSpear = new AbstractSpear(player.room.world, null, player.abstractCreature.pos, player.room.game.GetNewID(), false, true);   //electric spear

            //if have nothing
            if (player == null || graspA?.grabbed == null || graspB?.grabbed == null)
            {
                return null;          //return null if have nothing to do
            }

            //if this scug is Marshaw (if not check it will affect EVERY SCUG)
            if (player.slugcatStats.name == marshaw)
            {
                // -- Crafts:
                // > Rock + Rock = Spear
                // > Spear + Grenade = Spear (explosive)
                // > Spear + Flashbang = Spear (electric | charged)

                // Flashbang + Grenade = SingularityBomb
                if (CraftingHelper(graspA, graspB, objType.FlareBomb, objType.ScavengerBomb))
                {
                    return new AbstractPhysicalObject(player.room.world, DLC_ObjType.SingularityBomb, null, player.abstractCreature.pos, player.room.game.GetNewID());
                }

                // Rock + Rock = Spear
                if (CraftingHelper(graspA, graspB, objType.Rock, objType.Rock))
                {
                    return spear;   //craft Spear
                }

                // Spear + Bomb = Explosion Spear
                if (CraftingHelper(graspA, graspB, objType.Spear, objType.ScavengerBomb))
                {
                    return explosiveSpear;
                }

                // Spear + Flashbang = Electric Spear (charged)
                if (CraftingHelper(graspA, graspB, objType.Spear, objType.FlareBomb))
                {
                    // set charge to 0, making it not charged (otherwise it would be op)
                    electricSpear.electricCharge = 0;
                    return electricSpear;
                }
            }

            return null;    //nothing to do. Is the final of the code
        }

        /// <summary>
        /// Just a grasps helper for short the code.
        /// </summary>
        /// <param name="graspA">The first hand</param>
        /// <param name="graspB">The second hand</param>
        /// <param name="obj1">The first object to be the ingredient</param>
        /// <param name="obj2">The second object to be the ingredient</param>
        /// <returns></returns>
        public static bool CraftingHelper(Creature.Grasp graspA, Creature.Grasp graspB, objType obj1, objType obj2)
        {
            var grabbedA = graspA.grabbed.abstractPhysicalObject.type;          //hand A
            var grabbedB = graspB.grabbed.abstractPhysicalObject.type;          //hand B

            return grabbedA == obj1 && grabbedB == obj2 || grabbedA == obj2 && grabbedB == obj1;
        }

        #endregion
        #region PUPIFY

        public static void Pupify(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            // marshaw is male
            orig(self, abstractCreature, world);

            //check the slugcat is marshaw
            if (self.slugcatStats.name == marshaw)
            {
                // makes marshaw not forced to be grown. grow its a option!!
                self.playerState.forceFullGrown = false;
                // set marshaw to be pup
                self.playerState.isPup = true;
            }
        }

        #endregion
    }
}