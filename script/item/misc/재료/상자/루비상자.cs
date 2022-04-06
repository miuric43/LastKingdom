using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 루비상자 : BasePotion
    {
        public override string Name { get { return "루비상자"+"     "; } }
        public override ulong BuyPrice { get { return 2000000; } }
        public override int SellPrice { get { return 1000000 * Quantity; } }
        public 루비상자()
            : base(265)
        {
        }

        public 루비상자(Serial serial)
            : base(serial)
        {
            m_ItemID = 265;
        }

        public override void Use(Player player)
        {
            if (Quantity >= 12 && player.GetFreeSlots() > 2)
              {
                  var newitem = new script.item.루비원석().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  
                  Quantity = Quantity - 1;
                  
            
        }
            else if (Quantity >= 3 && Quantity <= 11)
            {



                var newitem = new script.item.루비원석().Inventory(player);
                World.NewItems.TryAdd(newitem.m_Serial, newitem);
                player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                string itdrop = "루비원석이" + (Quantity - 1) + "개남았습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 10, 0, (short)itdrop.Count(), itdrop).Compile());
                Quantity = Quantity - 1;

            }
              else
              {
                  var newitem = new script.item.루비원석().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  base.Use(player);
              }
    }
}
}
