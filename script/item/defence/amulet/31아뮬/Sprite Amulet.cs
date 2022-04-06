using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class SpriteAmulet : BaseArmor
    {
        public override string Name { get { return "SPRITE AMULET"; } }

     
        public override int ACBase { get { return 0; } }
        public override int MPBonus { get { return 0; } }
        public override int HitBonus { get { return 0; } }

        
        public override int MenBonus { get { return 31; } }
       

        

        public override int StrReq { get { return 0; } }
        public override int DexReq { get { return 0; } }
        public override int LevelReq { get { return 10; } }

        public override int InitMinHits { get { return 65; } }
        public override int InitMaxHits { get { return 65; } }
        public override ulong BuyPrice { get { return 200000; } }
        public override int SellPrice { get { return 100000; } }

        public override Class ClassReq { get { return 0; } }
        public override ArmorType ArmorType { get { return ArmorType.Amulet; } }

        public SpriteAmulet()
            : base(2)
        {
        }

        public SpriteAmulet(Serial serial)
            : base(serial)
        {
        }
    }
}
