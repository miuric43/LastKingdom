using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.script.spells;
namespace LKCamelot.model
{
    public class CastHandler
    {
        io.IOClient client;
        PlayerHandler handler;
        static int tempMusic = 2;
        public CastHandler(io.IOClient client, PlayerHandler handler)
        {
            this.client = client;
            this.handler = handler;
        }

        public void CreateMagicEffect(Point2D target, string map, byte sprite, int time = 1500)
        {
            int mobile = Serial.NewMobile;
            World.SendToAll(new QueDele(map, new CreateMagicEffect(mobile, 1, (short)target.X, (short)target.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, sprite }, 0).Compile()));
            var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + time, map, new DeleteObject(mobile).Compile());
            tmp.tempser = mobile;
            World.TickQue.Add(tmp);
        }

        public void TakeDamage(Player caster, Player target, script.spells.Spell spell)
        {
            if (caster.Map != "대전장")
            {
                float h = ((float)caster.Hit / ((float)caster.Hit + (float)target.Hit)) * 200;

                if (h >= 100 || new Random().Next(0, 100) < (int)h)
                {
                    int take = spell.DamBase + (spell.DamPl * spell.Level + (spell.DamPl * spell.SLevel2));// +(spell.DamPl * spell.SLevel2);
                    if (spell.ManaCostPl != 0)
                    {
                        take += (caster.GetStat("men") / spell.menCoff);
                        take += (caster.GetStat("str") / spell.strCoff);
                        take += (caster.GetStat("dex") / spell.dexCoff);
                        take += (caster.GetStat("vit") / spell.vitCoff);
                    }
                    if (spell is script.spells.DemonDeath)
                        take = Convert.ToInt32(caster.HP * 0.5) + caster.GetStat("dex");

                    if (target.Color == 0)
                        caster.pkpinkktime = Server.tickcount.ElapsedMilliseconds;

                    target.HPCur -= (take - target.AC);
                    if (target.Map == "Rest" && target.Color != 1)
                    {
                        if (caster.Map != "대전장" && caster.Map != "큰대전장")
                        {
                            caster.pklastpk.Add(Server.tickcount.ElapsedMilliseconds);
                            caster.pklastred = Server.tickcount.ElapsedMilliseconds;
                            string text2 = caster.Name + " " + "→ PK " + " " + (target as Player).Name + "→ Die " + "맵 " + caster.Map + "                                 ";
                            World.SendToAll(new QueDele(caster.Serial, "all", new UpdateChatBox(7, 0x65, 1, (short)text2.Count(), text2).Compile()));
                        }
                        if (caster.TakeDam2 == true)
                        {
                            string damg = string.Format("데미지:{0}" + "            ", take, 0);
                            caster.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)damg.Count(), damg).Compile());
                        }
                        World.SendToAll(new QueDele(caster.m_Map, new HitAnimation(target.Serial,
                           Convert.ToByte(((((float)target.m_HPCur / (float)target.HP) * 100) * 1))).Compile()));
                    }
                }
            }
            if(caster.Map == "대전장" && caster.Color != target.Color)
            {
                float h = ((float)caster.Hit / ((float)caster.Hit + (float)target.Hit)) * 200;

                if (h >= 100 || new Random().Next(0, 100) < (int)h)
                {
                    int take = spell.DamBase + (spell.DamPl * spell.Level + (spell.DamPl * spell.SLevel2));// +(spell.DamPl * spell.SLevel2);
                    if (spell.ManaCostPl != 0)
                    {
                        take += (caster.GetStat("men") / spell.menCoff);
                        take += (caster.GetStat("str") / spell.strCoff);
                        take += (caster.GetStat("dex") / spell.dexCoff);
                        take += (caster.GetStat("vit") / spell.vitCoff);
                    }
                    if (spell is script.spells.DemonDeath)
                        take = Convert.ToInt32(caster.HP * 0.5) + caster.GetStat("dex");

                    if (target.Color == 0)
                        caster.pkpinkktime = Server.tickcount.ElapsedMilliseconds;

                    target.HPCur -= (take - target.AC);
                    if (target.Map == "Rest" && target.Color != 1)
                    {
                        if (caster.Map != "대전장" && caster.Map != "큰대전장")
                        {
                            caster.pklastpk.Add(Server.tickcount.ElapsedMilliseconds);
                            caster.pklastred = Server.tickcount.ElapsedMilliseconds;
                            string text2 = caster.Name + " " + "→ PK " + " " + (target as Player).Name + "→ Die " + "맵 " + caster.Map + "                                 ";
                            World.SendToAll(new QueDele(caster.Serial, "all", new UpdateChatBox(7, 0x65, 1, (short)text2.Count(), text2).Compile()));
                        }
                        if (caster.TakeDam2 == true)
                        {
                            string damg = string.Format("데미지:{0}" + "            ", take, 0);
                            caster.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)damg.Count(), damg).Compile());
                        }
                        World.SendToAll(new QueDele(caster.m_Map, new HitAnimation(target.Serial,
                           Convert.ToByte(((((float)target.m_HPCur / (float)target.HP) * 100) * 1))).Compile()));
                    }
                }
            }
        }



        public void HandleCast(int header, script.spells.Spell castspell, Player player, int target = 0, short castx = 0, short casty = 0)
        {
            if (castspell is VIEW)
            {
                var castonView = World.NewMonsters.Where(xe => xe.Value.m_Serial == target
                    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                if (castonView.Value != null)
                {
                    string info = string.Format("<몬스터정보>" + "                                 ");
                    string info2 = string.Format("이름:{0}", castonView.Value.Name + "                                 ");
                    string info3 = string.Format("체력:{0}/{1}", castonView.Value.HPCur, castonView.Value.HP + "                                 ");
                    string info4 = string.Format("파괴:{0}", castonView.Value.Dam + "                                 ");
                    string info5 = string.Format("적중:{0}", castonView.Value.Hit + "                                 ");
                    string info7 = string.Format("방어:{0}", castonView.Value.AC + "                                 ");
                    string info6 = string.Format("경험치:{0}", castonView.Value.XP + "                                 ");
                    

                    client.SendPacket(new UpdateChatBox(0xff, 10, 0, (short)info.Count(), info).Compile());
                    client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info2.Count(), info2).Compile());
                    client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info3.Count(), info3).Compile());
                    client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info4.Count(), info4).Compile());
                    client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info5.Count(), info5).Compile());
                    client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info7.Count(), info7).Compile());
                    client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info6.Count(), info6).Compile());
                    
                    return;
                }
                else
                {
                    var tele = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                     && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                     && xe.Value.Serial == (Serial)target
                     && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                    if (tele.Value != null)
                    {
                        string info = string.Format("<유저정보>" + "                                 ");
                        string info2 = string.Format("이름:{0}", tele.Value.Name + "                                 ");
                        string info3 = string.Format("생명:{0}, 마나:{1}", tele.Value.HP, tele.Value.MP + "                                 ");
                        string info4 = string.Format("방어:{0} 적중:{1}", tele.Value.AC, tele.Value.Hit + "                                 ");
                        string info5 = string.Format("힘: {0} 지력: {1} 숙련: {2} 생명: {3}", tele.Value.m_Str, tele.Value.m_Men, tele.Value.m_Dex, tele.Value.m_Vit + "                                 ");
                        string info6 = string.Format("레벨: {0}", tele.Value.Level + "                                 ");





                        client.SendPacket(new UpdateChatBox(0xff, 10, 0, (short)info.Count(), info).Compile());
                        client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info2.Count(), info2).Compile());
                        client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info3.Count(), info3).Compile());
                        client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info4.Count(), info4).Compile());
                        client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info5.Count(), info5).Compile());
                        client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)info6.Count(), info6).Compile());

                        tele.Value.Loc = new Point2D(player.Loc.X, player.Loc.Y);
                        tele.Value.Map = player.Map;
                        return;
                    }
                    return;
                }
            }
            if (castspell is Teleport)
            {
                var teleportdist = ((castspell.Level / 2) * 2);
                if (teleportdist <= 3) teleportdist = 4;
                if (teleportdist > 12) teleportdist = 12;
                if (World.Dist2d(castx, casty, player.X, player.Y) <= teleportdist
                    && player.MPCur > castspell.RealManaCost(player))
                {
                    World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, 1, 1, castspell.Seq).Compile()));
                    var nmap = LKCamelot.model.Map.FullMaps.Where(xe => xe.Key == player.Map).FirstOrDefault().Value;
                    TiledMap curmap = null;
                    try
                    {
                        curmap = LKCamelot.model.Map.loadedmaps[nmap];
                    }
                    catch
                    {
                        Console.WriteLine(string.Format("Failed to nmap at {0}", nmap));
                    }
                    LKCamelot.model.MyPathNode randomtile;
                    try
                    {
                        randomtile = curmap.tiles[castx, casty];
                    }
                    catch
                    {
                        return;
                    }
                    if (randomtile.IsWall)
                        return;

                    player.MPCur -= castspell.RealManaCost(player);
                    castspell.CheckLevelUp(player);

                    player.Loc = new Point2D(castx, casty);
                    World.SendToAll(new QueDele(player.Map, new MoveSpriteTele(player.Serial, player.Face, player.X, player.Y).Compile()));
                    int mobile = Serial.NewMobile;
                    World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)player.X, (short)player.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 116 }, 0).Compile()));
                    var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
                    tmp.tempser = mobile;
                    World.TickQue.Add(tmp);
                    return;
                }
            }
            else if (castspell is Trace && player.tempLocate != null && player.tempLocateMap != null)
            {
                player.Loc = player.tempLocate;
                player.Map = player.tempLocateMap;
                if (tempMusic != 2)
                {
                    client.SendPacket(new PlayMusic(tempMusic).Compile());
                }
                return;
            }
            else if (castspell is ComeBack)
            {

                player.tempLocate = new Point2D(player.X, player.Y);
                player.tempLocateMap = player.Map;
                player.Loc = new Point2D(98, 100);
                player.Map = "Village1";
                if (player.musicNumber != 2)
                {
                    tempMusic = player.musicNumber;
                }
                client.SendPacket(new PlayMusic(1001).Compile());

                return;
            }
            else if (castspell is RECALL)
            {
                var tele = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                    && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                    && xe.Value.Serial == (Serial)target
                    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                if (tele.Value != null)
                {

                    tele.Value.Loc = new Point2D(player.Loc.X, player.Loc.Y);
                    tele.Value.Map = player.Map;

                    

                    return;
                }
                else
                {
                    
                    return;
                }
                
            }
            else if (castspell is Pickup)
            {
                var item1 = World.NewItems.Where(xe => xe.Value.m_Map != null
                        && xe.Value.m_Map == player.Map
                        /*&& xe.Value.Loc.X != player.X && xe.Value.Loc.Y != player.Y*/&& xe.Value.m_Serial == target)
                       .FirstOrDefault();
                if (item1.Value != null)
                {
                    World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, 1, 1, castspell.Seq).Compile()));
                   
                    item1.Value.PickUp(player);
                    string text = item1.Value.Name + " 을{를} 획득하였습니다";


                    World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);

                    return;
                }
                else
                {

                    return;
                }

            }
            else if (castspell is PlusHeal)
            {
                var tele2 = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                    && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                    && xe.Value.Serial == (Serial)target
                    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                if (tele2.Value != null)
                {
                    int mobile = Serial.NewMobile;
                    World.SendToAll(new QueDele(tele2.Value.Map, new CreateMagicEffect(mobile, 1, (short)tele2.Value.X, (short)tele2.Value.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 2 }, 0).Compile()));
                    var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, tele2.Value.m_Map, new DeleteObject(mobile).Compile());
                    tmp.tempser = mobile;
                    World.TickQue.Add(tmp);
                    tele2.Value.HPCur = tele2.Value.HP;
                    
                    
                    World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, 1, 1, castspell.Seq).Compile()));
                    return;
                }
                else
                {

                    return;
                }

            }
            else if (castspell is Transparency)
            {
                if (player.Transparancy != 0)
                {
                    player.Transparancy = 0;
                    World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
                }
                else
                {
                    player.Transparancy = (Byte)(castspell.Level * 20);
                    World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
                }
                return;
            }
            else if (castspell is SharpEye)
            {
                var playcaston12 = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value.loggedIn
    && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= 15
    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).ToList();
                if (playcaston12 != null)
                {
                    foreach (var mm in playcaston12)
                    {
                        if (mm.Value.Name != player.Name)
                        {
                            mm.Value.Transparancy = 0;
                            World.SendToAll(new QueDele(mm.Value.Map, new SetObjectEffectsPlayer(mm.Value).Compile()));
                        }
                    }
                }
                //                return;
            }

            if (castspell is StoneCurse)
            {
                var castonView = World.NewMonsters.Where(xe => xe.Value.m_Serial == target
                    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                if (castonView.Value != null)
                {
                    return;
                }
                else
                {
                    var tele = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                     && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                     && xe.Value.Serial == (Serial)target
                     && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                    if (tele.Value != null)
                    {
                        tele.Value.Walkbuff_time = 1000;   
                    }
                    return;
                }
            }
            if (castspell is Freezing)
            {
                var castonView = World.NewMonsters.Where(xe => xe.Value.m_Serial == target
                    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                if (castonView.Value != null)
                {
                    return;
                }
                else
                {
                    var tele = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                     && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                     && xe.Value.Serial == (Serial)target
                     && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                    if (tele.Value != null)
                    {
                        int mobile = Serial.NewMobile;
                        World.SendToAll(new QueDele(tele.Value.Map, new CreateMagicEffect(mobile, 1, (short)tele.Value.X, (short)tele.Value.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 62 }, 0).Compile()));
                        var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 10000, tele.Value.m_Map, new DeleteObject(mobile).Compile());
                        tmp.tempser = mobile;
                        World.TickQue.Add(tmp);
                    }
                    return;
                }
            }
            if (castspell is Butterfly)
            {
                

                var tele = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                    && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                    && xe.Value.Serial == (Serial)target
                    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();

                

                if (tele.Value != null)
                {
                    tele.Value.BHitbuff_time = 500;
                    tele.Value.BHitbuff = 500;
                    tele.Value.client.SendPacket(new UpdateCharStats(tele.Value).Compile());

                    int mobile = Serial.NewMobile;
                    World.SendToAll(new QueDele(tele.Value.Map, new CreateMagicEffect(mobile, 1, (short)tele.Value.X, (short)tele.Value.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 50 }, 0).Compile()));
                    var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, tele.Value.m_Map, new DeleteObject(mobile).Compile());
                    tmp.tempser = mobile;
                    World.TickQue.Add(tmp);
                    World.SendToAll(new QueDele(tele.Value.Map, new CurveMagic(tele.Value.Serial, 1, 1, castspell.Seq).Compile()));

                    return;
                }              
                else
                {
                    
                    if (player.Hit < 100)
                    {
                        player.BHitbuff_time = 500;
                        player.BHitbuff = 30;
                        player.client.SendPacket(new UpdateCharStats(player).Compile());

                        int mobile = Serial.NewMobile;
                        World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)player.X, (short)player.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 50 }, 0).Compile()));
                        var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
                        tmp.tempser = mobile;
                        World.TickQue.Add(tmp);



                        World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, 1, 1, castspell.Seq).Compile()));
                        return;
                    }
                    if (player.Hit > 100)
                    {

                        player.BHitbuff_time = 500;
                        player.BHitbuff = (player.Hit / 100) * 30;
                        player.client.SendPacket(new UpdateCharStats(player).Compile());

                        int mobile = Serial.NewMobile;
                        World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)player.X, (short)player.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 50 }, 0).Compile()));
                        var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
                        tmp.tempser = mobile;
                        World.TickQue.Add(tmp);



                        World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, 1, 1, castspell.Seq).Compile()));
                        return;
                    }
                }

            

            }
            if (castspell is FireWall)
            {
                var castonView = World.NewMonsters.Where(xe => xe.Value.m_Serial == target
                    && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                if (castonView.Value != null)
                {
                    return;
                }
                else
                {
                    var tele = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                     && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                     && xe.Value.Serial == (Serial)target
                     && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();
                    if (tele.Value != null)
                    {
                        int mobile = Serial.NewMobile;
                        World.SendToAll(new QueDele(tele.Value.Map, new CreateMagicEffect(mobile, 1, (short)tele.Value.X, (short)tele.Value.Y, new byte[] { 4, 7, 7, 7, 7, 7, 7, 7, 7, 62 }, 0).Compile()));
                        var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 10000, tele.Value.m_Map, new DeleteObject(mobile).Compile());
                        tmp.tempser = mobile;
                        World.TickQue.Add(tmp);
                    }
                    //return;
                }
            }
            var caston = World.NewMonsters.Where(xe => xe.Value.m_Serial == target
                                && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.X, player.Y) <= castspell.Range
                                && xe.Value.Alive
                && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map
                ).Select(xe => xe.Value);

            var playcaston = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
                && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
                && xe.Value.Serial == (Serial)target
                && xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();

            if (castspell.mType == LKCamelot.library.MagicType.Casted || castspell.mType == LKCamelot.library.MagicType.Target)
            {
                caston = World.NewMonsters.Where(xe => xe.Value.m_Map != null
                       && xe.Value.m_Map == player.Map
                       && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.X, player.Y) <= castspell.Range
                       && xe.Value.Alive)
                      .Select(xe => xe.Value);
            }


            if (playcaston.Key != null
                && !(player.Map == "Village1" || player.Map == "Rest" || player.Map == "Arnold" || player.Map == "Loen" || player.TakeDam == false)
                )
            {

                if (castspell is ISingle)
                {
                    if (player.MPCur < castspell.RealManaCost(player))
                        return;
                    player.MPCur -= castspell.RealManaCost(player);
                    castspell.CheckLevelUp(player);

                    CreateMagicEffect(playcaston.Value.Loc, playcaston.Value.Map, (byte)castspell.Seq.OnImpactSprite, 1500);

                    TakeDamage(player, playcaston.Value, castspell);
                    return;
                }

                if (castspell.Name == "DEMON DEATH")
                {
                    if (player.HPCur < (int)(player.HP * 0.50))
                    {
                        return;
                    }
                    var miyamo = player.Equipped.Where(xe => xe.GetType() == typeof(script.item.MiyamotosStick)).FirstOrDefault();
                    var recast = castspell.RecastTime;
                    //if (miyamo != null)
                    {
                        recast -= 1000;
                        
                        recast -= (player.m_Men / 3000) * 300;
                    }
                    
                    if (LKCamelot.Server.tickcount.ElapsedMilliseconds - recast > castspell.Cooldown)
                    {
                        castspell.Cooldown = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                    }
                    else
                        return;


                    player.HPCur -= (int)(player.HPCur * 0.5);                    
                    castspell.CheckLevelUp(player);
                    //System.IO.Stream soundStream0 = (Properties.Resources._33);
                    //new System.Media.SoundPlayer(soundStream0).Play();

                    int mobile = Serial.NewMobile;
                    World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)playcaston.Value.m_Loc.X, (short)playcaston.Value.m_Loc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, (byte)castspell.Seq.OnImpactSprite }, 0).Compile()));
                    var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
                    tmp.tempser = mobile;
                    World.TickQue.Add(tmp);

                    TakeDamage(player, playcaston.Value, castspell);

                    return;
                }

                if (player.MPCur < castspell.RealManaCost(player))
                    return;
                player.MPCur -= castspell.RealManaCost(player);
                castspell.CheckLevelUp(player);
                TakeDamage(player, playcaston.Value, castspell);
                World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial,
                    castx, casty, castspell.Seq).Compile()));
            }


            switch (castspell.mType)
            {
                case (LKCamelot.library.MagicType.Target2):
                    foreach (var targete in caston)
                    {
                        if (castspell is ISingle)
                        {
                            if (player.MPCur < castspell.RealManaCost(player))
                                return;
                            player.MPCur -= castspell.RealManaCost(player);
                            castspell.CheckLevelUp(player);

                            CreateMagicEffect(targete.m_Loc, targete.m_Map, (byte)castspell.Seq.OnImpactSprite, 1500);

                            targete.TakeDamage(player, castspell);
                            return;
                        }

                        if (castspell.Name == "DEMON DEATH")
                        {
                            if (player.HPCur < (int)(player.HP * 0.50))
                                return;

                            var miyamo = player.Equipped.Where(xe => xe.GetType() == typeof(script.item.MiyamotosStick)).FirstOrDefault();
                            var recast = castspell.RecastTime;
                            if (miyamo != null)
                            {
                                recast -= 1000;
                                recast -= miyamo.Stage * 300;
                            }

                            if (LKCamelot.Server.tickcount.ElapsedMilliseconds - recast > castspell.Cooldown)
                            {
                                castspell.Cooldown = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                            }
                            else
                                return;

                            player.HPCur -= (int)(player.HPCur * 0.5);
                            castspell.CheckLevelUp(player);

                            int mobile = Serial.NewMobile;
                            World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)targete.m_Loc.X, (short)targete.m_Loc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, (byte)castspell.Seq.OnImpactSprite }, 0).Compile()));
                            var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
                            tmp.tempser = mobile;
                            World.TickQue.Add(tmp);

                            targete.TakeDamage(player, castspell);

                            return;
                        }

                        if (player.MPCur < castspell.RealManaCost(player))
                            return;
                        player.MPCur -= castspell.RealManaCost(player);
                        castspell.CheckLevelUp(player);
                        targete.TakeDamage(player, castspell);
                        World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial,
                            castx, casty, castspell.Seq).Compile()));
                    }

                    break;

                case (LKCamelot.library.MagicType.Casted):
                    if (player.MPCur < castspell.RealManaCost(player))
                        return;
                    player.MPCur -= castspell.RealManaCost(player);

                    if (castspell.Cast(player))
                        return;

                    foreach (var targete in caston)
                        targete.TakeDamage(player, castspell);

                    World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial,
                      1, 1, castspell.Seq).Compile()));

                    break;
                case (LKCamelot.library.MagicType.Target):
                    if (player.MPCur < castspell.RealManaCost(player))
                        return;
                    player.MPCur -= castspell.RealManaCost(player);
                    if (castspell.Cast(player))
                        return;

                    World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, 1, 1, castspell.Seq).Compile()));
                    foreach (var targetee in caston)
                    {
                        int mobile = Serial.NewMobile;
                        World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)targetee.m_Loc.X, (short)targetee.m_Loc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, (byte)castspell.Seq.OnImpactSprite }, 0).Compile()));
                        // World.SendToAll(new QueDele(player.Map, new SetObjectEffectsMonsterSpell(targetee, castspell.Seq.OnImpactSprite).Compile()));
                        targetee.TakeDamage(player, castspell);
                        var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 1000, player.m_Map, new DeleteObject(mobile).Compile());
                        tmp.tempser = mobile;
                        World.TickQue.Add(tmp);
                    }
                    break;
            }
        }
    }
}
