using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;
using OpenTK.Graphics.OpenGL;
using NAudio.Wave;
using render;

namespace esufhkehfksdfkjceshk
{
    [ServiceContract]
    public interface IMyobject
    {
        [OperationContract]
        object GetCommandString(object i, string player);
        [OperationContract]
        void MoveX(string name, double x);
        [OperationContract]
        void MoveY(string name, double y);
        [OperationContract]
        void CreateBullet(string name, double spx, double spy);
        [OperationContract]
        int state(string name);
        [OperationContract]
        void say(string say);
        [OperationContract]
        void addBlock(string name, int type);
        [OperationContract]
        string logOrCreate(string name, Color color);
    }

   

    public partial class FormMain : Form
    {
        

        public FormMain()
        {
            InitializeComponent();
            panel2.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - 78;
            panel2.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 - 90;
        }

        //------------------------------------------------------------------
        Render render = new Render();
        static Uri tcpUri;
        static EndpointAddress address;
        static BasicHttpBinding binding;
        static ChannelFactory<IMyobject> factory;
        static IMyobject service;

        string sayString="";
        string formText = "";

        public static float kamera=1;

        LoopStream loop = new LoopStream(new WaveFileReader(@"sound/move.wav"));
        WaveOut waveOut = new WaveOut();
        static CachedSound SoundFire = new CachedSound(@"sound/fire.wav");

        static Point mousePos = new Point();

        //------------------------------------------------------------------



