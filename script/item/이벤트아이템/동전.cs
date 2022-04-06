using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 마법진 : BasePotion
    {
        public override string Name { get { return "마법진" + "     "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 마법진()
            : base(224)
        {
        }

        public 마법진(Serial serial)
            : base(serial)
        {
            m_ItemID = 224;
        }

        public override void Use(Player player)
        {
            player.HPCur += player.HP / 2;
            base.Use(player);
        }
    }
}
