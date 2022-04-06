#define PROMOCAP12

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using LKCamelot.net;
using LKCamelot.util;
using LKCamelot.model;
using LKCamelot.script;

using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

using System.Media;

namespace LKCamelot.io
{
    public partial class IOClient
    {
        public static uint stat = 0;
        static int temps = 15;
        public void UpdateChat(string text, byte c1 = 0xff, byte c2 = 0xff, byte cb = 1)
        {
            SendPacket(new UpdateChatBox(c1, c2, 1, (short)text.Count(), text).Compile());
        }
        private void PlaySound()
        {
            SoundPlayer OPCALL_SOUND = new SoundPlayer("C:/1000.wav");
            OPCALL_SOUND.PlaySync();            
        }
        public void HandleGMChat(string[] str2)
        {
            if (adminChk(player.Name) == "0")
                return;
            string[] uk = str2[0].Split('\x0');
            if (uk.Length > 1)
                str2[0] = uk[1];
            else
                str2[0] = uk[0];



            switch (str2[0])
            {
                case "@tele":
                    GMTele(str2);
                    break;
                case "@learn":
                    GMLearn(str2);
                    break;
                case "@invis":
                    GMInvis(str2);
                    break;
                case "@createitem":
                    GMCreateItem(str2);
                    break;
                case "@kick":
                    GMKick(str2);
                    break;
                case "@itemarray":
                    GMItemArray(str2);
                    break;
                case "@tap":
                    GMTapPlayer(str2);
                    break;
            }
        }

