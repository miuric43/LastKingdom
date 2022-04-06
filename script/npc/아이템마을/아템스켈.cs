using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class 아템스켈 : BaseNPC
    {
        public override string Name { get { return "아템스켈"; } }
        public override string Map { get { return "ItemVillage"; } }

        public override int ID { get { return (int)LKCamelot.library.NPCs.아템스켈; } }
        public override int X { get { return 96; } }
        public override int Y { get { return 114; } }
        public override int Face { get { return 0; } }
        public override int Sprite { get { return 28; } }
        public override int aSpeed { get { return 1; } }
        public override int aFrames { get { return 0; } }

        public 아템스켈()
        {
        }
    }
}