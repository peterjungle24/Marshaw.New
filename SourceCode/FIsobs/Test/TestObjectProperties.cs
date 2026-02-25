using UnityEngine;
using Fisobs;
using Fisobs.Core;
using Fisobs.Properties;

namespace SourceCode.FIsobs
{
    internal class TestObjectProperties : ItemProperties
    {
        public override void Grabability(Player player, ref Player.ObjectGrabability grabability)
            => grabability = Player.ObjectGrabability.OneHand;
        public override void Throwable(Player player, ref bool throwable)
            => throwable = true;
    }
}
