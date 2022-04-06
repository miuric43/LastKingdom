using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class 안내문 : BaseNPC
    {
        public override string Name { get { return "안내문"; } }
        public override string Map { get { return "GM존"; } }
       
        public override int ID { get { return (int)LKCamelot.library.NPCs.안내문; } }
        public override int X { get { return 6; } }
        public override int Y { get { return 6; } }
        public override int Face { get { return 0; } }
        public override int Sprite { get { return 33; } }
        public override int aSpeed { get { return 1; } }
        public override int aFrames { get { return 1; } }

        public 안내문()
        {
        }
    }
}