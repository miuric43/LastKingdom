using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 인형 : BasePotion
    {
        public override string Name { get { return "인형" + "     "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 인형()
            : base(227)
        {
        }

        public 인형(Serial serial)
            : base(serial)
        {
            m_ItemID = 227;
        }

        public override void Use(Player player)
        {
            player.HPCur += player.HP / 2;
            base.Use(player);
        }
    }
}
