namespace LKCamelot.script.spells
{
    public class GM8 : Spell
    {
        public override string Name { get { return "GM8"; } }
        public override int SpellLearnedIcon { get { return 80; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Casted; } }

        public override int DamBase { get { return 60000; } }
        public override int DamPl { get { return 500; } }
        public override int ManaCost { get { return -78; } } //78
        public override int ManaCostPl { get { return 6; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Shaman; } }
        public override int menCoff { get { return 10; } }
        public override Buff BuffEffect
        {
            get
            {
                var tbuff = new Buff();
                tbuff.Hit = 95;
                tbuff.Hitpl = 9;
                return tbuff;
            }
        }
        public override bool Cast(LKCamelot.model.Player player)
        {

            CheckLevelUp(player);
            player.AddBuff(this);
            return true;
        }
        public override SpellSequence Seq
        {
            get
            {
                return new SpellSequence(
                    217,  //oncast
                    0,  //moving
                    0,  //impact
                    0,  //thickness
                    3,  //type
                    30,  //speed
                    0  //streak
                    );
            }
        }

        public GM8()
        {
        }
    }
}