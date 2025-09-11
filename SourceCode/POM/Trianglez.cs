using SourceCode.Helpers;
using RegionObjects;

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
    public class Trianglez : Stalk
    {
        ManualLogSource logger { get => Plugin.logger; }

        public Trianglez(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
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