using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.POM
{
    public static class MyTrigger_Hooks
    {
        public static void OnHooks()
        {
            On.RainWorld.Awake += RainWorld_Awake;

            MyTrigger.onTriggerEvent.Invoke(MyTrigger.Id);
        }

        private static void RainWorld_Awake(On.RainWorld.orig_Awake orig, RainWorld self)
        {
            MyTrigger.onTriggerEvent += OnTrigger;
        }

        private static void OnTrigger(string id)
        {
            if (id == "no more")
                Debug.Log("This is the first day of end of your lives!");
        }
    }
}
