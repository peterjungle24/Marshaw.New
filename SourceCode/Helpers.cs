﻿namespace SourceCode.Helpers
{
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
        public static string GetModFolder()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }
        public static string GetFile(string file)
        {
            var path = GetModFolder();
            return path + "/" + file;
        }
    }
    public static class FunHelpers
    {
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
        public static Color RGB(float r, float g, float b)
        {
            return new Color(r /255, g /255, b /255);
        }
    }
}