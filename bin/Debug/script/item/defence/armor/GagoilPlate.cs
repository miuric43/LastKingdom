using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class GagoilPlate : BaseArmor
    {
        public override string Name { get { return "GAGOIL PLATE"; } }

        public override int DamBase { get { return 0; } }
        public override int ACBase { get { return 250; } }

        public override int StrReq { get { return 1020; } }
        public override int DexReq { get { return 450; } }


        public override int InitMinHits { get { return 300; } }
        public override int InitMaxHits { get { return 300; } }


        public override int APStage
        {
            get
            {
                var ret = 0;
                if (Parent != null)
                {
                    if (Parent.Class.HasFlag(Class.Knight) || Parent.Class.HasFlag(Class.Swordsman))
                        ret = 4;


                }
                return ret;
            }
        }
        public override int SellPrice { get { return 250000; } }


        public override Class ClassReq { get { return Class.Swordsman | Class.Knight; } }
        public override ArmorType ArmorType { get { return ArmorType.Armor; } }

        public GagoilPlate()
            : base(229)
        {
        }

        public GagoilPlate(Serial serial)
            : base(serial)
        {
        }
    }
}
