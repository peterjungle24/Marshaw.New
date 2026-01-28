using SourceCode.Helpers;
using SourceCode.Utilities;

namespace SourceCode.POM
{
    public class GreenScreen_Data : ManagedData
    {
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
            "Container"
            )]
        public ContainerLayers container;
        [Vector2Field("scale", 15, 15, Vector2Field.VectorReprType.circle, "Scale")]
        public Vector2 scale;

        //the custom fields are added as a parameter for the base class
        public GreenScreen_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class GreenScreen : UpdatableAndDeletable, IDrawable
    {
        PlacedObject self;
        LogUtils.Logger logger => Plugin.log;
        ContainerLayers container;
        Vector2 scale;

        public GreenScreen(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
        }
        public override void Update(bool eu)
        {
            base.Update(eu);

            container = (ContainerLayers)PomHelpers.GetEnumField<GreenScreen_Data, ContainerLayers>(self, "container");
            scale = PomHelpers.GetVector2Field<GreenScreen_Data>(self, "scale");
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];
            sLeaser.sprites[0] = new FSprite("Futile_White", true);

            sLeaser.sprites[0].color = Color.green;

            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float Float, Vector2 camPos)
        {
            sLeaser.sprites[0].x = self.pos.x - camPos.x;
            sLeaser.sprites[0].y = self.pos.y - camPos.y;
            sLeaser.sprites[0].scaleX = scale.x;
            sLeaser.sprites[0].scaleY = scale.y;
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette pal)
        { }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer fContainer)
        {
            fContainer ??= rCam.ReturnFContainer(container.ToString() );

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fContainer.AddChild(fsprite);
            }
        }

    }
    public class GreenScreen_REPR : ManagedRepresentation
    {
        public GreenScreen_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
