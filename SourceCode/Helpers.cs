namespace SourceCode.Helpers
{
    /// <summary>Credits for TheVileOne ;)</summary>
    public static class RoomHelpers
    {
        /// <summary>
        /// Finds all objects belonging to a room instance at or within a given distance_checker from an origin point
        /// </summary>
        /// <param name="room">The room to check</param>
        /// <param name="origin">The point to compare the distance_checker to</param>
        /// <param name="distanceThreshold">The desired distance_checker to the origin point</param>
        /// <returns>All results that match the specified conditions</returns>
        public static IEnumerable<PhysicalObject> FindObjectsNearby(this Room room, Vector2 origin, float distanceThreshold)
        {
            if (room == null) return new PhysicalObject[0]; //Null data returns an critical set
            return room.GetAllObjects().Where(o => Custom.Dist(origin, o.firstChunk.pos) <= distanceThreshold);
        }
        /// <summary>
        /// Finds all objects belonging to a room instance at or within a given distance_checker from an origin point that are of the type T
        /// </summary>
        /// <param name="room">The room to check</param>
        /// <param name="origin">The point to compare the distance_checker to</param>
        /// <param name="distanceThreshold">The desired distance_checker to the origin point</param>
        /// <returns>All results that match the specified conditions</returns>
        public static IEnumerable<PhysicalObject> FindObjectsNearby<T>(this Room room, Vector2 origin, float distanceThreshold) where T : PhysicalObject
        {
            if (room == null) return new PhysicalObject[0]; //Null data returns an critical set

            return room.GetAllObjects().OfType<T>().Where(o => RWCustom.Custom.Dist(origin, o.firstChunk.pos) <= distanceThreshold);
        }
        /// <summary>
        /// Returns all objects belonging to a room instance
        /// </summary>
        /// <param name="room">The room to check</param>
        public static IEnumerable<PhysicalObject> GetAllObjects(this Room room)
        {
            if (room == null) yield break; //Null data returns an critical set

            for (int m = 0; m < room.physicalObjects.Length; m++)
            {
                for (int n = 0; n < room.physicalObjects[m].Count; n++)
                {
                    yield return room.physicalObjects[m][n]; //Returns the objects one at a time
                }
            }
        }
    }
    public static class PomHelpers
    {
        public static bool GetBoolField<T>(PlacedObject self, string field) where T : ManagedData
        {
            var thing = ((T)self.data);
            return thing.GetValue<bool>(field);
        }
        public static int GetIntField<T>(PlacedObject self, string field) where T : ManagedData
        {
            var thing = ((T)self.data);
            return thing.GetValue<int>(field);
        }
        public static string GetStringField<T>(PlacedObject self, string field) where T : ManagedData
        {
            // make the cast inside the variable
            var thing = ((T)self.data);
            // gets a field from that "ManagedData" class
            return thing.GetValue<string>(field);
        }
        public static Color GetColorField<T>(PlacedObject self, string field) where T : ManagedData
        {
            // make the cast inside the variable
            var thing = ((T)self.data);
            // gets a field from that "ManagedData" class
            return thing.GetValue<Color>(field);
        }
        public static float GetFloatField<T>(PlacedObject self, string field) where T : ManagedData
        {
            // make the cast inside the variable
            var thing = ((T)self.data);
            // gets a field from that "ManagedData" class
            return thing.GetValue<float>(field);
        }
        public static Vector2 GetVector2Field<T>(PlacedObject self, string field) where T : ManagedData
        {
            // make the cast inside the variable
            var thing = ((T)self.data);
            // gets a field from that "ManagedData" class
            return thing.GetValue<Vector2>(field);
        }
        public static Enum GetEnumField<T, E>(PlacedObject self, string field) where T : ManagedData where E : struct, Enum
        {
            var thing = ( (T)self.data );
            return thing.GetValue<E>(field);
        }
    }
    public static class PathHelpers
    {
        /// <summary>
        /// Just gets the mod folder
        /// </summary>
        /// <returns></returns>
        public static string GetModFolder()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) );
        }
        /// <summary>
        /// Gets a file inside the mod folder
        /// </summary>
        /// <returns>the file path, aka the file itself</returns>
        public static string GetFile(string file)
        {
            var path = GetModFolder();
            return path + "/" + file;
        }
        /// <summary>
        /// Gets a file image, for FSprite constructors
        /// </summary>
        /// <remarks>PLEASE DONT SPECIFY THE FILE EXTENSION, I STRUGGLED A LOT WITH THIS</remarks>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFSpriteImage(string file)
        {
            return Futile.atlasManager.LoadImage(PathHelpers.GetFile(file)).name;
        }
    }
    public static class FunHelpers
    {
        /// <summary>Just turns a ASCII table (in the bytes arrays) to string.</summary>
        /// <param name="bytes">the byte array used to check the ASCII</param>
        /// <returns>Translated string from bytes</returns>
        public static string BytesArrayAsString(byte[] bytes)
        {
            string str = "";
            foreach (var bit in bytes)
            {
                char character = (char)bit;
                str += character;
            }

            return str;
        }
        /// <summary>
        /// Returns a new color using RGB instead
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color RGB(float r, float g, float b)
        {
            return new Color(r /255, g /255, b /255);
        }
    }
}