using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;

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

            public void move()
            {
                switch (dir)
                {
                    case 0: y -= speed; break;
                    case 1: x += speed; break;
                    case 2: y += speed; break;
                    case 3: x -= speed; break;
                }
            }

            public bool explosion(playerclass p)
            {
                int dx = 0;
                int dy = 0;
                if (p.x > this.x) dx = p.x - this.x;
                else dx = this.x - p.x;
                if (p.y > this.y) dy = p.y - this.y;
                else dy = this.y - p.y;

                if (dx < distToExpl && dy < distToExpl) return true;

                return false;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        static Bitmap bmp = new Bitmap(512, 512);
        static Graphics gr = Graphics.FromImage(bmp);
        static Uri tcpUri;
        static EndpointAddress address;
        static BasicHttpBinding binding;
        static ChannelFactory<IMyobject> factory;
        static IMyobject service;

        string sayString="";
        string coordPlayer = "";

        List<playerclass> playerslist = new List<playerclass>();
        List<bullet> bulletList = new List<bullet>();

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string htt = textBox3.Text;
                if (htt == "") htt = "localhost";
                tcpUri = new Uri("http://" + htt + "/");
                address = new EndpointAddress(tcpUri);
                binding = new BasicHttpBinding();
                factory = new ChannelFactory<IMyobject>(binding, address);
                service = factory.CreateChannel();
                richTextBox1CHAT.Text += service.GetCommandString(textBox2.Text) + Environment.NewLine;
                pictureBox1.Visible = true;
                button2.Enabled = false;
                textBox2.Enabled = false;
                playerslist.Clear();
                textBox3.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
                timer1.Enabled = true;
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


            gr.Clear(Color.White);
            gr.ResetTransform();
            var player = playerslist.Where(c => c.name == textBox2.Text).FirstOrDefault();
            gr.TranslateTransform(-player.x+200, -player.y+200);
            coordPlayer = player.x.ToString() + " " + player.y.ToString();

            foreach (var a in playerslist)
            {
                Color t = Color.Black;
                if (a.state == 0) t = Color.Blue;
                Brush b = new SolidBrush(t);
                gr.FillRectangle(b, a.x - 8, a.y - 8, 16, 16);

                switch (a.direction)
                {
                    case 0: gr.FillRectangle(Brushes.Red, a.x - 2, a.y - 12, 4, 10); break;
                    case 1: gr.FillRectangle(Brushes.Red, a.x + 2, a.y - 2, 10, 4); break;
                    case 2: gr.FillRectangle(Brushes.Red, a.x - 2, a.y + 2, 4, 10); break;
                    case 3: gr.FillRectangle(Brushes.Red, a.x - 12, a.y - 2, 10, 4); break;
                }

            }
            foreach (var a in bulletList)
            {
                gr.FillRectangle(Brushes.Red, a.x - 4, a.y - 4, 8, 8);
            }
            pictureBox1.Image = bmp;
            //gr.ResetTransform();
        }



        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {        }

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


    }
}
