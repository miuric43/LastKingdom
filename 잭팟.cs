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
    public partial class 잭팟 : Form
    {
        public 잭팟()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            Server.quiz_time = int.Parse(textBox1.Text) * 1000;
            long temp = Server.quiz_time / 1000;


            if (temp > 9)
            {
                string chat = "퀴즈문제 : " + Server.quiz_q + "                        ";
                World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0xff, 20737, (short)chat.Count(), chat).Compile()));

                string chat2 = "퀴즈상품 : " + Server.quiz_p + "                        ";
                World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0xff, 20737, (short)chat2.Count(), chat2).Compile()));

                

            }
            MessageBox.Show("퀴즈 출력시간이 " + temp + "초로 설정되었습니다.                       ");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Question = textBox2.Text;
            Server.quiz_q = Question;
            MessageBox.Show("퀴즈문제가 " + Question + "으로 설정되었습니다.                       ");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Answer = textBox3.Text;
            Server.quiz_a = Answer;
            MessageBox.Show("퀴즈정답이 " + Answer + "으로 설정되었습니다.                       ");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ulong Product = ulong.Parse(textBox4.Text);
            Server.quiz_p = Product;
            MessageBox.Show("퀴즈상품이 " + Product + "으로 설정되었습니다.                       ");            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Server.quiz_start = true;
            MessageBox.Show("퀴즈가 시작되었습니다!                       ");
            string chat = "퀴즈가 시작되었습니다!                       ";
            World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0xff, 20737, (short)chat.Count(), chat).Compile()));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Server.quiz_start = false;
            Server.quiz_time_temp = 0;
            MessageBox.Show("퀴즈가 중지되었습니다!                       ");
            string chat = "퀴즈가 중지되었습니다!                       ";
            World.SendToAll(new QueDele("all", new UpdateChatBox(0x08, 0xff, 20737, (short)chat.Count(), chat).Compile()));            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
