using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class HitBuff : Item
    {
        public override string Name { get { return "HitBuff"; } }
        public override int SellPrice { get { return 0; } }
        public override ulong BuyPrice { get { return 0; } }
        public HitBuff()
            : base(148)
        {
        }

        public HitBuff(Serial serial)
            : base(serial)
        {
            m_ItemID = 148;
        }
    }
}
