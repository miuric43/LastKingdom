using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class DamBuff : Item
    {
        public override string Name { get { return "DamBuff"; } }
        public override int SellPrice { get { return 0; } }
        public override ulong BuyPrice { get { return 0; } }
        public DamBuff()
            : base(159)
        {
        }

        public DamBuff(Serial serial)
            : base(serial)
        {
            m_ItemID = 159;
        }
    }
}
