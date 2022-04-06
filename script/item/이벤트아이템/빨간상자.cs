using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 스크롤 : BasePotion
    {
        public override string Name { get { return "스크롤" + "     "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 스크롤()
            : base(216)
        {
        }

        public 스크롤(Serial serial)
            : base(serial)
        {
            m_ItemID = 216;
        }

        public override void Use(Player player)
        {
            player.HPCur += player.HP / 2;
            
            base.Use(player);
        }
    }
}
