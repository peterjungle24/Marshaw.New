using UnityEngine;
using Fisobs;
using Fisobs.Core;
using Fisobs.Items;
using Fisobs.Sandbox;

namespace SourceCode.FIsobs
{
    internal class TestObjectFisobs : Fisob
    {
        public static readonly AbstrObjType abstr = new AbstrObjType("TestObject", true);
        public static readonly MultiplayerUnlocks.SandboxUnlockID unlock = new MultiplayerUnlocks.SandboxUnlockID("TestObject", true);

        public TestObjectFisobs() : base(abstr)
        {
            this.Icon = new SimpleIcon("Circle20", Color.yellow);
            this.RegisterUnlock(unlock, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat, data: 0);
        }

        public override AbstrPhyObject Parse(World world, EntitySaveData saveData, SandboxUnlock unlock)
        {
            // Centi shield data is just floats separated by ; characters.
            string[] p = saveData.CustomData.Split(';');

            if (p.Length < 5)
            {
                p = new string[5];
            }

            var result = new TestObjectAbstract(world, saveData.Pos, saveData.ID)
            {
                scaleX = float.TryParse(p[2], out var x) ? x : 1,
                scaleY = float.TryParse(p[3], out var y) ? y : 1,
            };

            // If this is coming from a sandbox unlock, the hue and size should depend on the data value (see CentiShieldIcon below).
            if (unlock is SandboxUnlock u)
            {
                if (u.Data == 0)
                {
                    result.scaleX += 0.2f;
                    result.scaleY += 0.2f;
                }
            }

            return result;
        }
    }
}
