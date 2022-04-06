using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class BlackSmith : BaseArmor
	{
        public override string Name { get { return "BLACKSMITH"; } }

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
        public override int APStage
        {
            get
            {
                var ret = 0;
                if (Parent != null)
                {
                    if (Parent.Class.HasFlag(Class.Knight) || Parent.Class.HasFlag(Class.Swordsman))
                        ret = 5;
                    else if (Parent.Class.HasFlag(Class.Shaman) || Parent.Class.HasFlag(Class.Wizard))
                        ret = 5;
                    else if (Parent.Class.HasFlag(Class.Beginner))
                        ret = 7;
                }
                return ret;
            }
        }
		public BlackSmith() : base (5)
		{
		}

        public BlackSmith(Serial serial)
            : base(serial)
		{
            
            
            m_ItemID = 5;
        
		}
	}
}
