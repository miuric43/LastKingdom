using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 선물상자 : BasePotion
    {
        public override string Name { get { return "선물상자" + "     "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 선물상자()
            : base(215)
        {
        }

        public 선물상자(Serial serial)
            : base(serial)
        {
            m_ItemID = 215;
        }

        public override void Use(Player player)
        {
            player.HPCur += player.HP / 2;
            base.Use(player);
        }
    }
}
