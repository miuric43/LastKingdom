using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 경험치물약 : BasePotion
    {
        public override string Name { get { return "경험치물약" + "       "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 경험치물약()
            : base(225)
        {
        }

        public 경험치물약(Serial serial)
            : base(serial)
        {
            m_ItemID = 225;
        }

        public override void Use(Player player)
                
  {

            if (Quantity >= 12)
            {



                   if (player.Level <= 99 )
            {
                player.XP = player.XP + 5000000;
                  Quantity = Quantity - 1;
                
            }
             if (player.Promo >= 1 && player.Promo <= 6)
                    {
                        player.XP = player.XP + 75000;
                        Quantity = Quantity - 1;
                       
                    }
             if (player.Promo >= 7)
                    {
                        player.XP = player.XP + 10000;
                        Quantity = Quantity - 1;
                       
                    }
                
                
           

            }
            else if (Quantity >= 3 && Quantity <= 11)
            {



                   if (player.Level <= 99 )
            {
                player.XP = player.XP + 5000000;
                  Quantity = Quantity - 1;
                
            }
             if (player.Promo >= 1 && player.Promo <= 6)
                    {
                        player.XP = player.XP + 75000;
                        Quantity = Quantity - 1;
                       
                    }
             if (player.Promo >= 7)
                    {
                        player.XP = player.XP + 10000;
                        Quantity = Quantity - 1;
                       
                    }
                

                string itdrop = "경험치물약이" + (Quantity-1) + "개남았습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());


            }
            else
            {
                   if (player.Level <= 99 )
            {
                player.XP = player.XP + 5000000;
                base.Use(player);
                
            }
             if (player.Promo >= 1 && player.Promo <= 6)
                    {
                        player.XP = player.XP + 75000;
                        base.Use(player);
                       
                    }
             if (player.Promo >= 7)
                    {
                        player.XP = player.XP + 10000;
                        base.Use(player);
                       
                    }
                
            }
        }
    }
}