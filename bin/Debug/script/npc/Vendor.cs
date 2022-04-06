using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.npc
{
    public class Vendor : BaseNPC
    {
        public override string Name { get { return "Vendor"; } }
        public override string Map { get { return "Village1"; } }
        public override string ChatString { get { return "노점상: 초보마법책을 판매하고 있습니다." + "                "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.Vendor; } }
        public override int X { get { return 128; } }
        public override int Y { get { return 90; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return 6; } }
        public override int aSpeed { get { return 1; } }
        public override int aFrames { get { return 8; } }

        List<script.item.Item> templ = new List<script.item.Item>()
            {
               new script.item.ThunderCrossBook(1), 
               new script.item.FlameRoundBook(1),
               new script.item.ButterflyBook(1),
               new script.item.WakondaBook(1),

               new script.item.PickUpBook(1), 
               new script.item.ReCallBook(1),
               new script.item.TransparencyBook(1),
               new script.item.SharpEyeBook(1),
               new script.item.StoneCurseBook(1),
               new script.item.FreezingBook(1),
               new script.item.플러스경험치(1),
               //new script.item.FireflyBook(1),

            };

        public override void Buy(model.Player player, int buyslot)
        {
            if (player.GetFreeSlot() != -1 && player.Gold >= templ[buyslot].BuyPrice)
            {
                LKCamelot.script.item.Item tempitem = null;
                if (buyslot == 0)
                {
                    tempitem = new script.item.ThunderCrossBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 1)
                {
                    tempitem = new script.item.FlameRoundBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 2)
                {
                    tempitem = new script.item.ButterflyBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 3)
                {
                    tempitem = new script.item.WakondaBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 4)
                {
                    tempitem = new script.item.PickUpBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 5)
                {
                    tempitem = new script.item.ReCallBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 6)
                {
                    tempitem = new script.item.TransparencyBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 7)
                {
                    tempitem = new script.item.SharpEyeBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 8)
                {
                    tempitem = new script.item.StoneCurseBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 9)
                {
                    tempitem = new script.item.FreezingBook().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                if (buyslot == 10)
                {
                    tempitem = new script.item.플러스경험치().Inventory(player);
                    player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
                        tempitem).Compile());
                }
                LKCamelot.model.World.NewItems.TryAdd(tempitem.m_Serial, tempitem);
                player.Gold -= (uint)templ[buyslot].BuyPrice;
            }

        }

        public Vendor()
        {
        }

        public override GUMP Gump
        {
            get
            {
                return new GUMP((int)LKCamelot.library.NPCs.Vendor, 0xff85, 0x03ff, 0x70, "Menu", templ);
            }
        }
    }
}
