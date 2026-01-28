using SourceCode.Helpers;
using SourceCode.Utilities;

namespace SourceCode.POM
{
    public class IndividualRender_Data : ManagedData
    {
        [StringField("imageName", "Futile_White", "Image Name")]
        public string imageName;
        [EnumField<ContainerLayers>(
            "container",
            ContainerLayers.Foreground,
            new ContainerLayers[] {
                ContainerLayers.Shadows,
                ContainerLayers.BackgroundShortcuts,
                ContainerLayers.Background,
                ContainerLayers.Midground,
                ContainerLayers.Items,
                ContainerLayers.Foreground,
                ContainerLayers.ForegroundLights,
                ContainerLayers.Shortcuts,
                ContainerLayers.Water,
                ContainerLayers.GrabShaders,
                ContainerLayers.Bloom,
                ContainerLayers.HUD,
                ContainerLayers.HUD2,
            },
            ManagedFieldWithPanel.ControlType.arrows,
            "Container")]
        public ContainerLayers container;
        [FloatField("scale", 0f, float.MaxValue, 3f, 0.1f, ManagedFieldWithPanel.ControlType.text, "Scale")]
        public float scale;
        [FloatField("alpha", 0f, 1f, 1f, 0.1f, ManagedFieldWithPanel.ControlType.slider, "Alpha")]
        public float alpha;
        [ColorField("tint", 1,1,1,1, ManagedFieldWithPanel.ControlType.button, "Tint")]
        public Color tint;

        //the custom fields are added as a parameter for the base class
        public IndividualRender_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class IndividualRender : UpdatableAndDeletable, IDrawable
    {
        PlacedObject self;
        LogUtils.Logger logger { get => Plugin.log; }
        ContainerLayers container;
        string fileName;
        float scale;
        float alpha;
        Color tint;

        public IndividualRender(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;

            fileName = PomHelpers.GetStringField<IndividualRender_Data>(self, "imageName");
        }

        public override void Update(bool eu)
        {
            scale = PomHelpers.GetFloatField<IndividualRender_Data>(self, "scale");
            alpha = PomHelpers.GetFloatField<IndividualRender_Data>(self, "alpha");
            tint = PomHelpers.GetColorField<IndividualRender_Data>(self, "tint");
            container = (ContainerLayers)PomHelpers.GetEnumField<IndividualRender_Data, ContainerLayers>(self, "container");
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];

            try
            {
                sLeaser.sprites[0] = new FSprite(PathHelpers.GetFSpriteImage(fileName), true);
            }
            catch (FutileException exception)
            {
                sLeaser.sprites[0] = new FSprite("Futile_White", true);
                logger.LogError($"\n{exception}\n");
            }

            sLeaser.sprites[0].shader = rCam.game.rainWorld.Shaders[ShaderList.slugg_CustomTextureDepth];

            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float Float, Vector2 camPos)
        {
            var position = new Vector2(self.pos.x - camPos.x, self.pos.y - camPos.y);

            sLeaser.sprites[0].color = tint;
            sLeaser.sprites[0].x = position.x;
            sLeaser.sprites[0].y = position.y;
            sLeaser.sprites[0].alpha = alpha;
            sLeaser.sprites[0].scale = scale;
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette pal) { }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer fContainer)
        {
            fContainer ??= rCam.ReturnFContainer(container.ToString() );

            foreach (FSprite fsprite in sLeaser.sprites)
                fContainer.AddChild(fsprite);
        }

        /****************************************************/
    }
    public class IndividualRender_REPR : ManagedRepresentation
    {
        public IndividualRender_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
