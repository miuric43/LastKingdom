using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class UAmulet : BaseArmor
    {
        public override string Name { get { return "UNIQUE AMULET"; } }

        public override int DamBase { get { return 150; } }
        public override int ACBase { get { return 0; } }

        public override int StrBonus { get { return 55; } }
        public override int MenBonus { get { return 55; } }
        public override int DexBonus { get { return 55; } }
        public override int VitBonus { get { return 55; } }
        //public override int ReduceSwing { get { return 150; } }
        public override int ReduceCast { get { return 150; } }


        public override int StrReq { get { return 0; } }
        public override int DexReq { get { return 0; } }
        public override int LevelReq { get { return 206 - (Stage + 5); } }

        public override int InitMinHits { get { return 65; } }
        public override int InitMaxHits { get { return 65; } }
        public override ulong BuyPrice { get { return 80000000; } }
        public override int SellPrice { get { return 50000000; } }

        public override Class ClassReq { get { return 0; } }
        public override ArmorType ArmorType { get { return ArmorType.Amulet; } }

        public UAmulet()
            : base(47)
        {
        }

        public UAmulet(Serial serial)
            : base(serial)
        {
        }
    }
}
