namespace SourceCode.POM
{
    public class LightTest_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        public LightTest_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class LightTest : UpdatableAndDeletable, IDrawable
    {
        TriangleMesh.Triangle[] triangles;
        TriangleMesh mesh;
        PlacedObject self;
        LogUtils.Logger logger { get => Plugin.log; }

        public LightTest(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;

            triangles = new TriangleMesh.Triangle[]
            {
                // support
                new(0, 1, 2),
                new(1, 2, 3),
                new(3, 2, 4),
                new(3, 4, 5),
                new(2, 4, 6),
            };
            // instantiate it
            mesh = new TriangleMesh("Futile_White", this.triangles, true, false);
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            // a new FSprite
            sLeaser.sprites = new FSprite[1];

            // initialize
            var triarray = this.triangles;
            var triangle = this.mesh;

            // set the triangle instead
            sLeaser.sprites[0] = triangle;

            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float Float, Vector2 camPos)
        {
            var tri = (sLeaser.sprites[0]) as TriangleMesh;
            var position = (self.pos - camPos);

            /// my positions
            var vertPos = new Vector2[]
            {
                new(50, 0),
                new(0, -50),
                new(0, 0),
                new(50, -50),
                new(50, 0),
                new(100, -50),
                new(0, 50),
            };

            tri.MoveVertice(0, vertPos[0] + position);
            tri.MoveVertice(1, vertPos[1] + position);
            tri.MoveVertice(2, vertPos[2] + position);
            tri.MoveVertice(3, vertPos[3] + position);
            tri.MoveVertice(4, vertPos[4] + position);
            tri.MoveVertice(5, vertPos[5] + position);
            tri.MoveVertice(6, vertPos[6] + position);

            for (var i = 0; i < 3; i++) tri.color = Color.blue;
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette pal) { }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer fContainer)
        {
            fContainer ??= rCam.ReturnFContainer("HUD");

            foreach (FSprite fsprite in sLeaser.sprites)
                fContainer.AddChild(fsprite);
        }
    }
    public class LightTest_REPR : ManagedRepresentation
    {
        public LightTest_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
