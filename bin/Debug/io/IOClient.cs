//#define NOSQL
#define PROMOCAP12

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

using LKCamelot.net;
using LKCamelot.util;
using LKCamelot.model;
using LKCamelot.script;

using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using System.Net.NetworkInformation;



namespace LKCamelot.io
{
    public partial class IOClient
    {
        public Connection connection;
        private long connectedAt;
        public bool CStatus = true;
        public LKCamelot.net.PacketWriter mm_stream;
        private object _bufferLock = new object();
        public CastHandler castHandler;
        public CombatHandler2 combatHandler;
        private long ChatTimeout = 0, lastcmd = 0;
        private int AliasStage = 0;
        private int AronStage = 0;
        bool firstlogin = false;
        public long keepalive = 0;
        public LKCamelot.model.Modules.NSA nsa;

        private readonly Stream incomingStream = new Stream();

        public Player player;
        public PlayerHandler handler;

        public IOClient(Connection s)
        {
            this.connection = s;
            this.connectedAt = Server.CurrentTimeMillis();
            this.mm_stream = new PacketWriter(255);
            this.castHandler = new CastHandler(this, handler);
            this.combatHandler = new CombatHandler2(this, handler);
            //    IOHostList.add(connectedFrom);
        }

        delegate void UpdatePacketInvoker(Byte[] packet);

        public void Parse(ConnectionDataEventArgs e)
        {
            incomingStream.AppendData(e.Data.ToArray());
            while (incomingStream.IsPacketAvailable())
            {
                var packet = incomingStream.PopPacket();

                try
                {
                    String eventListener = packet.ToFormatedHexString();
                    /*                    if (eventListener.Substring(1,2).Equals("17"))
                                        {
                        
                                            System.IO.Stream soundStream0 = (Properties.Resources._0);
                                            new System.Media.SoundPlayer(soundStream0).Play();
                                        }
                    */
                    if (eventListener.Substring(1, 2).Equals("46"))
                    {

                        KeyValuePair<string, Player> play = new KeyValuePair<string, Player>(player.Name, player);
                        var pot = World.NewItems.Where(xe => xe.Value.Parent != null && (xe.Value.Parent == play.Value || xe.Value.ParSer == play.Value.Serial) && (xe.Value.Name == "Life Drug" || xe.Value.Name == "Full Life Drug")).FirstOrDefault();
                        if (pot.Value != null)
                            pot.Value.Use(play.Value);
                    }
                    if (eventListener.Substring(1, 2).Equals("47"))
                    {
                        KeyValuePair<string, Player> play = new KeyValuePair<string, Player>(player.Name, player);
                        var pot = World.NewItems.Where(xe => xe.Value.Parent != null && (xe.Value.Parent == play.Value || xe.Value.ParSer == play.Value.Serial) && (xe.Value.Name == "Magic Drug" || xe.Value.Name == "Full Magic Drug")).FirstOrDefault();
                        if (pot.Value != null)
                        {
                            pot.Value.Use(play.Value);
                        }
                    }
                   
                    HandleIncoming(packet);
                }
                catch (NotImplementedException) { }
                catch (Exception ee)
                {
                    Console.WriteLine("Unknown process exception: {0}", ee.StackTrace);
                }
            }
            incomingStream.Flush();
        }
        
        public void SendPacket(Byte[] packet)
        {
            if (nsa != null)
            {
                nsa.AppendPacketOut(packet);
            }

            lock (this._bufferLock)
                connection.Send(packet);
        }

        public void Process()
        {
        }

