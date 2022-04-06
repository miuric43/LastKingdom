using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 자수정해골 : BasePotion
    {
        public override string Name { get { return "자수정해골"+"       "; } }
        public override ulong BuyPrice { get { return 2000000; } }
        public override int SellPrice { get { return 1000000 * Quantity; } }

        public 자수정해골()
            : base(255)
        {
        }

        public 자수정해골(Serial serial)
            : base(serial)
        {
            m_ItemID = 255;
        }

        public override void Use(Player player)
        {
            if (Quantity >= 1003 && player.GetFreeSlots() > 2)
              {
                  var newitem = new script.item.성장의검().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  string itdrop = "성장의검이 제작되었습니다." + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());
                  Quantity = Quantity - 1000;
                  
            
        }
            else if (Quantity == 1001 && player.GetFreeSlots() > 2)
              {
                  var newitem = new script.item.성장의검().Inventory(player);
                  World.NewItems.TryAdd(newitem.m_Serial, newitem);
                  player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                  string itdrop = "성장의검이 제작되었습니다." + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());
                  Quantity = Quantity - 1000;
                  base.Use(player);

              }
              else
              {
                  string itdrop2 = "수량이" + (1002 - Quantity - 1) + "개 부족합니다" + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop2.Count(), itdrop2).Compile());
              }
    }
}
}
