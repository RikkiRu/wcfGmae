using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.ServiceModel;
using CommunicationInterface;
using System.Timers;

namespace CommunicationInterface
{
    public class MyObject : IMyobject
    {
        public static List<playerclass> playerslist = new List<playerclass>();
        public static List<bullet> bulletlist = new List<bullet>();

        public static string retPlayerList()
        {
            string res = "";
            foreach(var a in playerslist)
            {
                res += a.name + "\t";
                res += a.state.ToString() + "\t";
                res += a.direction.ToString() + "\t";
                res += a.color.ToString() + "\t";
                res += a.x.ToString() + "\t";
                res += a.y.ToString() + "\t";
                res += "\n";
            }
            return res;
        }


        public static string retBullet()
        {
            string res = "";
            foreach (var a in bulletlist)
            {
                res += a.x.ToString() + "\t";
                res += a.y.ToString() + "\t";
                res += a.dir.ToString() + "\t";
                res += "\n";
            }
            return res;
        }

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
                    case 0: y-=speed; break;
                    case 1: x+=speed; break;
                    case 2: y+=speed; break;
                    case 3: x-=speed; break;
                }
            }

            public bool explosion(playerclass p)
            {
                int dx=0;
                int dy=0;
                if (p.x > this.x) dx = p.x - this.x;
                else dx = this.x - p.x;
                if (p.y > this.y) dy = p.y - this.y;
                else dy = this.y - p.y;

                if (dx < distToExpl && dy < distToExpl) return true;

                return false;
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
            bulletlist.Add(new bullet(find.x, find.y, find.direction));
        }

        public void MoveX(string name, int x)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
            if (find == null) return;
            find.x += x;
            if (x > 0) find.direction = 1;
            else find.direction = 3;
            }
        }

        public void MoveY(string name, int y)
        {
            playerclass find = playerslist.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
                if (find == null) return;
                find.y += y;
                if (y > 0) find.direction = 2;
                else find.direction = 0;
            }
        }

        public object GetCommandString(object i)
        {
            if(i is int)
            switch (Convert.ToInt32(i))
            {
                case 1: Console.WriteLine(" - запрос списка игроков"+DateTime.Now.ToString());
                    return MyObject.retPlayerList() + "!" + MyObject.retBullet();
                default:
                    return "Получил " + Convert.ToString(i);
                    
            }
            
            else
            {
                playerclass find = playerslist.Where(c => c.name == i.ToString()).FirstOrDefault();
                if (find == null)
                {
                    Console.WriteLine("New player! - "+i.ToString());
                    playerclass player = new playerclass(i.ToString());
                    playerslist.Add(player);
                    return "New player (" + i + ") created";
                }
                else return "Вы подключились";
            }
        }

       static public void work( object source, ElapsedEventArgs e )
        {
           foreach(var a in bulletlist)
           {
               a.move();
               foreach (var b in playerslist)
               {
                   if(a.explosion(b))
                   {
                       b.state = 0;
                   }
               }
           }
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
            ServiceHost host = new ServiceHost(typeof(MyObject), new Uri("http://"+http+"/"));
            host.AddServiceEndpoint(typeof(IMyobject), new BasicHttpBinding(), "");
            host.Open();
            Console.WriteLine("Сервер запуще222н");


            Timer time = new Timer(100);
            time.Elapsed += new ElapsedEventHandler(MyObject.work);
            time.Start();

            MyObject.playerslist.Add(new MyObject.playerclass("vasia"));
            MyObject.bulletlist.Add(new MyObject.bullet(0, 0, 1));

            Console.ReadLine();

            host.Close();
        }

       
    }
}
