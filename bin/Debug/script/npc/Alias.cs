using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class Alias : BaseNPC
    {
        public override string Name { get { return "Alias"; } }
        public override string Map { get { return "Village1"; } }
        public override string ChatString { get { return "알리아스: 물약이 필요하면 '시약 물약주세요.'라고 말하세요" + "                     "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.SweepingGuy; } }
        public override int X { get { return 90; } }
        public override int Y { get { return 173; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return (int)LKCamelot.library.NPCs.SweepingGuy; } }
        public override int aSpeed { get { return 4; } }
        public override int aFrames { get { return 11; } }

        public Alias()
        {
        }
    }
}