        public void HandleChat(string[] str2, string str)
        {
            string[] ustr = str2[0].Split('\x0');

            str2[0] = ustr[1];


            for (int i = 0; i < str2.Length; i++)
            {
                str2[i] = str2[i].Replace("\x0", "");
                str2[i] = str2[i].Replace("?", "");

            }
            if (str2[0] == "@이동")
            {
                if (str2.Count() > 0)
                {
                    HandleGo(str2[1]);
                    SendPacket((new SetProfessions(player.ProfessionString).Compile()));
                }
            }
            /*
            else if (str2[0] == "@결투정보")
            {
                string Point = string.Format("{0}전 {1}승 {2}패 승점 {3}점 " + "                                    ", player.m_Win + player.m_Loss, player.m_Win, player.m_Loss, player.m_PvPPoint);
                string Rank = string.Format(player.PVPRank + "              ");
                
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Point.Count(), Point).Compile());
                if (player.PVPSubRank == 0 )
                {
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Rank.Count(), Rank).Compile());
                }
                if (player.PVPSubRank >= 1 && player.PVPSubRank <= 9)
                {
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Rank.Count(), Rank).Compile());
                }
                if (player.PVPSubRank >= 10 && player.PVPSubRank <= 12)
                {
                    SendPacket(new UpdateChatBox(0xf2, 0xf2, 1, (short)Rank.Count(), Rank).Compile());
                }

            }
            */
            else if (str2[0] == "@버프테스트")
            {
                player.NormalXp = false;
                player.PlusXp = true;
                player.ACbuff_time = 10000;
            }
            else if (str2[0] == "@버프테스트2")
            {
                player.NormalXp = true;
                player.PlusXp = false;
                player.ACbuff_time = 10000;
            }
            else if (str2[0] == "@버프타임")
            {
                string plyb3 = "버프효과 : " + player.ACbuff_time + "           ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)plyb3.Count(), plyb3).Compile());
                string plyb2 = "버프효과 : " + player.ACbuff + "            ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)plyb2.Count(), plyb2).Compile());
                string plyb = "남은시간 : " + (player.ACbuff_time - 100) + "            ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)plyb.Count(), plyb).Compile());
            }
            else if (str2[0] == "@관리자추가")
            {
                if (adminChk(player.Name) == "0")
                    return;
                using (StreamWriter sw = new StreamWriter("worldAdmin.txt", true, Encoding.GetEncoding("euc-kr")))
                {
                    sw.WriteLine(string.Format(",{0}", str2[1]));

                }
                String pppfile = str2[1] + "님이 관리자로 등록 되었습니다." + "                                             ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)pppfile.Count(), pppfile).Compile());
                var plr2 = handler.add.Where(xe => xe.Value != null && xe.Value.GetFreeSlots() > 3 && xe.Value.Name == str2[1].ToUpper() && xe.Value.loggedIn).FirstOrDefault().Value;
                string achat = "연구소 서버관리자로 임명되었습니다." + "                                             ";
                plr2.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                string achat2 = "기타내용" + "                                             ";
                plr2.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());

            }
            else if (str2[0] == "@명령어")
            {
                UpdateChat("@이동, @버그, @서열, @초기화, @자동마나 0.10, @자동생명 0.10, " + "              ");
                UpdateChat("@속성 힘 100, @송금, @autoloot, @자동공격 " + "              ");
                UpdateChat("@보관 0 (1,2..), @ping, @정보, @pkstats " + "              ");
            }
            else if (str[0] == '*')
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    var message = str.Remove(0, 1);
                    message = message + "                      ";
                    message = "[" + player.Name + "] : " + message;
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)message.Count(), message.ToString()).Compile()));
                }

            }
            else if (str[0] == '~')
            {
                if (player.Gold >= 1000 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 10000 > lastcmd)
                {
                    lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;

                    var message = str.Remove(0, 1);
                    message = message + "                      ";
                    message = "[거래]" + player.Name + " : " + message + "                           ";
                    Console.WriteLine(message);
                    WriteBug(message);
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x00, 0x02, 7, (short)message.Count(), message).Compile()));
                    player.Gold = player.Gold - 1000;
                }
                else
                {
                    string achat = "잠시만 기다려주세요." + "              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
            }
            else if (str[0] == '!')
            {

                if (player.Level >= 220 && player.Promo == 7 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 3000 > lastcmd)
                {
                    lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;

                    var message = str.Remove(0, 1);
                    message = message + "                      ";
                    message = "[" + player.Name + "] : " + message;
                    Console.WriteLine(message);
                    WriteBug(message);
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x25, 0x65, 5, (short)message.Count(), message).Compile()));

                }
                else if (player.Level >= 201 && player.Promo == 6 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 3000 > lastcmd)
                {
                    lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;

                    var message = str.Remove(0, 1);
                    message = message + "                      ";
                    message = "[" + player.Name + "] : " + message;
                    Console.WriteLine(message);
                    WriteBug(message);
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0, 0xff, 0, (short)message.Count(), message).Compile()));

                }

                else if (player.Level >= 101 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 3000 > lastcmd)
                {

                    lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;

                    var message = str.Remove(0, 1);
                    message = message + "                      ";
                    message = "[" + player.Name + "] : " + message;
                    Console.WriteLine(message);
                    WriteBug(message);
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0xff, 0xff, 7, (short)message.Count(), message).Compile()));

                }
                else if (player.Level >= 5 && player.Promo == 0 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 3000 > lastcmd)
                {

                    lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;

                    var message = str.Remove(0, 1);
                    message = message + "                      ";
                    message = "[" + player.Name + "] : " + message;
                    Console.WriteLine(message);
                    WriteBug(message);
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0x02, 0, (short)message.Count(), message.ToString()).Compile()));

                }
                else
                {
                    string achat = "잠시만 기다려주세요." + "              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }



            }
            else if (str[0] == '~' || str2[0] == "@tele" || str2[0] == "@learn"
                || str2[0] == "@invis" || str2[0] == "@createitem" || str2[0] == "@kick" || str2[0] == "@tap")
            {
                HandleGMChat(str2);
            }
            else if (str2[0] == "@교환")
            {
                SendPacket(new ExchangeBox("테스트    ").Compile());
            }
            else if (str2[0] == "@돈교환")
            {
                SendPacket(new GoldBox("테스트    ").Compile());
            }
            else if (str2[0] == "@레이드")
            {

                var whisp2 = Server.playerHandler.add.Values.Where(xe => xe.member.Count != 0 && xe.loggedIn && xe.member[0] == player.member[0] && xe.Map == player.Map).ToList();
                foreach (var mem in whisp2)
                {
                    var 좌표 = Util.RandomMinMax(1, 15);
                    mem.BossMonster_time = 5000;
                    mem.Loc = new Point2D(10, 17);
                    mem.Map = "레이드";
                }

            }
            else if (str2[0] == "@밝기")
            {
                var keys = handler.add.Values.Where(xe => xe != null && xe.loggedIn).ToList();//&& xe.loggedIn


                foreach (var member in keys)
                {

                    member.client.SendPacket(new SetBrightness(100000).Compile());
                }

            }
            else if (str2[0] == "@파티가입" && player.member.Count() == 0)
            {
                var whisp = handler.add.Values.Where(xe => xe != null && xe.member.Count() != 0 && xe.member[0] == str2[1]).ToList();
                var whisp12 = handler.add.Values.Where(xe => xe != null && xe.member.Count() != 0 && xe.Name == str2[1]).FirstOrDefault();
                if (whisp12 != null)
                {
                    if (whisp12.Level > player.Level && whisp12.Level - player.Level > 10)
                    {
                        String achat22 = "레벨 차이가 10이 넘습니다                     ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                        return;
                    }
                    else if (whisp12.Level < player.Level && player.Level - whisp12.Level > 10)
                    {
                        String achat22 = "레벨 차이가 10이 넘습니다                     ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                        return;
                    }
                }
                foreach (var mem in whisp)
                {
                    if (mem.member.Count() < 4)
                    {
                        mem.member.Add(player.Name);
                        String achat = player.Name + "님이 파티에 가입하였습니다                     ";
                        mem.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    else
                    {
                        String achat3 = "파티가 꽉찼습니다                     ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat3.Count(), achat3).Compile());
                        return;
                    }
                }
                var whisp2 = handler.add.Values.Where(xe => xe != null && xe.member.Count() != 0 && xe.member[0] == str2[1]).FirstOrDefault();
                if (whisp2 != null)
                {
                    player.member = new List<String>(whisp2.member);
                }



                String achat2 = "파티에 가입하였습니다                     ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
            }
            else if (str2[0] == "@파티생성" && player.member.Count() == 0)
            {
                if (player.Promo <= 1)
                {
                    player.member.Add(player.Name);
                    String achat0 = "[" + player.Name + "] 파티를 생성하였습니다                     ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat0.Count(), achat0.ToString()).Compile()));
                    return;
                }
                else if (player.Promo <= 3 && player.Gold >= 5000000)
                {
                    player.Gold -= 5000000;
                    player.member.Add(player.Name);
                    String achat12 = "[" + player.Name + "] 파티를 생성하였습니다                     ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat12.Count(), achat12.ToString()).Compile()));
                    return;
                }
                else if (player.Promo <= 4 && player.Gold >= 10000000)
                {
                    player.Gold -= 10000000;
                    player.member.Add(player.Name);
                    String achat12 = "[" + player.Name + "] 파티를 생성하였습니다                     ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat12.Count(), achat12.ToString()).Compile()));
                    return;
                }
                else if (player.Promo <= 5 && player.Gold >= 20000000)
                {
                    player.Gold -= 20000000;
                    player.member.Add(player.Name);
                    String achat12 = "[" + player.Name + "] 파티를 생성하였습니다                     ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat12.Count(), achat12.ToString()).Compile()));
                    return;
                }
                else if (player.Promo <= 6 && player.Gold >= 30000000)
                {
                    player.Gold -= 30000000;
                    player.member.Add(player.Name);
                    String achat12 = "[" + player.Name + "] 파티를 생성하였습니다                     ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat12.Count(), achat12.ToString()).Compile()));
                    return;
                }
                else if (player.Promo <= 7 && player.Gold >= 50000000)
                {
                    player.Gold -= 50000000;
                    player.member.Add(player.Name);
                    String achat12 = "[" + player.Name + "] 파티를 생성하였습니다                     ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat12.Count(), achat12.ToString()).Compile()));
                    return;
                }
                else
                {
                    String achat22 = "파티생성에 필요한 돈이 부족합니다                     ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                    return;
                }
            }
            else if (str2[0] == "@파티해체" && player.member.Count() != 0)
            {
                player.member.Clear();
                String achat22 = "파티가 해체되었습니다                     ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                return;
            }


            else if (str2[0] == "@파티탈퇴" && player.member.Count() != 0)
            {

                var whisp = handler.add.Values.Where(xe => xe != null && xe.member.Count() != 0).ToList();
                foreach (var mem in whisp)
                {
                    if (mem.member.Contains(player.Name))
                    {
                        //                            mem.member.Clear();
                        mem.member = new List<string>();

                        return;
                    }
                    String achat22 = "파티에서 나왔습니다                     ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                    return;
                }
            }
            
            else if (str2[0] == "@공성전시작")
            {

                var whisp2 = Server.playerHandler.add.Values.Where(xe => xe.Cmember.Count != 0 && xe.loggedIn && xe.Cmember[0] == player.Cmember[0] && xe.Map == player.Map).ToList();
                foreach (var mem in whisp2)
                {
                    
                    mem.Warfare_time = 10000;
                    mem.Loc = new Point2D(98, 78);
                    mem.Map = "공성맵";
                }

            }
            else if (str2[0] == "@공성참가" && player.Cmember.Count() == 0)
            {
                var whisp = handler.add.Values.Where(xe => xe != null && xe.Cmember.Count() != 0 && xe.Cmember[0] == str2[1]).ToList();
                var whisp12 = handler.add.Values.Where(xe => xe != null && xe.Cmember.Count() != 0 && xe.Name == str2[1]).FirstOrDefault();
                if (whisp12 != null)
                {
                    if (player.guildName == "")
                    {
                        String achat22 = "길드가 없습니다                     ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                        return;
                    }
                   
                }
                foreach (var mem in whisp)
                {
                    if (mem.Cmember.Count() < 4)
                    {
                        mem.Cmember.Add(player.Name);
                        String achat = player.Name + "님이 공성리스트에 추가되었습니다                     ";
                        mem.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    else
                    {
                        String achat3 = "공성인원이 꽉찼습니다                     ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat3.Count(), achat3).Compile());
                        return;
                    }
                }
                var whisp2 = handler.add.Values.Where(xe => xe != null && xe.Cmember.Count() != 0 && xe.Cmember[0] == str2[1]).FirstOrDefault();
                if (whisp2 != null)
                {
                    player.Cmember = new List<String>(whisp2.Cmember);
                }



                String achat2 = "공성에 참가 하였습니다                     ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
            }
            else if (str2[0] == "@공성접수" && player.Cmember.Count() == 0)
            {
                if (adminChk(player.Name) == "0")
                    return;
                player.Cmember.Add(player.Name);
                String achat0 = "[" + player.Name + "] 공성접수가 시작되었습니다.                      ";
                World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat0.Count(), achat0.ToString()).Compile()));
                String achat1 = "[" + player.Name + "] 공성에 참여하실분은 '@공성참가 GM최후' 라고 입력해주세요                      ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat1.Count(), achat1.ToString()).Compile()));
                    return;
                
               
            }
            else if (str2[0] == "@공성파기" && player.Cmember.Count() != 0)
            {
                if (adminChk(player.Name) == "0")
                    return;
                player.Cmember.Clear();
                String achat22 = "공성전이 취소되었습니다                     ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                return;
            }


            else if (str2[0] == "@공성탈퇴" && player.Cmember.Count() != 0)
            {

                var whisp = handler.add.Values.Where(xe => xe != null && xe.Cmember.Count() != 0).ToList();
                foreach (var mem in whisp)
                {
                    if (mem.Cmember.Contains(player.Name))
                    {
                        //                            mem.Cmember.Clear();
                        mem.Cmember = new List<string>();

                        return;
                    }
                    String achat22 = "공성전에서 나왔습니다                     ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat22.Count(), achat22).Compile());
                    return;
                }
            }
            
            else if (str2[0] == "@길드창설" && player.guildName == "" && str2[1] != null && str2[1].Trim() != "")
            {
                if (player.Gold >= 1000000000 && player.Promo >= 7)
                {
                    player.Gold -= 1000000000;
                    player.guildName = str2[1];
                    player.guildTitle = "수장";
                    string message = "[" + player.guildName + "] : " + "길드가 창설되었습니다." + "                           ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)message.Count(), message.ToString()).Compile()));
                    message = "[" + player.Name + "]" + " 님이 " + "[" + player.guildName + "]" + "길드의 수장이 되었습니다." + "                              ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)message.Count(), message.ToString()).Compile()));
                }
                else if (player.Gold <= 1000000000)
                {
                    string achat = "길드창설에 십억원이 필요합니다." + "              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
                else if (player.Promo <= 6)
                {
                    string achat = "길드창설은 7승이상 가능합니다." + "              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
            }
            else if (str[0] == ';' && player.guildName != "")
            {
                string currentPlayer = player.Name;
                string currentGuild = player.guildName;
                var message = str.Remove(0, 1);
                var whisp = handler.add.Values.Where(xe => xe != null && xe.loggedIn && xe.guildName == currentGuild).ToList();
                foreach (var member in whisp)
                {
                    string message2 = currentPlayer + ": " + message + "                                ";
                    member.client.SendPacket(new UpdateChatBox(0xff, 0x70, 1, (short)message2.Count(), message2).Compile());
                }
            }
            else if (str2[0] == "@길드수락" && player.guildName != "" && str2[1] != null && str2[1].Trim() != "" && player.guildTitle == "수장")
            {
                var whisp = handler.add.Values.Where(xe => xe != null && xe.Name == str2[1] && xe.guildName == "").FirstOrDefault();
                if (whisp != null)
                {
                    whisp.guildName = player.guildName;
                    whisp.guildTitle = "길드원";
                    string message = whisp.Name + " 님이" + " 길드가입이 되었습니다." + "                                    ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());

                    string message2 = player.guildName + "길드에 가입되었습니다" + "                                    ";
                    whisp.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message2.Count(), message2).Compile());
                }
                else
                {
                    string message = whisp.Name + " 님이 다른길드에 가입이 되어있습니다." + "                                    ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());
                }
            }
            else if (str2[0] == "@부수장임명" && player.guildName != "" && str2[1] != null && str2[1].Trim() != "" && player.guildTitle == "수장")
            {
                var whisp = handler.add.Values.Where(xe => xe != null && xe.Name == str2[1] && xe.guildName == "").FirstOrDefault();
                if (whisp != null)
                {
                    whisp.guildName = player.guildName;
                    whisp.guildTitle = "부수장";
                    string message = whisp.Name + " 님이" + " 부수장으로 길드가입이 되었습니다." + "                                    ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());

                    string message2 = player.guildName + "길드에 가입되었습니다" + "                                    ";
                    whisp.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message2.Count(), message2).Compile());
                }
                else
                {
                    string message = whisp.Name + " 님이 다른길드에 가입이 되어있습니다." + "                                    ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());
                }
            }
            else if (str2[0] == "@길드탈퇴" && player.guildName != "")
            {


                {
                    player.guildName = "";
                    player.guildTitle = "";
                    string message = "길드탈퇴완료." + "                     ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());
                }
            }
            else if (str2[0] == "@길드표시")
            {
                if (player.guildName == "")
                {
                    string achat = "현재 가입된 길드가 없습니다." + "                   ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
                else
                {
                    var whisp = handler.add.Values.Where(xe => xe.guildName != "" && xe.guildName == player.guildName).ToList();
                    string achat = "가입된길드 :" + player.guildName + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 10, 1, (short)achat.Count(), achat).Compile());
                    achat = "수장: " + whisp.Where(xe => xe.guildTitle == "수장").FirstOrDefault().Name + "                                      ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    var tempMem = whisp.Where(xe => xe != null && xe.guildTitle == "부수장").FirstOrDefault();
                    if (tempMem != null)
                    {
                        achat = "부수장: " + tempMem.Name + "                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    achat = "길드원:                     ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    foreach (var member in whisp)
                    {
                        if (member.guildTitle != "수장" && member.guildTitle != "부수장")
                        {
                            string aachat = "";
                            if (member.loggedIn)
                            {
                                aachat = member.Name + " [접속중]                              ";
                            }
                            else
                            {
                                aachat = member.Name + "                                      ";
                            }
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)aachat.Count(), aachat).Compile());
                        }
                    }
                }
            }
            else if (str2[0] == "@길드서열")
            {
                var whisp = handler.add.Values.Where(xe => xe.guildName != "" && (xe.guildTitle == "수장" || xe.guildTitle == "부수장")).ToList();
                Dictionary<string, int> diction = new Dictionary<string, int>();
                SortedDictionary<int, string> diction1 = new SortedDictionary<int, string>();
                SortedDictionary<string, int> diction2 = new SortedDictionary<string, int>();
                Dictionary<string, string> diction3 = new Dictionary<string, string>();

                foreach (var member in whisp)
                {
                    if (!diction.ContainsKey(member.guildName))
                    {
                        diction.Add(member.guildName, member.m_Men + member.m_Str + member.m_Vit + member.m_Dex);
                        if (member.guildTitle == "수장")
                        {
                            diction3.Add(member.guildName, member.Name);
                        }
                    }
                    else
                    {
                        diction2.Add(member.guildName, diction[member.guildName] + member.m_Men + member.m_Str + member.m_Vit + member.m_Dex);
                        if (member.guildTitle == "수장")
                        {
                            diction3.Add(member.guildName, member.Name);
                        }

                        if (diction.Remove(member.guildName))
                        {
                        }
                    }
                }
                foreach (var key in diction.Keys)
                {
                    diction2.Add(key, diction[key]);
                }

                //                diction2.OrderBy(xe => diction2.Keys);
                int count = 1;

                var myList = diction2.Values.ToList();
                myList.Sort();
                myList.Reverse();
                //                myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                foreach (var key in myList)
                {
                    string nname = diction2.FirstOrDefault(x => x.Value == key).Key;
                    string achat = count + "위 - " + "[" + nname + "]" + "길드 - 수장 [" + diction3[nname] + "]                           ";
                    SendPacket(new UpdateChatBox(60, 0x70, 1, (short)achat.Count(), achat).Compile());
                    count++;
                }
            }
            else if (str2[0] == "@길드")
            {
                if (player.guildName == "")
                {
                    string achat = "현재 가입된 길드가 없습니다." + "                   ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
                else
                {
                    var whisp = handler.add.Values.Where(xe => xe.guildName != "" && xe.guildName == player.guildName).ToList();
                    string achat = "가입된길드 :" + player.guildName + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 10, 1, (short)achat.Count(), achat).Compile());
                    achat = "수장: " + whisp.Where(xe => xe.guildTitle == "수장").FirstOrDefault().Name + "                                      ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    var tempMem = whisp.Where(xe => xe != null && xe.guildTitle == "부수장").FirstOrDefault();
                    if (tempMem != null)
                    {
                        achat = "부수장: " + tempMem.Name + "                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    achat = "길드원:                     ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    foreach (var member in whisp)
                    {
                        if (member.guildTitle != "수장" && member.guildTitle != "부수장")
                        {
                            string aachat = "";
                            if (member.loggedIn)
                            {
                                aachat = member.Name + " [접속중]                              ";
                            }
                            else
                            {
                                aachat = member.Name + "                                      ";
                            }
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)aachat.Count(), aachat).Compile());
                        }
                    }
                }
            }
            else if (str2[0] == "@공속")
            {
                player.m_AttackSpeed = 0;
            }
            else if (str2[0] == "@홍팀참가")
            {
                player.Color = 1;
            }
            else if (str2[0] == "@청팀참가")
            {
                player.Color = 3;
            }
            else if (str2[0] == "@생년월일" && str2[1] != null && str2[1].Trim() != "")
            {
                if (player.Gold >= 100 && player.Bday == null)
                {
                    player.Gold -= 100;
                    player.Bday = str2[1];

                    string message = "생년월일이 등록되었습니다" + "                           ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());
                }
                else if (player.Gold <= 100)
                {
                    string achat = "생년월일등록에 100원이 필요합니다." + "              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }

            }
            else if (str2[0] == "@생년월일확인")
            {
                if (player.Level >= 0 && player.Bday != null)
                {
                    string plyb = "등록된 생년월일" + player.Bday + "                                 ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)plyb.Count(), plyb).Compile());
                }
                else
                {
                    string plyb = "등록된 생년월일이 없습니다" + "                         ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)plyb.Count(), plyb).Compile());

                }
            }
            else if (str2[0] == "@생년월일삭제")
            {
                if (player.Level >= 0 && player.Bday != null)
                {
                    player.Bday = null;
                }
                else
                {
                    string plyb = "등록된 생년월일이 없습니다" + "                         ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)plyb.Count(), plyb).Compile());

                }
            }
            else if (str2[0] == "@버프")
            {


                string buff3 = "<현재 버프상태>" + "                ";
                SendPacket(new UpdateChatBox(0x00, 0xff, 20737, (short)buff3.Count(), buff3).Compile());
                {
                    if (player.PlusXp == true)
                    {
                        string buff = "경험치버프 on" + "                ";
                        SendPacket(new UpdateChatBox(0xff, 10, 1, (short)buff.Count(), buff).Compile());
                    }
                    if (player.PlusXp == false)
                    {
                        string buff = "경험치버프 off" + "                ";
                        SendPacket(new UpdateChatBox(7, 0x65, 1, (short)buff.Count(), buff).Compile());
                    }
                    if (player.NormalXp == true)
                    {
                        string buff = "일반 on" + "                ";
                        SendPacket(new UpdateChatBox(0xff, 10, 1, (short)buff.Count(), buff).Compile());
                    }
                    if (player.NormalXp == false)
                    {
                        string buff = "일반 off" + "                ";
                        SendPacket(new UpdateChatBox(7, 0x65, 1, (short)buff.Count(), buff).Compile());
                    }
                }
            }
            else if (str2[0] == "@창")
            {
                System.Diagnostics.Process.Start("http://upload2.inven.co.kr/upload/2017/03/08/bbs/i13376646467.jpg");
                
            }
            else if (str2[0] == "@소리")
            {
                PlaySound();                
            }
            else if (str2[0] == "@배팅")
            {
                if (!Server.quiz_start)
                {
                    string txt = "퀴즈가 진행중이 아닙니다.                       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)txt.Count(), txt).Compile());
                }
                else if (Server.quiz_a == str2[1])
                {
                    Server.quiz_start = false;

                    string message = player.Name + "님! 축하드립니다. 잭팟에 당첨되었습니다. " + "                        ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)message.Count(), message.ToString()).Compile()));
                    string message2 = "최종 수령액은 " + Server.quiz_p + "원입니다                           ";
                    World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)message2.Count(), message2.ToString()).Compile()));

                    player.Bank += Server.quiz_p;
                }
                else if (Server.quiz_a != str2[1])
                {
                    string plyb = "잭팟에 실패하였습니다.                       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)plyb.Count(), plyb).Compile());
                    player.Gold -= 10000;
                    Server.quiz_p += 10000;
                }
        }
            else if (str2[0] == "@이름변경" && str2[1] != null && str2[1].Trim() != "")
            {
                var Namelest = handler.add.Where(xe => xe.Key != null && xe.Value != null && xe.Value.Name != str2[1].ToUpper()).FirstOrDefault().Value;

                if (player.Gold >= 100 && Namelest.Name != str2[1] && player.Bday == str2[2])
                {
                    player.Gold -= 100;
                    player.Name = str2[1];
                    player.client.SendPacket(new CreateChar(player, player.Serial).Compile());
                    string message = "이름이 변경되었습니다" + "                           ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());
                    player.client.SendPacket(new SetLevelGold(player).Compile());
                }
                else if (player.Gold <= 100)
                {
                    string achat = "변경실패." + "              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }

            }
            else if (str2[0] == "@암호변경" && str2[1] != null && str2[1].Trim() != "")
            {
                if (player.Gold >= 100 && player.Bday == str2[2])
                {
                    player.Gold -= 100;
                    player.Pass = str2[1];
                    player.client.SendPacket(new CreateChar(player, player.Serial).Compile());

                    string message = "암호가 변경되었습니다" + "                           ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)message.Count(), message).Compile());
                }
                else if (player.Gold <= 100)
                {
                    string achat = "변경실패." + "              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }

            }
            else if (str2[0] == "@포인트증가")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    switch (str2[1])
                    {
                        case "만":
                            player.Point = player.Point + 10000;
                            UpdateChat("포인트가 10000점 증가했습니다. " + "              ");
                            break;
                    }
                }
            }

            else if (str2[0] == "@포인트")
            {
                string Point = string.Format("포인트:{0}" + "                                    ", player.Point, 0);
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Point.Count(), Point).Compile());
            }
            else if (str == "승급 신청")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Aron").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Level >= 80)
                {
                    if (AronStage == 0)
                    {
                        string achat = "[아론]: 정말로 승급하겠느냐?." + "                 ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                        string achat2 = "[아론]: 승급하시려면 \"네\" 라고 채팅창에 입력" + "                 ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        AronStage = 4;
                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                    }
                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "턴")
            {
                var 턴 = player.m_Str + player.m_Men + player.m_Dex + player.m_Vit + player.m_Extra - 15909 - 291;
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Aron").FirstOrDefault();
                bool exp = false;
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Level == 231 && player.m_XP >= 3000000 && 턴 < 700000000000)
                {
                    while (!exp)
                    {
                        if (player.m_XP >= 3000000)
                        {
                            player.XP -= 3000000;
                            player.Extra = player.Extra + 1;
                        }
                        else
                        {
                            exp = true;
                        }
                    }
                }
                else if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Level == 231 && player.m_XP >= 10000000 && 턴 > 700000000000)
                {
                    while (!exp)
                    {
                        if (player.m_XP >= 10000000)
                        {
                            player.XP -= 10000000;
                            player.Extra = player.Extra + 1;
                        }
                        else
                        {
                            exp = true;
                        }
                    }
                }
                player.client.SendPacket(new UpdateCharStats(player).Compile());
                string achat2 = "[아론]: 턴이 되었다." + "                    ";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                str = player.Name + " : " + str + "" + "                             " + "     ";

            }
            else if (str == "환생")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Aron").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.GetFreeSlots() > 5 && player.Level >= 231 && player.m_Str + player.m_Vit + player.m_Men + player.m_Dex + player.m_Extra >= 19300 && player.m_Tun <= 9 && player.Gold >= 1000000000)
                {

                    {
                        var total = player.m_Str + player.m_Dex + player.m_Men + player.m_Vit + player.Extra;
                        player.Extra = (uint)total;
                        player.m_Str = 0;
                        player.m_Dex = 0;
                        player.m_Men = 0;
                        player.m_Vit = 0;
                        player.m_HPCur = player.HP;
                        player.m_MPCur = player.MP;
                        player.Extra = player.Extra - 19300;
                        player.Extra += 500;
                        player.m_Level = 1;
                        player.m_Promo = 0;
                        player.m_Tun += 1;
                        player.Gold -= 1000000000;
                        player.XP = 0;
                        player.tempLocate = new Point2D(18, 18);
                        player.tempLocateMap = null;
                        SendPacket(new SetLevelGold(player).Compile());
                        SendPacket(new UpdateCharStats(player).Compile());
                        string achat2 = "[아론]: 환생이 되었다!." + "                    ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        String achat0 = "[" + player.Name + "] 님께서 환생셨습니다                     ";
                        World.SendToAll(new QueDele(player.Serial, "all", new UpdateChatBox(0x08, 0xff, 20737, (short)achat0.Count(), achat0.ToString()).Compile()));
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "재료교환")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Aron").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.GetFreeSlots() > 5 && player.Qitem.Name == "루비원석" + "     " && player.Qitem.Quantity >= 203)
                {

                    {                        
                        player.Qitem.Delete(player);
                        var newitem = new script.item.TaeguksonPlate().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                        string itdrop = "태극손 플레이트가 제작되었습니다." + "                                          ";
                        player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());

                        string achat2 = "[아론]: 재료교환이 완료되었다!." + "                    ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        str = player.Name + " : " + str + "" + "                             " + "     ";

                       
                    }

                }
                else
                {
                    str = "실패                  ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "완료")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "튜토리얼").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Level <= 20 && player.Gold >= 1000)
                {

                    {
                        player.Gold = player.Gold - 1000;
                        var newitem = new script.item.초심자의선물().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                        string achat2 = "[7승검객]: 수고했다." + "       ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        player.Loc = new Point2D(98, 100);
                        player.Map = "Village1";
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }            
                else if (str2[0] == "@숙련도")
            {
                var strdam = "";
                if (player.TakeDam == true)
                {
                    player.Slevel = false;
                    strdam = "숙련도버프종료." + "             ";
                }
                else
                {
                    player.Slevel = true;
                    strdam = "숙련도버프시작." + "             ";
                }
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)strdam.Count(), strdam).Compile());
            }
            else if (str2[0] == "@대인공격")
            {
                var strdam = "";
                if (player.TakeDam == true)
                {
                    player.TakeDam = false;
                    strdam = "대인공격종료." + "             ";
                }
                else
                {
                    player.TakeDam = true;
                    strdam = "대인공격시작." + "             ";
                }
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)strdam.Count(), strdam).Compile());
            }
            else if (str2[0] == "@데미지출력")
            {
                var strdam = "";
                if (player.TakeDam2 == true)
                {
                    player.TakeDam2 = false;
                    strdam = "데미지출력종료." + "             ";
                }
                else
                {
                    player.TakeDam2 = true;
                    strdam = "데미지출력시작." + "             ";
                }
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)strdam.Count(), strdam).Compile());
            }
            else if (str2[0] == "@외치기")
            {
                var chats = "";
                if (player.chat)
                {
                    player.chat = false;
                    chats = "외치기시작." + "             ";
                }
                else
                {
                    player.chat = true;
                    chats = "외치기종료." + "             ";
                }
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)chats.Count(), chats).Compile());
            }
            else if (str2[0] == "@로또")
            {
                var PlyGold = Convert.ToUInt16(str2[2]);
                if (player.Gold >= PlyGold)

                    try
                    {
                        if (Convert.ToUInt64(str2[1]) < 0)
                            return;
                    }
                    catch { return; }

                var Number = Util.Random(1, 9);


                if (Number == str[1])
                {
                    string text = "당첨" + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());


                }
                else if (Number != str[1])
                {
                    string text = "꽝." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }

                string tra = string.Format("추첨번호 : {0}" + "                          ", Number);
                SendPacket(new UpdateChatBox(0xff, 10, 0, (short)tra.Count(), tra).Compile());

                string tra2 = string.Format("선택번호 : {0}" + "                          ", str2[1]);
                SendPacket(new UpdateChatBox(0xff, 10, 0, (short)tra2.Count(), tra2).Compile());
            }

            else if (str2[0] == "마나물약산다")
            {
                try
                {
                    if (Convert.ToUInt64(str2[1]) < 0)
                        return;
                }
                catch { return; }
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= (Convert.ToUInt64(str2[1]) * 500))
                {

                    {
                        //lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                        player.Gold = player.Gold - (Convert.ToUInt64(str2[1]) * 500);
                        var newitem = new script.item.FullMagicDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += str[1];
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "        ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }

            else if (str == "마나물약산다100")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= 50000)
                {

                    {
                        player.Gold = player.Gold - 50000;
                        var newitem = new script.item.FullMagicDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += 99;
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "       ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "마나물약산다1000")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= 500000 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 60000 > lastcmd)
                {

                    {
                        lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                        player.Gold = player.Gold - 500000;
                        var newitem = new script.item.FullMagicDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += 999;
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "       ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        Console.WriteLine(str);
                        물약(str);
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "마나물약산다10000")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= 5000000 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 60000 > lastcmd)
                {

                    {
                        lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                        player.Gold = player.Gold - 5000000;
                        var newitem = new script.item.FullMagicDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += 9999;
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "       ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        Console.WriteLine(str);
                        물약(str);
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str2[0] == "생명물약산다")
            {
                try
                {
                    if (Convert.ToUInt64(str2[1]) < 0)
                        return;
                }
                catch { return; }
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= (Convert.ToUInt64(str2[1]) * 500))
                {

                    {
                        //lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                        player.Gold = player.Gold - (Convert.ToUInt64(str2[1]) * 500);
                        var newitem = new script.item.FullLifeDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += str[1];
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "        ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }           
            else if (str2[0] == "@능력치")
            {
                if (adminChk(player.Name) == "0")
                    return;
                try
                {
                    if (Convert.ToUInt64(str2[1]) < 0)
                        return;
                }
                catch { return; }

                
                
                {
                    
                    player.Extra += uint.Parse(str2[1]);
                    SendPacket(new UpdateCharStats(player).Compile());

                }
                
            }
            /*
        else if (str == "생명물약산다")
        {
            var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
            if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                && player.Gold >= 25000)
            {

                {
                    player.Gold = player.Gold - 25000;
                    var newitem = new script.item.FullLifeDrug().Inventory(player);
                    World.NewItems.TryAdd(newitem.m_Serial, newitem);
                    (newitem as script.item.Item).Quantity += 49;
                    player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                    string achat2 = "[로엔]: 구매완료!" + "       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                }

            }
            else
            {
                str = player.Name + " : " + str + "" + "                             " + "     ";

                World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
            }
        }*/
            else if (str == "생명물약산다100")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= 50000)
                {

                    {
                        player.Gold = player.Gold - 50000;
                        var newitem = new script.item.FullLifeDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += 99;
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "       ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "생명물약산다1000")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= 500000 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 60000 > lastcmd)
                {

                    {
                        lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                        player.Gold = player.Gold - 500000;
                        var newitem = new script.item.FullLifeDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += 999;
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "       ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        Console.WriteLine(str);
                        물약(str);
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "생명물약산다10000")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Loen").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Gold >= 5000000 && LKCamelot.Server.tickcount.ElapsedMilliseconds - 60000 > lastcmd)
                {

                    {
                        lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                        player.Gold = player.Gold - 5000000;
                        var newitem = new script.item.FullLifeDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        (newitem as script.item.Item).Quantity += 9999;
                        player.client.SendPacket(new AddItemToInventory2(newitem).Compile());

                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                        string achat2 = "[로엔]: 구매완료!" + "       ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                        Console.WriteLine(str);
                        물약(str);
                    }

                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "리턴")
            {

                if (adminChk(player.Name) == "0")
                    return;
                {
                    foreach (var it in player.Equipped2.Values)
                    {
                        it.Unequip(player, it.InvSlot);
                    }
                    player.Extra = 0;

                    player.m_Str = 0;
                    player.m_Dex = 0;
                    player.m_Men = 0;
                    player.m_Vit = 0;
                    player.m_HPCur = player.HP;
                    player.m_MPCur = player.MP;


                    player.m_Level = 1;
                    player.m_Promo = 0;
                    SendPacket(new UpdateCharStats(player).Compile());
                    SendPacket(new SetLevelGold(player).Compile());


                }


            }
            else if (str2[0] == "@뷰")
            {
                var plr = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                //if (plr.Info == false)

                if (str2[1] != "")
                {


                    var tele2 = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                    string name2 = string.Format("<" + "케릭명:{0}" + "            ", tele2.Name + ">");
                    SendPacket(new UpdateChatBox(0xff, 10, 1, (short)name2.Count(), name2).Compile());
                    if (player.Weapon != null)
                    {
                        string Weapon = string.Format("무기:{0}" + "            ", tele2.Weapon.Name + "+" + tele2.Weapon.Stage);
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Weapon.Count(), Weapon).Compile());
                    }
                    if (player.Weapon == null)
                    {
                        string Weapon = string.Format("무기:" + "미착용        ");
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Weapon.Count(), Weapon).Compile());
                    }
                    //
                    if (player.Head != null)
                    {
                        string Head = string.Format("투구:{0}" + "            ", tele2.Head.Name + "+" + tele2.Head.Stage);
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Head.Count(), Head).Compile());
                    }
                    if (player.Head == null)
                    {
                        string Head = string.Format("투구:" + "미착용        ");
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Head.Count(), Head).Compile());
                    }
                    //
                    if (player.Armor != null)
                    {
                        string Armor = string.Format("갑옷:{0}" + "            ", tele2.Armor.Name + "+" + tele2.Armor.Stage);
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Armor.Count(), Armor).Compile());
                    }
                    if (player.Armor == null)
                    {
                        string Armor = string.Format("갑옷:" + "미착용        ");
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Armor.Count(), Armor).Compile());
                    }
                    //
                    if (player.Amulet != null)
                    {
                        string Amulet = string.Format("목걸이:{0}" + "            ", tele2.Amulet.Name + "+" + tele2.Amulet.Stage);
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Amulet.Count(), Amulet).Compile());
                    }
                    if (player.Amulet == null)
                    {
                        string Amulet = string.Format("목걸이:" + "미착용        ");
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Amulet.Count(), Amulet).Compile());
                    }
                    //
                    if (player.Ring != null)
                    {
                        string Ring = string.Format("반지:{0}" + "            ", tele2.Ring.Name + "+" + tele2.Ring.Stage);
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Ring.Count(), Ring).Compile());
                    }
                    if (player.Ring == null)
                    {
                        string Ring = string.Format("반지:" + "미착용        ");
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Ring.Count(), Ring).Compile());
                    }
                    //
                    if (player.Shields != null)
                    {
                        string Shields = string.Format("방패:{0}" + "            ", tele2.Shields.Name + "+" + tele2.Shields.Stage);
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Shields.Count(), Shields).Compile());
                    }
                    if (player.Shields == null)
                    {
                        string Shields = string.Format("방패:" + "미착용        ");
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Shields.Count(), Shields).Compile());
                    }

                }
                else
                {
                    var plr2 = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                    string achat2 = plr2.Name + "님이 정보를 공개하지 않았습니다." + "                                                             ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                }
            }
            else if (str2[0] == "@속성뷰")
            {

                {
                    if (str2[1] != "")
                    {

                        {




                            var tele = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;

                            string name = string.Format("<" + "케릭명:{0}" + "            ", tele.Name + ">");
                            string Level = string.Format("레벨:{0}" + "            ", tele.Level);
                            string Str = string.Format("힘:{0}" + "            ", tele.m_Str);
                            string Men = string.Format("지력:{0}" + "            ", tele.m_Men);
                            string Dex = string.Format("숙련:{0}" + "            ", tele.m_Dex);
                            string Vit = string.Format("생명:{0}" + "            ", tele.m_Vit);
                            string Dam = string.Format("파괴:{0}" + "            ", tele.Dam);
                            string Hit = string.Format("적중:{0}" + "            ", tele.Hit);
                            string AC = string.Format("방어:{0}" + "            ", tele.AC);

                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)name.Count(), name).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Level.Count(), Level).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Str.Count(), Str).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Men.Count(), Men).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Dex.Count(), Dex).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Vit.Count(), Vit).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Dam.Count(), Dam).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Hit.Count(), Hit).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)AC.Count(), AC).Compile());





                        }

                    }
                }

            }
            else if (str2[0] == "@drops")
            {

            }

            else if (str2[0] == "@ping")
            {

                long totalTime = 0;
                int timeout = 60;
                System.Net.NetworkInformation.Ping pingSender = new Ping();

                for (int i = 0; i < 1; i++)
                {
                    PingReply reply = pingSender.Send(connection.RemoteEndPoint.Address, timeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        totalTime += reply.RoundtripTime;
                    }
                }
                long res = totalTime / 1;
                string achat = res + " ms";
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
            }
            else if (str == "체력물약 주세요")
            {
                if (player.m_Map == "Village1" && World.Dist2d(player.Loc, new Point2D(90, 173)) <= 4)
                {
                    AliasStage = 1;
                    string achat = "[알리아스]: 나는 체력물약을 가지고 있다 물약이 필요하느냐?" + "                  ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str2[0] == "@버그")
            {
                if (LKCamelot.Server.tickcount.ElapsedMilliseconds - 3000 > lastcmd)
                {
                    lastcmd = LKCamelot.Server.tickcount.ElapsedMilliseconds;

                    string bug = player.Name + " : " + str.Substring(4);
                    Console.WriteLine(bug);
                    WriteBug(bug);
                    string achat = "버그신고완료." + "       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
                else
                {
                    string achat = "잠시만 기다려주세요.";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
            }
            else if (str2[0] == "@보관")
            {
                try { Convert.ToInt32(str2[1]); }
                catch { return; }
                player.BankTab = Convert.ToInt32(str2[1]);
                var invslots = (player.BankTab * 12) + 40;
                for (int x = 0; x < 12; x++)
                {
                    SendPacket(new DeleteEntrustSlot((byte)x).Compile());
                }
                foreach (var itm in player.BankContent.Where(xe => xe.InvSlot >= invslots && xe.InvSlot < (invslots + 12)))
                {
                    SendPacket(new AddItemToEntrust(itm).Compile());
                }
            }
            else if (str2[0] == "하이")
            {
                int mobile = Serial.NewMobile;
                World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)player.X, (short)player.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 196 }, 0).Compile()));
                var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
                tmp.tempser = mobile;
                World.TickQue.Add(tmp);
                str = player.Name + " : " + str + "" + "                             " + "     ";

                World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
            }
            else if (str == "@초기화")
            {
                if (player.GetFreeSlots() > 5)
                {
                    foreach (var it in player.Equipped2.Values)
                    {
                        it.Unequip(player, it.InvSlot);
                    }

                    var total = player.m_Str + player.m_Dex + player.m_Men + player.m_Vit + player.Extra;
                    player.Extra = (uint)total;
                    player.m_Str = 0;
                    player.m_Dex = 0;
                    player.m_Men = 0;
                    player.m_Vit = 0;
                    player.m_HPCur = player.HP;
                    player.m_MPCur = player.MP;
                    SendPacket(new UpdateCharStats(player).Compile());
                }
                else
                {
                    string achat = "여유 슬롯이 5개 이상이어야 합니다" + "             ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                }
            }
            else if (str == "마나물약 주세요")
            {
                if (player.m_Map == "Village1" && World.Dist2d(player.Loc, new Point2D(90, 173)) <= 2)
                {
                    AliasStage = 2;
                    string achat = "[알리아스]: 나는 마나물약을 가지고 있다 물약이 필요하느냐?" + "                  ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str == "네")
            {
                if (player.m_Map == "Village1" && World.Dist2d(player.Loc, new Point2D(90, 173)) <= 2
                    && AliasStage > 0)
                {
                    if (AliasStage == 1)
                    {
                        AliasStage = 0;
                        var newitem = new script.item.PromoteLifeDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        SendPacket(new AddItemToInventory2(newitem).Compile());
                    }
                    if (AliasStage == 2)
                    {
                        AliasStage = 0;
                        var newitem = new script.item.PromoteMagicDrug().Inventory(player);
                        World.NewItems.TryAdd(newitem.m_Serial, newitem);
                        SendPacket(new AddItemToInventory2(newitem).Compile());
                    }
                }

                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Aron").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && (player.Level >= 80 && player.Level <= 99) || (player.Level == (100 + (20 * player.Promo))))
                {
#if PROMOCAP12
                    if (player.Promo == 12)
                        return;
#else
                    if (player.Promo == 7)
                        return;
#endif

                    if (AronStage >= 1)
                    {
                        AronStage = 0;
                        if (player.Promo == 0)
                        {
                            player.XP = 0;
                            player.Level = 101;
                            player.Extra += 30;
                        }
                        else if (player.Promo >= 1)
                        {
                            player.XP = 0;
                            if (player.Promo == 1) player.Extra += 50;
                            if (player.Promo == 2) player.Extra += 80;
                            if (player.Promo == 3) player.Extra += 120;
                            if (player.Promo == 4) player.Extra += 180;
                            if (player.Promo == 5) player.Extra += 260;
                            if (player.Promo == 6) player.Extra += 360;
#if PROMOCAP12
                            if (player.Promo == 7) player.Extra += 480;
                            if (player.Promo == 8) player.Extra += 620;
                            if (player.Promo == 9) player.Extra += 780;
                            if (player.Promo == 10) player.Extra += 960;
                            if (player.Promo == 11) player.Extra += 1160;
#endif
                            if (player.Promo == 6 && player.GetFreeSlots() > 0)
                            {
                                script.item.Item prize = null;
                                if (player.Class == LKCamelot.library.Class.Knight)
                                    prize = new script.item.Excalibur().Inventory(player);
                                if (player.Class == LKCamelot.library.Class.Swordsman)
                                    prize = new script.item.DaeungDaegum().Inventory(player);
                                if (player.Class == LKCamelot.library.Class.Wizard)
                                    prize = new script.item.Kassandra().Inventory(player);
                                if (player.Class == LKCamelot.library.Class.Shaman)
                                    prize = new script.item.TaegkFan().Inventory(player);
                                if (player.Class == LKCamelot.library.Class.Beginner)
                                    prize = new script.item.GoldVest().Inventory(player);
                                try
                                {
                                    World.NewItems.TryAdd(prize.m_Serial, prize);
                                    SendPacket(new AddItemToInventory2(prize).Compile());
                                }
                                catch { Console.WriteLine("failed to add promo item"); }
                            }

                            player.Level++;

                        }
                        SendPacket(new SetLevelGold(player).Compile());
                        SendPacket(new UpdateCharStats(player).Compile());
                        str = player.Name + " : " + str + "" + "                             " + "     ";

                        World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                    }
                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str2[0] == "@시간")
            {
                string achat = DateTime.Now.ToLocalTime().ToString("현재시간 yyyy/MM/dd tt hh:mm" + "            ");
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
            }
            else if (str2[0] == "@운영자")
            {
                if (adminChk(player.Name) == "0")
                    return;


                {
                    player.Loc = new Point2D(10, 15);
                    player.Map = "GM존";

                }
            }
            else if (str2[0] == "@렙업")
            {
                if (adminChk(player.Name) == "0")
                    return;


                {
                    int iCurLevel = player.Level;
                    if (player.Promo == 0)
                    {
                        for (; iCurLevel < 99; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }
                    if (player.Promo == 1)
                    {
                        for (; iCurLevel < 120; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }
                    if (player.Promo == 2)
                    {
                        for (; iCurLevel < 140; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }
                    if (player.Promo == 3)
                    {
                        for (; iCurLevel < 160; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }
                    if (player.Promo == 4)
                    {
                        for (; iCurLevel < 180; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }
                    if (player.Promo == 5)
                    {
                        for (; iCurLevel < 200; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }
                    if (player.Promo == 6)
                    {
                        for (; iCurLevel < 220; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }
                    if (player.Promo == 7)
                    {
                        for (; iCurLevel < 231; ++iCurLevel)
                        {
                            player.XP = player.XPNext + 1;
                        }
                    }

                }
            }


            else if (str2[0] == "@돈증가")
            {
                if (adminChk(player.Name) == "0")
                    return;


                {
                    switch (str2[1])
                    {
                        case "백만":
                            player.Gold = player.Gold + 1000000;
                            break;
                        case "천만":
                            player.Gold = player.Gold + 10000000;
                            break;
                        case "억":
                            player.Gold = player.Gold + 100000000;
                            break;
                        case "십억":
                            player.Gold = player.Gold + 1000000000;
                            break;
                        case "이십억":
                            player.Bank = player.Bank + 2000000000;
                            string achat = "예금이 20억 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                            break;
                        case "백억":
                            player.Bank = player.Bank + 10000000000;
                            string achat2 = "예금이 100억 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                            break;
                        case "천억":
                            player.Bank = player.Bank + 100000000000;
                            string achat3 = "예금이 1000억 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat3.Count(), achat3).Compile());
                            break;
                        case "일조":
                            player.Bank = player.Bank + 1000000000000;
                            string achat4 = "예금이 1조 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat4.Count(), achat4).Compile());
                            break;
                        case "십조":
                            player.Bank = player.Bank + 10000000000000;
                            string achat5 = "예금이 10조 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat5.Count(), achat5).Compile());
                            break;
                        case "백조":
                            player.Bank = player.Bank + 100000000000000;
                            string achat6 = "예금이 100조 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat6.Count(), achat6).Compile());
                            break;
                        case "천조":
                            player.Bank = player.Bank + 1000000000000000;
                            string achat7 = "예금이 1000조 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat7.Count(), achat7).Compile());
                            break;
                        case "일경":
                            player.Bank = player.Bank + 10000000000000000;
                            string achat8 = "예금이 1경 증가하였습니다." + "                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat8.Count(), achat8).Compile());
                            break;

                    }
                }
            }
            else if (str2[0] == "@예금증가")
            {
                if (adminChk(player.Name) == "0")
                    return;


                {
                    switch (str2[1])
                    {
                        case "최대":
                            player.Bank = player.Bank + 9223372036854775807;
                            break;





                    }
                }
            }
            else if (str2[0] == "@돈선물")
            {
                if (adminChk(player.Name) == "0")
                    return;


                {
                    if (str2[1] != "")
                    {
                        switch (str2[2])
                        {

                            case "백만":
                                var plr = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                                plr.Gold = plr.Gold + 1000000;
                                break;
                            case "천만":
                                var plr2 = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                                plr2.Gold = plr2.Gold + 10000000;
                                break;
                            case "억":
                                var plr3 = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                                plr3.Gold = plr3.Gold + 100000000;
                                break;
                            case "십억":
                                var plr4 = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                                plr4.Gold = plr4.Gold + 1000000000;
                                break;
                            case "이십억":
                                var plr5 = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                                plr5.Gold = plr5.Gold + 2000000000;
                                break;
                        }
                    }
                }
            }
            else if (str2[0] == "@만렙")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    player.XP = 0;
                    player.m_Str = 0;
                    player.m_Dex = 0;
                    player.m_Men = 0;
                    player.m_Vit = 0;
                    player.m_HPCur = player.HP;
                    player.m_MPCur = player.MP;
                    player.m_Level = 231;
                    player.Extra = player.Extra + 19300;
                    player.client.SendPacket(new UpdateCharStats(player).Compile());
                    player.client.SendPacket(new SetLevelGold(player).Compile());
                }
            }
            else if (str2[0] == "@자동마나")
            {

                if (!player.AutoMana)
                {
                    player.AutoMana = true;
                    string AutoMana = "자동마나적용" + "       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)AutoMana.Count(), AutoMana).Compile());
                }
                else
                {
                    player.AutoMana = false;
                    string AutoMana2 = "자동마나비적용" + "       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)AutoMana2.Count(), AutoMana2).Compile());
                }
                if (str2.Count() > 1)
                {
                    try
                    {
                        var temp = Convert.ToDouble(str2[1]);
                        if (temp > 1) temp = 1;
                        if (temp < 0) temp = 0;
                        player.AutoManaP = temp;
                    }
                    catch { }

                }

            }
            else if (str2[0] == "@정보")
            {
                if (player.Level >= 0 && player.Level <= 230)
                {
                    string hpmp = string.Format("생명:{0}, 마나:{1}, 레벨:{2}" + "                    ", player.HP, player.MP, player.Level);
                    string pro = string.Format("{0} : 승급" + "        ", player.Promo);
                    string stats = string.Format("힘:{0}, 지력:{1}, 숙련:{2}, 생명:{3}" + "                       ",
                        player.GetStat("str"), player.GetStat("men"), player.GetStat("dex"), player.GetStat("vit"));
                    string ats = string.Format("방어:{0}, 파괴:{1}, 적중:{2}, 여유스텟:{3}" + "                        ",
                        player.AC, player.Dam, player.Hit, player.Extra);
                    string golds = string.Format("Gold:{0}, Diamonds:{1}",
                         player.Gold, 0);
                    string tun = string.Format("환:{0}" + "            ", player.m_Tun, 0);
                    string As = string.Format("공속:{0}" + "            ", player.AttackSpeed, 0);
                    string Cs = string.Format("캐속:{0}" + "            ", player.AttackSpeed, 0);
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)hpmp.Count(), hpmp).Compile());
                    SendPacket(new UpdateChatBox(0xff, 10, 0, (short)pro.Count(), pro).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)stats.Count(), stats).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)ats.Count(), ats).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)golds.Count(), golds).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)tun.Count(), tun).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)As.Count(), As).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Cs.Count(), Cs).Compile());

                }
                if (player.Level >= 231 && player.Promo == 7)
                {
                    string hpmp = string.Format("생명:{0}, 마나:{1}, 레벨:{2}" + "                    ", player.HP, player.MP, player.Level);
                    string pro = string.Format("{0}:승급" + "        ", player.Promo);
                    string stats = string.Format("힘:{0}, 지력:{1}, 숙련:{2}, 생명:{3}" + "                 ",
                        player.GetStat("str"), player.GetStat("men"), player.GetStat("dex"), player.GetStat("vit"));
                    string ats = string.Format("방어:{0}, 파괴:{1}, 적중:{2}, 여유스텟:{3}" + "                        ",
                        player.AC, player.Dam, player.Hit, player.Extra);
                    string golds = string.Format("Gold:{0}, Diamonds:{1}",
                         player.Gold, 0);
                    string Turn = string.Format("턴횟수:{0}" + "            ", player.m_Str + player.m_Men + player.m_Dex + player.m_Vit + player.m_Extra - 19300);
                    string tun = string.Format("환:{0}" + "            ",
                         player.m_Tun, 0);
                    string As = string.Format("공속:{0}" + "            ", player.AttackSpeed, 0);
                    string Cs = string.Format("캐속:{0}" + "            ", player.AttackSpeed, 0);
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)hpmp.Count(), hpmp).Compile());
                    SendPacket(new UpdateChatBox(0xff, 10, 0, (short)pro.Count(), pro).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)stats.Count(), stats).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)ats.Count(), ats).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)golds.Count(), golds).Compile());
                    SendPacket(new UpdateChatBox(0xff, 30, 0, (short)Turn.Count(), Turn).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)tun.Count(), tun).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)As.Count(), As).Compile());
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Cs.Count(), Cs).Compile());

                }
            }
            else if (str2[0] == "@pkstats")
            {
                string hpmp = string.Format("TempPKCount:{0}, RedTime:{1}",
                    player.pklastpk.Count, ((player.pklastpk.Count * player.pkRedDelay) / 1000) / 60 + "m");
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)hpmp.Count(), hpmp).Compile());
            }
            else if (str2[0] == "@속성")
            {
                try
                {
                    var extras = Convert.ToUInt16(str2[2]);
                    var stat = str2[1];
                    if (player.Extra >= extras)
                    {
                        if (stat == "힘") player.AddStat(ref player.m_Str, extras);
                        if (stat == "지력") player.AddStat(ref player.m_Men, extras);
                        if (stat == "숙련") player.AddStat(ref player.m_Dex, extras);
                        if (stat == "생명") player.AddStat(ref player.m_Vit, extras);
                        SendPacket(new UpdateCharStats(player).Compile());
                    }
                }
                catch
                {
                    string rankr = "속성값이 잘못되었습니다. ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)rankr.Count(), rankr).Compile());
                }
            }
            else if (str2[0] == "@저장")
            {
                if (adminChk(player.Name) == "0")
                    return;


                {

                    World.SaveAll();



                    string chat = "[알림]: 여기는 최후개발서버입니다." + "                      ";
                    World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)chat.Count(), chat).Compile()));
                    chat = "[알림]: 게임존에서 최후의승자를 불러주세요!" + "                           ";
                    World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)chat.Count(), chat).Compile()));
                    chat = "[알림]: 서버저장완료!" + "                           ";
                    World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)chat.Count(), chat).Compile()));
                }
            }
            else if (str2[0] == "@자동생명")
            {
                if (!player.AutoHP)
                {
                    player.AutoHP = true;
                    string AutoHP = "자동생명적용" + "       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)AutoHP.Count(), AutoHP).Compile());
                }
                else
                {
                    player.AutoHP = false;
                    string AutoHP2 = "자동생명비적용" + "       ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)AutoHP2.Count(), AutoHP2).Compile());
                }
                if (str2.Count() > 1)
                {
                    try
                    {
                        var temp = Convert.ToDouble(str2[1]);
                        if (temp > 1) temp = 1;
                        if (temp < 0) temp = 0;
                        player.AutoHPP = temp;
                    }
                    catch { }
                }
            }

            else if (str2[0] == "@텔")
            {
                if (adminChk(player.Name) == "0")
                    return;
                if (str2.Count() == 2)
                {
                    var teleon = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1]).FirstOrDefault().Value;
                    if (teleon != null)
                    {
                        player.Loc = new Point2D(teleon.Loc.X, teleon.Loc.Y);
                        player.Map = teleon.Map;
                    }

                }
                else if (str2.Count() == 4)
                {
                    player.Loc = new Point2D(Convert.ToInt16(str2[2]), Convert.ToInt16(str2[3]));
                    player.Map = str2[1];
                }
            }
            else if (str2[0] == "@마법")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    try
                    {
                        string activatorstring = "LKCamelot.script.spells.";
                        var tempspell = Activator.CreateInstance(Type.GetType(activatorstring + str2[1]));
                        (tempspell as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                        (tempspell as script.spells.Spell).SLevel2 = 0;
                        (tempspell as script.spells.Spell).Level = 12;
                        player.m_MagicLearned.Add((tempspell as script.spells.Spell));
                        player.client.SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
                    }
                    catch { return; }
                }
            }
            else if (str2[0] == "@기사마법")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    try
                    {



                        var tempspell = new script.spells.Twister();


                        {
                            (tempspell as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell as script.spells.Spell).SLevel2 = 1;
                            (tempspell as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
                        }
                        var tempspell2 = new script.spells.FrameStrike();
                        {
                            (tempspell2 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell2 as script.spells.Spell).SLevel2 = 1;
                            (tempspell2 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell2 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell2 as script.spells.Spell)).Compile());
                        }

                        var tempspell3 = new script.spells.MagicArmor();
                        {
                            (tempspell3 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell3 as script.spells.Spell).SLevel2 = 1;
                            (tempspell3 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell3 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell3 as script.spells.Spell)).Compile());
                        }
                        var tempspell4 = new script.spells.MentalSword();
                        {
                            (tempspell4 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell4 as script.spells.Spell).SLevel2 = 1;
                            (tempspell4 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell4 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell4 as script.spells.Spell)).Compile());
                        }
                    }
                    catch { return; }
                }
            }
            else if (str2[0] == "@검객마법")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    try
                    {



                        var tempspell = new script.spells.Demon();


                        {
                            (tempspell as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell as script.spells.Spell).SLevel2 = 1;
                            (tempspell as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
                        }
                        var tempspell2 = new script.spells.DemonDeath();
                        {
                            (tempspell2 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell2 as script.spells.Spell).SLevel2 = 1;
                            (tempspell2 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell2 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell2 as script.spells.Spell)).Compile());
                        }

                        var tempspell3 = new script.spells.Execution();
                        {
                            (tempspell3 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell3 as script.spells.Spell).SLevel2 = 1;
                            (tempspell3 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell3 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell3 as script.spells.Spell)).Compile());
                        }
                        var tempspell4 = new script.spells.Execution2();
                        {
                            (tempspell4 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell4 as script.spells.Spell).SLevel2 = 1;
                            (tempspell4 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell4 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell4 as script.spells.Spell)).Compile());
                        }
                        var tempspell5 = new script.spells.Execution3();
                        {
                            (tempspell5 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell5 as script.spells.Spell).SLevel2 = 1;
                            (tempspell5 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell5 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell5 as script.spells.Spell)).Compile());
                        }
                        var tempspell6 = new script.spells.FlyingSword();
                        {
                            (tempspell6 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell6 as script.spells.Spell).SLevel2 = 1;
                            (tempspell6 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell6 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell6 as script.spells.Spell)).Compile());
                        }
                        var tempspell7 = new script.spells.GuardianSword();
                        {
                            (tempspell7 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell7 as script.spells.Spell).SLevel2 = 1;
                            (tempspell7 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell7 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell7 as script.spells.Spell)).Compile());
                        }
                        var tempspell8 = new script.spells.Triple();
                        {
                            (tempspell8 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell8 as script.spells.Spell).SLevel2 = 1;
                            (tempspell8 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell8 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell8 as script.spells.Spell)).Compile());
                        }
                    }
                    catch { return; }
                }
            }
            else if (str2[0] == "@위저드마법")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    try
                    {



                        var tempspell = new script.spells.BigBang();


                        {
                            (tempspell as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell as script.spells.Spell).SLevel2 = 1;
                            (tempspell as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
                        }
                        var tempspell2 = new script.spells.Butterfly();
                        {
                            (tempspell2 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell2 as script.spells.Spell).SLevel2 = 1;
                            (tempspell2 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell2 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell2 as script.spells.Spell)).Compile());
                        }

                        var tempspell3 = new script.spells.CurveShock();
                        {
                            (tempspell3 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell3 as script.spells.Spell).SLevel2 = 1;
                            (tempspell3 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell3 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell3 as script.spells.Spell)).Compile());
                        }
                        var tempspell4 = new script.spells.DeadlyBoom();
                        {
                            (tempspell4 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell4 as script.spells.Spell).SLevel2 = 1;
                            (tempspell4 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell4 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell4 as script.spells.Spell)).Compile());
                        }
                        var tempspell5 = new script.spells.DragonOfFire();
                        {
                            (tempspell5 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell5 as script.spells.Spell).SLevel2 = 1;
                            (tempspell5 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell5 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell5 as script.spells.Spell)).Compile());
                        }
                        var tempspell6 = new script.spells.FireHawk();
                        {
                            (tempspell6 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell6 as script.spells.Spell).SLevel2 = 1;
                            (tempspell6 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell6 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell6 as script.spells.Spell)).Compile());
                        }
                        var tempspell7 = new script.spells.Freezing();
                        {
                            (tempspell7 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell7 as script.spells.Spell).SLevel2 = 1;
                            (tempspell7 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell7 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell7 as script.spells.Spell)).Compile());
                        }
                        var tempspell8 = new script.spells.GrandBigBang();
                        {
                            (tempspell8 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell8 as script.spells.Spell).SLevel2 = 1;
                            (tempspell8 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell8 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell8 as script.spells.Spell)).Compile());
                        }
                        var tempspell9 = new script.spells.MagicShield();
                        {
                            (tempspell9 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell9 as script.spells.Spell).SLevel2 = 1;
                            (tempspell9 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell9 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell9 as script.spells.Spell)).Compile());
                        }
                        var tempspell10 = new script.spells.RainbowArmor();
                        {
                            (tempspell10 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell10 as script.spells.Spell).SLevel2 = 1;
                            (tempspell10 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell10 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell10 as script.spells.Spell)).Compile());
                        }
                        var tempspell11 = new script.spells.ThunderStorm();
                        {
                            (tempspell11 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell11 as script.spells.Spell).SLevel2 = 1;
                            (tempspell11 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell11 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell11 as script.spells.Spell)).Compile());
                        }
                        var tempspell12 = new script.spells.UltraBigBang();
                        {
                            (tempspell12 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell12 as script.spells.Spell).SLevel2 = 1;
                            (tempspell12 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell12 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell12 as script.spells.Spell)).Compile());
                        }
                    }
                    catch { return; }
                }
            }
            else if (str2[0] == "@샤먼마법")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    try
                    {



                        var tempspell = new script.spells.Assassin();


                        {
                            (tempspell as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell as script.spells.Spell).SLevel2 = 1;
                            (tempspell as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
                        }
                        var tempspell2 = new script.spells.AssassinSpecial();
                        {
                            (tempspell2 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell2 as script.spells.Spell).SLevel2 = 1;
                            (tempspell2 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell2 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell2 as script.spells.Spell)).Compile());
                        }

                        var tempspell3 = new script.spells.BlackHand();
                        {
                            (tempspell3 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell3 as script.spells.Spell).SLevel2 = 1;
                            (tempspell3 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell3 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell3 as script.spells.Spell)).Compile());
                        }
                        var tempspell4 = new script.spells.FireProtector();
                        {
                            (tempspell4 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell4 as script.spells.Spell).SLevel2 = 1;
                            (tempspell4 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell4 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell4 as script.spells.Spell)).Compile());
                        }
                        var tempspell5 = new script.spells.MagmaHand();
                        {
                            (tempspell5 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell5 as script.spells.Spell).SLevel2 = 1;
                            (tempspell5 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell5 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell5 as script.spells.Spell)).Compile());
                        }
                        var tempspell6 = new script.spells.Revelation();
                        {
                            (tempspell6 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell6 as script.spells.Spell).SLevel2 = 1;
                            (tempspell6 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell6 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell6 as script.spells.Spell)).Compile());
                        }
                        var tempspell7 = new script.spells.StoneCurse();
                        {
                            (tempspell7 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell7 as script.spells.Spell).SLevel2 = 1;
                            (tempspell7 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell7 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell7 as script.spells.Spell)).Compile());
                        }
                        var tempspell8 = new script.spells.TeagueShield();
                        {
                            (tempspell8 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell8 as script.spells.Spell).SLevel2 = 1;
                            (tempspell8 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell8 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell8 as script.spells.Spell)).Compile());
                        }
                        var tempspell9 = new script.spells.Wakonda();
                        {
                            (tempspell9 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell9 as script.spells.Spell).SLevel2 = 1;
                            (tempspell9 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell9 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell9 as script.spells.Spell)).Compile());
                        }
                        var tempspell10 = new script.spells.WindySpirit();
                        {
                            (tempspell10 as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                            (tempspell10 as script.spells.Spell).SLevel2 = 1;
                            (tempspell10 as script.spells.Spell).Level = 1;
                            player.m_MagicLearned.Add((tempspell10 as script.spells.Spell));
                            player.client.SendPacket(new CreateSlotMagic2((tempspell10 as script.spells.Spell)).Compile());
                        }
                    }
                    catch { return; }
                }
            }
            else if (str2[0] == "@스킬")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    if (str2[1] != "")
                    {
                        try
                        {
                            var plr2 = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;




                            string activatorstring = "LKCamelot.script.spells.";
                            var tempspell = Activator.CreateInstance(Type.GetType(activatorstring + str2[2]));
                            (tempspell as script.spells.Spell).Slot = plr2.GetFreeSpellSlot();
                            (tempspell as script.spells.Spell).SLevel2 = 100;
                            (tempspell as script.spells.Spell).Level = 12;
                            plr2.m_MagicLearned.Add((tempspell as script.spells.Spell));
                            SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
                        }
                        catch { return; }
                    }
                }

            }
            else if (str2[0] == "@은신")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    if (player.Transparancy == 0)
                    {
                        player.Transparancy = 100;
                        World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
                    }
                    else player.Transparancy = 0;
                }
            }
            else if (str2[0] == "@취소")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    if (player.Transparancy == 100)
                    {
                        player.Transparancy = 0;
                        World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
                    }
                    else player.Transparancy = 100;
                }
            }
            else if (str2[0] == "@템생성")
            {
                if (adminChk(player.Name) == "0")
                    return;
                script.item.Item temp;
                try
                {
                    temp = (script.item.Item)Activator.CreateInstance(Type.GetType("LKCamelot.script.item." + str2[1]));

                }
                catch { return; }

                var newitem = temp.Inventory(player);
                World.NewItems.TryAdd(newitem.m_Serial, newitem);
                SendPacket(new AddItemToInventory2(newitem).Compile());

            }
            else if (str2[0] == "@생성2")
            {
                if (adminChk(player.Name) == "0")
                    return;
                script.item.Item temp;
                try
                {
                    temp = (script.item.Item)Activator.CreateInstance(Type.GetType("LKCamelot.script.item." + str2[1]));
                    (temp as script.item.Item).Stage = 50;
                }
                catch { return; }

                var newitem = temp.Inventory(player);
                World.NewItems.TryAdd(newitem.m_Serial, newitem);
                SendPacket(new AddItemToInventory2(newitem).Compile());

            }
            else if (str2[0] == "@생성3")
            {
                if (adminChk(player.Name) == "0")
                    return;
                script.item.Item temp;
                try
                {
                    temp = (script.item.Item)Activator.CreateInstance(Type.GetType("LKCamelot.script.item." + str2[1]));
                    (temp as script.item.Item).Quantity = 10000;
                }
                catch { return; }

                var newitem = temp.Inventory(player);
                World.NewItems.TryAdd(newitem.m_Serial, newitem);
                SendPacket(new AddItemToInventory2(newitem).Compile());

            }
            else if (str2[0] == "@추적")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    if (str2.Count() == 2)
                    {
                        var teleon = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1]).FirstOrDefault().Value;
                        if (player != null)
                        {
                            string name = string.Format("<케릭명:{0}" + "        ", teleon.Name + ">");
                            string hpmp = string.Format("<맵위치:{0}" + "        ", teleon.m_Map + ">");
                            string zone = string.Format("<X좌표:{0}" + "        ", teleon.X + ">");
                            string zone2 = string.Format("<Y좌표:{0}" + "        ", teleon.Y + ">");
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)hpmp.Count(), name).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)hpmp.Count(), hpmp).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)zone.Count(), zone).Compile());
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)zone2.Count(), zone2).Compile());
                        }

                    }
                }
            }
            else if (str2[0] == "@소환")
            {
                if (adminChk(player.Name) == "0")
                    return;
                if (str2.Count() == 2)
                {
                    var teleon = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1]).FirstOrDefault().Value;
                    if (player != null)
                    {
                        teleon.Loc = new Point2D(player.Loc.X, player.Loc.Y);
                        teleon.Map = player.Map;
                    }

                }
                else if (str2.Count() == 4)
                {
                    player.Loc = new Point2D(Convert.ToInt16(str2[2]), Convert.ToInt16(str2[3]));
                    player.Map = str2[1];
                }
            }
            else if (str2[0] == "@맵")
            {
                string hpmp = string.Format("맵위치:{0}" + "         ", player.m_Map);
                string zone = string.Format("X좌표:{0}" + "         ", player.X);
                string zone2 = string.Format("Y좌표:{0}" + "         ", player.Y);
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)hpmp.Count(), hpmp).Compile());
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)zone.Count(), zone).Compile());
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)zone2.Count(), zone2).Compile());
            }
            else if (str2[0] == "@킥")
            {
                if (adminChk(player.Name) == "0")
                    return;
                {
                    if (str2[1] != "")
                    {


                        var plr = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                        plr.loggedIn = false;
                        World.w_server.Disconnect(plr.client.connection);
                    }
                }
            }
            else if (str2[0] == "@셀프킥")
            {
                var plr = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                {
                    if (str2[1] != "" && plr.Bday == str2[2])
                    {


                        
                        plr.loggedIn = false;
                        World.w_server.Disconnect(plr.client.connection);
                    }
                }
            }
            /*
                        else if (str2[0] == "@로또")
                        {
                            var PlyGold = Convert.ToUInt16(str2[2]);
                            if (player.Gold >= PlyGold)

                                try
                                {
                                    if (Convert.ToUInt64(str2[1]) < 0)
                                        return;
                                }
                                catch { return; }

                            var Number = Util.Random(1, 9);


                            if (Number == str[1])
                            {
                                string text = "당첨" + "                              ";
                                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());


                            }
                            else if (Number != str[1])
                            {
                                string text = "꽝." + "                              ";
                                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                            }

                            string tra = string.Format("추첨번호 : {0}" + "                          ", Number);
                            SendPacket(new UpdateChatBox(0xff, 10, 0, (short)tra.Count(), tra).Compile());

                            string tra2 = string.Format("선택번호 : {0}" + "                          ", str2[1]);
                            SendPacket(new UpdateChatBox(0xff, 10, 0, (short)tra2.Count(), tra2).Compile());
                        }*/
            else if (str2[0] == "@송금")
            {
                if (str2.Count() <= 1 || player.m_Map != "Loen")
                    return;
                try
                {
                    if (Convert.ToUInt64(str2[2]) < 0)
                        return;
                }
                catch { return; }

                var tradep = handler.add.Where(xe => xe.Key != null && xe.Value != null && xe.Value.Name == str2[1].ToUpper() && xe.Value.Map == "Loen").FirstOrDefault().Value;
                if (tradep != null && tradep != null && tradep.Name != player.Name && player.Bank >= Convert.ToUInt64(str2[2]))
                {
                    player.Bank -= Convert.ToUInt64(str2[2]);
                    tradep.Bank += Convert.ToUInt64(str2[2]);
                    string ply = string.Format("{0}님 예금에서 {1}원이 {2}님께 전달되었습니다" + "                  ", player.Name, str2[2], tradep.Name);
                    string tra = string.Format("{0}님 에게서 {1}원이 입금되었습니다" + "                          ", player.Name, str2[2]);


                    tradep.client.SendPacket(new UpdateChatBox(0xff, 10, 0, (short)tra.Count(), tra).Compile());
                    SendPacket(new UpdateChatBox(0xff, 10, 0, (short)ply.Count(), ply).Compile());
                }
                else
                {
                    string text = "로엔외 다른맵이거나 잔액을 확인하세요." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());


                }
            }
            else if (str2[0] == "@입금")
            {
                if (str2.Count() <= 2 || player.m_Map != "Loen")
                    return;
                try
                {
                    if (Convert.ToUInt64(str2[1]) < 0)
                        return;
                }
                catch { return; }

                //var tradep = handler.add.Where(xe => xe.Key != null && xe.Value != null && xe.Value.Name == str2[1].ToUpper() && xe.Value.Map == "Loen").FirstOrDefault().Value;
                if (player.Gold >= Convert.ToUInt64(str2[1]))
                {
                    player.Gold -= Convert.ToUInt64(str2[1]);
                    player.Bank += Convert.ToUInt64(str2[1]);
                    //tradep.Gold += Convert.ToUInt64(str2[2]);
                }
                else
                {
                    string text = "로엔외 다른맵이거나 잔액을 확인하세요." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());


                }
            }
            else if (str2[0] == "@출금")
            {
                if (str2.Count() <= 2 || player.m_Map != "Loen")
                    return;
                try
                {
                    if (Convert.ToUInt64(str2[1]) < 0)
                        return;
                }
                catch { return; }

                //var tradep = handler.add.Where(xe => xe.Key != null && xe.Value != null && xe.Value.Name == str2[1].ToUpper() && xe.Value.Map == "Loen").FirstOrDefault().Value;
                if (player.Bank >= Convert.ToUInt64(str2[1]))
                {
                    player.Bank -= Convert.ToUInt64(str2[1]);
                    player.Gold += Convert.ToUInt64(str2[1]);
                    //tradep.Gold += Convert.ToUInt64(str2[2]);
                }
                else
                {
                    string text = "로엔외 다른맵이거나 잔액을 확인하세요." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());


                }
            }
            else if (str2[0] == "@잔액조회")
            {
                string Point = string.Format("출금가능 잔액:{0} 원" + "                                    ", player.Bank, 0);
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)Point.Count(), Point).Compile());
                if (player.Bank >= 1000000000 && player.Bank < 10000000000)
                {
                    string text = "10억이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                if (player.Bank >= 10000000000 && player.Bank < 100000000000)
                {
                    string text = "100억이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                if (player.Bank >= 100000000000 && player.Bank < 1000000000000)
                {
                    string text = "1000억이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                if (player.Bank >= 1000000000000 && player.Bank < 10000000000000)
                {
                    string text = "1조이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                if (player.Bank >= 10000000000000 && player.Bank < 100000000000000)
                {
                    string text = "1조억이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                if (player.Bank >= 100000000000000 && player.Bank < 1000000000000000)
                {
                    string text = "100조이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                if (player.Bank >= 1000000000000000 && player.Bank < 10000000000000000)
                {
                    string text = "1000조이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                if (player.Bank >= 10000000000000000 && player.Bank < 100000000000000000)
                {
                    string text = "1경이상보유." + "                              ";
                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text.Count(), text).Compile());
                }
                else
                {
                    return;
                }

            }
            else if (str2[0] == "@cast")
            {
                /*   if (str2.Count() <= 7)
                       return;

                   int par;
                   for (int x = 1; x < 9; x++)
                   {
                       if (int.TryParse(str2[x], out par) == false)
                           return;
                   }

                   World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, Convert.ToInt16(str2[1]), Convert.ToInt16(str2[2]), new script.spells.SpellSequence(Convert.ToInt32(str2[3]), Convert.ToInt32(str2[4]),
                       Convert.ToInt32(str2[5]), Convert.ToInt32(str2[6]), Convert.ToInt32(str2[7]), Convert.ToInt32(str2[8]), Convert.ToInt32(str2[9]))).Compile()));
               */
            }
            else if (str2[0] == "@서열")
            {
                if (LKCamelot.Server.tickcount.ElapsedMilliseconds - 2000 > ChatTimeout)
                {
                    ChatTimeout = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                    string rankr = "";
                    var keys = handler.add.Values.Where(xe => xe != null && xe.Name.ToUpper() != "GM승자" && xe.loggedIn && xe.Name.ToUpper() != "GM최후").ToList();//&& xe.loggedIn
                    var kl = keys.OrderByDescending(xe => xe.Level).ToList();
                    if (kl.Count > 25)
                        kl.RemoveRange(24, kl.Count - 24 - 1);

                    foreach (var rnk in kl)
                    {

                        rankr += rnk.Name + ", " + "              ";
                        if (rankr.Count() / 35 >= 1)
                        {

                            rankr = rankr.Substring(0, rankr.Count() - 2);
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)rankr.Count(), rankr).Compile());
                            rankr = "";
                        }

                    }
                    if (rankr.Count() > 0)
                    {
                        rankr = rankr.Substring(0, rankr.Count() - 2);
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)rankr.Count(), rankr).Compile());
                        rankr = "";
                    }
                    var online = handler.add.Values.Where(xe => xe != null && xe.Name.ToUpper() != "GM승자" && xe.Name.ToUpper() != "GM최후" && xe.apistate == 0).ToList();
                    rankr = string.Format("현재 생성된 유저는 {0}", online.Count) + "명 입니다" + "                                ";
                    SendPacket(new UpdateChatBox(0x08, 0x02, 0, (short)rankr.Count(), rankr).Compile());
                    var online2 = handler.add.Values.Where(xe => xe != null && xe.Name.ToUpper() != "GM승자" && xe.Name.ToUpper() != "GM최후" && xe.apistate == 0 && xe.loggedIn).ToList();
                    rankr = string.Format("현재 접속자수는 {0}", online2.Count) + "명 입니다" + "                                ";
                    SendPacket(new UpdateChatBox(0x08, 0x02, 0, (short)rankr.Count(), rankr).Compile());

                }


            }
            else if (str2[0] == "@autoloot")
            {
                var strloot = "";
                if (player.AutoLoot)
                {
                    player.AutoLoot = false;
                    strloot = "Autoloot disabled.";
                }
                else
                {
                    player.AutoLoot = true;
                    strloot = "Autoloot enabled.";
                }
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)strloot.Count(), strloot).Compile());
            }
            else if (str2[0] == "@자동")
            {
                var strloot = "";
                if (player.AutoHit)
                {
                    player.AutoHit = false;
                    strloot = "자동공격종료." + "          ";
                }
                else
                {
                    player.AutoHit = true;
                    strloot = "자동공격시작." + "           ";
                }
                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)strloot.Count(), strloot).Compile());
            }

            else if (str2[0] == "전직")
            {
                var AronNpc = World.NewNpcs.Where(xe => xe.Value.Name == "Aron").FirstOrDefault();
                if (AronNpc.Value != null && World.Dist2d(player.X, player.Y, AronNpc.Value.X, AronNpc.Value.Y) < 7
                    && player.Level >= 5 && player.Promo == 0)
                {
                    switch (str2[1])
                    {
                        case "기사":
                            player.Class = LKCamelot.library.Class.Knight;
                            string text2 = "아론: 너는 이제 기사가 되었다" + "                       ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text2.Count(), text2).Compile());
                            str = player.Name + " : " + str + "" + "                             " + "     ";
                            World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                            break;
                        case "검객":
                            player.Class = LKCamelot.library.Class.Swordsman;
                            str = player.Name + " : " + str + "" + "                             " + "     ";
                            string text3 = "아론: 너는 이제 검객이 되었다" + "                       ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text3.Count(), text3).Compile());
                            World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                            break;
                        case "위자드":
                            player.Class = LKCamelot.library.Class.Wizard;
                            str = player.Name + " : " + str + "" + "                             " + "     ";
                            string text4 = "아론: 너는 이제 위자드가 되었다" + "                       ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text4.Count(), text4).Compile());
                            World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                            break;
                        case "샤먼":
                            player.Class = LKCamelot.library.Class.Shaman;
                            str = player.Name + " : " + str + "" + "                             " + "     ";
                            string text5 = "아론: 너는 이제 샤먼이 되었다" + "                       ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)text5.Count(), text5).Compile());
                            World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                            break;
                    }
                }
                else
                {
                    str = player.Name + " : " + str + "" + "                             " + "     ";

                    World.SendToAllRange(new QueDele(player.Map, new UpdateChatBox(0xff, 0xff, 0, (short)str.Count(), str).Compile()), player, 10);
                }
            }
            else if (str[0] == '/')
            {
                if (str2.Count() <= 1)
                    return;
                str = str + "                      ";
                var str3 = player.Name + "=> " + str.Substring(str2[0].Count());

                var parsename = str2[0].ToString().Substring(1).ToUpper();
                var whisp = handler.add.Where(xe => xe.Value != null && xe.Value.Name == parsename).FirstOrDefault();
                if (whisp.Key != null && whisp.Value != null && whisp.Value.loggedIn)
                {
                    var str1 = whisp.Value.Name + "<= " + str.Substring(str2[0].Count());
                    SendPacket(new UpdateChatBox(40, 0x70, 1, (short)str1.Count(), str1).Compile());
                    whisp.Value.client.SendPacket(new UpdateChatBox(0xff, 0x70, 1, (short)str3.Count(), str3).Compile());
                }
                else
                {
                    string achat = parsename + "님이 접속중이 아닙니다." + "                 ";
                    SendPacket(new UpdateChatBox(60, 0x70, 1, (short)achat.Count(), achat).Compile());
                }
            }
            else
            {
                str = player.Name + " : " + str + "  ";

                World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, str).Compile()), player, 10);
            }
        }
        public string adminChk(string str)
        {
            String pppfile = "";
            using (StreamReader srsrsr = new StreamReader("worldAdmin.txt", Encoding.GetEncoding("euc-kr")))
            {
                while (!srsrsr.EndOfStream)
                {
                    pppfile = pppfile + srsrsr.ReadLine();
                }
            }
            if (pppfile.IndexOf(str) > 0)
            {
                pppfile = "1";
            }
            else
            {
                pppfile = "0";
            }
            return pppfile;
        }
        public void GMKick(string[] str2)
        {
            if (str2[1] != "")
            {
                var plr = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1].ToUpper()).FirstOrDefault().Value;
                plr.loggedIn = false;
                World.w_server.Disconnect(plr.client.connection);
            }
        }

        public void GMCreateItem(string[] str2)
        {
            if (player.Name != "GM최후" && player.Name != "GM승자")
                return;

            script.item.Item temp;
            try
            {
                temp = (script.item.Item)Activator.CreateInstance(Type.GetType("LKCamelot.script.item." + str2[1]));
            }
            catch { return; }

            var newitem = temp.Inventory(player);
            World.NewItems.TryAdd(newitem.m_Serial, newitem);
            SendPacket(new AddItemToInventory2(newitem).Compile());
        }

        public void GMInvis(string[] str2)
        {
            if (player.Transparancy == 0)
            {
                player.Transparancy = 100;
                World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
            }
            else player.Transparancy = 0;
        }

        public void GMItemArray(string[] str2)
        {

            if (player.Transparancy == 0)
            {
                player.Transparancy = 100;
                World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
            }
            else player.Transparancy = 0;
        }

        public void GMTapPlayer(string[] str2)
        {
            if (str2[1] != "")
            {
                LKCamelot.model.Modules.NSA.Tap(str2[1]);
            }
        }

        public void GMLearn(string[] str2)
        {
            try
            {
                string activatorstring = "LKCamelot.script.spells.";
                var tempspell = Activator.CreateInstance(Type.GetType(activatorstring + str2[1]));
                (tempspell as script.spells.Spell).Slot = player.GetFreeSpellSlot();
                (tempspell as script.spells.Spell).SLevel2 = 99;
                (tempspell as script.spells.Spell).Level = 12;
                player.m_MagicLearned.Add((tempspell as script.spells.Spell));
                SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
            }
            catch { return; }
        }

        public void GMTele(string[] str2)
        {
            if (str2.Count() == 2)
            {
                var teleon = handler.add.Where(xe => xe.Value != null && xe.Value.Name == str2[1]).FirstOrDefault().Value;
                if (teleon != null)
                {
                    player.Loc = new Point2D(teleon.Loc.X, teleon.Loc.Y);
                    player.Map = teleon.Map;
                }

            }
            else if (str2.Count() == 4)
            {
                player.Loc = new Point2D(Convert.ToInt16(str2[2]), Convert.ToInt16(str2[3]));
                player.Map = str2[1];
            }
        }
    }
}
