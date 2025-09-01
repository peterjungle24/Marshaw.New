namespace SourceCode.Helpers
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
    }
}