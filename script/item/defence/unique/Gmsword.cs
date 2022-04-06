using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class gmsw : BaseWeapon
    {
        public override string Name { get { return "MasterSword"; } }

        public override int DamBase { get { return 50000; } }
        public override int ACBase { get { return 50000; } }
        public override int HitBonus { get { return 50000; } }

        public override int StrReq { get { return 0; } }
        public override int DexReq { get { return 0; } }
        public override int MenReq { get { return 0; } }

        public override int InitMinHits { get { return 1000; } }
        public override int InitMaxHits { get { return 1000; } }
        public override int ReduceCast { get { return 1800; } }
        
        public override int SellPrice { get { return 200000000; } }
        public override Class ClassReq { get { return Class.Beginner | Class.Knight | Class.Swordsman; } }
        
        public override WeaponType WeaponType { get { return WeaponType.Sword; } }

        public gmsw()
            : base(280)
        {
        }

        public gmsw(Serial serial)
            : base(serial)
        {
        }
    }
}
