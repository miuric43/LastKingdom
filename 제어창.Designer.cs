namespace LKCamelot
{
    partial class 제어창
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.Nametext = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.정보ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.경험치배율ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.이벤트ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.잭팟설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "공지";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(231, 21);
            this.textBox1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(118, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "관리자추가";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Nametext
            // 
            this.Nametext.Location = new System.Drawing.Point(12, 54);
            this.Nametext.Name = "Nametext";
            this.Nametext.Size = new System.Drawing.Size(100, 21);
            this.Nametext.TabIndex = 3;
            this.Nametext.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 24);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(442, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.정보ToolStripMenuItem,
            this.이벤트ToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(442, 24);
            this.menuStrip2.TabIndex = 5;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // 정보ToolStripMenuItem
            // 
            this.정보ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.경험치배율ToolStripMenuItem});
            this.정보ToolStripMenuItem.Name = "정보ToolStripMenuItem";
            this.정보ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.정보ToolStripMenuItem.Text = "정보";
            // 
            // 경험치배율ToolStripMenuItem
            // 
            this.경험치배율ToolStripMenuItem.Name = "경험치배율ToolStripMenuItem";
            this.경험치배율ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.경험치배율ToolStripMenuItem.Text = "경험치배율";
            this.경험치배율ToolStripMenuItem.Click += new System.EventHandler(this.경험치배율ToolStripMenuItem_Click);
            // 
            // 이벤트ToolStripMenuItem
            // 
            this.이벤트ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.잭팟설정ToolStripMenuItem});
            this.이벤트ToolStripMenuItem.Name = "이벤트ToolStripMenuItem";
            this.이벤트ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.이벤트ToolStripMenuItem.Text = "이벤트";
            // 
            // 잭팟설정ToolStripMenuItem
            // 
            this.잭팟설정ToolStripMenuItem.Name = "잭팟설정ToolStripMenuItem";
            this.잭팟설정ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.잭팟설정ToolStripMenuItem.Text = "잭팟설정";
            this.잭팟설정ToolStripMenuItem.Click += new System.EventHandler(this.잭팟설정ToolStripMenuItem_Click);
            // 
            // 제어창
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 200);
            this.Controls.Add(this.Nametext);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.menuStrip2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "제어창";
            this.Text = "제어창";
            this.Load += new System.EventHandler(this.제어창_Load);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox Nametext;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 정보ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 경험치배율ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 이벤트ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 잭팟설정ToolStripMenuItem;
    }
}