using UnityEngine;
using Fisobs;
using Fisobs.Core;

namespace SourceCode.FIsobs
{
    internal class TestObjectAbstract : AbstrPhyObject
    {
        public float scaleX;
        public float scaleY;

        public TestObjectAbstract(World world, WorldCoordinate pos, EntityID ID) : base(world, TestObjectFisobs.abstr, null, pos, ID)
        {
            scaleX = 1;
            scaleY = 1;
        }

        public override void Realize()
        {
            base.Realize();

            if (this.realizedObject == null) this.realizedObject = new TestObject(this, Room.realizedRoom.MiddleOfTile(pos.Tile), Vector2.zero );
        }
        public override string ToString() => this.SaveToString($"{scaleX};{scaleY}");

    }
}
