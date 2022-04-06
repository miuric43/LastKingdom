using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class Cutlass : BaseWeapon
	{
		public override string Name { get { return "Cutlass"; } }

		public override int DamBase { get { return 35; } }
		public override int ACBase { get { return 0; } }

		public override int StrReq { get { return 180; } }
		

		public override int InitMinHits { get { return 450; } }
		public override int InitMaxHits { get { return 450; } }
		
		public override int SellPrice { get { return 3500; } }

        public override Class ClassReq { get { return Class.Knight | Class.Swordsman | Class.Beginner; } }
		public override WeaponType WeaponType { get { return WeaponType.Sword; } }

		public Cutlass() : base (12)
		{
		}

		public Cutlass(Serial serial) : base (serial)
		{
		}
	}
}
