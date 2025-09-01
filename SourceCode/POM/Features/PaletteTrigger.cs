using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.POM
{
    public class PaletteTrigger_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        [Vector2Field("scale", 10, 10, Vector2Field.VectorReprType.rect, "Scale")]
        public Vector2 scale;
        [IntegerField("palette", 0, int.MaxValue, 0, ManagedFieldWithPanel.ControlType.text, "Palette")]
        public int palette;

        public PaletteTrigger_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class PaletteTrigger : UpdatableAndDeletable
    {
        ManualLogSource logger { get => Plugin.logger; }
        PlacedObject self;
        Vector2 scale;
        int palette;
        bool state;

        public PaletteTrigger(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;

            scale = Helpers.PomHelpers.GetVector2Field < PaletteTrigger_Data>(self, "scale");
        }

        public override void Update(bool eu)
        {
            var rect = new Rect(self.pos, scale);
            palette = Helpers.PomHelpers.GetIntField<PaletteTrigger_Data>(self, "palette");

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
                    if (rect.Contains(chunk.pos) && state == false)
                    {
                        ChangePalette(rcam);
                        state = true;
                    }
                }
            }
        }

        private void ChangePalette(RoomCamera rcam)
        {
            rcam.ChangeMainPalette(palette);
        }
    }
    public class PaletteTrigger_REPR : ManagedRepresentation
    {
        public PaletteTrigger_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
