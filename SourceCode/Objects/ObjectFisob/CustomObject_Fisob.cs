using Fisobs;
using Fisobs.Core;
using Fisobs.Items;
using Fisobs.Properties;
using Fisobs.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Objects
{
    internal class CustomObjectFisob : Fisob
    {
        public CustomObjectFisob() :base(customObjectType)
        {
            this.Icon = new CustomObjectIcon();
            this.SandboxPerformanceCost = new(linear: 0.2f, exponential: 0);
        }

        public override objPhy Parse(World world, EntitySaveData entitySaveData, SandboxUnlock unlock)
        {
            var result = new AbstractCustomObject(world, entitySaveData.Pos, entitySaveData.ID);
            return result;
        }
        public override ItemProperties Properties(PhysicalObject forObject)
        {
            return properties;
        }

        public static readonly AbstractPhysicalObject.AbstractObjectType customObjectType = new("CustomObject", true);
        public static readonly CustomObjectProperties properties = new();
    }
}
