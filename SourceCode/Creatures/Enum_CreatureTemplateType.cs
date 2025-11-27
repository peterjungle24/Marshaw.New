using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.Creatures.Lizards;
using SourceCode.Creatures.GrapleWorm;

namespace SourceCode.Creatures
{
    public static class Enum_CreatureTemplateType
    {
        public static HashSet<CreatureTemplate.Type> hash;
        public static CreatureTemplate.Type ValveLizard = new("ValveLizard", true);
        public static CreatureTemplate.Type GlowSait = new("GlowSait", true);

        static Enum_CreatureTemplateType()
        {
            hash = 
            [
                #region Lizards
                ValveLizard,
                #endregion
                #region GrapleWorm
                GlowSait,
                #endregion
            ];
        }

        public static void UnregisterValues()
        {
            #region Lizards
            if (ValveLizard is not null) { ValveLizard.Unregister(); ValveLizard = null!; }
            #endregion
            #region GrapleWorms
            if (GlowSait is not null) { GlowSait.Unregister(); GlowSait = null; }
            #endregion
        }
    }
    public static class Enum_SandboxUnlockID
    {
        public static HashSet<MultiplayerUnlocks.SandboxUnlockID> hash;
        public static MultiplayerUnlocks.SandboxUnlockID ValveLizard = new("ValveLizard", true);
        public static MultiplayerUnlocks.SandboxUnlockID GlowSait = new("GlowSait", true);

        static Enum_SandboxUnlockID()
        {
            hash =
            [
                #region Lizards
                ValveLizard,
                #endregion
                #region GrapleWorm
                GlowSait,
                #endregion
            ];
        }

        public static void UnregisterValues()
        {
            #region Lizards
            if (ValveLizard is not null) { ValveLizard.Unregister(); ValveLizard = null!; }
            #endregion
            #region GrapleWorms
            if (GlowSait is not null) { GlowSait.Unregister(); GlowSait = null; }
            #endregion
        }
    }
}
