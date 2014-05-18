namespace client
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2_nickname = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1CHAT = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox4forSay = new System.Windows.Forms.TextBox();
            this.glControl1 = new OpenTK.GLControl();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button3color = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3coord = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3password = new System.Windows.Forms.Label();
            this.textBox1password = new System.Windows.Forms.TextBox();
            this.timer1move = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.button2.Location = new System.Drawing.Point(3, 142);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(203, 53);
            this.button2.TabIndex = 1;
            this.button2.Text = "В бой!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // textBox2_nickname
            // 
            this.textBox2_nickname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.textBox2_nickname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2_nickname.Location = new System.Drawing.Point(3, 20);
            this.textBox2_nickname.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2_nickname.Name = "textBox2_nickname";
            this.textBox2_nickname.Size = new System.Drawing.Size(201, 26);
            this.textBox2_nickname.TabIndex = 3;
            this.textBox2_nickname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.textBox3.Location = new System.Drawing.Point(3, 218);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(201, 22);
            this.textBox3.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 198);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "IP";
            // 
            // richTextBox1CHAT
            // 
            this.richTextBox1CHAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.richTextBox1CHAT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1CHAT.Location = new System.Drawing.Point(3, 2);
            this.richTextBox1CHAT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.richTextBox1CHAT.Name = "richTextBox1CHAT";
            this.richTextBox1CHAT.ReadOnly = true;
            this.richTextBox1CHAT.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1CHAT.Size = new System.Drawing.Size(261, 294);
            this.richTextBox1CHAT.TabIndex = 7;
            this.richTextBox1CHAT.TabStop = false;
            this.richTextBox1CHAT.Text = "";
            this.richTextBox1CHAT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Name";
            // 
            // textBox4forSay
            // 
            this.textBox4forSay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox4forSay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4forSay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox4forSay.Location = new System.Drawing.Point(3, 297);
            this.textBox4forSay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox4forSay.MaxLength = 50;
            this.textBox4forSay.Name = "textBox4forSay";
            this.textBox4forSay.Size = new System.Drawing.Size(261, 27);
            this.textBox4forSay.TabIndex = 9;
            this.textBox4forSay.TabStop = false;
            this.textBox4forSay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox4forSay_KeyDown);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Margin = new System.Windows.Forms.Padding(5);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(931, 567);
            this.glControl1.TabIndex = 12;
            this.glControl1.TabStop = false;
            this.glControl1.VSync = false;
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.glControl1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // colorDialog1
            // 
            this.colorDialog1.Color = System.Drawing.Color.White;
            // 
            // button3color
            // 
            this.button3color.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.button3color.BackColor = System.Drawing.Color.Chartreuse;
            this.button3color.Location = new System.Drawing.Point(3, 101);
            this.button3color.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3color.Name = "button3color";
            this.button3color.Size = new System.Drawing.Size(203, 34);
            this.button3color.TabIndex = 13;
            this.button3color.Text = "Цвет";
            this.button3color.UseVisualStyleBackColor = false;
            this.button3color.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.label3coord);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.glControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(931, 567);
            this.panel1.TabIndex = 15;
            // 
            // label3coord
            // 
            this.label3coord.AutoSize = true;
            this.label3coord.Location = new System.Drawing.Point(36, 15);
            this.label3coord.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3coord.Name = "label3coord";
            this.label3coord.Size = new System.Drawing.Size(66, 17);
            this.label3coord.TabIndex = 19;
            this.label3coord.Text = "Welcome";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Red;
            this.panel4.Controls.Add(this.button4);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Location = new System.Drawing.Point(397, 15);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.MaximumSize = new System.Drawing.Size(208, 123);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(208, 123);
            this.panel4.TabIndex = 18;
            this.panel4.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(4, 4);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(200, 48);
            this.button4.TabIndex = 1;
            this.button4.Text = "Respawn";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 75);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Выход из игры";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.Controls.Add(this.richTextBox1CHAT);
            this.panel3.Controls.Add(this.textBox4forSay);
            this.panel3.Location = new System.Drawing.Point(12, 230);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(267, 326);
            this.panel3.TabIndex = 17;
            this.panel3.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.OliveDrab;
            this.panel2.Controls.Add(this.label3password);
            this.panel2.Controls.Add(this.textBox1password);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button3color);
            this.panel2.Controls.Add(this.textBox2_nickname);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(395, 130);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.MaximumSize = new System.Drawing.Size(208, 246);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel2.Size = new System.Drawing.Size(208, 246);
            this.panel2.TabIndex = 16;
            // 
            // label3password
            // 
            this.label3password.AutoSize = true;
            this.label3password.Location = new System.Drawing.Point(3, 50);
            this.label3password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3password.Name = "label3password";
            this.label3password.Size = new System.Drawing.Size(69, 17);
            this.label3password.TabIndex = 15;
            this.label3password.Text = "Password";
            // 
            // textBox1password
            // 
            this.textBox1password.Location = new System.Drawing.Point(3, 70);
            this.textBox1password.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1password.Name = "textBox1password";
            this.textBox1password.Size = new System.Drawing.Size(200, 22);
            this.textBox1password.TabIndex = 4;
            // 
            // timer1move
            // 
            this.timer1move.Interval = 50;
            this.timer1move.Tick += new System.EventHandler(this.timer1move_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 567);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "Tanks by Igor and Andrew";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2_nickname;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1CHAT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox4forSay;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button3color;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3password;
        private System.Windows.Forms.TextBox textBox1password;
        private System.Windows.Forms.Label label3coord;
        private System.Windows.Forms.Timer timer1move;
    }
}

