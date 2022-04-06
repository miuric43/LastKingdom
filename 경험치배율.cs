using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LKCamelot.net;
using LKCamelot.util;
using LKCamelot.model;
using LKCamelot.script;

using LKCamelot.io;

namespace LKCamelot
{
    public partial class 경험치배율 : Form
    {
        public 경험치배율()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Server.Exp_Ratio = Int32.Parse(textBox1.Text);
                var temp = Server.Exp_Ratio;                
                string chat = "<알림> : " + "경험치배율이 " + textBox1.Text + " 배로 적용되었습니다" + "                                      ";
                World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0xff, 20737, (short)chat.Count(), chat).Compile()));
            }
            else
            {
                return;
            }
        }
    }
    
}
