﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.spells
{
    public class GM : Spell
    {
        public override string Name { get { return "GM"; } }
        public override int SpellLearnedIcon { get { return 80; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Casted; } }

        public override int DamBase { get { return 0; } }
        public override int DamPl { get { return 0; } }
        public override int ManaCost { get { return -48; } }
        public override int ManaCostPl { get { return 2; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Wizard; } }
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
                    194,  //oncast
                    0,  //moving
                    0,  //impact
                    0,  //thickness
                    0,  //type
                    0,  //speed
                    0  //streak
                    );
            }
        }

        public GM()
        {

        }
    }
}