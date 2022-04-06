using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 지력초기화물약 : BasePotion
    {
        public override string Name { get { return "MenReset Drug"; } }
        public override ulong BuyPrice { get { return 5000000; } }
        public override int SellPrice { get { return 4000000; } }

        public 지력초기화물약()
            : base(25)
        {
        }

        public 지력초기화물약(Serial serial)
            : base(serial)
        {
            m_ItemID = 25;
        }

        public override void Use(Player player)
          {

              if (player.GetFreeSlots() > 5 && player.Level >= 101 && Quantity >= 12 && player.m_Men >= 500)
                {
                   foreach (var it in player.Equipped2.Values)
                    {
                        it.Unequip(player, it.InvSlot);
                    }

                    var total = 500 + player.Extra;
                    player.Extra = (uint)total;
                    player.m_Men -= 500;
                    
                    player.m_HPCur = player.HP;
                    player.m_MPCur = player.MP;
                    player.client.SendPacket(new UpdateCharStats(player).Compile());
                    Quantity = Quantity - 1;

                }
              else if (Quantity >= 3 && Quantity <= 11 && player.GetFreeSlots() > 5 && player.Level >= 101 && player.m_Men >= 500)
             {
                   foreach (var it in player.Equipped2.Values)
                    {
                        it.Unequip(player, it.InvSlot);
                    }

                    var total = 500 + player.Extra;
                    player.Extra = (uint)total;
                    player.m_Men -= 500;
                    
                    player.m_HPCur = player.HP;
                    player.m_MPCur = player.MP;
                    player.client.SendPacket(new UpdateCharStats(player).Compile());
                    Quantity = Quantity - 1;
                    string itdrop = "지력초기화물약이" + (Quantity - 1) + "개남았습니다" + "                                          ";
                    player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());

                }
              else if (Quantity <= 3 && player.GetFreeSlots() > 5 && player.Level >= 101 && player.m_Men >= 500)
            {
               foreach (var it in player.Equipped2.Values)
                    {
                        it.Unequip(player, it.InvSlot);
                    }

                    var total = 500 + player.Extra;
                    player.Extra = (uint)total;
                    player.m_Men -= 500;
                    
                    player.m_HPCur = player.HP;
                    player.m_MPCur = player.MP;
                    player.client.SendPacket(new UpdateCharStats(player).Compile());
                base.Use(player);

            }
            else
            {
                string itdrop = "조건이 맞지 않습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());
            }
        }
    }
}