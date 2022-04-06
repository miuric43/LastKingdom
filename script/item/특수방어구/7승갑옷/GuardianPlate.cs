using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class GuardianPlate : BaseArmor, IAura
    {
        public override string Name { get { return "GUARDIAN PLATE"; } }

        public override int DamBase { get { return 0; } }
        public override int ACBase { get { return 400; } }

        public override int ReduceSwing { get { return 150; } }
        public override int ReduceCast { get { return 150; } }

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
        public override int APStage { get { return 4; } }

        public override int SellPrice { get { return 10000000; } } 

        public override int LevelReq { get { return 231; } }
        public override Class ClassReq { get { return Class.Knight; } }
        public override ArmorType ArmorType { get { return ArmorType.Armor; } }

        public GuardianPlate()
            : base(248)
        {
        }

        public GuardianPlate(Serial serial)
            : base(serial)
        {
        }
    }
}
