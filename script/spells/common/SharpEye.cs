namespace LKCamelot.script.spells
{
    public class SharpEye : Spell
    {
        public override string Name { get { return "SHARP EYE"; } } //entrace of dun
        public override int SpellLearnedIcon { get { return 8; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Casted; } }

        public override int DamBase { get { return 0; } }
        public override int DamPl { get { return 0; } }
        public override int ManaCost { get { return 70; } }
        public override int ManaCostPl { get { return 6; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Shaman | LKCamelot.library.Class.Beginner | LKCamelot.library.Class.Knight | LKCamelot.library.Class.Swordsman | LKCamelot.library.Class.Wizard; } }




        public override SpellSequence Seq
        {
            get
            {
                return new SpellSequence(
                    0,  //oncast
                    0,  //moving
                    6,  //impact
                    0,  //thickness
                    3,  //type
                    0,  //speed
                    0  //streak
                    );
            }
        }

        public SharpEye()
        {
        }
    }
}