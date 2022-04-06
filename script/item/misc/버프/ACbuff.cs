using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class ACBuff : Item
    {
        public override string Name { get { return "ACBuff"; } }
        public override int SellPrice { get { return 0; } }
        public override ulong BuyPrice { get { return 0; } }
        public ACBuff()
            : base(146)
        {
        }

        public ACBuff(Serial serial)
            : base(serial)
        {
            m_ItemID = 146;
        }
    }
}
