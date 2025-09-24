namespace RegionObjects
{
    public class Stalk : UpdatableAndDeletable, IDrawable
    {
        public TriangleMesh.Triangle[] triangles;
        public TriangleMesh mesh;
        public PlacedObject self;

        public Stalk()
        {
            //StalkSegments.Initialize();

            // a new array of triangle
            triangles = new TriangleMesh.Triangle[]
            {
                // support
                new(0, 1, 2),
                new(2, 1, 3),
                new(3, 2, 4),
                new(4, 3, 5),
                new(5, 4, 1),
                new(1, 5, 2),
                new(2, 1, 8),
                new(8, 2, 9),

                // cool thing
                new(0, 2, 10),
                new(0, 10, 12),
                new(1, 3, 11),
                new(1, 11, 13),
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
                // support
                new(200, 200),
                new(180, 200),
                new(200, 180),
                new(180, 180),
                new(200, 100),
                new(180, 100),
                new(200, 140),
                new(180, 140),
                new(200, 120),
                new(180, 120),

                // cool thing
                new(220, 200),
                new(100, 200),
                new(200, 220),
                new(180, 220),
            };

            for (var i = 0; i < vertPos.Length; i++)
            {
                tri.MoveVertice(i, vertPos[i] + position);
            }
            for (var i = 0; i < 3; i++)
            {
                tri.color = Color.blue;
            }
            tri.verticeColors[1] = Color.green;
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette pal) { }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer fContainer)
        {
            fContainer ??= rCam.ReturnFContainer("HUD");

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fContainer.AddChild(fsprite);
            }
        }
    }
}