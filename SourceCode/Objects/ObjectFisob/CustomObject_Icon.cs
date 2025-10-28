using Fisobs;
using Fisobs.Core;
using Fisobs.Items;
using Fisobs.Sandbox;
using SourceCode.Helpers;

namespace SourceCode.Objects
{
    sealed class CustomObjectIcon : Icon
    {
        public override int Data(objPhy apo)
        {
            return apo is AbstractCustomObject obj ? (int)(obj.hue * 100f) : 0;
        }
        public override Color SpriteColor(int data)
        {
            return Color.yellow;
        }
        public override string SpriteName(int data)
        {
            return PathHelpers.GetFile("sprites/CrossHair");
        }
    }
}
