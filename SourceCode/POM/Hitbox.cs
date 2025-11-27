using SourceCode.Helpers;
using SourceCode.Utilities;

namespace SourceCode.POM
{
    public class Hitbox_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        [Vector2Field("scale", 10, 10, Vector2Field.VectorReprType.rect, "Scale")]
        public Vector2 scale;

        //the custom fields are added as a parameter for the base class
        public Hitbox_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class Hitbox : UpdatableAndDeletable, IDrawable
    {
        PlacedObject self;
        LogUtils.Logger logger { get => Plugin.log; }
        Vector2 scale;

        public Hitbox(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;

            scale = Helpers.PomHelpers.GetVector2Field<Hitbox_Data>(self, "scale");
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[1];
            sLeaser.sprites[0] = new FSprite("Futile_White", true);

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
            fContainer ??= rCam.ReturnFContainer(ContainerLayers.Foreground.ToString() );

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fContainer.AddChild(fsprite);
            }
        }
    }
    public class Hitbox_REPR : ManagedRepresentation
    {
        public Hitbox_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}


// HTML using <code>