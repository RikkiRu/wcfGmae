using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.ServiceModel;
using CommunicationInterface;
using System.Timers;
using System.Drawing;

namespace CommunicationInterface
{
    public class MyObject : IMyobject
    {
        public static List<playerclass> playerslist = new List<playerclass>();
        public static List<bullet> bulletlist = new List<bullet>();
        public static List<block> blockList = new List<block>();

        public static string sayList="";
        public static int sayCount=0;
        public static int visibleDistance=150;
        public static int boxTank = 10;
        public static int boxBlock = 10;

        public static bool IsInDistance (int x, int y, int ax, int ay, int distance)
        {
            int dx;
            int dy;

            if (ax > x) dx = ax - x;
            else dx = x - ax;
            if (ay > y) dy = ay - y;
            else dy = y - ay;

            if (dx < distance && dy < distance) return true;
            return false;
        }

        public static string retPlayerList(int x, int y)
        {
            string res = "";
            foreach(var a in playerslist)
            {
               if (IsInDistance(x, y, a.x, a.y, visibleDistance))
               {
                    res += a.name + "\t";
                    res += a.state.ToString() + "\t";
                    res += a.direction.ToString() + "\t";
                    res += a.color.R.ToString() + "_" + a.color.G + "_" + a.color.B + "\t";
                    res += a.x.ToString() + "\t";
                    res += a.y.ToString() + "\t";
                    res += "\n";
               }
            }
            return res;
        }

        public static string retBullet(int x, int y)
        {
            string res = "";
            foreach (var a in bulletlist)
            {
                if (IsInDistance(x, y, a.x, a.y, visibleDistance))
                {
                    res += a.x.ToString() + "\t";
                    res += a.y.ToString() + "\t";
                    res += a.dir.ToString() + "\t";
                    res += "\n";
                }
            }
            return res;
        }

        public static string retBlock(int x, int y)
        {
            string res = "";
            foreach (var a in blockList)
            {
                if (IsInDistance(x, y, a.x, a.y, visibleDistance))
                {
                    res += a.x.ToString() + "\t";
                    res += a.y.ToString() + "\t";
                    res += a.type.ToString() + "\t";
                    res += "\n";
                }
            }
            return res;
        }

        public class playerclass
        {
            public string name;
            public int x;
            public int y;
            public Color color;
            public int direction;
            public int state;

            public playerclass(string _name, Color color)
            {
                name = _name;

                x = 0;
                y = 0;
                while (!tryMove(0, 0))
                {
                    x += 100;
                }

                this.color = color;
                state = 1;
            }

            public bool tryMove(int x, int y)
            {
                int dx = this.x + x;
                int dy = this.y + y;

                foreach (var a in playerslist)
                {
                    if (a == this) continue;

                    if(IsInDistance(dx, dy, a.x, a.y, boxTank)) return false;
                }

                foreach (var a in blockList)
                {
                    if (IsInDistance(dx, dy, a.x, a.y, boxBlock)) return false;
                }

                this.x = dx;
                this.y = dy;
                return true;
            }
        }

        public class bullet
        {
            public int x;
            public int y;
            public int dir;
            const int speed = 5;
            const int distToExpl = 5;
            public int lifetime = 30;
            public bool forDelele = false;

            public bullet(int x, int y, int dir)
            {
                this.x = x;
                this.y = y;
                this.dir = dir;
            }

            public void move()
            {
                lifetime--;
                if (lifetime < 0) forDelele = true;
                switch (dir)
                {
                    case 0: y -= speed; break;
                    case 1: x += speed; break;
                    case 2: y += speed; break;
                    case 3: x -= speed; break;
                }
            }

            public bool explosion(int px, int py, int size)
            {
                int dx = 0;
                int dy = 0;
                if (px > this.x) dx = px - this.x;
                else dx = this.x - px;
                if (py > this.y) dy = py - this.y;
                else dy = this.y - py;

                if (dx < size && dy < size) return true;

                return false;
            }
        }

        public class block
        {
            public int x;
            public int y;
            public int type;
            public int lifes=5;
            public bool forDelete=false;

            public block(int x, int y, int type)
            {
                this.x = x;
                this.y = y;
                this.type = type;
            }

