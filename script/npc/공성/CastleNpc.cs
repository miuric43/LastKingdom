using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class CastleNpc : BaseNPC
    {
        public override string Name { get { return "CastleNpc"; } }
        public override string Map { get { return "Rest"; } }
        public override string ChatString { get { return "길드석 : 현재 성주를 확인하시려면 클릭!                     "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.CastleNpc; } }
        public override int X { get { return 20; } }
        public override int Y { get { return 20; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return 13; } }
        public override int aSpeed { get { return 4; } }
        public override int aFrames { get { return 1; } }

        public CastleNpc()
        {
        }
    }
}
