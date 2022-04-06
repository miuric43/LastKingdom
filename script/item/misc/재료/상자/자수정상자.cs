using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 자수정상자 : BasePotion
    {
        public override string Name { get { return "자수정상자"+"     "; } }
        public override ulong BuyPrice { get { return 2000000; } }
        public override int SellPrice { get { return 1000000 * Quantity; } }
        public 자수정상자()
            : base(262)
        {
        }

        public 자수정상자(Serial serial)
            : base(serial)
        {
            m_ItemID = 262;
        }

        public override void Use(Player player)
        {
            if (Quantity >= 12 && player.GetFreeSlots() > 2)
              {
                  var newitem = new script.item.자수정원석().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  
                  Quantity = Quantity - 1;
                  
            
        }
            else if (Quantity >= 3 && Quantity <= 11)
            {



                var newitem = new script.item.자수정원석().Inventory(player);
                World.NewItems.TryAdd(newitem.m_Serial, newitem);
                player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                string itdrop = "자수정원석이" + (Quantity - 1) + "개남았습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 10, 0, (short)itdrop.Count(), itdrop).Compile());
                Quantity = Quantity - 1;

            }
              else
              {
                  var newitem = new script.item.자수정원석().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  base.Use(player);
              }
    }
}
}
