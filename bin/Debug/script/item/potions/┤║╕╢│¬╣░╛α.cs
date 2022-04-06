using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 뉴마나물약 : BasePotion
    {
        public override string Name { get { return "뉴 마나물약" + "    "; } }
        public override ulong BuyPrice { get { return 500000; } }
        public override int SellPrice { get { return 400000; } }

        public 뉴마나물약()
            : base(147)
        {
        }

        public 뉴마나물약(Serial serial)
            : base(serial)
        {
            m_ItemID = 147;
        }

        public override void Use(Player player)
        {
            if (player.Level >= 50 && player.Gold >= 2000)
            {
                player.MPCur = player.MP;
                player.Gold = player.Gold - 2000;
            }
        }
    }
}
