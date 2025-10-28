using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCode.RoomScripts
{
    public class RSCR_WhateverIsThis : UpdatableAndDeletable
    {
        private float[] rightMost;
        private bool stoodUp;
        private bool[] pushRight;
        private int counter;
        private bool showedControls;
        private int showControlsCounter;
        public TutorialControlsPageOwner tutCntrlPgOwner;

        public RSCR_WhateverIsThis(Room room)
        {
            this.room = room;

            Debug.Log("\n\nWhat the hell are you talking about??\n\n");
        }
    }
}
