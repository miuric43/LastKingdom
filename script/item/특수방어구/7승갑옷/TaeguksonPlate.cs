using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class TaeguksonPlate : BaseArmor, IAura
    {
        public override string Name { get { return "TAEGUKSON PLATE"; } }

        public override int DamBase { get { return 0; } }
        public override int ACBase { get { return 400; } }

        public override int StrBonus { get { return 55; } }
        public override int DexBonus { get { return 55; } }
        public override int MenBonus { get { return 55; } }
        public override int VitBonus { get { return 55; } }

        public override int InitMinHits { get { return 300; } }
        public override int InitMaxHits { get { return 300; } }
        public int Aura()
        {
            return 177;
        }

        public override void Equip(Player player)
        {
            base.Equip(player);
            World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
        }

        public override void Unequip(Player player, int slot)
        {
            base.Unequip(player, slot);
            World.SendToAll(new QueDele(player.Map, new SetObjectEffectsPlayer(player).Compile()));
        }
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

        public override int SellPrice { get { return 10000000; } } 

        public override int LevelReq { get { return 231; } }
        public override Class ClassReq { get { return Class.Shaman; } }
        public override ArmorType ArmorType { get { return ArmorType.Armor; } }

        public TaeguksonPlate()
            : base(249)
        {
        }

        public TaeguksonPlate(Serial serial)
            : base(serial)
        {
        }
    }
}
