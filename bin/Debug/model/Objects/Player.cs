﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.library;

namespace LKCamelot.model
{
    public class Player : BaseObject
    {
        public LKCamelot.io.IOClient client;
        public Serial Serial;
        public string Name, Pass;
        public Class m_Class;
        public byte Stage, LightRad, Transparancy;
        public string m_Map;
        public Point2D m_Loc;
        public short Face;
        public ushort m_Str, m_Men, m_Dex, m_Vit, m_AC, m_Hit, m_Dam;
        public uint m_Extra;
        public short bStr, bMen, bDex, bVit;
        public ushort apStr, apMen, apDex, apVit;
        public short pStr, pMen, pDex, pVit;
        public int m_HP, m_MP, m_HPCur, m_MPCur;
        public short m_Level;
        public int m_XP, m_XPNext;
        public ulong m_Gold;
        private SpriteEquip m_eqFlags;
        public byte[] Buffs = new byte[] { 0, 0, 0, 0 };
        public List<script.spells.Spell> m_MagicLearned = new List<script.spells.Spell>();
        public List<script.spells.Spell> m_Buffs = new List<script.spells.Spell>();
        public List<Serial> InstancedObjects = new List<Serial>();
        public int m_AttackSpeed = 450;
        public int m_CastSpeed = 1500;
        public int m_MoveSpeed = 1;
        public int RegenTick = 15000;
        public long LastRegen = 0, autohittick = 0, lastmove = 0;
        public bool AutoMana, AutoHP, AutoHit, chat = false, TakeDam = false, AutoLoot = true, TakeDam2 = false, PlusXp = false, NormalXp = true, Slevel = false;
        public double AutoManaP, AutoHPP;
        public int m_Promo, BankTab = 0;
        public bool loggedIn = false;
        public int apistate = 0;
        public long keepalive = Server.tickcount.ElapsedMilliseconds;
        public List<long> pklastpk = new List<long>();
        public int pkhistory = 0;
        public long pklastred = 0;
        public int color;
        public long pkpinkktime = 0;
        public long pkPinkDelay = 20000;
        public long pkRedDelay = 1200000;
        public int musicNumber = 2;
        public string tempLocateMap;
        public Point2D tempLocate;
        public string guildName;
        public string guildTitle;
        public ulong m_Point;
        public ulong m_Tun;
        public string Bday;
        public ulong m_Bank;
        public int ACbuff = 0;
        public int Dambuff = 0;
        public int Hitbuff = 0;
        public int ACbuff_time = 0;
        public int Dambuff_time = 0;
        public int Hitbuff_time = 0;
        public int BHitbuff = 0;
        public int BHitbuff_time = 0;
        public List<String> member = new List<String>();
        public int BossMonster_time = 0;
        public int Walkbuff_time = 0;
        public int m_Win = 0;
        public int m_Loss = 0;
        public int m_PvPPoint;
        public List<String> Cmember = new List<String>();
        public int Warfare_time = 0;
        public System.Collections.Concurrent.ConcurrentDictionary<int, script.item.Item> Equipped2 =
            new System.Collections.Concurrent.ConcurrentDictionary<int, script.item.Item>();

        public int pktemp
        {
            get { return pklastpk.Count; }
        }
        public int Color
        {
            get
            {
                return color;
                /*
                int col = 0;
                if (pkpinkktime > 0)
                    col = 2;
                if (pklastpk.Count > 0)
                    col = 1;
                return col;
                */
            }
            set
            {
                if (color == value)
                {
                    return;

                }
                color = value;
                World.SendToAll(new QueDele(m_Map, new ChangeObjectSpritePlayer(this).Compile()));
            }
        }
        /*
        public int Color
        {
            get
            {
                return color;
                int col = 0;
                if (pkpinkktime > 0)
                    col = 2;
                if (pklastpk.Count > 0)
                    col = 1;
                return col;SpawnTime
            }
            set
            {
                if (color == value)
                    return;
                color = value;
                World.SendToAll(new QueDele(m_Map, new ChangeObjectSpritePlayer(this).Compile()));
            }
        }*/
        public string ProfessionString
        {
            get
            {
                string ret = "";
                
                if (guildName != "")
                    ret += "길드: " + guildName + "\n\t" + "                                                 ";                
                if (Level > 0)
                    ret += "내용없음" + "                      " + "\n\t";
                /*
                StringBuilder sb = new StringBuilder();
                if (guildName != "")
                    sb.Append(string.Format("가입된길드:{0}", guildName + "                      "));
                if (Level > 1)
                    sb.Append(string.Format("내용없음" + "                      "));
                if (Level > 2)
                    sb.Append(string.Format("내용없음" + "                      "));
                  */
                if (ret != "")
                    ret = ret.Substring(0, ret.Count() - 2);
                return ret;
            }
        }
        public Player Clone()
        {
            var temp = new Player();
            temp.Name = Name;
            temp.guildName = guildName;
            temp.guildTitle = guildTitle;
            temp.Bday = Bday;
            temp.m_Class = Class;
            temp.Stage = Stage;
            temp.LightRad = LightRad;
            temp.Transparancy = Transparancy;
            temp.m_Map = Map;
            temp.m_Loc = new Point2D(m_Loc.X, m_Loc.Y);
            temp.Face = Face;
            temp.m_Str = m_Str;
            temp.m_Men = m_Men;
            temp.m_Dex = m_Dex;
            temp.m_Vit = m_Vit;
            temp.m_Extra = Extra;
            temp.m_HPCur = m_HPCur;
            temp.m_MPCur = m_MPCur;
            temp.m_Level = m_Level;
            temp.m_XP = XP;
            temp.m_Gold = Gold;
            temp.m_Point = Point;
            temp.m_MagicLearned = m_MagicLearned.ConvertAll<script.spells.Spell>(item => item);
            temp.m_AttackSpeed = m_AttackSpeed;
            temp.m_CastSpeed = m_CastSpeed;            
            temp.m_Tun = Tun;
            temp.m_Bank = Bank;
            temp.color = Color;

            return temp;
        }

        public int AttackSpeed
        {
            get
            {
                var temp = m_AttackSpeed;
                foreach (var item in Equipped2.Values)
                    temp -= item.ReduceSwing;

                return temp;
            }
            set { m_AttackSpeed = value; }
        }

        public int CastSpeed
        {
            get
            {
                var temp = m_CastSpeed;
                foreach (var item in Equipped2.Values)
                    temp -= item.ReduceCast;

                if (Class == Class.Swordsman || Class == Class.Knight)
                   temp -= GetStat("men") / 10;
                

                if (temp <= 300)
                    temp = 300;

                return temp;
            }
            set { m_CastSpeed = value; }
        }

