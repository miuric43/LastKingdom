using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 뉴생명물약 : BasePotion
    {
        public override string Name { get { return "뉴 생명물약" + "    "; } }
        public override ulong BuyPrice { get { return 500000; } }
        public override int SellPrice { get { return 400000; } }

        public 뉴생명물약()
            : base(146)
        {
        }

        public 뉴생명물약(Serial serial)
            : base(serial)
        {
            m_ItemID = 146;
        }

        public override void Use(Player player)
        {
            if (player.Level >= 50 && player.Gold >= 2000)
            {
                player.HPCur = player.HP;
                player.Gold = player.Gold - 2000;
            }
        }
    }
}