        private void button1_Click(object sender, EventArgs e)
        {        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2_nickname.Text == "") textBox2_nickname.Text = "unknown";
                Render.name = textBox2_nickname.Text;
                string htt = textBox3.Text;
                if (htt == "") htt = "localhost";
                tcpUri = new Uri("http://" + htt + "/");
                address = new EndpointAddress(tcpUri);
                binding = new BasicHttpBinding();
                factory = new ChannelFactory<IMyobject>(binding, address);
                service = factory.CreateChannel();
                richTextBox1CHAT.Text += service.logOrCreate(textBox2_nickname.Text, colorDialog1.Color) + Environment.NewLine;
                button2.Enabled = false;
                button3.Enabled = false;
                textBox2_nickname.Enabled = false;
                panel3.Visible = true;
                Render.playerslist.Clear();
                textBox3.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
                timer2.Enabled = true;
                service.say(DateTime.Now.ToShortTimeString() + " ) " + textBox2_nickname.Text + " connected");
                glControl1.Focus();
                panel2.Visible = false;
                panel1.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                panel1.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
               
            }
            catch
            {
                richTextBox1CHAT.Text += "Connect to server failed"+Environment.NewLine;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {        }

        void setText()
        {
            richTextBox1CHAT.Text = sayString;
        }

        public void method()
        {
            try
            {
                string x = service.GetCommandString(1, textBox2_nickname.Text).ToString();

                Render.playerslist.Clear();
                Render.bulletList.Clear();
                Render.blockList.Clear();

                string[] x2 = x.Split('&');
                string[] allPla = x2[0].Split('\n');
                string[] allBull = x2[1].Split('\n');
                sayString = x2[2];
                string[] allBlock = x2[3].Split('\n');

                for (int i = 0; i < allPla.GetLength(0); i++)
                {
                    if (allPla[i] == "" || allPla[i]==" ") continue;
                    string[] temp = allPla[i].Split('\t');
                    Render.playerclass p = new Render.playerclass(temp[0], Convert.ToDouble(temp[6]), Convert.ToDouble(temp[7]));
                    p.state = Convert.ToInt32(temp[1]);
                    p.direction = Convert.ToInt32(temp[2]);
                    p.headDir = Convert.ToInt32(temp[8]);

                    string[] colStr = temp[3].Split('_');
                    p.color = Color.FromArgb(Convert.ToInt32(colStr[0]), Convert.ToInt32(colStr[1]), Convert.ToInt32(colStr[2]));

                    p.x = Convert.ToDouble(temp[4]);
                    p.y = Convert.ToDouble(temp[5]);

                    if (p.name == Render.name) p.headDir = calculateAngle();
                    Render.playerslist.Add(p);
                }

                for (int i = 0; i < allBull.GetLength(0); i++)
                {
                    if (allBull[i] == "" || allBull[i] == " ") continue;

                    string[] temp = allBull[i].Split('\t');
                    Render.bullet b = new Render.bullet(Convert.ToDouble(temp[0]), Convert.ToDouble(temp[1]));
                    Render.bulletList.Add(b);
                }

                for (int i = 0; i < allBlock.GetLength(0); i++)
                {
                    if (allBlock[i] == "" || allBlock[i] == " ") continue;
                    string[] temp = allBlock[i].Split('\t');
                    Render.block b = new Render.block(Convert.ToDouble(temp[0]), Convert.ToDouble(temp[1]), temp[2], Convert.ToInt32(temp[3]), Convert.ToDouble(temp[4]), Convert.ToDouble(temp[5]));
                    Render.blockList.Add(b);
                }
                
            }
            catch (Exception ex)
            {
                sayString += ex.Message + Environment.NewLine;
            }
        }

    

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (button2.Enabled) return;
                var player = Render.playerslist.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
                if (player == null) return;
                if (player.state == 0) return;

                switch (e.KeyCode)
                {
                    case Keys.W:
                        service.MoveY(textBox2_nickname.Text, -1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.S:
                        service.MoveY(textBox2_nickname.Text, 1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.A:
                        service.MoveX(textBox2_nickname.Text, -1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.D:
                        service.MoveX(textBox2_nickname.Text, 1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.Space:
                        dPoint t = calculateSpeed(player.headDir);
                        service.CreateBullet(textBox2_nickname.Text, t.x,t.y);
                        AudioPlaybackEngine.Instance.PlaySound(SoundFire);
                        break;
                    case Keys.Q:
                        service.addBlock(textBox2_nickname.Text, 0);
                        break;
                }
            }
            catch
            { }
            switch (e.KeyCode)
            {
                case Keys.R: kamera*=2; break;
                case Keys.F: kamera/=2; break;
                case Keys.Enter: textBox4forSay.Focus(); break;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            method();
            Thread.Sleep(50);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = formText;

            try
            {
                Render.RenderMain();
                glControl1.SwapBuffers();
            }
            catch (Exception ex)
            {
                sayString += ex.Message;
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            service.say(" "+textBox2_nickname.Text+" : "+textBox4forSay.Text);
            textBox4forSay.Text = "";
            glControl1.Focus();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            glControl1.Focus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                AudioPlaybackEngine.Instance.Dispose();
                service.say(DateTime.Now.ToShortTimeString() + " ) " + textBox2_nickname.Text + " disconnected");
            }
            catch
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;

            GL.ClearColor(0.3f, 0.7f, 0.4f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Render.matrix(glControl1.Width, glControl1.Height);

            Render.texBullet = new Render.Textures(@"tex/bullet.png");
            Render.texTank = new Render.Textures(@"tex/tank.png");
            Render.texTankBroke = new Render.Textures(@"tex/tankBroke.png");
            Render.texBlocks.Add("brick", new Render.Textures(@"tex/wall.png"));
            Render.texTankHead = new Render.Textures(@"tex/tankHead.png");

            waveOut.Init(loop);
            waveOut.Play();
            waveOut.Pause();

            //glControl1.SwapBuffers();
        }

        private void timer2_Tick(object sender, EventArgs e)
        { 
            setText();
            waveOut.Pause();     
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button3.BackColor = colorDialog1.Color;
        }

        private void button4playerList_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //matrix(glControl1.Width, glControl1.Height);
            //glControl1.SwapBuffers();
        }

        private void textBox4forSay_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter: if(textBox4forSay.Text!="") service.say(" " + textBox2_nickname.Text + " : " + textBox4forSay.Text);
                    textBox4forSay.Text = "";
                    glControl1.Focus();  break;
            }
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        int calculateAngle()
        {
            //return Math.Atan2(
            double w = glControl1.Size.Width / 2 - mousePos.X;
            double h = glControl1.Size.Height / 2 - mousePos.Y;
            //if (w < 0) w = -w;
            //if (h < 0) h = -h;
            double res = Math.Atan2(w, h) * 57;
            //formText = res.ToString();
            return -(int)res;
        }

        dPoint calculateSpeed(int angle)
        {
            double nAnge = angle/57.0;
            dPoint res = new dPoint();
            double resX = (Math.Sin(nAnge));
            double resY = (Math.Cos(nAnge));
            formText = angle.ToString()+"    x "+resX.ToString() + "   y " + resY.ToString();
            res.x = (resX);
            res.y = -(resY);
            return res;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos.X = e.X;
            mousePos.Y = e.Y;
        }

    }

    class dPoint
    {
        public double x;
        public double y;
    }
}
