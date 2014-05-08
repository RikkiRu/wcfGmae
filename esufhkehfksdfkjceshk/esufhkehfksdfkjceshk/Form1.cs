using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;
using OpenTK.Graphics.OpenGL;

namespace esufhkehfksdfkjceshk
{
    [ServiceContract]
    public interface IMyobject
    {
        [OperationContract]
        object GetCommandString(object i);
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
    }

   

    public partial class Form1 : Form
    {
        public class playerclass
        {
            public string name;
            public int x;
            public int y;
            public int color;
            public int direction;
            public int state;

            public playerclass(string _name)
            {
                name = _name;
                x = 50;
                y = 50;
                color = 0;
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

        public Form1()
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

        static List<playerclass> playerslist = new List<playerclass>();
        static List<bullet> bulletList = new List<bullet>();
        static Textures texBullet;
        static Textures texTank;
        static Image tBullet = Image.FromFile(@"tex/bullet.png");
        static Image tTank = Image.FromFile(@"tex/tank.png");
        

        public float ortoX;
        public float ortoY;
        //------------------------------------------------------------------



        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == "") textBox2.Text = "unknown";
                string htt = textBox3.Text;
                if (htt == "") htt = "localhost";
                tcpUri = new Uri("http://" + htt + "/");
                address = new EndpointAddress(tcpUri);
                binding = new BasicHttpBinding();
                factory = new ChannelFactory<IMyobject>(binding, address);
                service = factory.CreateChannel();
                richTextBox1CHAT.Text += service.GetCommandString(textBox2.Text) + Environment.NewLine;
                button2.Enabled = false;
                textBox2.Enabled = false;
                
                playerslist.Clear();
                textBox3.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
                timer1.Enabled = true;
                service.say(DateTime.Now.ToShortTimeString() + ") " + textBox2.Text + " connected"); 
            }
            catch
            {
                richTextBox1CHAT.Text += "Connect to server failed"+Environment.NewLine;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //method();
            richTextBox1CHAT.Text = sayString;
            this.Text = coordPlayer;

            try
            {
                Render();
                glControl1.SwapBuffers();
            }
            catch
            {
            }
        }

        public void method()
        {
            try
            {
                string x = service.GetCommandString(1).ToString();
                playerslist.Clear();
                bulletList.Clear();

                string[] x2 = x.Split('&');
                string[] allPla = x2[0].Split('\n');
                string[] allBull = x2[1].Split('\n');
                sayString = x2[2];

                for (int i = 0; i < allPla.GetLength(0); i++)
                {
                    if (allPla[i] == "") continue;
                    string[] temp = allPla[i].Split('\t');
                    playerclass p = new playerclass(temp[0]);
                    p.state = Convert.ToInt32(temp[1]);
                    p.direction = Convert.ToInt32(temp[2]);
                    p.color = Convert.ToInt32(temp[3]);
                    p.x = Convert.ToInt32(temp[4]);
                    p.y = Convert.ToInt32(temp[5]);
                    playerslist.Add(p);
                }

                for (int i = 0; i < allBull.GetLength(0); i++)
                {
                    if (allBull[i] == "") continue;

                    string[] temp = allBull[i].Split('\t');
                    bullet b = new bullet(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
                    bulletList.Add(b);
                }

                
            }
            catch
            {
                //timer1.Enabled = false;
                //textBox1.Text += "Connect to server failed";
            }
        }

        void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.PushMatrix();

            var player = playerslist.Where(c => c.name == textBox2.Text).FirstOrDefault();
            int tX = -player.x+50;
            int tY = -player.y+50;
            GL.Translate(tX, tY, 0);
            coordPlayer = player.x.ToString() + " " + player.y.ToString();

            foreach (var a in playerslist)
            {
                Color t = Color.White;
                if (a.state == 0) t = Color.Blue;
                GL.Color3(t);
                float angle = a.direction * 90.0f;

                GL.PushMatrix();
                GL.Translate(a.x, a.y, 0);
                GL.Rotate(angle, 0, 0, 1);
                drawQuad(texTank, -4, -4, 4, 4);
                GL.PopMatrix();
            }
            GL.Color3(Color.White);
            foreach (var a in bulletList)
            {
                drawQuad(texBullet, a.x - 2, a.y - 2, a.x + 2, a.y + 2);
            }

            GL.PopMatrix();
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
            if (button2.Enabled) return;
            switch (e.KeyCode)
            {
                case Keys.W:
                    service.MoveY(textBox2.Text, -5);
                    break;
                case Keys.S:
                    service.MoveY(textBox2.Text, 5);
                    break;
                case Keys.A:
                    service.MoveX(textBox2.Text, -5);
                    break;
                case Keys.D:
                    service.MoveX(textBox2.Text, 5);
                    break;
                case Keys.Space:
                    service.CreateBullet(textBox2.Text, 0);
                    break;
                   
            }
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
            service.say(textBox2.Text+": "+textBox4forSay.Text);
            textBox4forSay.Text = "";
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Focus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                service.say(DateTime.Now.ToShortTimeString() + ") " + textBox2.Text + " disconnected");
            }
            catch
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            matrix(glControl1.Width, glControl1.Height);

            texBullet = new Textures(@"tex/bullet.png");
            texTank = new Textures(@"tex/tank.png");

            drawQuad(texTank, 100, 100, -100, -100);
            glControl1.SwapBuffers();
        }


    }
}
