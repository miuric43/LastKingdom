using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class PointShop : BaseNPC
    {
        public override string Name { get { return "PointShop"; } }
        public override string Map { get { return "포인트"; } }
        public override string ChatString { get { return "PointShop : 포인트상점 오픈." + "             "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.PointShop; } }
        public override int X { get { return 13; } }
        public override int Y { get { return 11; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return 8; } }
        public override int aSpeed { get { return 2; } }
        public override int aFrames { get { return 4; } }
        
        List<script.item.Item> templ = new List<script.item.Item>()
            {
                new script.item.Knife(1),
               
               
             
            };

        public override void Buy(model.Player player, int buyslot)
        {
            if (player.GetFreeSlot() != -1 && player.Point >= templ[buyslot].BuyPrice)
            {
                LKCamelot.script.item.Item tempitem = null;
                if (buyslot == 0)
                {
                    tempitem = new script.item.Knife().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                
                
                LKCamelot.model.World.NewItems.TryAdd(tempitem.m_Serial, tempitem);
                player.Point -= (uint)templ[buyslot].BuyPrice;
            }

        }

        public PointShop()
        {
        }

        public override GUMP Gump
        {
            get
            {
                return new GUMP((int)LKCamelot.library.NPCs.PointShop, 0xff85, 0x03ff, 0x70, "Menu", templ);
            }
        }
    }
}
