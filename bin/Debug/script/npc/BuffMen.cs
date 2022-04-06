using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.net;
using LKCamelot.util;
using LKCamelot.model;
using LKCamelot.script;

namespace LKCamelot.script.npc
{
    public class BuffMen : BaseNPC
    {
        public override string Name { get { return "BuffMen"; } }
        public override string Map { get { return "Rest"; } }
        public override string ChatString { get { return "버프맨: 초보마법책을 판매하고 있습니다." + "                "; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.BuffMen; } }
        public override int X { get { return 20; } }
        public override int Y { get { return 15; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return 6; } }
        public override int aSpeed { get { return 1; } }
        public override int aFrames { get { return 8; } }

        List<script.item.Item> templ = new List<script.item.Item>()
            {
               new script.item.ACBuff(1),
               new script.item.HitBuff(1),
               new script.item.DamBuff(1),


            };

        public override void Buy(model.Player player, int buyslot)
        {
            if (player.GetFreeSlot() != -1 && player.Gold >= templ[buyslot].BuyPrice)
            {
                //LKCamelot.script.item.Item tempitem = null;
                if (buyslot == 0)
                {
                    int mobile = Serial.NewMobile;
                    World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)player.X, (short)player.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 50 }, 0).Compile()));
                    var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 1500, player.m_Map, new DeleteObject(mobile).Compile());
                    tmp.tempser = mobile;
                    World.TickQue.Add(tmp);

                    player.ACbuff_time = 500;
                    player.ACbuff = 500;
                    player.client.SendPacket(new UpdateCharStats(player).Compile());                    
                }
                if (buyslot == 1)
                {
                    int mobile = Serial.NewMobile;
                    World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)player.X, (short)player.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 50 }, 0).Compile()));
                    var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 1500, player.m_Map, new DeleteObject(mobile).Compile());
                    tmp.tempser = mobile;
                    World.TickQue.Add(tmp);

                    player.Hitbuff_time = 500;
                    player.Hitbuff = 500;
                    player.client.SendPacket(new UpdateCharStats(player).Compile());
                }
                if (buyslot == 2)
                {
                    int mobile = Serial.NewMobile;
                    World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)player.X, (short)player.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 50 }, 0).Compile()));
                    var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 1500, player.m_Map, new DeleteObject(mobile).Compile());
                    tmp.tempser = mobile;
                    World.TickQue.Add(tmp);

                    player.Dambuff_time = 500;
                    player.Dambuff = 500;
                    player.client.SendPacket(new UpdateCharStats(player).Compile());
                }
                //LKCamelot.model.World.NewItems.TryAdd(tempitem.m_Serial, tempitem);
                player.Gold -= (uint)templ[buyslot].BuyPrice;
            }

        }

        public BuffMen()
        {
        }

        public override GUMP Gump
        {
            get
            {
                return new GUMP((int)LKCamelot.library.NPCs.BuffMen, 0xff85, 0x03ff, 0x70, "Menu", templ);
            }
        }
    }
}
