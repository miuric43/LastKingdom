using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 턴 : BasePotion
    {
        public override string Name { get { return "턴엘릭서"+"       "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 턴()
            : base(223)
        {
        }

        public 턴(Serial serial)
            : base(serial)
        {
            m_ItemID = 223;
        }

        public override void Use(Player player)
        {
              if (Quantity >= 12)
              {
            player.Extra = player.Extra + 1;
            player.client.SendPacket(new UpdateCharStats(player).Compile());
            Quantity = Quantity - 1;
            
            
        }
              else if (Quantity >= 3 && Quantity <= 11)
              {
                  player.Extra = player.Extra + 1;
                  player.client.SendPacket(new UpdateCharStats(player).Compile());
                  Quantity = Quantity - 1;
                  string itdrop = "턴엘릭서가" + (Quantity - 1) + "개남았습니다" + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());
              }
              else
              {
                  player.Extra = player.Extra + 1;
                  player.client.SendPacket(new UpdateCharStats(player).Compile());
                  base.Use(player);
              }
    }
}
}
