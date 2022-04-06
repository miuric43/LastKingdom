namespace LKCamelot.script.spells
{
    public class Execution2 : Spell
    {
        public override string Name { get { return "EXECUTION2"; } }
        public override int SpellLearnedIcon { get { return 51; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Target; } }

        public override int DamBase { get { return 290; } }
        public override int DamPl { get { return 10; } }
        public override int ManaCost { get { return -67; } }
        public override int ManaCostPl { get { return 5; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Swordsman; } }
        
        public override int menCoff { get { return 12; } }
        public override SpellSequence Seq
        {
            get
            {
                return new SpellSequence(
                    0,  //oncast
                    0,  //moving
                    182,  //impact
                    0,  //thickness
                    3,  //type
                    16,  //speed
                    0  //streak
                    );
            }
        }

        public Execution2()
        {
        }
    }
}