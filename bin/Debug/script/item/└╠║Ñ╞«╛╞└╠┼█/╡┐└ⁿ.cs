using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 동전 : BasePotion
    {
        public override string Name { get { return "동전" + "     "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 동전()
            : base(218)
        {
        }

        public 동전(Serial serial)
            : base(serial)
        {
            m_ItemID = 218;
        }

        public override void Use(Player player)
        {
            player.HPCur += player.HP / 2;
            base.Use(player);
        }
    }
}
