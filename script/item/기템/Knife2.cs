using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class BKnife : BaseWeapon
	{
		public override string Name { get { return "Beginner KNIFE"; } }

		public override int DamBase { get { return 150; } }
		public override int ACBase { get { return 0; } }

		public override int StrReq { get { return 0; } }
		public override int DexReq { get { return 0; } }

		public override int InitMinHits { get { return 50; } }
		public override int InitMaxHits { get { return 50; } }
     		public override ulong BuyPrice { get { return 500; } }
     		public override int SellPrice { get { return 100; } }

		public override Class ClassReq { get { return Class.Beginner | Class.Knight | Class.Swordsman; } }
		public override WeaponType WeaponType { get { return WeaponType.Sword; } }

		public BKnife() : base (9)
		{
		}

		public BKnife(Serial serial) : base (serial)
		{
		}
	}
}
