namespace LKCamelot.script.spells
{
    public class Execution3 : Spell, ISingle
    {
        public override string Name { get { return "EXECUTION3"; } }
        public override int SpellLearnedIcon { get { return 51; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Target; } }

        public override int DamBase { get { return 1000; } }
        public override int DamPl { get { return 100; } }
        public override int ManaCost { get { return -78; } }
        public override int ManaCostPl { get { return 6; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Swordsman; } }
        public override int vitCoff { get { return 8; } }
        public override int Range
        {
            get
            {
                return 2;
            }
        }
        public override SpellSequence Seq
        {
            get
            {
                return new SpellSequence(
                    0,  //oncast
                    0,  //moving
                    192,  //impact
                    0,  //thickness
                    3,  //type
                    0,  //speed
                    0  //streak
                    );
            }
        }

        public Execution3()
        {
        }
    }
}