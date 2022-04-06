using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class Dai2 : Item
    {
        public override string Name { get { return "DAIYEN FOOELS"; } }
        public override int SellPrice { get { return 114000; } }
        public override ulong BuyPrice { get { return 250000; } }

        public Dai2()
            : base(162)
        {
        }

        public Dai2(Serial serial)
            : base(serial)
        {
            m_ItemID = 162;
        }
    }
}
