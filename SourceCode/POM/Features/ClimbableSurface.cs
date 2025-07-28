using SourceCode.Helpers;

namespace SourceCode.Pom.Features
{
    public class ClimbableSurface_Data : ManagedData
    {
        [Vector2Field("size", 0, 0, Vector2Field.VectorReprType.rect, "SIZE")]
        public Vector2 size;
        //the custom fields are added as a parameter for the base class
        public ClimbableSurface_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class ClimbableSurface : UpdatableAndDeletable
    {
        PlacedObject self;
        ManualLogSource logger { get => Plugin.logger; }
        Vector2 size;

        public ClimbableSurface(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;

            size = PomHelpers.GetVector2Field<ClimbableSurface_Data>(self, "size");
        }

        public override void Update(bool eu)
        {
            var rect = new Rect(self.pos, size);

            // i like this more than Foreach :D
            for (var i = 0; i < this.room.game.Players.Count; i++)
            {
                // if the list of players is not null
                if (this.room.game.Players[i] != null)
                {
                    var plr = room.game.Players[i]?.realizedCreature as Player; // get player
                    var rcam = this.room.game.cameras[0];                       // get rcam
                    var chunk = plr.mainBodyChunk;                              // main body

                    // checks if the BODY of the player its inside
                    if (rect.Contains(chunk.pos))
                        // climb
                        ClimbFunction(plr);
                }
            }
        }

        private void ClimbFunction(Player self)
        {
            // direction variable
            int direction = 0;
            // according from the player directional input, wi  ll change the direction
            // allows to "jump" at the same direction of the wall, instead of the opposite side
            if (self.input[0].x < 0) direction = -1;
            if (self.input[0].x > 0) direction = 1;

            // keybind for "climb" upwards (upwards is the right word?)
            // and also checks if the slide counter is upper than 0, we dont want to climb the non walls :monksilly:
            if (self.input[0].y > 0 && self.wallSlideCounter > 0)
            {
                // jump
                self.WallJump(direction);

                Debug.Log("Something/ClimbFunction(Player self) -> here.");
                logger.LogWarning("Something/ClimbFunction(Player self) -> here.");
            }
        }
    }
    public class ClimbableSurface_REPR : ManagedRepresentation
    {
        public ClimbableSurface_REPR(PlacedObject.Type type, ObjectsPage objectPage, PlacedObject placedObject) : base(type, objectPage, placedObject)
        {
            // nothing
            
        }
    }
}