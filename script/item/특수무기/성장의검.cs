using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
    public class 성장의검 : BaseWeapon, IProc
    {
        public override string Name { get { return "성장의검"; } }

        public override int DamBase { get { return 380 + (Quantity / 50); } }
        public override int ACBase { get { return 0; } }

        public override int StrReq { get { return 3000; } }
        public override int DexReq { get { return 3000; } }

        public override int SellPrice { get { return 750000000; } }

        public override int InitMinHits { get { return 500; } }
        public override int InitMaxHits { get { return 500; } }

        public override Class ClassReq { get { return Class.Knight; } }
        public override WeaponType WeaponType { get { return WeaponType.Sword; } }

        public int Proc(Player player, script.monster.Monster mob, Player play = null)
        {
            int take = 0;
            Point2D targetLoc = (play != null) ? play.Loc : mob.m_Loc;
            if (Util.Random(Stage, 100) >= 100 * (10 * 0.1))
            {
                take += Util.Dice(((Stage + Stage) * 500 + (Stage + Stage)), 50, (player.GetStat("str") + player.GetStat("dex")) / 32);
                int mobile = Serial.NewMobile;
                World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)targetLoc.X, (short)targetLoc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 189 }, 0).Compile()));
                var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 1300, player.m_Map, new DeleteObject(mobile).Compile());
                tmp.tempser = mobile;
                World.TickQue.Add(tmp);
            }
            return take;
        }

        public 성장의검()
            : base(181)
        {
        }

        public 성장의검(Serial serial)
            : base(serial)
        {
        }
    }
}
