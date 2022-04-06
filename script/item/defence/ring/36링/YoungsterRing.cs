using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class YoungsterRing : BaseArmor
    {
        public override string Name { get { return "RING OF YOUNGSTER"; } }

        public override int DamBase { get { return 0; } }
        public override int ACBase { get { return 0; } }
        public override int ReduceSwing { get { return 0; } }

        
        public override int VitBonus { get { return 36; } }

        public override int StrReq { get { return 0; } }
        public override int DexReq { get { return 0; } }

        public override int LevelReq { get { return 10; } }

        public override int InitMinHits { get { return 65; } }
        public override int InitMaxHits { get { return 65; } }
        public override ulong BuyPrice { get { return 1000000; } }
        public override int SellPrice { get { return 300000; } }

        public override Class ClassReq { get { return 0; } }
        public override ArmorType ArmorType { get { return ArmorType.Ring; } }

        public YoungsterRing()
            : base(240)
        {
        }

        public YoungsterRing(Serial serial)
            : base(serial)
        {
        }
    }
}
