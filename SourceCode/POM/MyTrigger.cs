using SourceCode.Helpers;

namespace SourceCode.POM
{
    public class MyTrigger_Data : ManagedData
    {
        [Vector2Field("scale", 1, 1, Vector2Field.VectorReprType.rect, "Scale")]
        public Vector2 scale;
        [StringField("id", "", "ID")]
        public string id;

        //the custom fields are added as a parameter for the base class
        public MyTrigger_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class MyTrigger : UpdatableAndDeletable
    {
        public delegate void OnTrigger(string id);
        public static OnTrigger onTriggerEvent;
        public static string Id;
        public string id;
        LogUtils.Logger logger => Plugin.log;
        PlacedObject self;
        Vector2 scale;

        public MyTrigger(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
            this.scale = PomHelpers.GetVector2Field<MyTrigger_Data>(self, "scale");
            this.id = PomHelpers.GetStringField<MyTrigger_Data>(self, "id");
            // acess.
            Id = id;

            logger.LogInfo($"{Color.yellow}There is: {Id}/{id}");
        }
    }
    public class MyTrigger_REPR : ManagedRepresentation
    {
        public MyTrigger_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
