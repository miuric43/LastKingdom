using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 샤먼물약 : BasePotion
    {
        public override string Name { get { return "샤먼전직물약"+"      "; } }
        public override ulong BuyPrice { get { return 5000000; } }
        public override int SellPrice { get { return 4000000; } }

        public 샤먼물약()
            : base(24)
        {
        }

        public 샤먼물약(Serial serial)
            : base(serial)
        {
            m_ItemID = 24;
        }

        public override void Use(Player player)
        {
            if (Quantity >= 12)
            {



                player.Class = LKCamelot.library.Class.Shaman;
                string text2 = "아론=> 너는 이제 샤먼이 되었다" + "                       ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0x70, 1, (short)text2.Count(), text2).Compile());
                Quantity = Quantity - 1;



            }
            else if (Quantity >= 3 && Quantity <= 11)
            {



                player.Class = LKCamelot.library.Class.Shaman;
                string text2 = "아론=> 너는 이제 샤먼이 되었다" + "                       ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0x70, 1, (short)text2.Count(), text2).Compile());
                Quantity = Quantity - 1;

                string itdrop = "샤먼전직물약이" + (Quantity - 1) + "개남았습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());


            }
            else
            {
                player.Class = LKCamelot.library.Class.Shaman;
                string text2 = "아론=> 너는 이제 샤먼이 되었다" + "                       ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0x70, 1, (short)text2.Count(), text2).Compile());
                base.Use(player);
            }
        }
    }
}
