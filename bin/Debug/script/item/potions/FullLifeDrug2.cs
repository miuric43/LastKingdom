using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class FullLifeDrug2 : BasePotion
    {
        public override string Name { get { return "Full Life Drug" + (script.item.BaseOre.OreTypeE)Stage + " : " + Quantity; } }
        
        public override ulong BuyPrice { get { return 500; } }

        public FullLifeDrug2()
            : base(22)
        {
        }

        public FullLifeDrug2(Serial serial)
            : base(serial)
        {
            m_ItemID =22;
        }

        public override void Use(Player player)
        {
            Quantity = Quantity - 1;
            
        }

    }
}
