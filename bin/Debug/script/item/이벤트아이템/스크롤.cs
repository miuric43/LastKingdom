using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 플러스경험치 : BasePotion
    {
        public override string Name { get { return "플러스경험치" + "       "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 플러스경험치()
            : base(268)
        {
        }

        public 플러스경험치(Serial serial)
            : base(serial)
        {
            m_ItemID = 268;
        }

        public override void Use(Player player)
                
  {
      if (Quantity >= 12 && player.PlusXp == false)
      {
          player.PlusXp = true;
          player.NormalXp = false;
          string chats = "플러스경험치 적용" + "             ";
          player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)chats.Count(), chats).Compile());
          Quantity = Quantity - 1;


      }
      else if (Quantity >= 3 && Quantity <= 11 && player.PlusXp == false)
      {
          Quantity = Quantity - 1;
          player.PlusXp = true;
          player.NormalXp = false;
          string chats = "플러스경험치 적용" + "                   ";
          player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)chats.Count(), chats).Compile());
          string itdrop = "플러스경험치가" + (Quantity - 1) + "개남았습니다" + "                                          ";
          player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());

      }
      else if (Quantity >= 1 && Quantity <= 2 && player.PlusXp == false)
      {
          player.PlusXp = true;
          player.NormalXp = false;
          string chats = "플러스경험치 적용" + "                        ";
          player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)chats.Count(), chats).Compile());

          base.Use(player);
      }
      else
      {

          string chats = "이미 플러스경험치가 적용중입니다." + "             ";
          player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)chats.Count(), chats).Compile());

      }
  }
    }
}