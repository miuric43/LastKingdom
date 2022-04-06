using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.model
{
    public class CombatHandler2
    {
        io.IOClient client;
        PlayerHandler handler;
        public double XPMulti = 1;
        public CombatHandler2(io.IOClient client, PlayerHandler handler)
        {
            this.client = client;
            this.handler = handler;
        }

        public void HandleCast(int header, script.spells.Spell spell, Player player, int target = 0, short castx = 0, short casty = 0)
        {
            if (spell is script.spells.Teleport)
                (spell as script.spells.Teleport).CastIt(player, new Point2D(castx, casty));
          //  if (spell is script.spells.Trace)
           //     (spell as script.spells.Trace).CastIt(player, new Point2D(castx, casty));
            
        }

        public void HandleMelee(Player play, int swingdir)
        {
            List<BaseObject> target = World.GetTileTarget(play, AdjecentTile(play, swingdir), swingdir);
            if (target == null)
                return;
            var take = play.Dam;

            foreach (var tar in target)
            {
                if (tar is Player)
                {
                    if (play.Map == "Village1" || play.Map == "Rest" || play.Map == "Arnold" || play.Map == "Loen" || play.TakeDam == false)
                        continue;

                    take -= (tar as Player).AC;
                    if (take <= 0)
                        take = 1;
                    if (play.Map != "대전장")
                    {
                        TakeDamage(play, tar, take);
                    }
                    if (play.Map == "대전장" && play.Color != (tar as Player).Color)
                    {
                        TakeDamage(play, tar, take);
                    }
                    if ((tar as Player).Color == 0)
                        play.pkpinkktime = Server.tickcount.ElapsedMilliseconds;

                    if ((tar as Player).Map == "Rest" && (tar as Player).Color != 1)
                    {
                       
                        if (play.Map != "대전장" && play.Map != "큰대전장")
                        {
                            play.pklastpk.Add(Server.tickcount.ElapsedMilliseconds);
                            play.pklastred = Server.tickcount.ElapsedMilliseconds;
                            string text2 = play.Name + " " + "→ PK " + " " + (tar as Player).Name + "→ Die " + "맵 " + play.Map + "                            ";
                            World.SendToAll(new QueDele(play.Serial, "all", new UpdateChatBox(7, 0x65, 1, (short)text2.Count(), text2).Compile()));
                        }
                        /*
                        if (play.PVPSubRank < (tar as Player).PVPSubRank)
                        {
                            if ((tar as Player).PVPSubRank - play.PVPSubRank <= 3)
                                {
                                play.m_Win += 1;                                
                                var temp = 5000;

                                if (play.PVPSubRank >= 0 && play.PVPSubRank <= 3)
                                    temp = (int)(temp * 1.0);
                                if (play.PVPSubRank >= 4 && play.PVPSubRank <= 4)
                                    temp = (int)(temp * 0.8);
                                if (play.PVPSubRank >= 5 && play.PVPSubRank <= 6)
                                    temp = (int)(temp * 0.05);
                                if (play.PVPSubRank >= 7 && play.PVPSubRank <= 10)
                                    temp = (int)(temp * 0.02);
                                if (play.PVPSubRank >= 7 && play.PVPSubRank <= 11)
                                    temp = (int)(temp * 0.02);
                                if (play.PVPSubRank >= 12)
                                    temp = (int)(temp * 0.01);
                                play.PvPPoint += temp * 1;
                                
                                Console.WriteLine("대전테스트");
                                }
                        }
                        if (play.PVPSubRank > (tar as Player).PVPSubRank)
                        {
                            if (play.PVPSubRank - (tar as Player).PVPSubRank <= 3)
                            {
                                play.m_Win += 1;
                                
                                var temp = 5000;

                                if (play.PVPSubRank >= 0 && play.PVPSubRank <= 3)
                                    temp = (int)(temp * 1.0);
                                if (play.PVPSubRank >= 4 && play.PVPSubRank <= 4)
                                    temp = (int)(temp * 0.8);
                                if (play.PVPSubRank >= 5 && play.PVPSubRank <= 6)
                                    temp = (int)(temp * 0.05);
                                if (play.PVPSubRank >= 7 && play.PVPSubRank <= 10)
                                    temp = (int)(temp * 0.02);
                                if (play.PVPSubRank >= 7 && play.PVPSubRank <= 11)
                                    temp = (int)(temp * 0.02);
                                if (play.PVPSubRank >= 12)
                                    temp = (int)(temp * 0.01);
                                play.PvPPoint += temp * 1;
                                Console.WriteLine("대전테스트2");
                            }
                        }
                        if (play.PVPSubRank == (tar as Player).PVPSubRank)
                        {
                            if (play.PVPSubRank - (tar as Player).PVPSubRank <= 3)
                            {
                                play.m_Win += 1;

                                var temp = 5000;

                                if (play.PVPSubRank >= 0 && play.PVPSubRank <= 3)
                                    temp = (int)(temp * 1.0);
                                if (play.PVPSubRank >= 4 && play.PVPSubRank <= 4)
                                    temp = (int)(temp * 0.8);
                                if (play.PVPSubRank >= 5 && play.PVPSubRank <= 6)
                                    temp = (int)(temp * 0.05);
                                if (play.PVPSubRank >= 7 && play.PVPSubRank <= 10)
                                    temp = (int)(temp * 0.02);
                                if (play.PVPSubRank >= 7 && play.PVPSubRank <= 11)
                                    temp = (int)(temp * 0.02);
                                if (play.PVPSubRank >= 12)
                                    temp = (int)(temp * 0.01);
                                play.PvPPoint += temp * 1;
                                Console.WriteLine("대전테스트3");
                            }
                        }

                        */
                    }
                    
                }
                else if (tar is script.monster.Monster)
                {
                    take -= (tar as script.monster.Monster).AC;
                    if (take <= 0)
                        take = 1;
                    TakeDamage(play, tar, take);
                }
            }
        }

        public void TakeDamage(Player player, object tar, int take)
        {
            float h = 0;
            if (tar is Player)
                h = ((float)player.Hit / ((float)player.Hit + (float)(tar as Player).Hit)) * 200;
            else if (tar is script.monster.Monster)
                h = ((float)player.Hit / ((float)player.Hit + (float)(tar as script.monster.Monster).Hit)) * 200;


            if (h >= 100 || new Random().Next(0, 100) < (int)h)
            {
                if (player.Weapon is script.item.IProc)
                {
                    if (tar is Player)
                        take += (player.Weapon as script.item.IProc).Proc(player, null, tar as Player);
                    else if (tar is script.monster.Monster)
                        take += (player.Weapon as script.item.IProc).Proc(player, tar as script.monster.Monster);
                }

                var swingbuff = player.m_Buffs.Where(xe => xe.BuffEffect.BuffType == script.spells.BuffCase.Triple
                    || xe.BuffEffect.BuffType == script.spells.BuffCase.Twister).FirstOrDefault();
                if (swingbuff != null)
                {
                    var ttt = swingbuff.Level * 0.05;
                    take = (int)(take * (ttt + 0.40d));
                }

                if (tar is Player)
                {
                    (tar as Player).HPCur -= take;
                    World.SendToAll(new QueDele((tar as Player).m_Map, new HitAnimation((tar as Player).Serial,
                        Convert.ToByte(((((float)(tar as Player).HPCur / (float)(tar as Player).HP) * 100) * 1))).Compile()));
                    if (player.TakeDam2 == true)
                    {
                        string damg = string.Format("데미지:{0}" + "            ", take, 0);
                        player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)damg.Count(), damg).Compile());
                    }
                }
                else if (tar is script.monster.Monster)
                {
                    (tar as script.monster.Monster).HPCur -= take;
                    World.SendToAll(new QueDele((tar as script.monster.Monster).m_Map, new HitAnimation((tar as script.monster.Monster).m_Serial,
                        Convert.ToByte(((((float)(tar as script.monster.Monster).HPCur / (float)(tar as script.monster.Monster).HP) * 100) * 1))).Compile()));

                    if (player.TakeDam2 == true)
                    {
                        string damg = string.Format("데미지:{0}" + "            ", take, 0);
                        player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)damg.Count(), damg).Compile());
                    }

                    if ((tar as script.monster.Monster).HPCur <= 0)
                    {
                        int ExpRatio = Server.Exp_Ratio;
                        /*  if ((tar as script.monster.Monster).Name == "성기사")
                          {
                              string text3 = "(" + player.Name + ")" + "님이 성기사를 쓰러뜨렸습니다" + "                    ";
                              World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)text3.Count(), text3).Compile()));
                              mem.BossMonster_time = 10;
                          }*/
                        if (player.member.Count != 0)
                        {
                            switch (player.member.Count())
                            {
                                case 2:
                                    XPMulti = 1.05 / (2);
                                    break;
                                case 3:
                                    XPMulti = 1.1 / (3);
                                    break;
                                case 4:
                                    XPMulti = 1.15 / 4;
                                    break;
                            }

                            double XPMultiTemp = XPMulti;
                            var whisp2 = Server.playerHandler.add.Values.Where(xe => xe.member.Count != 0 && xe.loggedIn && xe.member[0] == player.member[0] && xe.Map == player.Map).ToList();
                            foreach (var mem in whisp2)
                            {
                                System.Diagnostics.Debug.Write(player.Name);
                                if (mem.Name != player.Name)
                                {
                                    XPMulti = XPMultiTemp / mem.member.Count() * 1.5;
                                }
                                else
                                {
                                    XPMulti = XPMultiTemp;
                                }
                                if ((tar as script.monster.Monster).Name == "성기사" + "   ")
                                {
                                    string text3 = "(" + player.Name + ")" + "님이 성기사를 쓰러뜨렸습니다" + "                    ";
                                    World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)text3.Count(), text3).Compile()));
                                    mem.BossMonster_time = 10;
                                }
                                if ((tar as script.monster.Monster).Name == "성물")
                                {
                                    string text3 = "(" + player.Name + ")" + "님이 성물을 파괴하였습니다" + "                    ";
                                    World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)text3.Count(), text3).Compile()));
                                    mem.Warfare_time = 10;
                                    Server.Castellan = player.guildName;
                                }
                                if (mem.Promo > 0 && mem.NormalXp == true && mem.PlusXp == false && DateTime.Now.Hour != 10)//조건 : 0 승급 이상 and 플러스경험치 미적용 and 두배타임 X
                                {
                                    var temp = (tar as script.monster.Monster).XP;


                                    if (mem.Promo >= 1 && mem.Promo <= 3)
                                        temp = (int)(temp * 0.015);
                                    if (mem.Promo >= 4 && mem.Promo <= 4)
                                        temp = (int)(temp * 0.008);
                                    if (mem.Promo >= 5 && mem.Promo <= 6)
                                        temp = (int)(temp * 0.002);
                                    if (mem.Promo >= 7)
                                        temp = (int)(temp * 0.002);

                                    mem.XP += temp * 1;// *3;
                                }
                                if (mem.Promo > 0 && mem.NormalXp == false && mem.PlusXp == true && DateTime.Now.Hour != 10)//조건 : 0 승급 이상 and 플러스경험치 적용 and 두배타임 X
                                {
                                    var temp = (tar as script.monster.Monster).XP;


                                    if (mem.Promo >= 1 && mem.Promo <= 3)
                                        temp = (int)(temp * 0.015);
                                    if (mem.Promo >= 4 && mem.Promo <= 4)
                                        temp = (int)(temp * 0.008);
                                    if (mem.Promo >= 5 && mem.Promo <= 6)
                                        temp = (int)(temp * 0.002);
                                    if (mem.Promo >= 7)
                                        temp = (int)(temp * 0.002);

                                    mem.XP += temp * 2;// *3;
                                }
                                if (mem.Promo > 0 && mem.NormalXp == true && mem.PlusXp == false && DateTime.Now.Hour == 10)//조건 : 0 승급 이상 and 플러스경험치 미적용 and 두배타임 OK
                                {
                                    var temp = (tar as script.monster.Monster).XP;


                                    if (mem.Promo >= 1 && mem.Promo <= 3)
                                        temp = (int)(temp * 0.015);
                                    if (mem.Promo >= 4 && mem.Promo <= 4)
                                        temp = (int)(temp * 0.008);
                                    if (mem.Promo >= 5 && mem.Promo <= 6)
                                        temp = (int)(temp * 0.002);
                                    if (mem.Promo >= 7)
                                        temp = (int)(temp * 0.002);

                                    mem.XP += temp * 2;// *3;
                                }
                                if (mem.Promo > 0 && mem.NormalXp == false && mem.PlusXp == true && DateTime.Now.Hour == 10)//조건 : 0 승급 이상 and 플러스경험치 적용 and 두배타임 OK
                                {
                                    var temp = (tar as script.monster.Monster).XP;


                                    if (mem.Promo >= 1 && mem.Promo <= 3)
                                        temp = (int)(temp * 0.015);
                                    if (mem.Promo >= 4 && mem.Promo <= 4)
                                        temp = (int)(temp * 0.008);
                                    if (mem.Promo >= 5 && mem.Promo <= 6)
                                        temp = (int)(temp * 0.002);
                                    if (mem.Promo >= 7)
                                        temp = (int)(temp * 0.002);

                                    mem.XP += temp * 4;// *3;
                                }
                                else if (mem.Promo == 0 && mem.PlusXp == false && mem.NormalXp == true && DateTime.Now.Hour != 10)//조건 : 비승급 and 플러스경험치 미적용 and 두배타임 X
                                {
                                    mem.XP += (tar as script.monster.Monster).XP * 1;// *3;
                                }
                                else if (mem.Promo == 0 && mem.PlusXp == true && mem.NormalXp == false && DateTime.Now.Hour != 10)//조건 : 비승급 and 플러스경험치 적용 and 두배타임 X
                                {
                                    mem.XP += (tar as script.monster.Monster).XP * 2;// *3;
                                }
                                else if (mem.Promo == 0 && mem.PlusXp == false && mem.NormalXp == true && DateTime.Now.Hour == 10)//조건 : 비승급 and 플러스경험치 미적용 and 두배타임 OK
                                {
                                    mem.XP += (tar as script.monster.Monster).XP * 2;// *3;
                                }
                                else if (mem.Promo == 0 && mem.PlusXp == true && mem.NormalXp == false && DateTime.Now.Hour == 10)//조건 : 비승급 and 플러스경험치 적용 and 두배타임 OK
                                {
                                    mem.XP += (tar as script.monster.Monster).XP * 4;// *3;
                                }
                                //mem.XP += (tar as script.monster.Monster).XP * 100;// *3;
                                if (mem.m_Map == "레이드")
                                {
                                    (tar as script.monster.Monster).DropLoot(mem);
                                    (tar as script.monster.Monster).m_Loc.X = (tar as script.monster.Monster).m_SpawnLoc.X;
                                    (tar as script.monster.Monster).m_Loc.Y = (tar as script.monster.Monster).m_SpawnLoc.Y;
                                }
                                if (mem.m_Map != "레이드")
                                {
                                    (tar as script.monster.Monster).DropLoot(player);
                                    (tar as script.monster.Monster).m_Loc.X = (tar as script.monster.Monster).m_SpawnLoc.X;
                                    (tar as script.monster.Monster).m_Loc.Y = (tar as script.monster.Monster).m_SpawnLoc.Y;
                                }
                            }
                        }
                        else
                        {
                            var whisp2 = Server.playerHandler.add.Values.Where(xe => xe.Cmember.Count != 0 && xe.loggedIn && xe.Cmember[0] == player.Cmember[0] && xe.Map == player.Map).ToList();
                            foreach (var mem in whisp2)
                                if ((tar as script.monster.Monster).Name == "성물")
                                {
                                    string text3 = "(" + player.Name + ")" + "님이 성물을 파괴하였습니다" + "                    ";
                                    World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)text3.Count(), text3).Compile()));
                                    mem.Warfare_time = 10;
                                    Server.Castellan = player.guildName;
                                }
                            if (player.Promo > 0 && player.NormalXp == true && player.PlusXp == false && DateTime.Now.Hour != 10)//조건 : 0 승급 이상 and 플러스경험치 미적용 and 두배타임 X
                            {
                                var temp = (tar as script.monster.Monster).XP;


                                if (player.Promo >= 1 && player.Promo <= 3)
                                    temp = (int)(temp * 0.015);
                                if (player.Promo >= 4 && player.Promo <= 4)
                                    temp = (int)(temp * 0.008);
                                if (player.Promo >= 5 && player.Promo <= 6)
                                    temp = (int)(temp * 0.002);
                                if (player.Promo >= 7)
                                    temp = (int)(temp * 0.002);

                                player.XP += temp * 1;// *3;
                            }
                            if (player.Promo > 0 && player.NormalXp == false && player.PlusXp == true && DateTime.Now.Hour != 10)//조건 : 0 승급 이상 and 플러스경험치 적용 and 두배타임 X
                            {
                                var temp = (tar as script.monster.Monster).XP;


                                if (player.Promo >= 1 && player.Promo <= 3)
                                    temp = (int)(temp * 0.015);
                                if (player.Promo >= 4 && player.Promo <= 4)
                                    temp = (int)(temp * 0.008);
                                if (player.Promo >= 5 && player.Promo <= 6)
                                    temp = (int)(temp * 0.002);
                                if (player.Promo >= 7)
                                    temp = (int)(temp * 0.002);

                                player.XP += temp * 2;// *3;
                            }
                            if (player.Promo > 0 && player.NormalXp == true && player.PlusXp == false && DateTime.Now.Hour == 10)//조건 : 0 승급 이상 and 플러스경험치 미적용 and 두배타임 OK
                            {
                                var temp = (tar as script.monster.Monster).XP;


                                if (player.Promo >= 1 && player.Promo <= 3)
                                    temp = (int)(temp * 0.015);
                                if (player.Promo >= 4 && player.Promo <= 4)
                                    temp = (int)(temp * 0.008);
                                if (player.Promo >= 5 && player.Promo <= 6)
                                    temp = (int)(temp * 0.002);
                                if (player.Promo >= 7)
                                    temp = (int)(temp * 0.002);

                                player.XP += temp * 2;// *3;
                            }
                            if (player.Promo > 0 && player.NormalXp == false && player.PlusXp == true && DateTime.Now.Hour == 10)//조건 : 0 승급 이상 and 플러스경험치 적용 and 두배타임 OK
                            {
                                var temp = (tar as script.monster.Monster).XP;


                                if (player.Promo >= 1 && player.Promo <= 3)
                                    temp = (int)(temp * 0.015);
                                if (player.Promo >= 4 && player.Promo <= 4)
                                    temp = (int)(temp * 0.008);
                                if (player.Promo >= 5 && player.Promo <= 6)
                                    temp = (int)(temp * 0.002);
                                if (player.Promo >= 7)
                                    temp = (int)(temp * 0.002);

                                player.XP += temp * 4;// *3;
                            }
                            else if (player.Promo == 0 && player.PlusXp == false && player.NormalXp == true && DateTime.Now.Hour != 10)//조건 : 비승급 and 플러스경험치 미적용 and 두배타임 X
                            {
                                player.XP += (tar as script.monster.Monster).XP * 1 * ExpRatio;// *3;
                            }
                            else if (player.Promo == 0 && player.PlusXp == true && player.NormalXp == false && DateTime.Now.Hour != 10)//조건 : 비승급 and 플러스경험치 적용 and 두배타임 X
                            {
                                player.XP += (tar as script.monster.Monster).XP * 2;// *3;
                            }
                            else if (player.Promo == 0 && player.PlusXp == false && player.NormalXp == true && DateTime.Now.Hour == 10)//조건 : 비승급 and 플러스경험치 미적용 and 두배타임 OK
                            {
                                player.XP += (tar as script.monster.Monster).XP * 2;// *3;
                            }
                            else if (player.Promo == 0 && player.PlusXp == true && player.NormalXp == false && DateTime.Now.Hour == 10)//조건 : 비승급 and 플러스경험치 적용 and 두배타임 OK
                            {
                                player.XP += (tar as script.monster.Monster).XP * 4;// *3;
                            }
                            //player.XP += (tar as script.monster.Monster).XP * 100;// *3;
                            if (player.m_Map != "레이드")
                            {
                                (tar as script.monster.Monster).DropLoot(player);
                                (tar as script.monster.Monster).m_Loc.X = (tar as script.monster.Monster).m_SpawnLoc.X;
                                (tar as script.monster.Monster).m_Loc.Y = (tar as script.monster.Monster).m_SpawnLoc.Y;
                            }
                        }
                    }
                }
            }
        }
        public static Point2D AdjecentTile(Player player, int swingloc)
        {
            if (swingloc == -1)
                swingloc = 7;
            if (swingloc == -2)
                swingloc = 6;
            if (swingloc == -3)
                swingloc = 5;
            if (swingloc == -4)
                swingloc = 4;
            if (swingloc == 8)
                swingloc = 0;
            if (swingloc == 9)
                swingloc = 1;
            if (swingloc == 10)
                swingloc = 2;
            if (swingloc == 11)
                swingloc = 3;

            if (swingloc == 0)
                return new Point2D(player.X, player.Y - 1);
            if (swingloc == 1)
                return new Point2D(player.X + 1, player.Y - 1);
            if (swingloc == 2)
                return new Point2D(player.X + 1, player.Y);
            if (swingloc == 3)
                return new Point2D(player.X + 1, player.Y + 1);
            if (swingloc == 4)
                return new Point2D(player.X, player.Y + 1);
            if (swingloc == 5)
                return new Point2D(player.X - 1, player.Y + 1);
            if (swingloc == 6)
                return new Point2D(player.X - 1, player.Y);
            if (swingloc == 7)
                return new Point2D(player.X - 1, player.Y - 1);

            return new Point2D(1, 1);
        }
    }
}
