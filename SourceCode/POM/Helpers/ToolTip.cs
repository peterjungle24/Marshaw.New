namespace SourceCode.POM
{
    //internal class ToolTip
    public class ToolTip_Data : ManagedData
    {
        [StringField("text", "", "Text")]
        public string text;
        [ColorField("textColor", 1, 1, 1, 1, ManagedFieldWithPanel.ControlType.text, "Text Color")]
        public Color textColor;
        [FloatField("scale", 0, 20f, 2f, 0.1f, ManagedFieldWithPanel.ControlType.text, "Scale")]
        public float scale;
        [FloatField("textScale", 0, 400, 2, 0.1f, ManagedFieldWithPanel.ControlType.text, "Text Scale")]
        public float textScale;
        [FloatField("yOffset", 0, 500, 0, 0.1f, ManagedFieldWithPanel.ControlType.text, "Y Offset")]
        public float yOffset;

        //the custom fields are added as a parameter for the base class
        public ToolTip_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class ToolTip : UpdatableAndDeletable, IDrawable
    {
        ManualLogSource logger { get => Plugin.logger; }
        PlacedObject self;
        FLabel ftext;
        Color[] color;
        string text;
        float scale;
        float textScale;
        Color textColor;
        float yOffset;
        IntRect rect;
        float textAlpha = 0;

        public ToolTip(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
            
            color = new Color[] { Color.blue, Color.cyan, Color.yellow, Color.green, Color.gray, Color.magenta, Color.red };
            ftext = new FLabel(Plugin.font, "");
        }

        public override void Update(bool eu)
        {
            base.Update(eu);

            text = Helpers.PomHelpers.GetStringField<ToolTip_Data>(self, "text");
            scale = Helpers.PomHelpers.GetFloatField<ToolTip_Data>(self, "scale");
            textScale = Helpers.PomHelpers.GetFloatField<ToolTip_Data>(self, "textScale");
            yOffset = Helpers.PomHelpers.GetFloatField<ToolTip_Data>(self, "yOffset");
            textColor = Helpers.PomHelpers.GetColorField<ToolTip_Data>(self, "textColor");

            var rcam = room.game.cameras[0];

            ftext.x = self.pos.x - rcam.pos.x;
            ftext.y = self.pos.y - rcam.pos.y + yOffset;
            ftext.color = textColor;
            ftext.scale = textScale;
            ftext.alpha = textAlpha;
            ftext.text = text;

            rcam.ReturnFContainer("HUD").AddChild(ftext);
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];
            sLeaser.sprites[0] = new FSprite("Futile_White", true);
            var num = Randomf.Range(0, color.Length);
            sLeaser.sprites[0].color = color[num];

            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float Float, Vector2 camPos)
        {
            sLeaser.sprites[0].x = self.pos.x - camPos.x;
            sLeaser.sprites[0].y = self.pos.y - camPos.y;
            sLeaser.sprites[0].scale = scale;

            rect = new IntRect( (int) sLeaser.sprites[0].x, (int)sLeaser.sprites[0].y, (int) sLeaser.sprites[0].width, (int) sLeaser.sprites[0].height);
            if (RWCustom.Custom.InsideRect(new IntVector2((int) Input.mousePosition.x, (int) Input.mousePosition.y), rect) )
            {
                //textAlpha = 1;
                Debug.Log("mouse inside!");
            }

            /*
            if (ModManager.DevTools == true) sLeaser.sprites[0].alpha = 1;
            else sLeaser.sprites[0].alpha = 0f;
            */
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette pal)
        { }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer fContainer)
        {
            fContainer ??= rCam.ReturnFContainer("Items");

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fContainer.AddChild(fsprite);
            }
        }

        /****************************************************/

    }
    public class ToolTip_REPR : ManagedRepresentation
    {
        public ToolTip_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }

}
