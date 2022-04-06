using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class Unknown : BaseWeapon
    {
        public override string Name { get { return "Unknown"; } }

        public override int DamBase { get { return 350; } }
        public override int ACBase { get { return 0; } }

        
        public override int LevelReq { get { return 101; } }

        public override int InitMinHits { get { return 1000; } }
        public override int InitMaxHits { get { return 1000; } }

        public override ulong BuyPrice { get { return 10000000; } }
        public override int SellPrice { get { return 5000000; } }

        public override Class ClassReq { get { return Class.Beginner; } }
        public override WeaponType WeaponType { get { return WeaponType.Sword; } }

        public Unknown()
            : base(181)
        {
        }

        public Unknown(Serial serial)
            : base(serial)
        {
        }
    }
}
