using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class FSofdk : BaseWeapon, IProc
    {
        public override string Name { get { return "Fire Sword of Death Knight"; } }

        public override int DamBase { get { return 113; } }
        public override int ACBase { get { return 0; } }

        public override int StrReq { get { return 215; } }
        public override int DexReq { get { return 315; } }

        public override int InitMinHits { get { return 500; } }
        public override int InitMaxHits { get { return 500; } }

        public override Class ClassReq { get { return Class.Knight | Class.Swordsman; } }
        public override WeaponType WeaponType { get { return WeaponType.Sword; } }

        public int Proc(Player player, script.monster.Monster mob, Player play = null)
        {
            int take = 0;
            Point2D targetLoc = (play != null) ? play.Loc : mob.m_Loc;
            if (Util.Random((20 + 15), 100) >= 100 * (10 * 0.1))
            {
                take += Util.Dice(((player.GetStat("str") + player.GetStat("dex")) / 1000), 10000, (player.GetStat("str") + player.GetStat("dex")) / 32);
                int mobile = Serial.NewMobile;
                World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)targetLoc.X, (short)targetLoc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 21 }, 0).Compile()));
                var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 1300, player.m_Map, new DeleteObject(mobile).Compile());
                tmp.tempser = mobile;
                World.TickQue.Add(tmp);
                
            }
            
            //player.HPCur = player.HP;
            return take;
        }

        public FSofdk()
            : base(181)
        {
        }

        public FSofdk(Serial serial)
            : base(serial)
        {
        }
    }
}
