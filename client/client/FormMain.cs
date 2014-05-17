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
using structureClasses;

namespace client
{
    [ServiceContract]
    public interface IMyobject
    {
        [OperationContract]
        string[] GetCommandString(object i, string player);
        [OperationContract]
        void Move(string name, double x, double y);
        [OperationContract]
        bool CreateBullet(string name, double spx, double spy);
        [OperationContract]
        void say(string say);
        [OperationContract]
        void addBlock(string name, int type);
        [OperationContract]
        string logOrCreate(string name, Color color,string password);
        [OperationContract]
        void setdirect(string name, int dir);
        [OperationContract]
        void ping(string name);
        [OperationContract]
        void respawn(string name);
        [OperationContract]
        string getGameObj(int nList, int id, bool update);
        [OperationContract]
        int[] getIds(int nlist, string name);
    }

   

    public partial class FormMain : Form
    {
        

        public FormMain()
        {
            InitializeComponent();
            panel2.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - 78;
            panel2.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 - 100;
            panel4.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - 78;
            panel4.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 - 50;
        }

        //------------------------------------------------------------------
        Render render = new Render();
        static Uri tcpUri;
        static EndpointAddress address;
        static BasicHttpBinding binding;
        static ChannelFactory<IMyobject> factory;
        static IMyobject service;

        static Random rand = new Random();
        string sayString="";
        string formText = "";
        static double SpeedSlow = 0.6;
        static double Speed = 1;

        public static float kamera=1;

        LoopStream loop = new LoopStream(new WaveFileReader(@"sound/move.wav"));
        WaveOut waveOut = new WaveOut();
        static CachedSound SoundFire = new CachedSound(@"sound/fire.wav");

        static Point mousePos = new Point();

        static bool[] flagsMove = { false, false, false, false };
        //------------------------------------------------------------------



        private void button1_Click(object sender, EventArgs e)
        {        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1password.Text == "") throw new Exception("Редиска! Ты не ввел пароль.");
                if (textBox2_nickname.Text == "") textBox2_nickname.Text = "unknown";
                Render.name = textBox2_nickname.Text;
                string htt = textBox3.Text;
                if (htt == "") htt = "localhost";
                tcpUri = new Uri("http://" + htt + "/");
                address = new EndpointAddress(tcpUri);
                binding = new BasicHttpBinding();
                factory = new ChannelFactory<IMyobject>(binding, address);
                service = factory.CreateChannel();

                string res = service.logOrCreate(textBox2_nickname.Text, colorDialog1.Color, textBox1password.Text);
                if (res == null) throw new Exception("Пароль не верный");
                richTextBox1CHAT.Text += res + Environment.NewLine;
                button2.Enabled = false;
                button3color.Enabled = false;
                textBox2_nickname.Enabled = false;
                panel3.Visible = true;
                textBox3.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
                timer2.Enabled = true;
                timer1move.Enabled = true;
                service.say(DateTime.Now.ToShortTimeString() + " ) " + textBox2_nickname.Text + " connected");
                glControl1.Focus();
                panel2.Visible = false;
                panel1.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                panel1.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {        }

        void setText()
        {
            richTextBox1CHAT.Text = sayString;
            richTextBox1CHAT.SelectionStart = richTextBox1CHAT.TextLength;
            //richTextBox1CHAT.ScrollToCaret();
        }

