using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 에메랄드원석 : BasePotion
    {
        public override string Name { get { return "에메랄드원석"+"       "; } }
        public override ulong BuyPrice { get { return 2000000; } }
        public override int SellPrice { get { return 1000000 * Quantity; } }

        public 에메랄드원석()
            : base(254)
        {
        }

        public 에메랄드원석(Serial serial)
            : base(serial)
        {
            m_ItemID = 254;
        }

        public override void Use(Player player)
        {
            if (Quantity >= 203 && player.GetFreeSlots() > 2)
              {
                  var newitem = new script.item.BackduPlate().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  string itdrop = "백두 플레이트가 제작되었습니다." + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());
                  Quantity = Quantity - 200;
                  
            
        }
            else if (Quantity == 201 && player.GetFreeSlots() > 2)
              {
                  var newitem = new script.item.BackduPlate().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  string itdrop = "백두 플레이트가 제작되었습니다." + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());
                  Quantity = Quantity - 200;
                  base.Use(player);

              }
              else
              {
                  string itdrop2 = "수량이" + (202 - Quantity - 1) + "개 부족합니다" + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop2.Count(), itdrop2).Compile());
              }
    }
}
}
