using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 샤먼상자 : BasePotion
    {
        public override string Name { get { return "+10 샤먼패키지" + "          "; } }
        public override ulong BuyPrice { get { return 1000000; } }

        public 샤먼상자()
            : base(226)
        {
        }

        public 샤먼상자(Serial serial)
            : base(serial)
        {
            m_ItemID = 226;
        }

        public override void Use(Player player)
           {
            if (Quantity >= 12)
            {



       var newitem = new script.item.PythonScale().Inventory(player);
            (newitem as script.item.Item).Stage = 10;
            World.NewItems.TryAdd(newitem.m_Serial, newitem);
            player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

            var newitem2 = new script.item.WarPike().Inventory(player);
            (newitem2 as script.item.Item).Stage = 10;
            World.NewItems.TryAdd(newitem2.m_Serial, newitem2);
            player.client.SendPacket(new AddItemToInventory2(newitem2).Compile());
            
                Quantity = Quantity - 1;
                
           

            }
            else if (Quantity >= 3 && Quantity <= 11)
            {



             var newitem = new script.item.PythonScale().Inventory(player);
            (newitem as script.item.Item).Stage = 10;
            World.NewItems.TryAdd(newitem.m_Serial, newitem);
            player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

            var newitem2 = new script.item.WarPike().Inventory(player);
            (newitem2 as script.item.Item).Stage = 10;
            World.NewItems.TryAdd(newitem2.m_Serial, newitem2);
            player.client.SendPacket(new AddItemToInventory2(newitem2).Compile());
                Quantity = Quantity - 1;

                string itdrop = "+10 샤먼패키지" + (Quantity-1) + "개남았습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());


            }
            else
            {
           var newitem = new script.item.PythonScale().Inventory(player);
            (newitem as script.item.Item).Stage = 10;
            World.NewItems.TryAdd(newitem.m_Serial, newitem);
            player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

            var newitem2 = new script.item.WarPike().Inventory(player);
            (newitem2 as script.item.Item).Stage = 10;
            World.NewItems.TryAdd(newitem2.m_Serial, newitem2);
            player.client.SendPacket(new AddItemToInventory2(newitem2).Compile());
            base.Use(player);
               
            }
        }
    }
}