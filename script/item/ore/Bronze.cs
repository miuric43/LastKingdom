using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class Bronze : BaseOre
    {
        public override string Name { get { return "구리 " + (script.item.BaseOre.OreTypeE)Stage + " : " + Quantity; } }
        public override ulong BuyPrice { get { return 500; } }
        public override int SellPrice { get { return 15000 * Quantity; } }

        public override void SetSprite()
        {
            if (Stage == (int)OreTypeE.PG)
                m_ItemID = 1;
            if (Stage == (int)OreTypeE.PN)
                m_ItemID = 2;
            if (Stage == (int)OreTypeE.PB)
                m_ItemID = 3;
        }

        public Bronze()
            : base(79)
        {
        }

        public Bronze(Serial serial)
            : base(serial)
        {
            m_ItemID = 22;
        }
    }
}
