using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.Objects
{
    internal class CustomObject_Hooks
    {
        private static LogUtils.Logger log { get => new LogUtils.Logger(Plugin.logger); }

        public static void OnHooks()
        {
            On.Room.AddObject += OnAddObject;
        }

        private static void OnAddObject(On.Room.orig_AddObject orig, Room self, UpdatableAndDeletable obj)
        {
            try
            {
                if (obj is CustomObject custom)
                {
                    var tilePos = self.GetTilePosition(custom.bodyChunks[0].pos);
                    var pos = new WorldCoordinate(self.abstractRoom.index, tilePos.x, tilePos.y, 0);
                    var abstr = new AbstractCustomObject(self.world, pos, self.game.GetNewID());

                    self.abstractRoom.AddEntity(abstr);
                }

                orig(self, obj);
            }
            catch (Exception ex)
            {
                log.LogError($"<CustomObject_Hooks.OnAddObject()>Exception thrown!\n{ex}");
            }
        }
    }
}