        /// <summary>
        /// //////////////////////////////////////////////////
        /// </summary>
        public void method()
        {
            service.setdirect(textBox2_nickname.Text, calculateAngle());
            gameObjPlayer player = Render.playerslist.elements.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();

            Render.playerslist.update(service.getIds(0, textBox2_nickname.Text));
            Render.bulletList.update(service.getIds(1, textBox2_nickname.Text));
            Render.blockList.update(service.getIds(2, textBox2_nickname.Text));

            Render.playerslist.clearDeleted();
            Render.bulletList.clearDeleted();
            Render.blockList.clearDeleted();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (button2.Enabled) return;
                var player = Render.playerslist.elements.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
                if (player == null) return;
                if (player.state == 0) return;

                switch (e.KeyCode)
                {
                    case Keys.W:
                        flagsMove[0] = true;
                        //service.MoveY(textBox2_nickname.Text, -1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.S:
                        flagsMove[2] = true;
                        //service.MoveY(textBox2_nickname.Text, 1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.A:
                        flagsMove[3] = true;
                        //service.MoveX(textBox2_nickname.Text, -1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.D:
                        flagsMove[1] = true;
                        //service.MoveX(textBox2_nickname.Text, 1);
                        if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Play();
                        break;
                    case Keys.Q:
                        service.addBlock(textBox2_nickname.Text, 0);
                        break;
                    case Keys.Tab:
                        List<string> l1 = new List<string>();
                        List<Color> l2 = new List<Color>();
                        List<int> l3 = new List<int>();
                        foreach(var a in Render.playerslist.elements)
                        {
                            l1.Add(a.name);
                            l2.Add(a.color);
                            l3.Add(a.frags);
                        }
                        new FormPlayerList(l1, l2, l3).ShowDialog();
                        break;
                }
            }
            catch
            {  }
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
            Thread.Sleep(30);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                var player = Render.playerslist.elements.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
                this.Text = "Подбито: " + player.frags;
                this.label3coord.Text = Render.coordPlayer;
                if ((player.state == 0) && (panel4.Visible == false))
                    panel4.Visible = true;
                if (player.state == 1) panel4.Visible = false;
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
                service.GetCommandString(2, textBox2_nickname.Text);
            }
            catch
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Color cl = Color.FromArgb(rand.Next(100, 256), rand.Next(100, 256), rand.Next(100, 256));
            colorDialog1.Color = cl;
            button3color.BackColor = cl;

            GL.ClearColor(0.3f, 0.7f, 0.4f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Render.matrix(glControl1.Width, glControl1.Height);

            Render.texBullet = new Render.Textures(@"tex/bullet.png");
            Render.texTank = new Render.Textures(@"tex/tank.png");
            Render.texTankBroke = new Render.Textures(@"tex/tankBroke.png");
            Render.texBlocks.Add("brick", new Render.Textures(@"tex/wall.png"));
            Render.texBlocks.Add("house", new Render.Textures(@"tex/dom.png"));
            Render.texBlocks.Add("houseB", new Render.Textures(@"tex/domB.png"));
            Render.texBlocks.Add("road", new Render.Textures(@"tex/road.png"));
            Render.texTankHead = new Render.Textures(@"tex/tankHead.png");
            Render.texBlocks.Add("tree", new Render.Textures(@"tex/tree.png"));
            Render.texBlocks.Add("treeB", new Render.Textures(@"tex/treeB.png"));
<<<<<<< HEAD
            Render.texBlocks.Add("houseD", new Render.Textures(@"tex/domD.png"));
            Render.texBlocks.Add("car", new Render.Textures(@"tex/car.png"));
            Render.texBlocks.Add("carB", new Render.Textures(@"tex/carB.png"));

            Render.playerslist.updElem = getPlayerUpdates;
            Render.playerslist.createElem = getFullPlayer;

            Render.bulletList.updElem = getBulletsUpd;
            Render.bulletList.createElem = getFullBullet;

            Render.blockList.updElem = getBlocksUpd;
            Render.blockList.createElem = getFullBlock;
=======
>>>>>>> 71b5d0683f0bb5cdcd35b290b663a44ed5a5dc5c

            waveOut.Init(loop);
            waveOut.Play();
            waveOut.Pause();
        }

