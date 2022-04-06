using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class MagicianAmulet : BaseArmor
    {
        public override string Name { get { return "MAGICIAN AMULET"; } }

     
        public override int ACBase { get { return 0; } }
        public override int MPBonus { get { return 0; } }
        public override int HitBonus { get { return 0; } }

        
        public override int MenBonus { get { return 36; } }
        

        

        public override int StrReq { get { return 0; } }
        public override int DexReq { get { return 0; } }
        public override int LevelReq { get { return 10; } }

        public override int InitMinHits { get { return 65; } }
        public override int InitMaxHits { get { return 65; } }
        public override ulong BuyPrice { get { return 1000000; } }
        public override int SellPrice { get { return 300000; } }

        public override Class ClassReq { get { return 0; } }
        public override ArmorType ArmorType { get { return ArmorType.Amulet; } }

        public MagicianAmulet()
            : base(238)
        {
        }

        public MagicianAmulet(Serial serial)
            : base(serial)
        {
        }
    }
}
