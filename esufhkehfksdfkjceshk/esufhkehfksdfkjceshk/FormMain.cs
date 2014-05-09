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

namespace esufhkehfksdfkjceshk
{
    [ServiceContract]
    public interface IMyobject
    {
        [OperationContract]
        object GetCommandString(object i, string player);
        [OperationContract]
        void MoveX(string name, int x);
        [OperationContract]
        void MoveY(string name, int y);
        [OperationContract]
        void CreateBullet(string name, int dir);
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
        public class playerclass
        {
            public string name;
            public int x;
            public int y;
            public Color color;
            public int direction;
            public int state;

            public playerclass(string _name)
            {
                name = _name;
                x = 50;
                y = 50;
                color = Color.White;
                state = 1;
            }
        }

        public class bullet
        {
            public int x;
            public int y;
            public int dir;
            const int speed = 5;
            const int distToExpl = 5;


            public bullet(int x, int y, int dir)
            {
                this.x = x;
                this.y = y;
                this.dir = dir;
            }
        }

        public class block
        {
            public int x;
            public int y;
            public int type;
            public int lifes = 5;
            public bool forDelete = false;

            public block(int x, int y, int type)
            {
                this.x = x;
                this.y = y;
                this.type = type;
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------
        static Uri tcpUri;
        static EndpointAddress address;
        static BasicHttpBinding binding;
        static ChannelFactory<IMyobject> factory;
        static IMyobject service;

        string sayString="";
        string coordPlayer = "";
        static int mainSize = 8;

        static List<playerclass> playerslist = new List<playerclass>();
        static List<bullet> bulletList = new List<bullet>();
        static List<block> blockList = new List<block>();

        static Textures texBullet;
        static Textures texTank;
        static Textures texTankBroke;
        static List<Textures> texBlocks=new List<Textures>();
      

        public float ortoX;
        public float ortoY;

        LoopStream loop = new LoopStream(new WaveFileReader(@"sound/move.wav"));
        WaveOut waveOut = new WaveOut();
        static CachedSound SoundFire = new CachedSound(@"sound/fire.wav");
       
        //------------------------------------------------------------------



        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2_nickname.Text == "") textBox2_nickname.Text = "unknown";
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
                playerslist.Clear();
                textBox3.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
                timer1.Enabled = true;
                timer2.Enabled = true;
                service.say(DateTime.Now.ToShortTimeString() + " ) " + textBox2_nickname.Text + " connected");
                glControl1.Focus();
                panel2.Visible = false;
            }
            catch
            {
                richTextBox1CHAT.Text += "Connect to server failed"+Environment.NewLine;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //method();

            this.Text = coordPlayer;

            try
            {
                Render();
                glControl1.SwapBuffers();
            }
            catch (Exception ex)
            {
                sayString += ex.Message;
            }
        }

        void setText()
        {
            richTextBox1CHAT.Text = sayString;
        }