        private void timer2_Tick(object sender, EventArgs e)
        { 
            setText();
            waveOut.Pause();     
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button3color.BackColor = colorDialog1.Color;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //matrix(glControl1.Width, glControl1.Height);
            //glControl1.SwapBuffers();
            try
            {
                Render.matrix(glControl1.Width, glControl1.Height);
            }
            catch { }
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
            switch (e.KeyCode)
            {
                case Keys.W:
                    flagsMove[0] = false;
                    break;
                case Keys.D:
                    flagsMove[1] = false;
                    break;
                case Keys.S:
                    flagsMove[2] = false;
                    break;
                case Keys.A:
                    flagsMove[3] = false;
                    break;
            }
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
            double resX = (Math.Sin(nAnge))*2;
            double resY = (Math.Cos(nAnge))*2;
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

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (button2.Enabled) return;
<<<<<<< HEAD
                var player = Render.playerslist.elements.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
                if (player == null) return;
                if (player.state == 0) return;
                dPoint t = calculateSpeed(player.headDir);
                if(service.CreateBullet(textBox2_nickname.Text, t.x, t.y)) AudioPlaybackEngine.Instance.PlaySound(SoundFire);
=======
                var player = Render.playerslist.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
                if (player == null) return;
                if (player.state == 0) return;
                dPoint t = calculateSpeed(player.headDir);
                service.CreateBullet(textBox2_nickname.Text, t.x, t.y);
                AudioPlaybackEngine.Instance.PlaySound(SoundFire);
>>>>>>> 71b5d0683f0bb5cdcd35b290b663a44ed5a5dc5c
            }
            catch { }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var player = Render.playerslist.elements.Where(c => c.name == textBox2_nickname.Text).FirstOrDefault();
                service.respawn(player.name);
                Thread.Sleep(100);
                panel4.Visible = false;
                glControl1.Focus();
            }
            catch { }
        }

        private void timer1move_Tick(object sender, EventArgs e)
        {
            if(flagsMove[0] && flagsMove[1])
            { 
                service.Move(textBox2_nickname.Text, SpeedSlow, -SpeedSlow);
                return;
            }

            if (flagsMove[1] && flagsMove[2])
            {
                service.Move(textBox2_nickname.Text, SpeedSlow, SpeedSlow);
                return;
            }

            if (flagsMove[2] && flagsMove[3])
            {
                service.Move(textBox2_nickname.Text, -SpeedSlow, SpeedSlow);
                return;
            }

            if (flagsMove[0] && flagsMove[3])
            {
                service.Move(textBox2_nickname.Text, -SpeedSlow, -SpeedSlow);
                return;
            }

            if (flagsMove[0])
            {
                service.Move(textBox2_nickname.Text, 0, -Speed);
                return;
            }

            if (flagsMove[1])
            {
                service.Move(textBox2_nickname.Text, Speed, 0);
                return;
            }

            if (flagsMove[2])
            {
                service.Move(textBox2_nickname.Text, 0, Speed);
                return;
            }

            if (flagsMove[3])
            {
                service.Move(textBox2_nickname.Text, -Speed, 0);
                return;
            }
        }

        string getPlayerUpdates(int id)
        {
            string res = service.getGameObj(0, id, true);
            return res;
        }
        gameObjPlayer getFullPlayer(int id)
        {
            gameObjPlayer pl = new gameObjPlayer();
            pl.setObj(service.getGameObj(0, id, false));
            return pl;
        }
        string getBulletsUpd(int id)
        {
            return service.getGameObj(1, id, true);
        }
        gameObjBullet getFullBullet(int id)
        {
            gameObjBullet b = new gameObjBullet();
            b.setObj(service.getGameObj(1, id, false));
            return b;
        }
        string getBlocksUpd(int id)
        {
            return service.getGameObj(2, id, true);
        }
        gameObjBlock getFullBlock(int id)
        {
            gameObjBlock b = new gameObjBlock();
            b.setObj(service.getGameObj(2, id, false));
            return b;
        }
    }

    class dPoint
    {
        public double x;
        public double y;
    }
}
