using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Objects
{
    public static class ObjectRegister
    {
        public static AbstrObjType item;
        public static AbstrObjType fireball;

        public static void RegisterValues()
        {
            item = new AbstrObjType("item", true);
            fireball = new AbstrObjType("fireball", true);
        }
        public static void UnregisterValues()
        {
            AbstrObjType this_item = item; this_item?.Unregister(); this_item = null;
            AbstrObjType this_fireball = fireball; this_fireball?.Unregister(); this_fireball = null;
        }
    }
}