        public void method()
        {
            try
            {
                string x = service.GetCommandString(1, textBox2_nickname.Text).ToString();

                playerslist.Clear();
                bulletList.Clear();
                blockList.Clear();

                string[] x2 = x.Split('&');
                string[] allPla = x2[0].Split('\n');
                string[] allBull = x2[1].Split('\n');
                sayString = x2[2];
                string[] allBlock = x2[3].Split('\n');

                for (int i = 0; i < allPla.GetLength(0); i++)
                {
                    if (allPla[i] == "" || allPla[i]==" ") continue;
                    string[] temp = allPla[i].Split('\t');
                    playerclass p = new playerclass(temp[0]);
                    p.state = Convert.ToInt32(temp[1]);
                    p.direction = Convert.ToInt32(temp[2]);

                    string[] colStr = temp[3].Split('_');
                    p.color = Color.FromArgb(Convert.ToInt32(colStr[0]), Convert.ToInt32(colStr[1]), Convert.ToInt32(colStr[2]));

                    p.x = Convert.ToInt32(temp[4]);
                    p.y = Convert.ToInt32(temp[5]);
                    playerslist.Add(p);
                }

                for (int i = 0; i < allBull.GetLength(0); i++)
                {
                    if (allBull[i] == "" || allBull[i] == " ") continue;

                    string[] temp = allBull[i].Split('\t');
                    bullet b = new bullet(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
                    bulletList.Add(b);
                }

                for (int i = 0; i < allBlock.GetLength(0); i++)
                {
                    if (allBlock[i] == "" || allBlock[i] == " ") continue;
                    string[] temp = allBlock[i].Split('\t');
                    block b = new block(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
                    blockList.Add(b);
                }
                
            }
            catch (Exception ex)
            {
                sayString += ex.Message + Environment.NewLine;
            }
        }

        void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            var player = playerslist.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
            int tX = -player.x+(int)ortoX/2;
            int tY = -player.y+(int)ortoY/2;
            GL.Translate(tX, tY, 0);
            coordPlayer = player.x.ToString() + " " + player.y.ToString();

            foreach (var a in blockList)
            {
               drawQuad(texBlocks[a.type], a.x - mainSize, a.y - mainSize, a.x + mainSize, a.y + mainSize);
            }

            foreach (var a in playerslist)
            {
                Color t = a.color;
                GL.Color3(t);
                float angle = a.direction * 90.0f;

                GL.LoadIdentity();
                GL.Translate(tX, tY, 0);
                GL.Translate(a.x, a.y, 0);
                GL.Rotate(angle, 0, 0, 1);

                if(a.state==1) drawQuad(texTank, -mainSize, -mainSize, mainSize, mainSize);
                else drawQuad(texTankBroke, -mainSize, -mainSize, mainSize, mainSize);
            }

            GL.LoadIdentity();
            GL.Translate(tX, tY, 0);
            GL.Color3(Color.White);

            foreach (var a in bulletList)
            {
                drawQuad(texBullet, a.x - 2, a.y - 2, a.x + 2, a.y + 2);
            }
        }

        static public void drawQuad(Textures t, float p1x, float p1y, float p2x, float p2y)
        {
            t.bind();
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(p1x, p1y);
            GL.TexCoord2(0, 1); GL.Vertex2(p1x, p2y);
            GL.TexCoord2(1, 1); GL.Vertex2(p2x, p2y);
            GL.TexCoord2(1, 0); GL.Vertex2(p2x, p1y);
            GL.End();
        }

        void matrix(int w, int h)
        {
            ortoX = 100;
            ortoY = 100;
            if (h == 0) h = 1;
            float aspect = (float)w / (float)h;
            GL.Viewport(0, 0, w, h);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            if (w < h) { GL.Ortho(0, ortoX, ortoY / aspect, 0, -2, 2); ortoY = ortoY / aspect; }
            else { GL.Ortho(0, ortoX * aspect, ortoY, 0, -2, 2); ortoX = ortoX * aspect; }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (button2.Enabled) return;
                var player = playerslist.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
                if (player == null) return;
                if (player.state == 0) return;

                switch (e.KeyCode)
                {
                    case Keys.W:
                        service.MoveY(textBox2_nickname.Text, -5);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.S:
                        service.MoveY(textBox2_nickname.Text, 5);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.A:
                        service.MoveX(textBox2_nickname.Text, -5);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.D:
                        service.MoveX(textBox2_nickname.Text, 5);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.Space:
                        service.CreateBullet(textBox2_nickname.Text, 0);
                        AudioPlaybackEngine.Instance.PlaySound(SoundFire);
                        break;
                    case Keys.Q:
                        service.addBlock(textBox2_nickname.Text, 0);
                        break;
                }
            }
            catch
            { }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            method();
            Thread.Sleep(50);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
            this.WindowState = FormWindowState.Maximized;

            GL.ClearColor(0.3f, 0.7f, 0.4f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            matrix(glControl1.Width, glControl1.Height);

            texBullet = new Textures(@"tex/bullet.png");
            texTank = new Textures(@"tex/tank.png");
            texTankBroke = new Textures(@"tex/tankBroke.png");
            texBlocks.Add(new Textures(@"tex/wall.png"));

            waveOut.Init(loop);
            waveOut.Play();
            waveOut.Pause();

            glControl1.SwapBuffers();
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
            List<string> names = new List<string>();
            List<Color> colors = new List<Color>();

            foreach (var a in playerslist)
            {
                names.Add(a.name);
                colors.Add(a.color);
            }

            FormPlayerList fpl = new FormPlayerList(names, colors);
            fpl.Show();
            glControl1.Focus();
            fpl.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            matrix(glControl1.Width, glControl1.Height);
            glControl1.SwapBuffers();
        }


    }
}
