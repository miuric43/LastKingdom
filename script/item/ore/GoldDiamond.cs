using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class GoldDiamond : BaseOre
    {
        public override string Name { get { return "GoldDiamond" + (script.item.BaseOre.OreTypeE)Stage + " : " + Quantity; } }
        public override ulong BuyPrice { get { return 500; } }
        public override int SellPrice { get { return 200000 * Quantity; } }
        public override void SetSprite()
        {
            if (Stage == (int)OreTypeE.PG)
                m_ItemID = 90;
            if (Stage == (int)OreTypeE.PN)
                m_ItemID = 91;
            if (Stage == (int)OreTypeE.PB)
                m_ItemID = 92;
        }

        public GoldDiamond()
            : base(53)
        {
        }

        public GoldDiamond(Serial serial)
            : base(serial)
        {
            m_ItemID = 22;
        }
    }
}
