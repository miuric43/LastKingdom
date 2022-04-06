using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using LKCamelot.net;
using LKCamelot.util;
using LKCamelot.model;
using LKCamelot.script;

using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
namespace LKCamelot.script.item
{
    public class FullMagicDrug : BasePotion
    {
        public override string Name { get { return "Full Magic Drug"; } }
        
       // public override string Name { get { return "Full Life Drug"; } }
        public override ulong BuyPrice { get { return 500; } }

        public FullMagicDrug() : base(23)
        {
        }

        public FullMagicDrug(Serial serial) : base(serial)
        {
            m_ItemID = 23;
        }

        public override void Use(Player player)
        {
            if (Quantity >= 12)
            {



                player.MPCur = player.MP;
                Quantity = Quantity - 1;



            }
            else if (Quantity >= 3 && Quantity <= 11)
            {



                player.MPCur = player.MP;
                Quantity = Quantity - 1;

                string itdrop = "큰마나물약이" + (Quantity - 1) + "개남았습니다" + "                                          ";
                player.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)itdrop.Count(), itdrop).Compile());


            }
            else
            {
                player.MPCur = player.MP;
                base.Use(player);
            }
        }
    }
}
