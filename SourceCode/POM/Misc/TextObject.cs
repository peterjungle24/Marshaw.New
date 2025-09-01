using SourceCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.POM
{
    public class Texted_Data : ManagedData
    {
        [StringField("text", "default text", "display text")]
        public string text;
        [ColorField("textColor", 1, 1, 1, 1, ManagedFieldWithPanel.ControlType.button, "text color")]
        public Color textColor;
        [FloatField("scale", 0, 100f, 2f, 0.01f, ManagedFieldWithPanel.ControlType.slider, "text scale")]
        public float scale;
        [FloatField("rotation", 0, 380f, 0f, 0.01f, ManagedFieldWithPanel.ControlType.slider, "text rotation")]
        public float rotation;
        [FloatField("alpha", 0.0f, 1f, 1f, 0.01f, ManagedFieldWithPanel.ControlType.slider, "text opacity/alpha")]
        public float alpha;

        //the custom fields are added as a parameter for the base class
        public Texted_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class Texted : UpdatableAndDeletable
    {
        PlacedObject self;
        ManualLogSource logger { get => Plugin.logger; }
        FLabel ftext;

        public Texted(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;

            this.ftext = new FLabel(Plugin.font, "");
        }

        public override void Update(bool eu)
        {
            var text = PomHelpers.GetStringField<Texted_Data>(self, "text");
            var color = PomHelpers.GetColorField<Texted_Data>(self, "textColor");
            var scale = PomHelpers.GetFloatField<Texted_Data>(self, "scale");
            var rotation = PomHelpers.GetFloatField<Texted_Data>(self, "rotation");
            var alpha = PomHelpers.GetFloatField<Texted_Data>(self, "alpha");
            var rcam = this.room.game.cameras[0];

            ftext.x = self.pos.x - rcam.pos.x;
            ftext.y = self.pos.y - rcam.pos.y;
            ftext.color = color;
            ftext.scale = scale;
            ftext.rotation = rotation;
            ftext.alpha = alpha;
            ftext.text = text;

            rcam.ReturnFContainer("HUD").AddChild(ftext);
        }
    }
    public class Texted_REPR : ManagedRepresentation
    {
        public Texted_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