            public void hit()
            {
                lifes--;
                if (lifes < 0) forDelete = true;
            }
        }

        public int state(string name)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            return find.state;
        }

        public void CreateBullet(string name, int dir)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find == null) return;
            if (find.state == 0) return;

            int mx = 0;
            int my = 0;
            int dist = boxTank;

            switch (find.direction)
            {
                case 0: my -= dist; break;
                case 1: mx += dist; break;
                case 2: my += dist; break;
                case 3: mx -= dist; break;
            }

            bulletlist.Add(new bullet(find.x+mx, find.y+my, find.direction));
        }

        public void MoveX(string name, int x)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
                if (find == null) return;
                find.tryMove(x, 0);
                if (x > 0) find.direction = 1;
                else find.direction = 3;
            }
        }

        public void say(string x)
        {
            if (sayCount > 5)
            {
                sayList = "";
                sayCount = 0;
            }
            string res = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != '&') res += x[i];
            }
            Console.WriteLine(res);
            sayList+=res+Environment.NewLine;
            sayCount++;
        }

        public void MoveY(string name, int y)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
                if (find == null) return;
                find.tryMove(0, y);
                if (y > 0) find.direction = 2;
                else find.direction = 0;
            }
        }

        public string logOrCreate(string name, Color color)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find == null)
            {
                Console.WriteLine("New player! - " + name);
                playerclass player = new playerclass(name, color);
                playerslist.Add(player);
                return "New player (" + name + ") created";
            }
            else return "Вы подключились";
        }

        public object GetCommandString(object i, string name)
        {
            if(i is int)
            switch (Convert.ToInt32(i))
            {
                case 1:
                    try
                    {
                        var player = playerslist.Where(c => c.name == name).FirstOrDefault();

                        Console.WriteLine(" - запрос списка игроков" + DateTime.Now.ToString());
                        string abv = MyObject.retPlayerList(player.x, player.y) + "&" + MyObject.retBullet(player.x, player.y) + "&" + MyObject.sayList + "&" + MyObject.retBlock(player.x, player.y); 
                        //Console.WriteLine(abv);
                        return abv;
                    }
                    catch
                    {
                        return " & & ";
                    }
                default:
                    return "Получил " + Convert.ToString(i);
            }
            return null;
        }

       static public void work( object source, ElapsedEventArgs e )
        {
           foreach(var a in bulletlist)
           {
               a.move();
               foreach (var b in playerslist)
               {
                   if(a.explosion(b.x, b.y, boxTank))
                   {
                       b.state = 0;
                       a.forDelele = true;
                   }
               }

               foreach (var b in blockList)
               {
                   if (a.explosion(b.x, b.y, boxBlock))
                   {
                       b.hit();
                       a.forDelele = true;
                   }
               }
           }

           for (int i = bulletlist.Count - 1; i >= 0; i--)
           {
               if (bulletlist[i].forDelele)
               {
                   bulletlist.Remove(bulletlist[i]);
               }
           }

           for (int i = blockList.Count - 1; i >= 0; i--)
           {
               if (blockList[i].forDelete)
               {
                   blockList.Remove(blockList[i]);
               }
           }
        }

       public void addBlock(string name, int type)
       {
           playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
           if (find == null) return;
           if (find.state == 0) return;

           int mx = 0;
           int my = 0;
           int dist = boxTank + boxBlock;

           switch (find.direction)
           {
               case 0: my -= dist; break;
               case 1: mx += dist; break;
               case 2: my += dist; break;
               case 3: mx -= dist; break;
           }
           blockList.Add(new block(find.x+mx, find.y+my, type));
       }
    }
}

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServiceHost host = new ServiceHost(typeof(MyObject), new Uri("http://19/"));
            string http;
            http = Console.ReadLine();
            if (http == "") http = "localhost";
            ServiceHost host = new ServiceHost(typeof(MyObject), new Uri("http://"+http+"/"));
            host.AddServiceEndpoint(typeof(IMyobject), new BasicHttpBinding(), "");
            host.Open();
            Console.WriteLine("Сервер запущен");


            Timer time = new Timer(30);
            time.Elapsed += new ElapsedEventHandler(MyObject.work);
            time.Start();

            Console.ReadLine();

            host.Close();
        }

       
    }
}
