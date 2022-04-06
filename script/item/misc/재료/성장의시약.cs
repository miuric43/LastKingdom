using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 성장의시약 : Item
    {
        public override string Name { get { return "성장의시약" + "     "; } }
        public override ulong BuyPrice { get { return 2000000; } }
        public override int SellPrice { get { return 1000000 * Quantity; } }
        public 성장의시약()
            : base(159)
        {
        }

        public 성장의시약(Serial serial)
            : base(serial)
        {
            m_ItemID = 159;
        }

        public override void Use(Player player)
        {

        }
    }
}
