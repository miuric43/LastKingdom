﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LKCamelot.script.spells
{
    public class GodBlade : Spell
    {
        public override string Name { get { return "GOD BLADE"; } }
        public override int SpellLearnedIcon { get { return 83; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Casted; } }

        public override int DamBase { get { return 0; } }
        public override int DamPl { get { return 0; } }
        public override int ManaCost { get { return 76; } }
        public override int ManaCostPl { get { return 0; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Beginner; } }
        public override Buff BuffEffect { 
            get 
            { 
                var tbuff = new Buff();
                tbuff.BuffType = BuffCase.ManaAsHP;
                tbuff.fDam = 0.85f;
                tbuff.fDampl = 0.20f;
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
                    183,   //oncast
                    0,  //moving
                    0,   //impact
                    0,   //thickness
                    0,   //type
                    3,   //speed
                    0    //streak
                    );
            }
        }

        public GodBlade()
        {
        }
    }
}
