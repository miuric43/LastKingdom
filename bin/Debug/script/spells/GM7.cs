namespace LKCamelot.script.spells
{
    public class GM7 : Spell
    {
        public override string Name { get { return "GM7"; } }
        public override int SpellLearnedIcon { get { return 80; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Target; } }

        public override int DamBase { get { return 60000; } }
        public override int DamPl { get { return 500; } }
        public override int ManaCost { get { return -78; } } //78
        public override int ManaCostPl { get { return 6; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Shaman; } }
        public override int menCoff { get { return 10; } }
        public override SpellSequence Seq
        {
            get
            {
                return new SpellSequence(
                    0,  //oncast
                    0,  //moving
                    218,  //impact
                    0,  //thickness
                    3,  //type
                    30,  //speed
                    0  //streak
                    );
            }
        }

        public GM7()
        {
        }
    }
}