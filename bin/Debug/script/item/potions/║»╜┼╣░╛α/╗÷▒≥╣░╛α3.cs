using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 색깔물약3 : BasePotion
    {
        public override string Name { get { return "색깔물약3"+"   "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 색깔물약3()
            : base(20)
        {
        }

        public 색깔물약3(Serial serial)
            : base(serial)
        {
            m_ItemID = 20;
        }

        public override void Use(Player player)
        {
            player.Color = 3;
            base.Use(player);
        }
    }
}
