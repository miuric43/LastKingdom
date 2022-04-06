using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.monster
{
    public class 튜토 : Monster
    {
        public override string Name { get { return "연습상대"; } }
        public override int HP { get { return 15; } }
        public override int Dam { get { return 1; } }
        public override int AC { get { return 0; } }
        public override int XP { get { return 1; } }
        public override int Hit { get { return 3; } }
        public override int Color { get { return 0; } }
        public override int SpawnTime { get { return 30000; } }
        public override Race Race { get { return Race.Demon; } }
        public virtual int WalkSpeed { get { return 500; } }
        
        public override LootPack Loot
        {
            get
            {
                return new LootPack(new LootPackEntry[]
                {
                    new LootPackEntry(1.0, typeof(script.item.SmallShield), "15d10+225", 1, 1, 1),
                    new LootPackEntry(1.0, typeof(script.item.Hood), "15d10+225", 1, 1, 1),
                    new LootPackEntry(2.0, typeof(script.item.WoodenSword), "15d10+225", 1, 1, 1),
                    new LootPackEntry(2.0, typeof(script.item.BambooKnife), "15d10+225", 1, 1, 1),
                    new LootPackEntry(5.0, typeof(script.item.Buckler), "15d10+225", 1, 1, 1),
                    new LootPackEntry(5.0, typeof(script.item.Rag), "15d10+225", 1, 1, 1),
                    new LootPackEntry(15.0, typeof(script.item.Gold), "10d30+100", 40, 1, 1),
                });
            }
        }

        public 튜토()
            : base(233)
        {
        }

        public 튜토(Serial temps, int x, int y, string map)
            : this(temps)
        {
            
            m_MonsterID = 233;
            m_Loc = new Point2D(x, y);
            m_SpawnLoc = new Point2D(m_Loc.X, m_Loc.Y);
            m_Map = map;
            m_Serial = temps;
        }

        public 튜토(Serial serial)
            : base(serial)
        {
        }
    }
    public class 건물 : Monster
    {
        public override string Name { get { return "GM"; } }
        public override int HP { get { return 15000; } }
        public override int Dam { get { return 0; } }
        public override int AC { get { return 10000; } }
        public override int Hit { get { return 60000; } }
        public override int XP { get { return 0; } }
        public override int Color { get { return 0; } }
        public override int SpawnTime { get { return 30000; } }
        public override Race Race { get { return Race.Animal; } }
        public override int WalkSpeed { get { return Int32.MaxValue; } }
        public override LootPack Loot
        {
            get
            {
                return new LootPack(new LootPackEntry[]
                {
                    
                });
            }
        }

        public 건물()
            : base(234)
        {
        }

        public 건물(Serial temps, int x, int y, string map)
            : this(temps)
        {
            m_MonsterID = 234;
            m_Loc = new Point2D(x, y);
            m_SpawnLoc = new Point2D(m_Loc.X, m_Loc.Y);
            m_Map = map;
            m_Serial = temps;
        }

        public 건물(Serial serial)
            : base(serial)
        {
        }
    }
}
