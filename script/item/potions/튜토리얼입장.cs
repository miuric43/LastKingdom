using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public class 튜토리얼입장 : BasePotion
    {
        public override string Name { get { return "튜토리얼입장권" + "       "; } }
        public override ulong BuyPrice { get { return 250; } }

        public 튜토리얼입장()
            : base(26)
        {
        }

        public 튜토리얼입장(Serial serial)
            : base(serial)
        {
            m_ItemID = 26;
        }

        public override void Use(Player player)
        {
              if (Quantity >= 12)
              {
                  player.Loc = new Point2D(9, 15);
                  player.Map = "튜토리얼";
            Quantity = Quantity - 1;
            
            
        }
              else if (Quantity >= 3 && Quantity <= 11)
              {
                  player.Loc = new Point2D(9, 15);
                  player.Map = "튜토리얼";
                  Quantity = Quantity - 1;
                  string itdrop = "튜토리얼입장권이" + (Quantity - 1) + "개남았습니다" + "                                          ";
                  player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());
              }
              else
              {
                  player.Loc = new Point2D(9, 15);
                  player.Map = "튜토리얼";
                  base.Use(player);
              }
    }
}
}
