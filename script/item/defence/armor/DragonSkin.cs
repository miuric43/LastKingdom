using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class DragonSkin : BaseArmor
    {
        public override string Name { get { return "DRAGON SKIN"; } }

        public override int DamBase { get { return 0; } }
        public override int ACBase { get { return 250; } }

        public override int StrReq { get { return 925; } }
        public override int DexReq { get { return 752; } }
        public override int MenReq { get { return 450; } }

        public override int InitMinHits { get { return 300; } }
        public override int InitMaxHits { get { return 300; } }


        public override int APStage
        {
            get
            {
                var ret = 0;
                if (Parent != null)
                {
                    if (Parent.Class.HasFlag(Class.Swordsman))
                        ret = 4;
                    else if (Parent.Class.HasFlag(Class.Shaman) || Parent.Class.HasFlag(Class.Wizard))
                        ret = 3;

                }
                return ret;
            }
        }
        public override int SellPrice { get { return 250000; } }

        public override int LevelReq { get { return 110; } }
        public override Class ClassReq { get { return Class.Wizard | Class.Shaman; } }
        public override ArmorType ArmorType { get { return ArmorType.Armor; } }

        public DragonSkin()
            : base(228)
        {
        }

        public DragonSkin(Serial serial)
            : base(serial)
        {
        }
    }
}
