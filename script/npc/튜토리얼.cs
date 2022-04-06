using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class 튜토리얼 : BaseNPC
    {
        public override string Name { get { return "튜토리얼"; } }
        public override string Map { get { return "튜토리얼"; } }
        public override string ChatString { get { return "7승검객 : 천원을 가지고 오면 선물을 주겠다." + "                                       "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.튜토리얼; } }
        public override int X { get { return 10; } }
        public override int Y { get { return 10; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return 18; } }
        
        public override int aSpeed { get { return 1; } }
        public override int aFrames { get { return 23; } }

        public 튜토리얼()
        {
        }
    }
}