using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.monster
{
    public class 성물 : Monster
    {
        public override string Name { get { return "성물"; } }
        public override int HP { get { return 100000; } }
        public override int Dam { get { return 0; } }
        public override int AC { get { return 0; } }
        public override int Hit { get { return 6000; } }
        public override int XP { get { return 0; } }
        public override int Color { get { return 0; } }
        public override int SpawnTime { get { return 30000; } }
        public override Race Race { get { return Race.Demon; } }    
        public override int WalkSpeed { get { return Int32.MaxValue; } }
        public override int AttackSpeed { get { return Int32.MaxValue; } }
        public override long Point { get { return 0 + Point2; } }

        public override LootPack Loot
        {
            get
            {
                return new LootPack(new LootPackEntry[]
                {
                   
                });
            }
        }

        public 성물()
           
            : base(140)
        {
        }

        public 성물(Serial temps, int x, int y, string map)
            : this(temps)
        {
            m_MonsterID = 140;
            m_Loc = new Point2D(x, y);
            m_SpawnLoc = new Point2D(m_Loc.X, m_Loc.Y);
            m_Map = map;
            m_Serial = temps;
        }

        public 성물(Serial serial)
            : base(serial)
        {
        }
    }
}
