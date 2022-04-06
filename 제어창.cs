using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using LKCamelot.net;
using LKCamelot.util;
using LKCamelot.model;
using LKCamelot.script;

using LKCamelot.io;

namespace LKCamelot
{
    public partial class 제어창 : Form
    {
        public Player player;
        public PlayerHandler handler;
        public static Server w_server;
        public 제어창()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                if (textBox1.Text == "")
                    return;
            string chat = "[공지사항] :" + textBox1.Text + "                      ";
            World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0xff, 20737, (short)chat.Count(), chat).Compile()));
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("worldAdmin.txt", true, Encoding.GetEncoding("euc-kr")))
            {
                sw.WriteLine(string.Format(",{0}", Nametext.Text));

            }
            //String pppfile = str2[1] + "님이 관리자로 등록 되었습니다." + "                                             ";
            //SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)pppfile.Count(), pppfile).Compile());            
            var plr2 = handler.add.Where(xe => xe.Value != null && xe.Value.GetFreeSlots() > 3 && xe.Value.Name == Nametext.ToString() && xe.Value.loggedIn).FirstOrDefault().Value;
            string achat = "연구소 서버관리자로 임명되었습니다." + "                                             ";
            plr2.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat.Count(), achat).Compile());
            string achat2 = "기타내용" + "                                             ";
            plr2.client.SendPacket(new UpdateChatBox(0xff, 0xff, 1, (short)achat2.Count(), achat2).Compile());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
        }

        private void 경험치배율ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            경험치배율 frm = new 경험치배율();
            frm.Show();
        }

        private void 제어창_Load(object sender, EventArgs e)
        {

        }

        private void 잭팟설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            잭팟 frm = new 잭팟();
            frm.Show();
        }
    }
}
