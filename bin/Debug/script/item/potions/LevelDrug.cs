using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 렙업물약 : BasePotion
    {
        public override string Name { get { return "렙업물약"+"   "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 렙업물약()
            : base(20)
        {
        }

        public 렙업물약(Serial serial)
            : base(serial)
        {
            m_ItemID = 20;
        }

        public override void Use(Player player)
        {
            player.XP = player.XP + 100000;
            base.Use(player);
        }
    }
}
