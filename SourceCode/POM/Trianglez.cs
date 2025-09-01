using SourceCode.Helpers;

namespace SourceCode.POM
{
    public class Trianglez_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        public Trianglez_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class Trianglez : UpdatableAndDeletable, IDrawable
    {
        PlacedObject self;
        ManualLogSource logger { get => Plugin.logger; }

        public Trianglez(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            // a new FSprite
            sLeaser.sprites = new FSprite[1];

            // initialize
            var triarray = new TriangleMesh.Triangle[]
            {
                // support
                new(0, 1, 2),
                new(2, 1, 3),
                new(3, 2, 4),
                new(4, 3, 5),
                new(5, 4, 6),
                new(6, 5, 7),
                new(7, 6, 8),
                new(8, 7, 9),

                // cool part
                new(0, 2, 10),
                new(0, 10, 12),
                new(1, 3, 11),
                new(1, 11, 13),
            };
            var triangle = new TriangleMesh("Futile_White", triarray, true, false);

            // set the triangle instead
            sLeaser.sprites[0] = triangle;

            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float Float, Vector2 camPos)
        {
            var tri = (sLeaser.sprites[0]) as TriangleMesh;
            var position = self.pos - camPos;
            /// my positions
            var vertPos = new Vector2[]
            {
                // support
                new(300, 300),
                new(280, 300),
                new(300, 280),
                new(280, 280),
                new(300, 260),
                new(280, 260),
                new(300, 240),
                new(280, 240),
                new(300, 220),
                new(280, 220),

                // cool thing
                new(320, 300),
                new(260, 300),
                new(300, 320),
                new(280, 320),
            };

            for (var i  = 0; i < vertPos.Length; i ++)
            {
                tri.MoveVertice(i, vertPos[i] );
            }
            for (var i = 5; i < vertPos.Length; i ++)
            {
                tri.color = Color.cyan;
            }
            
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
    public class Trianglez_REPR : ManagedRepresentation
    {
        public Trianglez_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}

/* Triangle Meshes - Initialize

before, i tried to use the "PlayerGraphics" hooks to show triangle meshes.
but it was sucks
so i did this POM object JUST FOR THIS

so..

-------------- [InitiateSprites] -------------- 
// a new FSprite
sLeaser.sprites = new FSprite[1];

// initialize
var triarray = new TriangleMesh.Triangle[] { new(A, B, C) };
var triangle = new TriangleMesh("Futile_White", triarray, true, false);

// set the triangle instead
sLeaser.sprites[0] = triangle;
--------------

we create a new FSprite array, normal here.
the array of triangles is essential for the Triangle Mesh, literaly
the triangle its the thing we use to draw triangles.
we assign the desired index with the triangles.

this is just for initialize, but all points are on the same coordinates.
it means we cant see it, and we need to move them

so, in DrawSprites, we should move vertices
-------------- [DrawSprites] -------------- 
( (sLeaser.sprites[0]) as TriangleMesh).MoveVertice(index, Vector2 pos);
--------------

the cast is essential, but i like to do it in a assigned variable instead
 it shortens the name and its better
in the "index" parameter, we use one of the numbers inside that triangle array from InitiateSprites
 if you set a number that doesnt exist there, you can likely get a null exception.
*/
/* Triangle Meshes - Triangle Array

each point inside the "new()" its a triangle, literally.
and each point CONNECTS another point

*/