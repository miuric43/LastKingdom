using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class Pearl : BaseOre
    {
        public override string Name { get { return "진주" + (script.item.BaseOre.OreTypeE)Stage + " : " + Quantity; } }
        public override int SellPrice { get { return 3000000; } }

        public override void SetSprite()
        {
            if (Stage == (int)OreTypeE.PG)
                m_ItemID = 1;
            if (Stage == (int)OreTypeE.PN)
                m_ItemID = 2;
            if (Stage == (int)OreTypeE.PB)
                m_ItemID = 3;
        }

        public Pearl()
            : base(80)
        {
        }

        public Pearl(Serial serial)
            : base(serial)
        {
            m_ItemID = 22;
        }
    }
}
