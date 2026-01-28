using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Objects
{
    public static class ObjectRegister
    {
        public static objType item;
        public static objType fireball;

        public static void RegisterValues()
        {
            item = new objType("item", true);
            fireball = new objType("fireball", true);
        }
        public static void UnregisterValues()
        {
            objType this_item = item; this_item?.Unregister(); this_item = null;
            objType this_fireball = fireball; this_fireball?.Unregister(); this_fireball = null;
        }
    }
}
