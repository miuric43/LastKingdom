using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class Miner : BaseArmor
	{
        public override string Name { get { return "Miner"; } }

		public override int DamBase { get { return 0; } }
		public override int ACBase { get { return 0; } }

		public override int StrReq { get { return 0; } }
		public override int DexReq { get { return 0; } }

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 80; } }
        public override ulong BuyPrice { get { return 3000; } }
        public override int SellPrice { get { return 3000; } }

		public override Class ClassReq { get { return 0; } }
		public override ArmorType ArmorType { get { return ArmorType.Armor; } }
        public override int APStage { get { return -1; } }
        //public override int APStage { get { return 6; } }
        public Miner() : base (5)
		{
		}

        public Miner(Serial serial)
            : base(serial)
		{
            
            
            m_ItemID = 5;
        
		}
	}
}
