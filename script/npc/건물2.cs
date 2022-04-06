using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class 건물2 : BaseNPC
    {
        public override string Name { get { return "건물2"; } }
        public override string Map { get { return "튜토리얼"; } }
     //   public override string ChatString { get { return "운영자존 : 어서오시게." + "                                       "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.건물2; } }
        public override int X { get { return 13; } }
        public override int Y { get { return 6; } }
        public override int Face { get { return 0; } }
        public override int Sprite { get { return 20; } }
        public override int aSpeed { get { return 1; } }
        public override int aFrames { get { return 23; } }

        public 건물2()
        {
        }
    }
}