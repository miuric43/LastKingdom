using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 초심자의선물 : BasePotion
    {
        public override string Name { get { return "초심자의선물" + "           "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 초심자의선물()
            : base(215)
        {
        }

        public 초심자의선물(Serial serial)
            : base(serial)
        {
            m_ItemID = 215;
        }

        public override void Use(Player player)
        {
            if (player.GetFreeSlots() > 5 && player.Level <= 10)
            {
                var newitem = new script.item.BKnife().Inventory(player);
                
                World.NewItems.TryAdd(newitem.m_Serial, newitem);
                player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                var newitem2 = new script.item.Barmor().Inventory(player);
                World.NewItems.TryAdd(newitem2.m_Serial, newitem2);
                player.client.SendPacket(new AddItemToInventory2(newitem2).Compile());

                player.Gold = player.Gold + 1000000;





                base.Use(player);
            }
        }
    }
}