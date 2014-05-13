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
        string logOrCreate(string name, Color color,string password);
        [OperationContract]
        void setdirect(string name, int dir);
        [OperationContract]
        void ping(string name);//////////////////////////empty
        [OperationContract]
        void respawn(string name);
    }

    public class MyObject : IMyobject
    {//
        public static List<playerclass> playerslist = new List<playerclass>();
        public static List<bullet> bulletlist = new List<bullet>();
        public static List<block> blockList = new List<block>();

        public static string sayList="";
        public static int sayCount=0;
        public static int visibleDistance=150;
        public static int boxTank = 10;
        public static int boxBlock = 10;

        public const int countOfMessages = 50;
        public void ping(string name)
        {
            //Console.WriteLine(name);
        }
        public static bool IsInDistance (double x, double y, double ax, double ay, double distX, double distY)
        {
            double dx;
            double dy;

            if (ax > x) dx = ax - x;
            else dx = x - ax;
            if (ay > y) dy = ay - y;
            else dy = y - ay;

            if (dx < distX && dy < distY) return true;
            return false;
        }

        public void respawn(string name)
        {
            var player = playerslist.Where(c => c.name == name).FirstOrDefault();
            player.state = 1;
            player.x = new Random().Next(300) - 150;
            player.y = new Random().Next(300) - 150;
            player.frags = 0;
        }


        public static string retPlayerList(double x, double y)
        {
            string res = "";
            foreach(var a in playerslist)
            {
               if (IsInDistance(x, y, a.x, a.y, visibleDistance, visibleDistance))
               {
                    res += a.name + "\t";
                    res += a.state.ToString() + "\t";
                    res += a.direction.ToString() + "\t";
                    res += a.color.R.ToString() + "_" + a.color.G + "_" + a.color.B + "\t";
                    res += a.x.ToString() + "\t";
                    res += a.y.ToString() + "\t";
                    res += a.sizeX.ToString() + "\t";
                    res += a.sizeY.ToString() + "\t";
                    res += a.headDir.ToString() + "\t";
                    res += a.frags.ToString() + "\t";
                    res += "\n";
               }
            }
            return res;
        }

        public static string retBullet(double x, double y)
        {
            string res = "";
            foreach (var a in bulletlist)
            {
                if (IsInDistance(x, y, a.x, a.y, visibleDistance, visibleDistance))
                {
                    res += a.x.ToString() + "\t";
                    res += a.y.ToString() + "\t";
                    res += "\n";
                }
            }
            return res;
        }

        public static string retBlock(double x, double y)
        {
            string res = "";
            foreach (var a in blockList)
            {
                if (IsInDistance(x, y, a.x, a.y, visibleDistance, visibleDistance))
                {
                    res += a.x.ToString() + "\t";
                    res += a.y.ToString() + "\t";
                    res += a.type.ToString() + "\t";
                    res += a.dir.ToString() + "\t";
                    res += a.sizeX.ToString() + "\t";
                    res += a.sizeY.ToString() + "\t";
                    res += "\n";
                }
            }
            return res;
        }

        public class playerclass
        {
            public string name;
            public double x;
            public double y;
            public double sizeX;
            public double sizeY;
            public Color color;
            public int direction;
            public int state;
            public int headDir;
            public int frags;
            public string password;

            public playerclass(string _name, Color color, string password)
            {
                name = _name;
                this.password = password;
                x = 0;
                y = 0;
                frags = 0;
                while (!tryMove(0, 0))
                {
                    x += 100;
                }

                sizeX = 10;
                sizeY = 10;
                if (name == "god") sizeX = sizeY = 50;

                this.color = color;
                state = 1;
            }

            public bool tryMove(double x, double y)
            {
                double dx = this.x + x;
                double dy = this.y + y;

                foreach (var a in playerslist)
                {
                    if (a == this || a.state==0) continue;

                    if (IsInDistance(dx, dy, a.x, a.y, a.sizeX + this.sizeX / 2, a.sizeY + this.sizeY / 2)) return false;
                }

                foreach (var a in blockList)
                {
                    if (a.isBlocakble && IsInDistance(dx, dy, a.x, a.y, a.sizeX + this.sizeX / 2, a.sizeY + this.sizeY / 2)) return false;
                }

                this.x = dx;
                this.y = dy;
                return true;
            }
        }

        public class bullet
        {
            public string nameplayer;
            public double x;
            public double y;
            double speedX;
            double speedY;
            const int distToExpl = 5;
            public int lifetime = 80;
            public bool forDelele = false;

            public bullet(string nameplayer,double x, double y, double speedX, double speedY)
            {
                this.nameplayer = nameplayer;
                this.speedX = speedX;
                this.speedY = speedY;
                this.x = x;
                this.y = y;
            }

            public void move()
            {
                lifetime--;
                if (lifetime < 0) forDelele = true;
                x += speedX;
                Console.WriteLine("x+" + speedX.ToString());
                y += speedY;
            }

            public bool explosion(double px, double py, double sizeX, double sizeY)
            {
                //return false;

                double dx = 0;
                double dy = 0;
                if (px > this.x) dx = px - this.x;
                else dx = this.x - px;
                if (py > this.y) dy = py - this.y;
                else dy = this.y - py;

                if (dx < sizeX && dy < sizeY) return true;

                return false;
            }
        }

        public class block
        {
            public double x;
            public double y;
            public int sizeX;
            public int sizeY;
            public string type;
            public int lifes;
            public bool forDelete=false;
            public int dir;
            public bool isBlocakble;

            public block(double x, double y, string type, int dir, bool isBlockable, int lifes, int SizeX, int SizeY)
            {
                this.x = x;
                this.y = y;
                this.type = type;
                this.dir = dir;
                this.isBlocakble = isBlockable;
                this.lifes = lifes;
                this.sizeX = SizeX;
                this.sizeY = SizeY;
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

        public void CreateBullet(string name, double spx, double spy)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find == null) return;
            if (find.state == 0) return;

            

            bulletlist.Add(new bullet(find.name, find.x+spx*6, find.y+spy*6, spx, spy));
        }

        public void MoveX(string name, double x)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
                if (find == null) return;

                int dir = 0;
                if (x > 0) dir = 1;
                else dir = 3;

                find.tryMove(x, 0);
                find.direction = dir;
            }
        }

        public void say(string x)
        {
            if (sayCount > countOfMessages)
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

        public void MoveY(string name, double y)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
                if (find == null) return;

                int dir = 0;
                
                if (y > 0) dir = 2;
                else dir = 0;

                find.tryMove(0, y);
                find.direction = dir;
            }
        }

        public string logOrCreate(string name, Color color,string password)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find == null)
            {
                Console.WriteLine("New player! - " + name);
                playerclass player = new playerclass(name, color, password);
                playerslist.Add(player);
                return "New player (" + name + ") created";
            }
            else 
            {
                if (find.password == password) return "Вы подключились";
            };
            return null;
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

        public void setdirect(string name,int dir)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if ((find != null) &&(find.state==1))
            {
                find.headDir = dir;
                //Console.WriteLine(dir);
            }
        }

       static public void work( object source, ElapsedEventArgs e )
        {
            try
            {
                foreach (var a in bulletlist)
                {
                    a.move();
                    foreach (var b in playerslist)
                    {
                        if (b.state != 0 && a.explosion(b.x, b.y, b.sizeX, b.sizeY))
                        {
                            b.state = 0;
                            var player = playerslist.Where(c => c.name == a.nameplayer).FirstOrDefault();
                            player.frags++;
                            a.forDelele = true;
                        }
                    }


                    foreach (var b in blockList)
                    {
                        if (a.explosion(b.x, b.y, b.sizeX, b.sizeY))
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
            catch
            {
            }
        }

       public void addBlock(string name, int type)
       {
           playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
           if (find == null) return;
           if (find.state == 0) return;

           double mx = 0;
           double my = 0;
           double dist = boxTank + boxBlock;

           switch (find.direction)
           {
               case 0: my -= dist; break;
               case 1: mx += dist; break;
               case 2: my += dist; break;
               case 3: mx -= dist; break;
           }
           blockList.Add(new block(find.x+mx, find.y+my, "brick", 0, true, 5, 100, boxBlock));
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