        public List<script.item.Item> Inventory
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.Parent == this || xe.Value.ParSer == this.Serial).Select(xe => xe.Value).ToList();
            }
        }

        public int Promo
        {
            get
            {
                if (m_Level < 101)
                    return 0;
                else
                    return ((m_Level - 101) / 20) + 1;
            }
        }
        public float PvPTotal
        {
            get
            {
                //if (m_Win == 0)
                  //  return 0F;
                if (m_Loss == 0)
                    return 100F;
                else
                    return (m_Loss / (m_Win + m_Loss)) * 100F;
            }
        }
        public string FullClass
        {
            get
            {
                if (Class == Class.Knight)
                {
                    if (Promo == 0) return "Knight";
                    if (Promo == 1) return "Dragon";
                    if (Promo == 2) return "Nova";
                    if (Promo == 3) return "The Lord";
                    if (Promo == 4) return "Royal Knight";
                    if (Promo == 5) return "Saint Knight";
                    if (Promo == 6) return "Crux";
                    if (Promo == 7) return "Guardianship";
                    if (Promo == 8) return "Paladin";
                    if (Promo == 9) return "Black Swordsman";
                    if (Promo == 10) return "Guardian Deity";
                    if (Promo == 11) return "Skull Knight";
                    if (Promo == 12) return "Berserker";
                }
                if (Class == Class.Swordsman)
                {
                    if (Promo == 0) return "Swordsman";
                    if (Promo == 1) return "Knight SwordGM승자";
                    if (Promo == 2) return "Elementalica";
                    if (Promo == 3) return "Dragon Cloud";
                    if (Promo == 4) return "Angela";
                    if (Promo == 5) return "Hae-DongSamurang";
                    if (Promo == 6) return "Back-DuGumsung";
                    if (Promo == 7) return "DaeungDaegum";
                    if (Promo == 8) return "Kensei";
                    if (Promo == 9) return "Royal Assassin";
                    if (Promo == 10) return "Fist of the South Star";
                    if (Promo == 11) return "Fist of the North Star";
                    if (Promo == 12) return "Transcended";
                }
                if (Class == Class.Shaman)
                {
                    if (Promo == 0) return "Shaman";
                    if (Promo == 1) return "Bodhisattva";
                    if (Promo == 2) return "Yogi";
                    if (Promo == 3) return "Diabolic Taoist";
                    if (Promo == 4) return "Zodiac Monk";
                    if (Promo == 5) return "Poong Back";
                    if (Promo == 6) return "Saint Purgatory";
                    if (Promo == 7) return "TaeGukSon";
                    if (Promo == 8) return "Saint";
                    if (Promo == 9) return "Wild Mage";
                    if (Promo == 10) return "Blooming Lotus";
                    if (Promo == 11) return "Seeded One";
                    if (Promo == 12) return "Eternal";
                }
                if (Class == Class.Wizard)
                {
                    if (Promo == 0) return "Wizard";
                    if (Promo == 1) return "Priest";
                    if (Promo == 2) return "Bishop";
                    if (Promo == 3) return "Martyr";
                    if (Promo == 4) return "Holy Wizard";
                    if (Promo == 5) return "Godly Messenger";
                    if (Promo == 6) return "Sacrosant";
                    if (Promo == 7) return "Glorianship";
                    if (Promo == 8) return "Grand Master";
                    if (Promo == 9) return "Archangel";
                    if (Promo == 10) return "Prophet";
                    if (Promo == 11) return "The Fallen";
                    if (Promo == 12) return "Godhand";
                }
                if (Class == Class.Beginner)
                {
                    if (Promo == 1) return "Jesus";
                    if (Promo == 2) return "Jesus2";
                    if (Promo == 3) return "Jesus3";
                    if (Promo == 4) return "Jesus4";
                    if (Promo == 5) return "Jesus5";
                    if (Promo == 6) return "Jesus6";
                    if (Promo == 7) return "Jesus7";
                    if (Promo == 8) return "Jesus8";
                    if (Promo == 9) return "Jesus9";
                    if (Promo == 10) return "Jesus10";
                    if (Promo == 11) return "Jesus11";
                    if (Promo == 12) return "Jesus12";
                }

                return "Beginner";
            }
        }
        public string PVPRank
        {
            get
            {
                if (m_PvPPoint > 0)
                {
                    if (PVPSubRank == 1) return "9급";
                    if (PVPSubRank == 2) return "8급";
                    if (PVPSubRank == 3) return "7급";
                    if (PVPSubRank == 4) return "6급";
                    if (PVPSubRank == 5) return "5급";
                    if (PVPSubRank == 6) return "4급";
                    if (PVPSubRank == 7) return "3급";
                    if (PVPSubRank == 8) return "2급";
                    if (PVPSubRank == 9) return "1급";
                    if (PVPSubRank == 10) return "1단";
                    if (PVPSubRank == 11) return "2단";
                    if (PVPSubRank >= 12) return "3단";

                }
               

                return "초보자";
            }
        }
        public int PVPSubRank
        {
            get
            {
                if (m_PvPPoint > 0)
                {
                    if (m_PvPPoint >= 0    && m_PvPPoint <= 999) return 0;
                    if (m_PvPPoint >= 1000 && m_PvPPoint <= 4999) return 1;
                    if (m_PvPPoint >= 5000 && m_PvPPoint <= 14999) return 2;
                    if (m_PvPPoint >= 15000 && m_PvPPoint <= 19999) return 3;
                    if (m_PvPPoint >= 20000 && m_PvPPoint <= 29999) return 4;
                    if (m_PvPPoint >= 30000 && m_PvPPoint <= 49999) return 5;
                    if (m_PvPPoint >= 50000 && m_PvPPoint <= 64999) return 6;
                    if (m_PvPPoint >= 65000 && m_PvPPoint <= 79999) return 7;
                    if (m_PvPPoint >= 80000 && m_PvPPoint <= 99999) return 8;
                    if (m_PvPPoint >= 100000 && m_PvPPoint <= 149999) return 9;
                    if (m_PvPPoint >= 150000 && m_PvPPoint <= 299999) return 10;
                    if (m_PvPPoint >= 300000 && m_PvPPoint <= 499999) return 11;
                    if (m_PvPPoint >= 500000) return 12;

                }


                return 0;
            }
        }
        public script.item.Item Qitem
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 1).FirstOrDefault().Value;
            }
        }
        public script.item.Item Weapon
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 27).FirstOrDefault().Value;
            }
        }
        public script.item.Item Head
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 25).FirstOrDefault().Value;
            }
        }
        public script.item.Item Armor
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 28).FirstOrDefault().Value;
            }
        }
        public script.item.Item Amulet
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 26).FirstOrDefault().Value;
            }
        }
        public script.item.Item Ring
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 30).FirstOrDefault().Value;
            }
        }
        public script.item.Item Shields
        {
            get
            {
                return World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 29).FirstOrDefault().Value;
            }
        }
        public short PromoLevel
        {
            get
            {
                if (Promo == 0)
                    return Level;
                if (Promo > 0)
                {
                    return (short)(((Level - 100) - (Promo * 20 - 20)));
                }

                return 1;
            }
        }

        public short Level
        {
            get { return m_Level; }
            set
            {
                if (value > 231)
                    value = 231;

                if (firstlevel && m_Level != value)
                {
                    m_Level = value;
                }
                m_Level = value;
                firstlevel = true;

            }
        }
        bool firstlevel = false;

        public byte[] Appearance
        {
            get
            {
                var temp = new byte[10];
                temp[1] = ClassBase;
                if ((Class & Class.Knight) != 0 || (Class & Class.Beginner) != 0)
                {
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Shield) != 0)
                        temp[3] = 1;
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Sword) != 0)
                        temp[4] = 1;
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Hammer) != 0)
                        temp[5] = 1;
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Axe) != 0)
                        temp[6] = 1;
                }
                if ((Class & Class.Swordsman) != 0)
                {
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Sword) != 0
                        || (eqFlags & LKCamelot.library.SpriteEquip.Hammer) != 0
                        || (eqFlags & LKCamelot.library.SpriteEquip.Axe) != 0)
                        temp[4] = 1;
                }
                if ((Class & Class.Shaman) != 0 || (Class & Class.Wizard) != 0)
                {
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Cane) != 0)
                        temp[7] = 1;
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Cane2) != 0)
                        temp[7] = 2;
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Spear) != 0)
                        temp[7] = 1;
                    if ((eqFlags & LKCamelot.library.SpriteEquip.Spear2) != 0)
                        temp[7] = 2;
                }

                var armor = World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot == 28)
                    .FirstOrDefault();
                if (armor.Value != null)
                {
                    var tempee = (armor.Value as script.item.BaseArmor).APStage;
                    temp[2] = (byte)tempee;
                }
                return temp;
            }
        }

        public List<script.item.Item> Equipped
        {
            get
            {
                return World.NewItems.Where(xe => (xe.Value.ParSer == Serial || xe.Value.Parent == this) && xe.Value.InvSlot >= 25 && xe.Value.InvSlot <= 30).Select(xe => xe.Value).ToList();
            }
        }

      /*  public uint Extra
        {
            get { return m_Extra; }
            set
            {
                if (value <= 0)
                    value = 0;
                m_Extra = value;
                if (m_Level == 231)
                {
                    if (m_XP >= 49163521)
                    {
                        m_XP = 0;
                        
                    }
                }
            }
        }*/
        public uint Extra
        {
            get { return m_Extra; }
            set
            {
                if (value <= 0)
                    value = 0;
                m_Extra = value;
            }
        }
        public void AddStat(ref ushort stat)
        {
            if (m_Extra == 0 || stat == ushort.MaxValue)
                return;
            Extra--;

            stat++;
        }

        public void AddStat(ref ushort stat, ushort amount)
        {
            if (m_Extra == 0 || amount + stat >= ushort.MaxValue)
                return;

            Extra -= amount;

            stat += amount;
        }

        public ushort GetStat(string stat, bool bas = false)
        {
            
            
            uint temp = 0;
            if (stat == "str")
            {
                if (m_Class == Class.Beginner)
                    temp += 5;
                if (m_Class == Class.Knight)
                    temp += 30;
                if (m_Class == Class.Swordsman)
                    temp += 20;
                if (m_Class == Class.Wizard)
                    temp += 15;
                if (m_Class == Class.Shaman)
                    temp += 15;
                foreach (var item in Equipped2.Values)
                    temp += (uint)item.StrBonus;
                temp += m_Str;
                
                return temp < ushort.MaxValue ? (ushort)temp : ushort.MaxValue;
            }
            if (stat == "men")
            {
                if (m_Class == Class.Beginner)
                    temp += 5;
                if (m_Class == Class.Knight)
                    temp += 10;
                if (m_Class == Class.Swordsman)
                    temp += 5;
                if (m_Class == Class.Wizard)
                    temp += 30;
                if (m_Class == Class.Shaman)
                    temp += 25;
                foreach (var item in Equipped2.Values)
                    temp += (uint)item.MenBonus;
                temp += m_Men;
                return temp < ushort.MaxValue ? (ushort)temp : ushort.MaxValue;
            }
            if (stat == "dex")
            {
                if (m_Class == Class.Beginner)
                    temp += 5;
                if (m_Class == Class.Knight)
                    temp += 15;
                if (m_Class == Class.Swordsman)
                    temp += 30;
                if (m_Class == Class.Wizard)
                    temp += 15;
                if (m_Class == Class.Shaman)
                    temp += 20;
                foreach (var item in Equipped2.Values)
                    temp += (uint)item.DexBonus;
                temp += m_Dex;
                return temp < ushort.MaxValue ? (ushort)temp : ushort.MaxValue;
            }
            if (stat == "vit")
            {
                if (m_Class == Class.Beginner)
                    temp += 5;
                else
                    temp += 10;
                foreach (var item in Equipped2.Values)
                    temp += (uint)item.VitBonus;
                temp += m_Vit;
                return temp < ushort.MaxValue ? (ushort)temp : ushort.MaxValue;
            }
            return 1;
        }

        public string SQLSaveString()
        {
            string temp = "";
            temp += "\'" + Convert.ToInt32(Serial) + "\', ";
            temp += "\'" + Name.ToString() + "\', ";
            temp += "\'" + Pass.ToString() + "\', ";
            temp += "\'" + Convert.ToInt32(Class) + "\', ";
            temp += "\'" + Stage + "\', ";
            temp += "\'" + LightRad + "\', ";
            temp += "\'" + Transparancy + "\', ";
            temp += "\'" + Map + "\', ";
            temp += "\'" + m_Loc.X + "\', ";
            temp += "\'" + m_Loc.Y + "\', ";
            temp += "\'" + Face + "\', ";
            temp += "\'" + apStr + "\', ";
            temp += "\'" + apMen + "\', ";
            temp += "\'" + apDex + "\', ";
            temp += "\'" + apVit + "\', ";
            temp += "\'" + Extra + "\', ";
            temp += "\'" + HP + "\', ";
            temp += "\'" + HPCur + "\', ";
            temp += "\'" + MP + "\', ";
            temp += "\'" + MPCur + "\', ";
            temp += "\'" + Level + "\', ";
            temp += "\'" + XP + "\', ";
            temp += "\'" + Gold + "\', ";
            temp += "\'" + Point + "\', ";
            temp += "\'" + guildName.ToString() + "\', ";
            temp += "\'" + guildTitle.ToString() + "\', ";
            temp += "\'" + Tun + "\', ";
            temp += "\'" + Bday.ToString() + "\', ";
            temp += "\'" + Bank + "\', ";
            temp += "\'" + MagicLearnedString() + "\', "; //Magic Learned
            //temp += "\'" + BuffString() + "\', "; //buff string
            temp += "\'450\', ";
            temp += "\'1500\', ";
            //temp += "\'" + AttackSpeed + "\', ";
            //temp += "\'" + CastSpeed + "\', ";
            temp = temp.Substring(0, (temp.Count() - 2));
            return temp;
        }

        public string MagicLearnedString()
        {
            if (m_MagicLearned.Count == 0)
                return "0";

            var temp = "";
            foreach (var spell in m_MagicLearned)
            {
                temp += spell.GetType().Name.ToString() + " " + spell.Slot + " " + spell.SLevel2 + " " + spell.Level + "-";
            }

            return temp;
        }

        public void LoadMagic(string mag)
        {
            if (mag.Count() > 3)
            {
                var temp = mag.Split('-');
                foreach (var spell in temp)
                {
                    if (spell == "")
                        continue;
                    var temp2 = spell.Split(' ');
                    var activatorstring = "";
                    if (temp2[0].IndexOf("LKCamelot.script.spells.") != -1)
                        activatorstring = null;
                    else
                        activatorstring = "LKCamelot.script.spells.";
                    object tempspell = null;
                    try
                    {
                        tempspell = Activator.CreateInstance(Type.GetType(activatorstring + temp2[0]));
                    }
                    catch { continue; }
                    (tempspell as script.spells.Spell).Slot = Convert.ToInt32(temp2[1]);
                    (tempspell as script.spells.Spell).SLevel2 = Convert.ToInt32(temp2[2]);
                    (tempspell as script.spells.Spell).Level = Convert.ToInt32(temp2[3]);
                    m_MagicLearned.Add((tempspell as script.spells.Spell));
                    //    client.SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
                }
            }
        }

        public string BuffString()
        {
            var temp = "6";
            return temp;
        }

        public ulong Gold
        {
            get { return m_Gold; }
            set
            {
                m_Gold = value;
                client.SendPacket(new LKCamelot.model.GoldChange(this.m_Gold).Compile());
            }
        }
        public ulong Point
        {
            get { return m_Point; }
            set
            {
                m_Point = value;
                client.SendPacket(new LKCamelot.model.PointChange(this.m_Point).Compile());
            }
        }

        public ulong Tun
        {
            get { return m_Tun; }
            set
            {
                m_Tun = value;
                client.SendPacket(new LKCamelot.model.TunChange(this.m_Tun).Compile());
            }
        }
        public ulong Bank
        {
            get { return m_Bank; }
            set
            {
                m_Bank = value;
                client.SendPacket(new LKCamelot.model.BankChange(this.m_Bank).Compile());
            }
        }
        public int PvPPoint
        {
            get { return m_PvPPoint; }
            set
            {
                m_PvPPoint = value;
                client.SendPacket(new LKCamelot.model.PvPPointChange(this.m_PvPPoint).Compile());
            }
        }
        /*public ulong Bday
        {
            get { return m_Bday; }
            set
            {
                m_Tun = value;
                client.SendPacket(new LKCamelot.model.BdayChange(this.m_Bday).Compile());
            }
        }*/
        public SpriteEquip eqFlags
        {
            get
            {
                SpriteEquip flags = 0;
                var equiped = World.NewItems.Where(xe => xe.Value.m_Parent != null && xe.Value.m_Parent == this
                    && xe.Value.InvSlot >= 25 && xe.Value.InvSlot <= 30)
                    .Select(xe => xe.Value);

                foreach (var gear in equiped)
                {
                    if (gear is script.item.BaseWeapon)
                    {
                        if ((gear as script.item.BaseWeapon).WeaponType == script.item.WeaponType.Axe)
                            flags |= library.SpriteEquip.Axe;
                        if ((gear as script.item.BaseWeapon).WeaponType == script.item.WeaponType.Sword)
                            flags |= library.SpriteEquip.Sword;
                        if ((gear as script.item.BaseWeapon).WeaponType == script.item.WeaponType.Hammer)
                            flags |= library.SpriteEquip.Hammer;
                        if ((gear as script.item.BaseWeapon).WeaponType == script.item.WeaponType.Cane)
                            flags |= library.SpriteEquip.Cane;
                        if ((gear as script.item.BaseWeapon).WeaponType == script.item.WeaponType.Cane2)
                            flags |= library.SpriteEquip.Cane2;
                        if ((gear as script.item.BaseWeapon).WeaponType == script.item.WeaponType.Spear)
                            flags |= library.SpriteEquip.Spear;
                        if ((gear as script.item.BaseWeapon).WeaponType == script.item.WeaponType.Spear2)
                            flags |= library.SpriteEquip.Spear2;
                    }
                    else if (gear is script.item.BaseArmor)
                    {
                        if ((gear as script.item.BaseArmor).ArmorType == script.item.ArmorType.Shield && (Class & Class.Swordsman) == 0
                            && (Class & Class.Shaman) == 0 && (Class & Class.Wizard) == 0)
                            flags |= library.SpriteEquip.Shield;
                    }
                }

                return flags;
            }
            set
            {
                m_eqFlags = value;
                World.SendToAll(new QueDele(Map, new ChangeObjectSpritePlayer(this).Compile()));
            }
        }

        public byte ClassBase
        {
            get
            {
                if (Class == Class.Beginner)
                    return 0;
                if (Class == Class.Knight)
                    return 1;
                if (Class == Class.Swordsman)
                    return 2;
                if (Class == Class.Wizard)
                    return 3;
                if (Class == Class.Shaman)
                    return 4;
                return 0;
            }
        }

        public Class Class
        {
            get { return m_Class; }
            set
            {
                if (value != m_Class)
                {
                    m_Class = value;
                    var removes = m_MagicLearned.Where(xe => xe.ClassReq != 0 && (xe.ClassReq & Class) == 0).Select(xe => xe).ToList();
                    foreach (var sp in removes)
                    {
                        m_MagicLearned.Remove(sp);
                        client.SendPacket(new DeleteSpell(sp.Slot).Compile());
                    }
                    var buffremoves = m_Buffs.Where(xe => xe.ClassReq != 0 && (xe.ClassReq & Class) == 0).Select(xe => xe).ToList();
                    foreach (var sp in buffremoves)
                    {
                        m_Buffs.Remove(sp);
                    }
                    if (!(value == Class.Beginner))
                    {
                        RemoveChangeGear();
                        client.SendPacket(new ChangeObjectSprite(Serial, 0, Appearance).Compile());
                        
                        client.SendPacket(new SetLevelGold(this).Compile());
                        client.SendPacket(new UpdateCharStats(this).Compile());
                        World.SendToAll(new QueDele(m_Map, new SetObjectEffectsPlayer(this).Compile()));
                    }
                }
            }
        }

        public void RemoveChangeGear()
        {
            var equiped = World.NewItems.Where(xe => xe.Value.Parent == this
    && xe.Value.InvSlot >= 25 && xe.Value.InvSlot <= 30).Select(xe => xe.Value);

            foreach (var item in equiped)
            {
                if (item.ClassReq != 0 && (item.ClassReq & Class) == 0)
                    item.Unequip(this, item.InvSlot);
            }
        }

        public List<script.spells.Spell> MagicLearned
        {
            get { return m_MagicLearned; }
        }

        public void SwapItems(LKCamelot.script.item.Item initem, LKCamelot.script.item.Item outitem, int outslot)
        {
            if (initem != null)
            {
                if (outitem == null)
                {
                    client.SendPacket(new DeleteItemSlot((byte)initem.InvSlot).Compile());
                    initem.InvSlot = (byte)outslot;
                    client.SendPacket(new AddItemToInventory2(initem).Compile());
                    try
                    {
                        //    World.DBConnect.WriteQue.Enqueue(() => World.DBConnect.UpdateItem(initem));
                        //  World.DBConnect.UpdateItem(initem);
                    }
                    catch { Console.WriteLine("Update Item on Swap1 Failed"); }
                }
                else
                {
                    client.SendPacket(new DeleteItemSlot((byte)initem.InvSlot).Compile());
                    client.SendPacket(new DeleteItemSlot((byte)outitem.InvSlot).Compile());
                    outitem.InvSlot = (byte)initem.InvSlot;
                    initem.InvSlot = (byte)outslot;
                    client.SendPacket(new AddItemToInventory2(initem).Compile());
                    client.SendPacket(new AddItemToInventory2(outitem).Compile());
                    try
                    {
                        //     World.DBConnect.WriteQue.Enqueue(() => World.DBConnect.UpdateItem(initem));
                        //     World.DBConnect.WriteQue.Enqueue(() => World.DBConnect.UpdateItem(outitem));
                        //  World.DBConnect.UpdateItem(initem);
                        // World.DBConnect.UpdateItem(outitem);
                    }
                    catch { Console.WriteLine("Update Item on Swap2 Failed"); }
                }

            }
        }

        public void DeleteMagic(int slot)
        {
            if (slot < 40)
            {
                var spell = m_MagicLearned.Where(xe => xe.Slot == slot).FirstOrDefault();
                if (spell != null)
                {
                    m_MagicLearned.Remove(spell);
                    client.SendPacket(new DeleteSpell(slot).Compile());
                }
            }
        }

        public void SwapMagic(int inslot, int outslot)
        {
            var spell1 = m_MagicLearned.Where(xe => xe.Slot == inslot).FirstOrDefault();
            var target1 = m_MagicLearned.Where(xe => xe.Slot == outslot).FirstOrDefault();

            if (spell1 != null)
            {
                if (target1 == null)
                {
                    m_MagicLearned[m_MagicLearned.IndexOf(spell1)].Slot = outslot;
                    client.SendPacket(new DeleteSpell(inslot).Compile());
                    client.SendPacket(new CreateSlotMagic2(m_MagicLearned[m_MagicLearned.IndexOf(spell1)]).Compile());
                }
                else
                {
                    m_MagicLearned[m_MagicLearned.IndexOf(spell1)].Slot = outslot;
                    m_MagicLearned[m_MagicLearned.IndexOf(target1)].Slot = inslot;
                    client.SendPacket(new DeleteSpell(inslot).Compile());
                    client.SendPacket(new DeleteSpell(outslot).Compile());
                    client.SendPacket(new CreateSlotMagic2(m_MagicLearned[m_MagicLearned.IndexOf(spell1)]).Compile());
                    client.SendPacket(new CreateSlotMagic2(m_MagicLearned[m_MagicLearned.IndexOf(target1)]).Compile());
                }
            }
        }

        public Point2D Loc
        {
            get { return m_Loc; }
            set
            {
                m_Loc = value;
                var CheckStep = script.map.Portal.Portals.Where(xe => xe.Map == Map
                    && (xe.Locs[0].X == X && xe.Locs[0].Y == Y)).FirstOrDefault();

                if (CheckStep != null)
                {
                    m_Loc.X = CheckStep.ArriveLoc.X;
                    m_Loc.Y = CheckStep.ArriveLoc.Y;
                    World.SendToAll(new QueDele(Serial, m_Map, new DeleteObject(Serial).Compile()));
                    Map = CheckStep.Dest;
                }
            }
        }

        public short X
        {
            get { return (short)m_Loc.X; }
            set
            {
                m_Loc.X = value;
                Loc = m_Loc;
            }
        }

        public short Y
        {
            get { return (short)m_Loc.Y; }
            set
            {
                m_Loc.Y = value;
                Loc = m_Loc;
            }
        }

        public bool LearnSpell(script.item.BaseSpellbook spellb)
        {
            var learned = m_MagicLearned.Find(xe => xe.Name.ToLower() == spellb.SpellTaught.Name.ToLower());
            if (learned != null)
            {
                if (GetStat("men") >= spellb.MenReq + (spellb.MenReqPl * learned.Level)
                    && GetStat("dex") >= spellb.DexReq + (spellb.DexReqPl * learned.Level)
                    && GetStat("str") >= spellb.StrReq + (spellb.StrReqPl * learned.Level)
                    && Level >= spellb.LevelReq + (spellb.LevelReqPl * learned.Level)
                    && (spellb.ClassReq == 0 || ((Class & spellb.ClassReq) != 0))
                    && learned.Level < 12)
                {
                    learned.Level++;
                    client.SendPacket(new CreateSlotMagic2(learned).Compile());
                    return true;
                }
            }
            else if (m_MagicLearned.Count() < 40)
            {
                if (GetStat("men") >= spellb.MenReq && GetStat("dex") >= spellb.DexReq && Level >= spellb.LevelReq
                    && (spellb.ClassReq == 0 || ((Class & spellb.ClassReq) != 0)))
                {
                    var spell2l = spellb.SpellTaught;
                    spell2l.Slot = GetFreeSpellSlot();
                    m_MagicLearned.Add(spell2l);
                    client.SendPacket(new CreateSlotMagic2(spell2l).Compile());
                    return true;
                }
            }
            return false;
        }

        public string LoadMap
        {
            get
            {
                var temp = LKCamelot.model.Map.FullMaps.Where(xe => xe.Key == m_Map).FirstOrDefault().Value;
                return temp;
            }
        }

        public List<script.item.Item> LocalItems
        {
            get
            {
                return World.NewItems.Values.Where(xe => xe.m_Map != null
                && xe.m_Map == Map)
                .Select(xe => xe).ToList();
            }
        }

        public List<script.monster.Monster> LocalMonsters
        {
            get
            {
                return World.NewMonsters.Values.Where(xe => xe.m_Map != null
                && xe.m_Map == Map && xe.m_Alive)
                .Select(xe => xe).ToList();
            }
        }

        public List<Player> LocalPlayers
        {
            get
            {
                var LocalPlayers = client.connection._server.OnlineConnections.Where(xe => xe.Client != null && xe.Client.player != null &&
                       xe.Client.player.Map != null && xe.Client.player.loggedIn && xe.Client.player.m_Map == m_Map && xe.Client.player.Name != Name).Select(xe => xe.Client.player).ToList();
                return LocalPlayers;
            }
        }

        public string Map
        {
            get { return m_Map; }
            set
            {
                //      if (m_Map != value)
                //      {
                var oldMap = m_Map;
                m_Map = value;
                client.SendPacket(new LoadWorld(this, 1).Compile());

                InstancedObjects.Clear();
                foreach (var play in PlayerHandler.getSingleton().add)
                {
                    if (play.Value != null && play.Value.InstancedObjects.Contains(Serial))
                    {
                        World.SendToAll(new QueDele(Serial, oldMap, new DeleteObject(Serial).Compile()));
                        play.Value.InstancedObjects.Remove(Serial);
                    }
                }
                foreach (var item in LocalItems)
                {
                    if (item != null && !InstancedObjects.Contains(Serial))
                    {
                        client.SendPacket(new CreateItemGround2(item, item.m_Serial).Compile());
                        InstancedObjects.Add(item.m_Serial);
                    }
                }
                foreach (var item in LocalMonsters)
                {
                    if (item != null && !InstancedObjects.Contains(Serial))
                    {
                        client.SendPacket(new CreateMonster(item, item.m_Serial).Compile());
                        client.SendPacket(new SetObjectEffectsMonster(item).Compile());
                        InstancedObjects.Add(item.m_Serial);
                    }
                }
                var OldPlayers = client.connection._server.OnlineConnections.Where(xe => xe.Client != null && xe.Client.player != null &&
                       xe.Client.player.Map != null && xe.Client.player.loggedIn && xe.Client.player.m_Map == oldMap && xe.Client.player.Name != Name).Select(xe => xe.Client.player).ToList();
                foreach (var item in OldPlayers)
                {
                    if (item == null)
                        continue;

                    if (item.InstancedObjects.Contains(Serial))
                    {
                        item.InstancedObjects.Remove(Serial);
                        item.client.SendPacket(new DeleteObject(Serial).Compile());
                    }

                }
                foreach (var item in LocalPlayers)
                {
                    if (item == null)
                        continue;

                    if (!InstancedObjects.Contains(Serial))
                    {
                        client.SendPacket(new CreateChar(item, item.Serial).Compile());
                        client.SendPacket(new SetObjectEffectsPlayer(item).Compile());
                        InstancedObjects.Add(item.Serial);
                    }

                    if (!item.InstancedObjects.Contains(Serial))
                    {
                        item.client.SendPacket(new CreateChar(this, Serial).Compile());
                        item.client.SendPacket(new SetObjectEffectsPlayer(this).Compile());
                        item.InstancedObjects.Add(Serial);
                    }
                }


                LoadNPCs();
                World.SendToAll(new QueDele(m_Map, new SetObjectEffectsPlayer(this).Compile()));
            }
        }

        public byte[] BuffArray
        {
            get
            {
                byte[] buffs = new byte[4] { 0, 0, 0, 0 };
                int itrx = 0;
                foreach (var buff in m_Buffs)
                {
                    if (buff.Name == "MENTAL SWORD")
                        continue;
                    if (buff.Seq.OnCastSprite == 0)
                        continue;
                    if (itrx > 3)
                        break;

                    buffs[itrx++] = (byte)buff.Seq.OnCastSprite;
                }
                var aura = Equipped2.Where(xe => (xe.Value is script.item.IAura)).FirstOrDefault();
                if (aura.Value != null)
                {
                    buffs[3] = (byte)(aura.Value as script.item.IAura).Aura();
                }

                return buffs;
            }
        }

        public void LoadNPCs()
        {
            var npcs = World.NewNpcs.Where(xe => xe.Value.Map == Map).ToList();
            foreach (var npc in npcs)
            {
                client.SendPacket(new CreateNPC2(npc.Value).Compile());
                if (npc.Value.ChatString != "" && World.Dist2d(npc.Value.X, npc.Value.Y, X, Y) < 13)
                {
                    client.SendPacket(new UpdateChatBox(0xff, 0xff, 1,
                        (short)npc.Value.ChatString.Count(), npc.Value.ChatString).Compile());
                }
            }
        }

        public byte[] ByteeqFlags
        {
            get
            {
                var eqArr = new byte[7];
                if ((eqFlags & SpriteEquip.Shield) != 0)
                    eqArr[0] = 1;
                if ((eqFlags & SpriteEquip.Sword) != 0)
                    eqArr[1] = 1;
                if ((eqFlags & SpriteEquip.Hammer) != 0)
                    eqArr[2] = 1;
                if ((eqFlags & SpriteEquip.Axe) != 0)
                    eqArr[3] = 1;

                return eqArr;
            }
        }

        public void AddBuff(script.spells.Spell buff)
        {
            if (m_Buffs.Contains(buff))
                m_Buffs.Remove(buff);

            if (!m_Buffs.Contains(buff))
            {
                m_Buffs.Add(buff);
                World.SendToAll(new QueDele(Map, new LKCamelot.model.SetObjectEffectsPlayer(this).Compile()));
                client.SendPacket(new LKCamelot.model.UpdateCharStats(this).Compile());
            }
        }

        public int AC
        {
            get
            {
                int basac = 0 + ACbuff;//GetStat("dex") / 10;
                foreach (var eq in Equipped2.Values)
                {
                    if (eq is script.item.BaseArmor)
                    {
                        basac += (eq as script.item.BaseArmor).FullAC;
                    }
                    if (eq is script.item.BaseWeapon)
                    {
                        basac += (eq as script.item.BaseWeapon).FullAC;
                    }
                }
                foreach (var buff in m_Buffs)
                {
                    if (buff.BuffEffect != null)
                    {
                        basac += buff.BuffEffect.AC + (buff.BuffEffect.ACpl * buff.Level);
                        var tfloat = buff.BuffEffect.fAC + (buff.BuffEffect.fACpl * buff.Level);
                        tfloat = tfloat * basac;
                        basac += (int)tfloat;
                    }
                }
                return basac;
            }
        }
        public int Hit
        {
            get
            {
                var Bashit = GetStat("dex") + Level + Hitbuff + BHitbuff;
                foreach (var item in Equipped2.Values)
                    Bashit += item.HitBonus;

                if (Bashit > ushort.MaxValue)
                    Bashit = ushort.MaxValue;

                foreach (var buff in m_Buffs)
                {
                    if (buff.BuffEffect != null)
                    {
                        Bashit += buff.BuffEffect.Hit + (buff.BuffEffect.Hitpl * buff.Level);
                        var tfloat = buff.BuffEffect.fHit + (buff.BuffEffect.fHitpl * buff.Level);
                        tfloat = tfloat * Bashit;
                        Bashit += (int)tfloat;
                    }
                }

                return Bashit;
            }


        }
        public int Dam
        {
            
            get


            {
                                
                    int basdam = GetStat("str") / 5 + Dambuff;                           
                foreach (var eq in Equipped2.Values)
                    {
                        if (eq is script.item.BaseWeapon)
                        {
                            basdam += (eq as script.item.BaseWeapon).FullDam;
                        }
                        if (eq is script.item.BaseArmor)
                        {
                            basdam += (eq as script.item.BaseArmor).FullDam;
                        }
                    }

                    foreach (var buff in m_Buffs)
                    {
                        if (buff.BuffEffect != null)
                        {
                            basdam += buff.BuffEffect.Dam + (buff.BuffEffect.Dampl * buff.Level);
                            var tfloat = buff.BuffEffect.fDam + (buff.BuffEffect.fDampl * buff.Level);
                            tfloat = tfloat * basdam;
                            basdam += (int)tfloat;
                        }
                    }


                    return basdam;
                }
            
            
        }

        public int HP
        {
            get
            {

                int bashp = 30 + (GetStat("vit") * 30) + (Level * 4);
                if (Class == Class.Knight)//직업이 기사일때
                    bashp = 30 + (GetStat("vit") * 3) + (Level * 4);
                if (Class == Class.Swordsman)//직업이 검객일때
                    bashp = 30 + (GetStat("vit") * 3) + (Level * 4);
                if (Class == Class.Wizard)//직업이 위저드일때
                    bashp = 30 + (GetStat("vit") * 3) + (Level * 4);
                if (Class == Class.Shaman)//직업이 샤먼일때
                    bashp = 30 + (GetStat("vit") * 3) + (Level * 4);
                else//그외(평민)
                    bashp = 30 + (GetStat("vit") * 3) + (Level * 4);

                var temp = bashp;
                //var temp = 30 + (GetStat("vit") * 3) + (Level * 4);
                foreach (var item in Equipped2.Values)
                    temp += item.HPBonus;

                return temp;
            }
        }
        public int MP
        {
            get
            {
                var temp = 30 + (GetStat("men") * 3) + (Level * 4);
                foreach (var item in Equipped2.Values)
                    temp += item.MPBonus;

                return temp;
            }
        }

        public int MPCur
        {
            get { return m_MPCur; }
            set
            {
                if (value > MP)
                    value = MP;
                if (value != m_MPCur)
                {
                    m_MPCur = value;
                    client.SendPacket(new SetMP(this).Compile());
                }
            }
        }
        
        public int HPCur
        {
            get { return m_HPCur; }
            set
            {
                if (value > HP)
                    value = HP;

                if (m_Buffs.Where(xe => xe.Name == "MAGIC SHIELD" || xe.Name == "TEAGUE SHIELD").FirstOrDefault() != null
                    && value < m_HPCur)
                {
                    if (MPCur > (m_HPCur - value))
                    {
                        MPCur -= (m_HPCur - value);
                        return;
                    }
                }

                if (value <= 0)
                {
                    if (Color == 1 && pklastpk.Count >= 5)
                    {
                        script.item.Item[] eqds = new script.item.Item[Equipped2.Count];
                        Equipped2.Values.CopyTo(eqds, 0);
                        if (eqds.Length == 0)
                        {
                            script.item.Item[] invn = new script.item.Item[Inventory.Count];
                            foreach (var item in invn)
                            {
                                if (item.InvSlot > 24)
                                    continue;
                                if (item.Blessed)
                                    continue;
                                if (Util.Random(0, 100) <= 10)
                                {
                                    item.Drop(this);
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in eqds)
                            {
                                if (item.Blessed)
                                    continue;

                                var chance = 0;
                                if (pklastpk.Count >= 2)
                                    chance += 20;
                                for (int x = 0; x < pklastpk.Count - 2; x++) 
                                    chance += 10;
                                
                                if (Util.Random(0, 100) <= chance)
                                {
                                    script.item.Item outi;
                                    Equipped2.TryRemove(item.m_Serial, out outi);
                                    item.Drop(this);
                                    break;
                                }
                            }
                        }
                    }
                    if (this.Map != "대전장")
                    {
                        Loc = new Point2D(20, 20);
                        Map = "Rest";
                        value = 1;

                        string text2 = "당신은 죽었습니다." + "           ";

                        client.SendPacket(new UpdateChatBox(7, 0x65, 1, (short)text2.Count(), text2).Compile());
                        //  World.SendToAll(new QueDele(Serial, Map, new DeleteObject(Serial).Compile()));
                    }
                    if(this.Map == "대전장")
                    {
                        
                        Loc = new Point2D(20, 20);
                        Map = "Rest";
                        value = 1;
                        string text2 = "당신은 패배하였습니다." + "           ";
                        client.SendPacket(new UpdateChatBox(7, 0x65, 1, (short)text2.Count(), text2).Compile());
                        this.m_Loss += 1;


                        if (this.m_PvPPoint > 0)
                        {
                            this.m_PvPPoint -= 100;
                            Console.WriteLine("대전테스트");
                        }
                    }
                }

                if (value < m_HPCur)
                {
                    byte val = 0;
                    try
                    {
                        val = Convert.ToByte(((((float)m_HPCur / (float)HP) * 100) * 1));
                    }
                    catch
                    {
                        Console.WriteLine(string.Format("HP Bug {0} {1}", m_HPCur, HP));
                    }

                    World.SendToAll(new QueDele(Map, new HitAnimation(Serial,
                        val).Compile()));
                }

                //   m_HPCur = (ushort)value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
                m_HPCur = value;
                try
                {
                    client.SendPacket(new SetHP(this).Compile());
                }
                catch
                {
                }
            }
        }

        public int XPNext { get { return LKCamelot.library.NextLevelC.NextLevelTable[Level]; } }
        public int XP
        {
            get { return m_XP; }
            set
            {
                if (value != m_XP)                
                    client.SendPacket(new XPChange(value).Compile());
                    m_XP = value;
                
                if (value > XPNext)
                {
                    if (Level >= 1 && Level <= 230)
                    {
                        int mobile = Serial.NewMobile;
                        World.SendToAll(new QueDele(Map, new CreateMagicEffect(mobile, 1, (short)X, (short)Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 198 }, 0).Compile()));
                        var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 3000, m_Map, new DeleteObject(mobile).Compile());
                        tmp.tempser = mobile;
                        World.TickQue.Add(tmp);

                    }
                    if (Level >= 10 && Bday == null)
                    {
                        string text = "생년월일앞자리 6숫자를 등록하세요" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                        text = "@생년월일 123456 스페이스바 엔터" + "                 " ;
                        client.SendPacket(new UpdateChatBox(0xff, 10, 0, (short)text.Count(), text).Compile());
                    }
                    if (Level == 4 && Promo == 0)
                    {
                        string text = "이제 직업을 가질수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                        text = "전직하는 방법 '전직 기사'" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                        text = "!외치기를 사용해 사람들과 어울려 보세요" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level == 98 && Promo == 0)
                    {
                        string text = "이제 1승급을 진행하실수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                        text = "승급하는방법 '승급 신청' + '네'" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level == 119 && Promo == 1)
                    {
                        string text = "이제 2승급을 진행하실수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level == 139 && Promo == 2)
                    {
                        string text = "이제 3승급을 진행하실수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level == 159 && Promo == 3)
                    {
                        string text = "이제 4승급을 진행하실수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level == 179 && Promo == 4)
                    {
                        string text = "이제 5승급을 진행하실수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level == 199 && Promo == 5)
                    {
                        string text = "이제 6승급을 진행하실수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level == 219 && Promo == 6)
                    {
                        string text = "이제 7승급을 진행하실수 있습니다 @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Promo == 7 && XP <= 49163521)
                    {
                        string text = "이제 턴을 할수 있습니다. @이동 아론" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                        text = "아론앞에서 턴 이라고 말해보세요" + "                    ";
                        client.SendPacket(new UpdateChatBox(0x25, 0x65, 5, (short)text.Count(), text).Compile());
                    }
                    if (Level <= 98 && Promo == 0)
                    {
                        Level++;
                        Extra += (ushort)(5 + (Level / 10));
                    }
                    else if (Promo == 1 && Level < 120)
                    {
                        Level++;
                        Extra += 30;
                    }
                    else if (Promo == 2 && Level < 140)
                    {
                        Level++;
                        Extra += 50;
                    }
                    else if (Promo == 3 && Level < 160)
                    {
                        Level++;
                        Extra += 80;
                    }
                    else if (Promo == 4 && Level < 180)
                    {
                        Level++;
                        Extra += 120;
                    }
                    else if (Promo == 5 && Level < 200)
                    {
                        Level++;
                        Extra += 180;
                    }
                    else if (Promo == 6 && Level < 220)
                    {
                        Level++;
                        Extra += 260;
                    }
                    else if (Promo == 7 && Level < 231)
                    {
                        Level++;
                        Extra += 360;
                    }
                        /*
                    else if (Promo == 7 && Level == 231)
                    {
                        m_XP = 0;
                        Extra += 1;

                    }
                        /*
                    else if (Promo == 8 && Level < 260)
                    {
                        Level++;
                        Extra += 480;
                    }
                    else if (Promo == 9 && Level < 280)
                    {
                        Level++;
                        Extra += 620;
                    }
                    else if (Promo == 10 && Level < 300)
                    {
                        Level++;
                        Extra += 780;
                    }
                    else if (Promo == 11 && Level < 320)
                    {
                        Level++;
                        Extra += 960;
                    }
                    else if (Promo == 12 && Level < 340)
                    {
                        Level++;
                        Extra += 1160;
                    }*/
                    else
                    {
                        value = LKCamelot.library.NextLevelC.NextLevelTable[Level];
                        m_XP = value;
                        return;
                    }
                    client.SendPacket(new SetLevelGold(this).Compile());
                    client.SendPacket(new UpdateCharStats(this).Compile());
                }
            }
        }

        public int GetFreeSlots()
        {
            var inventory = World.NewItems.Where(xe => xe.Value.m_Parent != null
    && (xe.Value.m_Parent == this || xe.Value.ParSer == this.Serial)
    && xe.Value.InvSlot <= 24)
    .OrderBy(xe => xe.Value.InvSlot).ToList();

            return 24 - inventory.Count;
        }

        public int GetFreeSlot()
        {
            var inventory = World.NewItems.Where(xe => xe.Value.m_Parent != null
                && (xe.Value.m_Parent == this || xe.Value.ParSer == this.Serial)
                && xe.Value.InvSlot <= 24)
                .OrderBy(xe => xe.Value.InvSlot).ToList();

            if (inventory.Count() == 0)
                return 0;
            if (inventory.Count() >= 24)
                return -1;

            if (inventory != null)
            {
                int itr = 0;
                foreach (var item in inventory)
                {
                    if (item.Value.InvSlot == -1)
                        continue;
                    if (item.Value.InvSlot != itr)
                        return itr;
                    itr++;
                }
            }
            return inventory.Count();
        }

        public int FreeBankSlot
        {
            get
            {
                var content = BankContent;
                if (content.Count == 0)
                    return 40 + (BankTab * 12);

                int itr = 40 + (BankTab * 12);
                foreach (var item in content.OrderBy(xe => xe.InvSlot).ToList())
                {
                    if (item.InvSlot != itr)
                        return itr;
                    itr++;
                }

                return content.Count() + 40 + (BankTab * 12);
            }
        }

        public List<script.item.Item> BankContent
        {
            get
            {
                var bank = World.NewItems.Values.Where(xe => xe.m_Parent != null
                && (xe.m_Parent == this || xe.ParSer == this.Serial)
                && xe.InvSlot >= 40 + (BankTab * 12) && xe.InvSlot < 52 + (BankTab * 12))
                .OrderBy(xe => xe.InvSlot).ToList();
                return bank;
            }
        }

        public int GetFreeSpellSlot()
        {
            if (m_MagicLearned.Count() == 0)
                return 0;

            if (m_MagicLearned != null)
            {
                int itr = 0;
                foreach (var item in m_MagicLearned.OrderBy(xe => xe.Slot).ToList())
                {
                    if (item.Slot != itr)
                        return itr;
                    itr++;
                }
            }
            return m_MagicLearned.Count();
        }

        public Player()
        {
        }

        public Player(LKCamelot.io.IOClient iocli)
        {
            this.client = iocli;
        }

        public void Load()
        {

        }

        public void CreateBeginner(string Name)
        {
            this.Serial = Serial.NewMobile;
            this.Name = Name.Split(',')[0];
            this.Pass = Name.Split(',')[1];
            this.m_Class = Class.Beginner;
            this.m_Str = 0;
            this.m_Men = 0;
            this.m_Dex = 0;
            this.m_Vit = 0;
            this.m_Extra = 0;
            /*
            this.m_Str = 55550;
            this.m_Men = 55550;
            this.m_Dex = 55550;
            this.m_Vit = 55550;
            */
            this.HPCur = HP;
            this.MPCur = MP;
            this.Face = 4;

            //    this.m_Promo = 0;
            this.m_Level = 1;
            this.m_XP = 0;
            this.m_Gold = 0;
            this.m_Point = 0;
            this.m_Tun = 0;
            this.m_Bank = 0;
            //this.m_Bday = 0;
            this.m_Loc = new Point2D(18, 18);
            this.m_Map = "Rest";
            this.AttackSpeed = 450;
            this.CastSpeed = 1500;
            this.guildName = null;

            
                var tempspell = new script.spells.ComeBack();
                (tempspell as script.spells.Spell).Slot = GetFreeSpellSlot();
                (tempspell as script.spells.Spell).SLevel2 = 1;
                (tempspell as script.spells.Spell).Level = 1;
                m_MagicLearned.Add((tempspell as script.spells.Spell));
                client.SendPacket(new CreateSlotMagic2((tempspell as script.spells.Spell)).Compile());
            
            
                var tempspell2 = new script.spells.Trace();
                (tempspell2 as script.spells.Spell).Slot = GetFreeSpellSlot();
                (tempspell2 as script.spells.Spell).SLevel2 = 1;
                (tempspell2 as script.spells.Spell).Level = 1;
                m_MagicLearned.Add((tempspell2 as script.spells.Spell));
                client.SendPacket(new CreateSlotMagic2((tempspell2 as script.spells.Spell)).Compile());

                var tempspell3 = new script.spells.Teleport();
                (tempspell3 as script.spells.Spell).Slot = GetFreeSpellSlot();
                (tempspell3 as script.spells.Spell).SLevel2 = 1;
                (tempspell3 as script.spells.Spell).Level = 1;
                m_MagicLearned.Add((tempspell3 as script.spells.Spell));
                client.SendPacket(new CreateSlotMagic2((tempspell3 as script.spells.Spell)).Compile());
            

            var newitem2 = new script.item.튜토리얼입장().Inventory(this);
            World.NewItems.TryAdd(newitem2.m_Serial, newitem2);
            var newitem3 = new script.item.Knife().Inventory(this);
            World.NewItems.TryAdd(newitem3.m_Serial, newitem3);
            var newitem4 = new script.item.LifeDrug().Inventory(this);
            World.NewItems.TryAdd(newitem4.m_Serial, newitem4);
            var newitem5 = new script.item.LifeDrug().Inventory(this);
            World.NewItems.TryAdd(newitem5.m_Serial, newitem5);
            var newitem6 = new script.item.LifeDrug().Inventory(this);
            World.NewItems.TryAdd(newitem6.m_Serial, newitem6);
            var newitem7 = new script.item.LifeDrug().Inventory(this);
            World.NewItems.TryAdd(newitem7.m_Serial, newitem7);
            var newitem8 = new script.item.LifeDrug().Inventory(this);
            World.NewItems.TryAdd(newitem8.m_Serial, newitem8);

            /*      var newitem1 = new script.item.Dai().Inventory(this);
                  World.NewItems.TryAdd(newitem1.m_Serial, newitem1);
                  var newitem2 = new script.item.Dai().Inventory(this);
                  World.NewItems.TryAdd(newitem2.m_Serial, newitem2);
                  var newitem3 = new script.item.Dai().Inventory(this);
                  World.NewItems.TryAdd(newitem3.m_Serial, newitem3);
                  var newitem4 = new script.item.Dai().Inventory(this);
                  World.NewItems.TryAdd(newitem4.m_Serial, newitem4);*/


        }
    }
}
