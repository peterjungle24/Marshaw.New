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
        public enum TriggerOption { Once, Update }
        public TriggerOption triggerOption = TriggerOption.Once;
        LogUtils.Logger logger => Plugin.log;
        PlacedObject self;
        Vector2 scale;
        string id;
        Rect rect;
        bool once = false;

        public MyTrigger(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
            this.scale = PomHelpers.GetVector2Field<MyTrigger_Data>(self, "scale");
            this.id = PomHelpers.GetStringField<MyTrigger_Data>(self, "id");
        }

        public override void Update(bool eu)
        {
            rect = new Rect(self.pos, scale);

            // i like this more than Foreach :D
            for (var i = 0; i < this.room.game.Players.Count; i++)
            {
                // if the list of players is not null
                if (this.room.game.Players[i] != null)
                {
                    // get player
                    var plr = room.game.Players[i]?.realizedCreature as Player;
                    // get rcam
                    var rcam = this.room.game.cameras[0];
                    // main body
                    var chunk = plr.mainBodyChunk;

                    // checks if the BODY of the player its inside
                    if (rect.Contains(chunk.pos) )
                    {
                        Debug.Log("torturing yourself >:)");
                    }
                }
            }
        }

    }
    public class MyTrigger_REPR : ManagedRepresentation
    {
        public MyTrigger_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}
