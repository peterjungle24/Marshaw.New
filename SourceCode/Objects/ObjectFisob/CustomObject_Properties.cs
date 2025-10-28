using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fisobs;
using Fisobs.Core;
using Fisobs.Items;
using Fisobs.Properties;
using Fisobs.Sandbox;

namespace SourceCode.Objects
{
    sealed class CustomObjectProperties : ItemProperties
    {
        public override void ScavWeaponPickupScore(Scavenger scav, ref int score) { score = 3; }
        public override void ScavCollectScore(Scavenger scav, ref int score) { score = 3; }
        public override void ScavWeaponUseScore(Scavenger scav, ref int score) { score = 2; }
        public override void Grabability(Player player, ref Player.ObjectGrabability grabability)
        {
            grabability = Player.ObjectGrabability.OneHand;
        }
    }
}
