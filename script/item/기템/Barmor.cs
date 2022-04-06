using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class Barmor : BaseArmor
	{
		public override string Name { get { return "Beginner Armor"; } }

		public override int DamBase { get { return 0; } }
		public override int ACBase { get { return 190; } }

		public override int StrReq { get { return 0; } }
		public override int DexReq { get { return 0; } }

		public override int InitMinHits { get { return 180; } }
		public override int InitMaxHits { get { return 180; } }
        public override ulong BuyPrice { get { return 15000; } }
        public override int SellPrice { get { return 3000; } }
        public override int APStage
        {
            get
            {
                var ret = 2;
                if (Parent != null)
                {
                    if (Parent.Class == (Class.Swordsman | Class.Knight))
                        ret = 2;
                    else if (Parent.Class == (Class.Shaman | Class.Wizard))
                        ret = 2;
                    else if (Parent.Class.HasFlag(Class.Beginner))
                        ret = 0;
                }
                
                return ret;
            }
        }
		public override Class ClassReq { get { return 0; } }
		public override ArmorType ArmorType { get { return ArmorType.Armor; } }

		public Barmor() : base (6)
		{
		}

		public Barmor(Serial serial) : base (serial)
		{
            m_ItemID = 6;
		}
	}
}
