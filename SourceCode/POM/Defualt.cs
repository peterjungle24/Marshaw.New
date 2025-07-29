namespace SourceCode.POM
{
    public class Defualt_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        public Defualt_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class Defualt : UpdatableAndDeletable
    {
        PlacedObject self;
        ManualLogSource logger { get => Plugin.logger; }

        public Defualt(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
        }

    }
    public class Defualt_REPR : ManagedRepresentation
    {
        public Defualt_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
