using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class Bluesmailpiece : BaseOre
    {
        public override string Name { get { return "Bluesmailpiece " + (script.item.BaseOre.OreTypeE)Stage + " : " + Quantity; } }
        public override ulong BuyPrice { get { return 500; } }

        public override void SetSprite()
        {
            if (Stage == (int)OreTypeE.PG)
                m_ItemID = 1;
            if (Stage == (int)OreTypeE.PN)
                m_ItemID = 2;
            if (Stage == (int)OreTypeE.PB)
                m_ItemID = 3;
        }

        public Bluesmailpiece()
            : base(99)
        {
        }

        public Bluesmailpiece(Serial serial)
            : base(serial)
        {
            m_ItemID = 22;
        }
    }
}
