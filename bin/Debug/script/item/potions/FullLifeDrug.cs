using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class FullLifeDrug : BasePotion
    {
        //public override string Name { get { return "Full Life Drug" + (script.item.BasePotion.OreTypeB)Stage + " : " + Quantity; } }
        public override string Name { get { return "Full Life Drug"; } }
        public override ulong BuyPrice { get { return 500; } }
        

        public FullLifeDrug()
            : base(22)
        {
        }

        public FullLifeDrug(Serial serial)
            : base(serial)
        {
            m_ItemID =22;
        }

        public override void Use(Player player)
        {
            if (Quantity >= 12)
            {



                player.HPCur = player.HP;
                Quantity = Quantity - 1;
                
           

            }
            else if (Quantity >= 3 && Quantity <= 11)
            {



                player.HPCur = player.HP;
                Quantity = Quantity - 1;

                string itdrop = "큰생명물약이" + (Quantity-1) + "개남았습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());


            }
            else
            {
                player.HPCur = player.HP;
                base.Use(player);
            }
        }
    }
}