        public short AttackRange(short x, short y, short x2, short y2)
        {
            if (x2 - x == 0 && y2 - y == 0)
                return 0;
            if (x2 - x == -1 && y2 - y == 1)
                return 1;
            if (x2 - x == -1 && y2 - y == 0)
                return 2;
            if (x2 - x == -1 && y2 - y == -1)
                return 3;
            if (x2 - x == 0 && y2 - y == -1)
                return 4;
            if (x2 - x == 1 && y2 - y == -1)
                return 5;
            if (x2 - x == 1 && y2 - y == 0)
                return 6;
            if (x2 - x == 1 && y2 - y == 1)
                return 7;
            if (x2 - x == 0 && y2 - y == 1)
                return 0;

            return -1;
        }
     //   System.Collections.Concurrent.ConcurrentDictionary<long, Point2D> walktrace = 
     //       new System.Collections.Concurrent.ConcurrentDictionary<long, Point2D>();
        System.Collections.Generic.List<long> walktrace = new System.Collections.Generic.List<long>();
        private bool Loaded = false;
        public void LoadPlayer()
        {
            if (player == null || Loaded)
                return;
            Loaded = true;

            if (!LKCamelot.model.Map.FullMaps.ContainsKey(player.Map))
            {
                player.m_Map = "Rest";
                player.m_Loc = new Point2D(15, 15);
            }

            var invent = World.NewItems.Where(xe => xe.Value.ParSer == player.Serial).Select(xe => xe.Value);
            foreach (var item in invent)
            {
                item.Parent = player;
            }
            SendPacket((new SetProfessions(player.ProfessionString).Compile()));
            SendPacket(new LoadWorld(player, 1).Compile());

            SendPacket(new UpdateCharStats(player).Compile());
            SendPacket(new SetLevelGold(player).Compile());

            player.LoadNPCs();

            foreach (var spell in player.MagicLearned)
                SendPacket(new CreateSlotMagic2(spell).Compile());

            foreach (var item in World.NewItems.Where(xe => xe.Value.m_Parent == player || xe.Value.ParSer == player.Serial).ToList())
            {
                if (item.Value.InvSlot >= 0 && item.Value.InvSlot <= 24)
                {
                    SendPacket(new AddItemToInventory2(item.Value).Compile());
                }
                else if (item.Value.InvSlot >= 25 && item.Value.InvSlot <= 30)
                {
                    player.Equipped2.TryAdd(item.Value.InvSlot, item.Value);
                    SendPacket(new EquipItem2(item.Value).Compile());
                }
            }

            player.Map = player.Map;

            //01 05 green 
            string text = "마지막왕국 최후팩입니다 "+"                  ";
            SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());           
            string text2 = "최후팩 Ver 1.6"+"                            ";
            SendPacket(new UpdateChatBox(0x50, 0x65, 5, (short)text2.Count(), text2).Compile());
            player.Loc = new Point2D(18, 18);
            player.Map = "Rest";
            string achat = DateTime.Now.ToLocalTime().ToString("현재시간 yyyy/MM/dd tt hh:mm" + "            ");
            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
            player.NormalXp = true;
            player.PlusXp = false;
            string texting = "(" + player.Name + ")" + "님이 접속하셨습니다." + "                    ";
            World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)texting.Count(), texting).Compile()));
            if (firstlogin)
            {
                
                text2 = "@이동 초보 에서 사냥을 시작하세요" + "                    ";
                SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text2.Count(), text2).Compile());
                text2 = "전직은 @이동 아론에서 할수 있습니다." + "                    ";
                SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text2.Count(), text2).Compile());
                text2 = "천원을 모으고 '완료' 라고 채팅창에 쳐보세요 " + "                            ";
                SendPacket(new UpdateChatBox(0x00, 0xff, 0, (short)text2.Count(), text2).Compile());
                
                string text3 = "(" + player.Name + ")" + "케릭이 생성되었습니다." + "                    ";
                World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0x02, 0, (short)text3.Count(), text3).Compile()));
                player.NormalXp = true;
                player.PlusXp = false;
            }

            World.SendToAll(new QueDele(player.Map, new ChangeObjectSpritePlayer(player).Compile()));
        }

        public void HandleGo(string goloc)
        {
            switch (goloc.ToLower())
            {
                case "마을":
                    player.Loc = new Point2D(98, 100);
                    player.Map = "Village1";
                    SendPacket(new PlayMusic(1000).Compile());
                    break;
                case "휴게실":
                case "death":
                    player.Loc = new Point2D(18, 18);
                    player.Map = "Rest";
                    SendPacket(new PlayMusic(1006).Compile());
                    break;
                case "아놀드":
                    player.Loc = new Point2D(8, 12);
                    player.Map = "Arnold";
                    SendPacket(new PlayMusic(4000).Compile());
                    break;
                case "아론":
                    player.Loc = new Point2D(98, 60);
                    player.Map = "Village1";
                    break;
                case "로엔":
                    player.Loc = new Point2D(8, 12);
                    player.Map = "Loen";
                    SendPacket(new PlayMusic(3000).Compile());
                    break;
                case "점원":
                    player.Loc = new Point2D(74, 98);
                    player.Map = "Village1";
                    break;
                case "노점상":
                    player.Loc = new Point2D(128, 94);
                    player.Map = "Village1";
                    break;
                case "알리아스":
                    player.Loc = new Point2D(90, 176);
                    player.Map = "Village1";
                    break;

                case "초보":
                    player.Loc = new Point2D(30, 19);
                    player.Map = "Beginner";
                    SendPacket(new PlayMusic(1007).Compile());
                    break;
                case "빅건":
                    player.Loc = new Point2D(97, 77);
                    player.Map = "Biggun1";
                    break;
                case "미로":
                    if (player.Level >= 141)
                    {
                        player.Loc = new Point2D(2, 2);
                        player.Map = "Miro1";
                    }
                    else
                    {
                        string achat = "3승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;
                case "피그미":
                    player.Loc = new Point2D(66, 123);
                    player.Map = "Village1";
                    break;
                case "스켈":
                    player.Loc = new Point2D(139, 137);
                    player.Map = "Village1";
                    break;
                case "터":                   
                        player.Loc = new Point2D(29, 32);
                        player.Map = "ELevel";
                        SendPacket(new PlayMusic(1002).Compile());                    
                   /*
                    if (player.guildName != "GM" && player.Gold >= 1000)
                    {

                        player.Gold -= 1000;
                        string ply = string.Format("{0}님 예금에서 1000원이 성주님께 전달되었습니다" + "                                 ", player.Name);
                        SendPacket(new UpdateChatBox(0xff, 10, 0, (short)ply.Count(), ply).Compile());


                        player.Loc = new Point2D(29, 32);
                        player.Map = "ELevel";
                        SendPacket(new PlayMusic(1002).Compile());
                        if (tradep.loggedIn == false)
                        {
                            tradep.Gold += 1000;
                            
                        }
                        if (tradep.loggedIn)
                        {
                            Console.WriteLine("송금테스트");
                            string tra = string.Format("{0}님 에게서 던전입장료 1000원을 전달받았습니다 " + "                               ", player.Name);
                            tradep.client.SendPacket(new UpdateChatBox(0xff, 10, 0, (short)tra.Count(), tra).Compile());
                        }
                    }*/                  
                    break;
                case "아이템":
                    player.Loc = new Point2D(93, 111);
                    player.Map = "ItemVillage";
                    break;
                case "가보자":
                    player.Loc = new Point2D(25, 25);
                    player.Map = "가보자";
                    break;
                case "보물섬":
                    if (player.Level >= 161)
                    {
                        player.Loc = new Point2D(35, 42);
                        player.Map = "TreasureLand";
                    }
                    else
                    {
                        string achat = "4승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;

                case "vv1":
                    if (player.Level >= 161)
                    {
                        player.Loc = new Point2D(25, 25);
                        player.Map = "VV1";
                    }
                    else
                    {
                        string achat = "4승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;
                case "vv2":
                    if (player.Level >= 161)
                    {
                        player.Loc = new Point2D(25, 25);
                        player.Map = "VV2";
                    }
                    else
                    {
                        string achat = "4승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;
                case "vv3":
                    if (player.Level >= 161)
                    {
                        player.Loc = new Point2D(25, 25);
                        player.Map = "VV3";
                    }
                    else
                    {
                        string achat = "4승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;
                case "vv4":
                    if (player.Level >= 161)
                    {
                        player.Loc = new Point2D(25, 25);
                        player.Map = "VV4";
                    }
                    else
                    {
                        string achat = "4승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;
                case "vv5":
                    if (player.Level >= 161)
                    {
                        player.Loc = new Point2D(25, 25);
                        player.Map = "VV5";
                    }
                    else
                    {
                        string achat = "4승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;

                case "광산":
                    player.Loc = new Point2D(15, 38);
                    player.Map = "Miner0";
                    break;

                case "축구장":
                    player.Loc = new Point2D(5, 18);
                    player.Map = "Soccer";
                    break;
                case "대전장":
                    player.Loc = new Point2D(10, 15);
                    player.Map = "대전장";
                    break;
                case "큰대전장":
                    player.Loc = new Point2D(10, 15);
                    player.Map = "큰대전장";
                    break;
                case "공성":
                    player.Loc = new Point2D(29, 34);
                    player.Map = "estart.map";
                    break;
                case "오엑스":
                    player.Loc = new Point2D(66, 66);
                    player.Map = "오엑스";
                    break;
                case "오엑스2":
                    player.Loc = new Point2D(10, 10);
                    player.Map = "오엑스2";
                    break;
                case "그레이트":
                    if (player.Level >= 221)
                    {
                        player.Loc = new Point2D(29, 34);
                        player.Map = "Great";
                    }
                    else
                    {
                        string achat = "7승이상 이동가능." + "                                                      ";
                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                    }
                    break;
                case "포인트":


                    player.Loc = new Point2D(10, 7);
                    player.Map = "포인트";                    
                    break;
                case "뉴광산":
                    player.Loc = new Point2D(8, 12);
                    player.Map = "뉴광산";
                    break;
            }
        }

        public void parseFace()
        {
            var face = player.Face;
            if (face == 0)
            {
                player.Y--;
            }
            if (face == 1)
            {
                player.X++;
                player.Y--;
            }
            if (face == 2)
            {
                player.X++;
            }
            if (face == 3)
            {
                player.X++;
                player.Y++;
            }
            if (face == 4)
            {
                player.Y++;
            }
            if (face == 5)
            {
                player.X--;
                player.Y++;
            }
            if (face == 6)
            {
                player.X--;
            }
            if (face == 7)
            {
                player.X--;
                player.Y--;
            }
        }
        public long LastAttack, LastCast;
        /*     public int ParseEquipSlot(Item item)
             {
                 var i = item.Sprite;

                 if (i == 0x04 || i == 0xB4 || i == 0xB2)
                 {
                     return LKCamelot.library.EquipSlot.Hat;
                 }
                 if (item.Sprite == 0x52)
                 {
                     return LKCamelot.library.EquipSlot.Amulet;
                 }
                 if ((i >= 9 && i <= 0x10) || (i >= 0x20 && i <= 0x24)
                     || i == 0xB5)
                 {
                     return LKCamelot.library.EquipSlot.Weapon;
                 }
                 if (i == 5 || i == 6 || i == 7 || i == 8 ||
                     i == 0xB1 || i == 0xB6 || i == 0xB8 || i == 0xB9)
                 {
                     return LKCamelot.library.EquipSlot.Armor;
                 }
                 if ((i >= 0x11 && i <= 0x13) || i == 0xB7)
                 {
                     return LKCamelot.library.EquipSlot.Shield;
                 }
                 if (item.Sprite == 1 || item.Sprite == 2 || item.Sprite == 0x53)
                 {
                     return LKCamelot.library.EquipSlot.Ring;
                 }

                 return -1;
             }*/

        public Point2D AdjecentTile(byte swingloc)
        {
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

        public void HandleIncoming(Byte[] data)
        {
            if (nsa != null)
            {
                nsa.AppendPacketIn(data);
            }
            PacketReader p = null;
            switch (data[0])
            {
                case 0x34: // Keep Alive
                    keepalive = Server.tickcount.ElapsedMilliseconds;
                    break;
                //Identifiy
                /*     case 0x37:
                         //     for (int x = 0; x < 40; x++)
                         //     {
                         //        SendPacket(new CreateSlotMagic(new MagicSpell((byte)(x+81), "Hii", 1, 1, (byte)x, library.MagicType.Casted)).Compile());
                         //    }
                         int y = 14;
                         byte sprite = 0;
                         for (int x = 0; x < 255; x++)
                         {
                             Thread.Sleep(100);
                             if (x != 0 && x % 19 == 0)
                             {
                                 y += 3;
                                 x -= 19;
                             }
                             //    SendPacket(new CreateMonster(new Monster(3, (short)(x+23),(short)y,"village.map", sprite.ToString(), sprite, 0), Serial.NewMobile).Compile());
                             sprite++;
                         }
                         break;*/
                case 0x3A: //find
                    if (player.Map == "Loen")
                    {
                        var slot = data[1] + 40 + (12 * player.BankTab);
                        var itemtofind = World.NewItems.Where(xe => xe.Value.ParSer == player.Serial && xe.Value.InvSlot == slot).FirstOrDefault();
                        if (itemtofind.Value != null)
                        {
                            if (player.GetFreeSlot() != -1)
                            {
                                SendPacket(new DeleteEntrustSlot((byte)data[1]).Compile());

                                itemtofind.Value.InvSlot = player.GetFreeSlot();
                                SendPacket(new AddItemToInventory2(itemtofind.Value).Compile());
                            }
                        }
                    }
                    break;
                case 0x36: //Entrust
                    if (player.Map == "Loen")
                    {
                        var itemtoentrust = World.NewItems.Where(xe => xe.Value.ParSer == player.Serial && xe.Value.InvSlot == data[1]).FirstOrDefault();
                        if (itemtoentrust.Value != null)
                        {
                            SendPacket(new DeleteItemSlot((byte)itemtoentrust.Value.InvSlot).Compile());
                            itemtoentrust.Value.InvSlot = player.FreeBankSlot;
                            SendPacket(new AddItemToEntrust(itemtoentrust.Value).Compile());
                        }
                    }
                    break;

                case 0x35: //sell
                    if (player.Map == "Arnold" && player.Gold <= 2000000000)
                    {
                        var itemtosell = World.NewItems.Where(xe => xe.Value.ParSer == player.Serial && xe.Value.InvSlot == data[1]).FirstOrDefault();
                        if (itemtosell.Value != null && itemtosell.Value.SellPrice > 0)
                        {
                            player.Gold += (uint)itemtosell.Value.SellPrice;
                            itemtosell.Value.Delete(player);
                        }
                    }
                    break;

                //Cast
                //  3D-00-00-01-00-00-00-0A-00-09-00
                case 0x3D:
                case 0x19:
                case 0x18:
                    if (LKCamelot.Server.tickcount.ElapsedMilliseconds - player.CastSpeed > LastCast)
                    {
                        LastCast = LKCamelot.Server.tickcount.ElapsedMilliseconds;
                        p = new PacketReader(data, data.Count(), true);
                        int spellslot = p.ReadInt16();
                        if (player.MagicLearned.Count() < spellslot)
                            return;
                        int castonid = p.ReadInt32();
                        short castx = p.ReadInt16();
                        short casty = p.ReadInt16();

                        script.spells.Spell castspell = player.MagicLearned.Where(xe => xe.Slot == spellslot).FirstOrDefault();
                        if (castspell == null) return;

                        castHandler.HandleCast(data[0], castspell, player, castonid, castx, casty);
                    }
                    break;
                //Attack
                case 0x17:
                    if (LKCamelot.Server.tickcount.ElapsedMilliseconds - player.AttackSpeed > LastAttack)
                    {
                        LastAttack = LKCamelot.Server.tickcount.ElapsedMilliseconds;

                        World.SendToAll(new QueDele(player.Serial, player.Map, new SwingAnimationChar(player.Serial, player.Face).Compile()));
                        combatHandler.HandleMelee(player, data[1]);
                    }
                    break;

                //NPC Shop
                case 0x45:
                    var npclook = World.NewNpcs.Where(xe => xe.Key == data[1]).FirstOrDefault();
                    if (npclook.Value != null)
                    {
                        if (npclook.Value.Name == "Loen")
                        {
                            SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());
                        }
                        if (npclook.Value.Name == "Arnold")
                        {
                            SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());
                        }
                        if (npclook.Value.Name == "Employee")
                        {
                            SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());
                        }
                        if (npclook.Value.Name == "Boy")
                        {
                            SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());
                        }
                        if (npclook.Value.Name == "Loen2")
                        {
                            SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());
                        }
                        if (npclook.Value.Name == "Vendor")
                        {
                            SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());
                        }
                        if (npclook.Value.Name == "튜토리얼" && player.Level <= 20 && player.Gold >= 1000)
                        {

                            player.Gold = player.Gold - 1000;
                            var newitem = new script.item.초심자의선물().Inventory(player);
                            World.NewItems.TryAdd(newitem.m_Serial, newitem);
                            player.client.SendPacket(new AddItemToInventory2(newitem).Compile());
                            string achat2 = "[7승검객]: 수고했다." + "         ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
                            achat2 = "상자 구성품은 직업에 따라 다르게 지급됩니다. " + "                            ";
                            SendPacket(new UpdateChatBox(0x00, 0xff, 0, (short)achat2.Count(), achat2).Compile());

                            player.Loc = new Point2D(98, 100);
                            player.Map = "Village1";
                        }
                        if (npclook.Value.Name == "튜토리얼" && player.Gold <= 999)
                        {

                            string text = "[7승검객] : 천원을 가지고 오너라 " + "                  ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 0, (short)text.Count(), text).Compile());
                        }
                        if (npclook.Value.Name == "건물")
                            {
                              
                                string text = "몹이나 잡아라 " + "                  ";
                                SendPacket(new UpdateChatBox(0xff, 0xff, 0, (short)text.Count(), text).Compile());
                            }
                        if (npclook.Value.Name == "건물2")
                        {

                            string text = "몹이나 잡아라 " + "                  ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 0, (short)text.Count(), text).Compile());
                        }
                        if (npclook.Value.Name == "건물3")
                        {

                            string text = "몹이나 잡아라 " + "                  ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 0, (short)text.Count(), text).Compile());
                        }
                        if (npclook.Value.Name == "건물4")
                        {

                            string text = "몹이나 잡아라 " + "                  ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 0, (short)text.Count(), text).Compile());
                        }
                        if (npclook.Value.Name == "안내문")
                        {

                            string text = "몹이나 잡아라 " + "                  ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 0, (short)text.Count(), text).Compile());
                        }
                         if (npclook.Value.Name == "아템하보")
                         {
                             //player.Loc = new Point2D(15, 46);
                             //player.Map = "ItemVillage";
                             SendPacket(new AutoMove(15, 46).Compile());

                             string text = "아템하보 X = 15 Y = 46 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "아템부쳐")
                         {
                             //player.Loc = new Point2D(190, 31);
                             //player.Map = "ItemVillage";
                             
                             SendPacket(new AutoMove(190, 31).Compile());

                             string text = "아템부쳐 X = 190 Y = 31 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "아템스켈")
                         {
                             //player.Loc = new Point2D(186, 6);
                             //player.Map = "ItemVillage";
                             SendPacket(new AutoMove(186, 6).Compile());

                             string text = "아템스켈 X = 186 Y = 6 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "아템골렘")
                         {
                             player.Loc = new Point2D(93, 117);
                             player.Map = "ItemVillage";
                             SendPacket(new AutoMove(8, 178).Compile());

                             string text = "아템골렘 X = 8 Y = 178 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "아템스톤")
                         {
                             player.Loc = new Point2D(93, 117);
                             player.Map = "ItemVillage";
                             SendPacket(new AutoMove(177, 162).Compile());

                             string text = "아템스톤 X = 177 Y = 162 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "아템더미")
                         {
                             player.Loc = new Point2D(93, 117);
                             player.Map = "ItemVillage";
                             SendPacket(new AutoMove(23, 136).Compile());

                             string text = "아템더미 X = 23 Y = 136 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "아템워더미")
                         {
                             player.Loc = new Point2D(93, 117);
                             player.Map = "ItemVillage";
                             SendPacket(new AutoMove(38, 143).Compile());

                             string text = "아템워더미 X = 38 Y = 143 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "아템머미")
                         {
                             player.Loc = new Point2D(93, 117);
                             player.Map = "ItemVillage";
                             SendPacket(new AutoMove(193, 189).Compile());

                             string text = "아템머미 X = 193 Y = 189 이동";


                             World.SendToAllRange(new QueDele(player.Map, new BubbleChat(player.Serial, text).Compile()), player, 10);
                         }
                         if (npclook.Value.Name == "PointShop")
                         {
                             SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());

                         }
                        if (npclook.Value.Name == "BuffMen")
                        {
                            SendPacket(new SpawnShopGump2(npclook.Value.Gump).Compile());
                            
                        }
                        if (npclook.Value.Name == "CastleNpc")
                        {
                            string text = "[길드석] :" + "현재 성역의 주인은 " + Server.Castellan + " 길드                   ";
                            SendPacket(new UpdateChatBox(0xff, 0xff, 0, (short)text.Count(), text).Compile());
                        }
                    }
                    break;

                case 0x2B: //2B-03-00-00-00-01-00-04-00-4D-65-6E-75-00
                    p = new PacketReader(data, data.Count(), true);
                    var npcid = p.ReadInt32();
                    var buyslot = p.ReadByte();

                    var npcitself = World.NewNpcs.Where(xe => xe.Key == npcid).FirstOrDefault();
                    if (npcitself.Value != null)
                        npcitself.Value.Buy(player, buyslot);

                    break;

                //Inventory
                case 0x00: //use
                    if (data[1] == 0)
                        return;
                    var itemu = World.NewItems.Where(xe => xe.Value.m_Parent == player
                      && xe.Value.InvSlot == data[1]).FirstOrDefault();

                    if (itemu.Value != null)
                        itemu.Value.Use(player);

                    break;

                case 0x20: //drop
                    var item = World.NewItems.Where(xe => xe.Value.m_Parent == player
                        && xe.Value.InvSlot == data[1]).FirstOrDefault();

                    if (item.Value != null )
                        item.Value.Drop(player);
                     script.item.Item ii = item.Value;
                     new Thread(ClearItem).Start(ii);
                    break;

                case 0x1F: //pickup
                    var item1 = World.NewItems.Where(xe => xe.Value.m_Map != null
                        && xe.Value.m_Map == player.Map
                        && xe.Value.Loc.X == player.X && xe.Value.Loc.Y == player.Y)
                       .FirstOrDefault();

                    if (item1.Value != null)
                        item1.Value.PickUp(player);

                    break;
                case 0x1E: //equip Use
                    // case 0x36:
                    var eitem = World.NewItems.Where(xe => xe.Value.m_Parent == player
                        && xe.Value.InvSlot == data[1]).FirstOrDefault();

                    if (eitem.Value != null)
                    {
                        if (eitem.Value is script.item.BaseArmor || eitem.Value is script.item.BaseWeapon)
                            eitem.Value.Equip(player);
                        if (eitem.Value is script.item.BaseSpellbook)
                            eitem.Value.Use(player);
                        if (eitem.Value is script.item.BasePotion)
                            eitem.Value.Use(player);
                    }

                    break;
                case 0x23://unequip
                    var uitem = World.NewItems.Where(xe => xe.Value.m_Parent == player
                        && xe.Value.InvSlot == (data[1] + 25)).FirstOrDefault();

                    if (uitem.Value != null)
                        uitem.Value.Unequip(player, data[1] + 25);
                    break;

                case 0x25: //swap items
                    var item11 = World.NewItems.Where(xe => xe.Value.ParSer == player.Serial && xe.Value.InvSlot == data[1]).FirstOrDefault().Value;
                    int sss = 0;
                    if (data.Count() > 3)
                        sss = data[3];

                    var target1 = World.NewItems.Where(xe => xe.Value.ParSer == player.Serial && xe.Value.InvSlot == sss).FirstOrDefault().Value;

                    player.SwapItems(item11, target1, sss);

                    break;
                    
                case 0x24: //drag,drop
                    var itemdragdrop = World.NewItems.Where(xe => xe.Value.ParSer == player.Serial && xe.Value.InvSlot == data[1]).FirstOrDefault().Value;
                    if (itemdragdrop != null)
                    {
                        p = new PacketReader(data, data.Count(), false);
                        var targetid = p.ReadInt32();

                        script.item.Item targeti = null;
                        World.NewItems.TryGetValue(targetid, out targeti);
                        if (targeti != null)
                        { 
                            string fail = targeti.Name + "+" + (targeti.Stage + 1) + " " + "강화에 실패하였습니다." + "          ";
                            string succ = targeti.Name + "+" + (targeti.Stage + 1) + " " + "강화에 성공하였습니다." + "             ";
                            
                            
                            
                            if ((itemdragdrop is script.item.Zel && targeti is script.item.BaseArmor)
                                || (itemdragdrop is script.item.Dai && targeti is script.item.BaseWeapon))
                            {
                                if (targeti.TryUpgrade())
                                {
                                    castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 42);
                                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)succ.Count(), succ).Compile());
                                    SendPacket(new DeleteObject(targeti.m_Serial).Compile());
                                    SendPacket(new CreateItemGround2(targeti, targeti.m_Serial).Compile());
                                    if (targeti.Stage >= 9)
                                    {
                                        string text3 = "[" + player.Name + "]" + "님이" + targeti.Name + "+" + (targeti.Stage) + "강화에 성공하였습니다" + "                    ";
                                        World.SendToAll(new QueDele("all", new UpdateChatBox(0x00, 0xff, 0, (short)text3.Count(), text3).Compile()));
                                    } 
                                }
                                else
                                {
                                    castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 56);
                                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)fail.Count(), fail).Compile());
                                    if (targeti.Stage >= 9)
                                    {
                                        string text3 = "[" + player.Name + "]" + "님이" + targeti.Name + "+" + (targeti.Stage + 1) + "강화에 실패하였습니다" + "                    ";
                                        World.SendToAll(new QueDele("all", new UpdateChatBox(200, 200, 0, (short)text3.Count(), text3).Compile()));
                                    }
                                }
                                 
                                itemdragdrop.Delete(player);
                            }
                        }
                        if (targeti != null)
                        {
                            string fail = targeti.Name + "+" + (targeti.Stage + 1) + " " + "강화에 실패하였습니다." + "          ";
                            string succ = targeti.Name + "+" + (targeti.Stage + 1) + " " + "강화에 성공하였습니다." + "             ";



                            if ((itemdragdrop is script.item.Zel2 && targeti is script.item.BaseArmor)
                                || (itemdragdrop is script.item.Dai2 && targeti is script.item.BaseWeapon))
                            {
                                if (targeti.TryUpgrade2())
                                {
                                    castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 42);
                                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)succ.Count(), succ).Compile());
                                    SendPacket(new DeleteObject(targeti.m_Serial).Compile());
                                    SendPacket(new CreateItemGround2(targeti, targeti.m_Serial).Compile());
                                }
                                else
                                {
                                    castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 56);
                                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)fail.Count(), fail).Compile());
                                }

                                itemdragdrop.Delete(player);
                            }
                        }
                        if (targeti != null)
                        {
                            string fail = targeti.Name + "합성에 실패하였습니다." + "                   ";
                            string succ = targeti.Name + "합성에 성공하였습니다." + "                   ";



                            if ((itemdragdrop is script.item.토파즈원석 && targeti is script.item.토파즈원석)
                                || (itemdragdrop is script.item.루비원석 && targeti is script.item.루비원석)
                                || (itemdragdrop is script.item.자수정원석 && targeti is script.item.자수정원석)
                                || (itemdragdrop is script.item.에메랄드원석 && targeti is script.item.에메랄드원석))
                            {
                                if (itemdragdrop.Quantity == 101)
                                {
                                    if (targeti.TryUpgrade3())
                                    {
                                        castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 190);
                                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)succ.Count(), succ).Compile());
                                        SendPacket(new DeleteObject(targeti.m_Serial).Compile());
                                        SendPacket(new CreateItemGround2(targeti, targeti.m_Serial).Compile());

                                    }
                                    else
                                    {
                                        castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 56);
                                        SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)fail.Count(), fail).Compile());


                                    }

                                    itemdragdrop.Delete(player);
                                }
                            }
                        }
                        if (targeti != null)
                        {
                            string fail = targeti.Name + "연성에 실패하였습니다." + "                   ";
                            string succ = targeti.Name + "연성에 성공하였습니다." + "                   ";



                            if ((itemdragdrop is script.item.성장의시약 && targeti is script.item.성장의검))
                            {

                                if (targeti.TryUpgrade4())
                                {
                                    castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 190);
                                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)succ.Count(), succ).Compile());
                                    SendPacket(new DeleteObject(targeti.m_Serial).Compile());
                                    SendPacket(new CreateItemGround2(targeti, targeti.m_Serial).Compile());

                                }
                                else
                                {
                                    castHandler.CreateMagicEffect(targeti.Loc, targeti.m_Map, 56);
                                    SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)fail.Count(), fail).Compile());


                                }

                                itemdragdrop.Delete(player);
                            }
                        }
                        if (targetid == 4)
                        {
                            if (itemdragdrop.Name == "Promote Life Drug" && AronStage == 4)
                            {
                                AronStage = 1;
                                itemdragdrop.Delete(player);
                                string achat = "[Aron]: Are you ready for the promotion?";
                                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                            }
                            if (itemdragdrop.Name == "Promote Magic Drug" && AronStage == 4)
                            {
                                AronStage = 2;
                                itemdragdrop.Delete(player);
                                string achat = "[Aron]: Are you ready for the promotion?";
                                SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
                            }
                        }
                    }
                    break;

                //Chat message
                case 0x16:

                    p = new PacketReader(data, data.Count(), false);
                    string str = p.ReadString();
                    string str1 = Encoding.Default.GetString(data);
                    string[] str3 = str1.Split('\x0');

                    var str2 = str1.Split(' ');
                    Console.Write(player.Name + ": " + str3[1] + "   " + "\r\n");
                    //Console.Write("read1: " + str3[1] +"    " +  "\r\n");
                    HandleChat(str2, str3[1]);

                    break;

                //Movement
                case 0x14:
                    if (player.Walkbuff_time == 0)
                    {
                        player.Face = data[1];
                        World.SendToAll(new QueDele(player.Serial, player.Map, new ChangeFace(player.Serial, player.Face).Compile()));
                    }
                    else
                    {
                        player.Loc = player.Loc;
                        player.Map = player.Map;
                        string text2 = "지금은 걸을수 없습니다. " + "                    ";
                        SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text2.Count(), text2).Compile());
                    }
                    break;
                case 0x15:
                    if (player.Walkbuff_time == 0)
                    {
                        if (Server.tickcount.ElapsedMilliseconds - player.lastmove < 150)                        
                        {
                            return;
                        }

                        long totalwalk = 0;
                        foreach (var loc in walktrace)
                        {
                            totalwalk += loc;
                        }

                        //  if(player.Name == "GM최후")
                        //  Console.WriteLine(string.Format("{0},{1}   {2}", px, py, Server.tickcount.ElapsedMilliseconds));           
                        if (Server.tickcount.ElapsedMilliseconds - player.lastmove > player.m_MoveSpeed)                        
                        {
                            p = new PacketReader(data, data.Count(), true);
                            player.Face = p.ReadInt16();
                            var px = p.ReadInt16();
                            var py = p.ReadInt16();

                            walktrace.Add(Server.tickcount.ElapsedMilliseconds - player.lastmove);
                            player.lastmove = Server.tickcount.ElapsedMilliseconds;
                            /*
                            if (walktrace.Count > 3 && totalwalk < 1600)
                            {
                                SendPacket(new MoveSpriteWalkChar(player.Serial, player.Face, player.X, player.Y).Compile());
                                return;
                            }

                            while (walktrace.Count > 4)
                                walktrace.Remove(walktrace.ElementAt(0)); 
                                   */
                            if (World.Dist2d(new Point2D(px, py), player.m_Loc) > 3)
                            {
                                SendPacket(new MoveSpriteWalkChar(player.Serial, player.Face, player.X, player.Y).Compile());
                                return;
                            }
                            player.X = px;
                            player.Y = py;

                            World.SendToAll(new QueDele(player.Serial, player.Map, new MoveSpriteWalkChar(player.Serial, player.Face,
                            player.X, player.Y).Compile()));
                            parseFace();
                        }
                        else
                        {
                            SendPacket(new MoveSpriteWalkChar(player.Serial, player.Face, player.X, player.Y).Compile());
                        }
                    }                                     
                    break;

                //Stats
                case 0x2C:
                    player.AddStat(ref player.m_Str);
                    SendPacket(new UpdateCharStats(player).Compile());
                    break;
                case 0x2D:
                    player.AddStat(ref player.m_Men);
                    SendPacket(new UpdateCharStats(player).Compile());
                    break;
                case 0x2E:
                    player.AddStat(ref player.m_Dex);
                    SendPacket(new UpdateCharStats(player).Compile());
                    break;
                case 0x2F:
                    player.AddStat(ref player.m_Vit);
                    SendPacket(new UpdateCharStats(player).Compile());
                    break;


                case 0x03: //Login
                    var result = TryLogin(data);
                    switch (result[0])
                    {
                        case '1':
                            var puser2 = result.Remove(0, 1).Split(',');
                            player = handler.add.Where(xe => xe.Value.Name == puser2[0]).FirstOrDefault().Value;
                            player.client = this;
                            player.loggedIn = true;
                            LoadPlayer();
                            SendPacket(new CloseLogin().Compile());
                            break;
                        case '2':
                            SendPacket(new WrongPass().Compile());
                            break;
                        case '3':
                            SendPacket(new WrongID().Compile());
                            break;
                        case '4': //invalid chars
                            SendPacket(new WrongID().Compile());
                            break;
                        case '5':                            
                            SendPacket(new AlreadyOnline().Compile());
                            break;
                        case '6':
                            player = new Player(this);
                            player.CreateBeginner(result.Remove(0, 1));
                            firstlogin = true;
                            LoadPlayer();
                            player.Pass = Cryption.CreateSaltedSHA256(player.Pass, player.Name);
                            try
                            {
                                handler.add.TryAdd(player.Name, player);
                                var touch = handler.add.Where(xe => xe.Key == player.Name).FirstOrDefault();
                                touch.Value.client = this;
                                touch.Value.loggedIn = true;
                                //   BinaryIO.SavePlayer(player);
                                // World.DBConnect.InsertPlayer(player);
                            }
                            catch
                            {
                                Console.WriteLine("failed to insert new player");
                            }
                            SendPacket(new CloseLogin().Compile());
                            break;
                    }
                    break;

                case 0x49: //delete magic
                    var magdelslot = data[1];
                    player.DeleteMagic(magdelslot);
                    break;

                case 0x26: //swap magic
                    if (data.Count() < 4)
                        player.SwapMagic(data[1], 0);
                    else
                        player.SwapMagic(data[1], data[3]);
                    break;

                case 0xff:
                    SendPacket(new PlayMusic(1002).Compile());
                    if (true) //Login
                    {
                        //         SendPacket(new CloseLogin().Compile());

                        //           SendPacket(new LoadWorld(1, 0x12, 0x12, new byte[] { 00, 04, 00, 00, 00, 00, 00, 00, 00, 03 },
                        //               01, 01, 00, 03, 04, 00, 00,
                        //               new byte[] { 0x69, 0x70, 0x69, 0x67, 0x6d, 0x79, 0x31, 0x2e, 0x6d, 0x61, 0x70, 00 }).Format());
                        break;

                    }
                case 0x01:
                    // 강제 접속종료 처리
                    var plr = handler.add.Where(xe => xe.Value != null && xe.Value.Name == player.Name.ToUpper()).FirstOrDefault().Value;
                    plr.loggedIn = false;
                    World.w_server.Disconnect(plr.client.connection);
                    
                    break;
            }
        }
        static object WriteLock = new object();
        public static void WriteBug(string bug)
        {
            lock (WriteLock)
            {
                System.IO.StreamWriter sr = new System.IO.StreamWriter("bugs.txt", true);
                sr.WriteLine(bug);
                sr.Close(); sr.Dispose();
            }
        }
        public static void 물약(string chat)
        {
            lock (WriteLock)
            {
                System.IO.StreamWriter sr = new System.IO.StreamWriter("물약.txt", true);
                sr.WriteLine(chat);
                sr.Close(); sr.Dispose();
            }
        }
        public string TryLogin(byte[] data)
        {
            byte[] user = new byte[10];
            byte[] pass = new byte[10];
            Buffer.BlockCopy(data, 1, user, 0, 10);
            Buffer.BlockCopy(data, 33, pass, 0, 10);

            int usercnt = 0, passcnt = 0;
            for (int x = 0; x < 10; x++)
            {
                if (user[x] == 00)
                    break;
                //if ((int)user[x] > 127)
                //    return "4";
                usercnt++;
            }
            for (int x = 0; x < 10; x++)
            {
                if (pass[x] == 00)
                    break;
                // if ((int)pass[x] > 127)
                //    return "4";
                passcnt++;
            }

            //string test = Encoding.UTF8.GetString(user, 0, usercnt).ToUpper().Trim();//
            string test = Encoding.Default.GetString(user, 0, usercnt).ToUpper().Trim();
            var test2 = Encoding.Default.GetString(pass, 0, passcnt);

            //SHA1

            //if (!System.Text.RegularExpressions.Regex.IsMatch(test, "^[가-힇a-zA-Z0-9]+$"))
            //return "4";

            // test : 아이디가 3글자 이상 되어야 함
            // test2 : 비밀번호가 5글자 이상 되어야 함
            //if (test.Count() < 3 || test2.Count() < 5)
            //return "4";

            if (handler.HasPlayer(test))
                return "5";

#if NOSQL
            return "6" + test + "," + test2;
#endif

            //  var info = BinaryIO.LoadName(test);
            var infos = handler.add.Where(xe => xe.Value.Name == test).FirstOrDefault().Value;
            if (infos == null)
                return "6" + test + "," + test2;

            List<string> info = new List<string>();
            info.Add(infos.Name); info.Add(infos.Pass);
            if (info != null && info[0] == test)
            {
                if (info[1].Count() <= 10)
                {
                    if (info[1] != test2)
                        return "2";
                }
                else
                {
                    if (!Cryption.CheckHashPass(info[1], test, test2))
                        return "2";
                }

                return "1" + test + "," + test2;
            }
            else
            {

                return "6" + test + "," + test2;
            }
        }
        public static void ClearItem(Object ii)
        {
            script.item.Item aa;
            Thread.Sleep(300000);//300초후에 아이템삭제
            try
            {
                aa = (script.item.Item)ii;
            }
            catch (InvalidCastException)
            {
                aa = null;
            }

            if (aa.InvSlot == -1)
            {

                World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0xff, 20737, (short)"청소완료    ".Count(), "청소완료    ".ToString()).Compile()));
                aa.DeleteGround();
                Console.WriteLine("바닥청소완료");

            }
            
        }
    }
}
