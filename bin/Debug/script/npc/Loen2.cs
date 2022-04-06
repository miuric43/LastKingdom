using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class Loen2 : BaseNPC
    {
        public override string Name { get { return "Loen"; } }
        public override string Map { get { return "Village1"; } }
        public override string ChatString { get { return "로엔 : 새로운물약을 판매중입니다." + "             "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.Loen2; } }
        public override int X { get { return 100; } }
        public override int Y { get { return 100; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return 3; } }
        public override int aSpeed { get { return 2; } }
        public override int aFrames { get { return 8; } }
        
        List<script.item.Item> templ = new List<script.item.Item>()
            {
                new script.item.생명초기화물약(1),
                new script.item.힘초기화물약(1),
                new script.item.숙련초기화물약(1),
                new script.item.지력초기화물약(1),
             
            };

        public override void Buy(model.Player player, int buyslot)
        {
            if (player.GetFreeSlot() != -1 && player.Gold >= templ[buyslot].BuyPrice)
            {
                LKCamelot.script.item.Item tempitem = null;
                if (buyslot == 0)
                {
                    tempitem = new script.item.생명초기화물약().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 1)
                {
                    tempitem = new script.item.힘초기화물약().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                       tempitem).Compile());
                }
                if (buyslot == 2)
                {
                    tempitem = new script.item.숙련초기화물약().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                       tempitem).Compile());
                }
                if (buyslot == 3)
                {
                    tempitem = new script.item.지력초기화물약().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                       tempitem).Compile());
                }
                
                LKCamelot.model.World.NewItems.TryAdd(tempitem.m_Serial, tempitem);
                player.Gold -= (uint)templ[buyslot].BuyPrice;
            }

        }

        public Loen2()
        {
        }

        public override GUMP Gump
        {
            get
            {
                return new GUMP((int)LKCamelot.library.NPCs.Loen2, 0xff85, 0x03ff, 0x70, "Menu", templ);
            }
        }
    }
}
