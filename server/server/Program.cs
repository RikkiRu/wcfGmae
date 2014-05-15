using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.ServiceModel;
using System.Timers;
using System.Drawing;


namespace server
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
        void ping(string name);
        [OperationContract]
        void respawn(string name);
    }

    public class MyObject : IMyobject
    {//
        public static List<playerclass> playerslist = new List<playerclass>();
        public static List<bullet> bulletlist = new List<bullet>();
        public static List<block> blockList = new List<block>();

        public static Random rand = new Random();
        public static List<string> sayList=new List<string>();
        public static int sayCount=6;
        public static int visibleDistance=150;
        public static int boxTank = 10;
        public static int boxBlock = 10;
        public static int boxBullet = 2;

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
            getRandomPlacePlayer(name);
        }

        public static void getRandomPlacePlayer(string name)
        {
            var player = playerslist.Where(c => c.name == name).FirstOrDefault();
            player.x = rand.Next(-300, 300);
            player.y = rand.Next(-300, 300);
            int mod = 1;
            if (rand.Next(0, 2) == 0) mod = -1;
            while (!player.tryMove(0, 0))
            {
                if (rand.Next(0, 2) == 0) player.x += 100 * mod;
                else player.y += 100 * mod;
            }
        }

        public static string retPlayerList(double x, double y)
        {
            string res = "";
            foreach(var a in playerslist)
            {
               if (a.isOnline)
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
                    res += a.color.R.ToString() + "_" + a.color.G + "_" + a.color.B + "\t";
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
            public bool isOnline;
            public int timeToShot = 0;

            public playerclass(string _name, Color color, string password)
            {
                name = _name;
                this.password = password;
                frags = 0;
                sizeX = 10;
                sizeY = 10;
                if (name == "god") sizeX = sizeY = 50;
                this.color = color;
                state = 1;
                //MyObject.getRandomPlacePlayer(_name);
            }

            public bool tryMove(double x, double y)
            {
                double dx = this.x + x;
                double dy = this.y + y;

                foreach (var a in playerslist)
                {
                    if (a == this || a.state==0 || a.isOnline==false) continue;

                    if (IsInDistance(dx, dy, a.x, a.y, a.sizeX + this.sizeX / 2, a.sizeY + this.sizeY / 2)) return false;
                }

                foreach (var a in blockList)
                {
                    if (a.isBlocakble && IsInDistance(dx, dy, a.x, a.y, a.sizeX + this.sizeX / 2, a.sizeY + this.sizeY / 2))
                    {
                        switch (a.type)
                        {
                            case "tree": a.type = "treeB"; a.isBlocakble = false; return true;
                            case "houseD": a.type = "houseB"; a.isBlocakble = false; return true;
                            case "car": a.type = "carB"; a.isBlocakble = false; return true;
                        }
                        return false;
                    }
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
                //Console.WriteLine("x+" + speedX.ToString());
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
            public Color color;

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
                this.color = Color.White;
            }

            public void hit()
            {
                lifes--;

                if (lifes < 0)
                {
                    switch (this.type)
                    {
                        case "house": type = "houseD"; lifes = 0; break;
                        case "houseD": type = "houseB"; isBlocakble = false; break;
                        case "car": type = "carB"; isBlocakble = false; break;
                        case "tree": type = "treeB"; isBlocakble = false; break;
                        default:  forDelete = true; break;
                    }
                }
            }
        }

        public int state(string name)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            return find.state;
        }

        public bool CreateBullet(string name, double spx, double spy)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.timeToShot != 0) { return false; }
            if (find == null) return false;
            if (find.state == 0) return false;
         
            bulletlist.Add(new bullet(find.name, find.x+spx*6, find.y+spy*6, spx, spy));
            find.timeToShot = 60;
            return true;
        }

        public void Move(string name, double x, double y)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
                if (find == null) return;
                if (!find.tryMove(x, y)) return;
                //Console.WriteLine(x.ToString() + " " + y.ToString());

                if(x>0 && y>0)
                {
                    find.direction = 3;
                    return;
                }
                if(x>0 && y<0)
                {
                    find.direction = 1;
                    return;
                }
                if(x<0 && y>0)
                {
                    find.direction = 5;
                    return;
                }
                if(x<0 && y<0)
                {
                    find.direction = 7;
                    return;
                }
                if(x>0)
                {
                    find.direction = 2;
                    return;
                }
                if(x<0)
                {
                    find.direction = 6;
                    return;
                }
                if(y>0)
                {
                    find.direction = 4;
                    return;
                }
                if(y<0)
                {
                    find.direction = 0;
                    return;
                }
            }
        }

        public void say(string x)
        {
            sayList.Add(x);
            if (sayList.Count > sayCount)
            {
                sayList.RemoveAt(0);
                sayCount--;
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
                MyObject.getRandomPlacePlayer(name);
                return "New player (" + name + ") created";
            }
            else 
            {
                if (find.password == password) return "Вы подключились";
            };
            return null;
        }

        static string retSay()
        {
            string res = "";
            foreach(var a in sayList)
            {
                res += a + Environment.NewLine;
            }
            return res;
        }

        ///getcomanstring
        ///getcomanstring
        ///getcomanstring

        public string[] GetCommandString(object i, string name)
        {
            if(i is int)
            switch (Convert.ToInt32(i))
            {
                case 1:
                    try
                    {
                        var player = playerslist.Where(c => c.name == name).FirstOrDefault();
                        player.isOnline = true;
                        //Console.WriteLine(" - запрос списка игроков" + DateTime.Now.ToString());
                        string[] abv = new string[4];
                        abv[0] = MyObject.retPlayerList(player.x, player.y);
                        abv[1] = MyObject.retBullet(player.x, player.y);
                        abv[2] = retSay();
                        abv[3] = MyObject.retBlock(player.x, player.y); 
                        //Console.WriteLine(abv);
                        return abv;
                    }
                    catch
                    {
                        return null;
                    }

                case 2: //игрок передал что он оффлайн
                    try
                    {
                        var player = playerslist.Where(c => c.name == name).FirstOrDefault();
                        player.isOnline = false;
                    }
                    catch { }
                    break;

                default:
                    return null;
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
                //Console.WriteLine(playerslist[0].direction.ToString());

                foreach (var a in playerslist)
                {
                    if (a.timeToShot > 0) a.timeToShot--;
                }

                foreach (var a in bulletlist)
                {
                    a.move();
                    foreach (var b in playerslist)
                    {
                        if (a.nameplayer!=b.name && b.isOnline && b.state != 0 && a.explosion(b.x, b.y, b.sizeX, b.sizeY))
                        {
                            b.state = 0;
                            var player = playerslist.Where(c => c.name == a.nameplayer).FirstOrDefault();
                            player.frags++;
                            a.forDelele = true;
                        }
                    }


                    foreach (var b in blockList)
                    {
                        if (b.isBlocakble && a.explosion(b.x, b.y, b.sizeX, b.sizeY))
                        {
                            b.hit();
                            a.forDelele = true;
                        }
                    }

                    foreach (var b in bulletlist)
                    {
                        if (a!=b && a.explosion(b.x, b.y, boxBullet, boxBullet))
                        {
                            a.forDelele = true;
                            b.forDelele = true;
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

        //делает всех оффлайн
       public static void setOffline(object source, ElapsedEventArgs e)
       {
           foreach (var a in playerslist) a.isOnline = false;
       }
    }

    class Program
    {
        static void Main(string[] args)
        {
       
            Console.WriteLine("Загрузка... ждите");
            //ServiceHost host = new ServiceHost(typeof(MyObject), new Uri("http://19/"));
            MyObject.blockList = generator.genAll();
            string http;
            Console.WriteLine("Нажмите enter");
            http = Console.ReadLine();
            if (http == "") http = "localhost";
            Console.WriteLine("Запуск... ждите");
            ServiceHost host = new ServiceHost(typeof(MyObject), new Uri("http://" + http + "/"));
            host.AddServiceEndpoint(typeof(IMyobject), new BasicHttpBinding(), "");
            host.Open();
            Console.WriteLine("Сервер запущен. Enter для выхода.");
            Timer time = new Timer(30);
            Timer timerSlow = new Timer(60000);
            time.Elapsed += new ElapsedEventHandler(MyObject.work);
            time.Start();
            timerSlow.Elapsed += new ElapsedEventHandler(MyObject.setOffline);
            timerSlow.Start();
            Console.ReadLine();

            host.Close();
        }

       
    }
}
