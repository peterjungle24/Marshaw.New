using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.POM
{
    public class ClimbableSurface_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        [Vector2Field("scale", 10, 10, Vector2Field.VectorReprType.rect, "Scale")]
        public Vector2 scale;

        public ClimbableSurface_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class ClimbableSurface : UpdatableAndDeletable
    {
        LogUtils.Logger logger { get => Plugin.log; }
        PlacedObject self;
        Vector2 scale;

        public ClimbableSurface(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;

            scale = Helpers.PomHelpers.GetVector2Field < ClimbableSurface_Data>(self, "scale");
        }

        public override void Update(bool eu)
        {
            var rect = new Rect(self.pos, scale);

            // i like this more than Foreach :D
            for (var i = 0; i < this.room.game.Players.Count; i++)
            {
                // if the list of players is not null
                if (this.room.game.Players[i] != null)
                {
                    // get player
                    var plr = room.game.Players[i]?.realizedCreature as Player;
                    // get rcam
                    var rcam = this.room.game.cameras[0];
                    // main body
                    var chunk = plr.mainBodyChunk;

                    // checks if the BODY of the player its inside
                    if (rect.Contains(chunk.pos))
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
                // jump
                self.WallJump(direction);
        }
    }
    public class ClimbableSurface_REPR : ManagedRepresentation
    {
        public ClimbableSurface_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object) { }
    }
}
