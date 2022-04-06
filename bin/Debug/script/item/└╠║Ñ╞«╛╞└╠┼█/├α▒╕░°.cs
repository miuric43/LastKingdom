using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 축구공 : BasePotion
    {
        public override string Name { get { return "축구공" + "     "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 축구공()
            : base(217)
        {
        }

        public 축구공(Serial serial)
            : base(serial)
        {
            m_ItemID = 217;
        }

        public override void Use(Player player)
        {
            player.HPCur += player.HP / 2;
            base.Use(player);
        }
    }
}
