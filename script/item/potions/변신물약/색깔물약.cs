using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 색깔물약 : BasePotion
    {
        public override string Name { get { return "색깔물약" + "   "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 색깔물약()
            : base(20)
        {
        }

        public 색깔물약(Serial serial)
            : base(serial)
        {
            m_ItemID = 20;
        }

        public override void Use(Player player)
        {
            player.Color = 1;
            base.Use(player);
        }
    }
}
