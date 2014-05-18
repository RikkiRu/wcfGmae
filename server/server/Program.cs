using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.ServiceModel;
using System.Timers;
using System.Drawing;
using structureClasses;

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
        void say(string say);
        [OperationContract]
        void addBlock(string name, int type);
        [OperationContract]
        string logOrCreate(string name, Color color, string password);
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

    public class MyObject : IMyobject
    {//
        public static Random rand = new Random();
        public static List<string> sayList = new List<string>();
        public static int sayCount = 6;
        public static int visibleDistance = 100;
        public static int bulletLife = 20;

        public void ping(string name)
        {
            //Console.WriteLine(name);
        }


        public void respawn(string name)
        {
            var player = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            player.state = 1;
            getRandomPlacePlayer(name);
        }

        public static void getRandomPlacePlayer(string name)
        {
            var player = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            player.x = rand.Next(-300, 300);
            player.y = rand.Next(-300, 300);
            int mod = 1;
            if (rand.Next(0, 2) == 0) mod = -1;
            while (!manager.players.tryMove(player.id, 0, 0))
            {
                if (rand.Next(0, 2) == 0) player.x += 100 * mod;
                else player.y += 100 * mod;
            }
        }

        public bool CreateBullet(string name, double spx, double spy)
        {
            gameObjPlayer find = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            if (find.timeToShot != 0) { return false; }
            if (find == null) return false;
            if (find.state == 0) return false;

            gameObjBullet bul = new gameObjBullet();
            bul.nameplayer = find.name;
            bul.x = find.x + spx * 6;
            bul.y = find.y + spy * 6;
            bul.speedX = spx;
            bul.speedY = spy;
            bul.sizeX = 5;
            bul.sizeY = 5;
            bul.lifetime = bulletLife;
            manager.bullets.elements.Add(bul);

            find.timeToShot = 60;
            return true;
        }

        public void Move(string name, double x, double y)
        {
            gameObjPlayer find = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            if (find.state != 0)
            {
                if (find == null) return;
                if (!manager.players.tryMove(find.id, x, y)) return;
                //Console.WriteLine(x.ToString() + " " + y.ToString());

                if (x > 0 && y > 0)
                {
                    find.direction = 3;
                    return;
                }
                if (x > 0 && y < 0)
                {
                    find.direction = 1;
                    return;
                }
                if (x < 0 && y > 0)
                {
                    find.direction = 5;
                    return;
                }
                if (x < 0 && y < 0)
                {
                    find.direction = 7;
                    return;
                }
                if (x > 0)
                {
                    find.direction = 2;
                    return;
                }
                if (x < 0)
                {
                    find.direction = 6;
                    return;
                }
                if (y > 0)
                {
                    find.direction = 4;
                    return;
                }
                if (y < 0)
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

        public string logOrCreate(string name, Color color, string password)
        {
            gameObjPlayer find = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            if (find == null)
            {
                Console.WriteLine("New player! - " + name);
                gameObjPlayer pl = new gameObjPlayer();
                pl.name = name;
                pl.color = color;
                pl.password = password;
                pl.state = 1;
                pl.sizeX = 10;
                pl.sizeY = 10;
                pl.isOnline = true;
                manager.players.elements.Add(pl);
                MyObject.getRandomPlacePlayer(name);
                return "New player (" + name + ") created";
            }
            else
            {
                if (find.password == password) { find.isOnline = true; return "Вы подключились"; }
            };
            return null;
        }

        static string retSay()
        {
            string res = "";
            foreach (var a in sayList)
            {
                res += a + Environment.NewLine;
            }
            return res;
        }

        public string[] GetCommandString(object i, string name)
        {
            if (i is int)
                switch (Convert.ToInt32(i))
                {
                    case 1:
                        return null;

                    case 2: //игрок передал что он оффлайн
                        try
                        {
                            var player = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
                            player.isOnline = false;
                        }
                        catch { }
                        break;

                    default:
                        return null;
                }
            return null;
        }

        public void setdirect(string name, int dir)
        {
            gameObjPlayer find = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            if ((find != null) && (find.state == 1))
            {
                find.headDir = dir;
            }
        }



        public void addBlock(string name, int type)
        {
            gameObjPlayer find = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            if (find == null) return;
            if (find.state == 0) return;

            double mx = 0;
            double my = 0;
            double dist = 20;

            switch (find.direction)
            {
                case 0: my -= dist; break;
                case 1: mx += dist; break;
                case 2: my += dist; break;
                case 3: mx -= dist; break;
            }

            gameObjBlock bl = new gameObjBlock();
            bl.x = find.x + mx;
            bl.y = find.y + my;
            bl.type = "brick";
            bl.color = Color.White;
            bl.isBlocakble = true;
            bl.sizeX = 5;
            bl.sizeY = 100;
            manager.blocks.elements.Add(bl);
        }

        //делает всех оффлайн
        public static void setOffline(object source, ElapsedEventArgs e)
        {
            foreach (var a in manager.players.elements) a.isOnline = false;
        }

        public string getGameObj(int nList, int id, bool update)
        {
            switch (nList)
            {
                //игроки
                case 0:
                    gameObjPlayer pl = manager.players.elements.Where(c => c.id == id).FirstOrDefault();
                    if (pl == null) return "";
                    return pl.getStr(update);

                //пульки пиу пиу
                case 1:
                    gameObjBullet bul = manager.bullets.elements.Where(c => c.id == id).FirstOrDefault();
                    if (bul == null) return "";
                    return bul.getStr(update);

                //объекты
                case 2:
                    gameObjBlock block = manager.blocks.elements.Where(c => c.id == id).FirstOrDefault();
                    if (block == null) return "";
                    return block.getStr(update);
            }

            return "";
        }

        public int[] getIds(int nlist, string name)
        {
            gameObjPlayer player = manager.players.elements.Where(c => c.name == name).FirstOrDefault();
            if (player == null) return null;
            double cx = player.x;
            double cy = player.y;

            switch (nlist)
            {
                case 0: return manager.players.getIds(cx, cy, visibleDistance);
                case 1: return manager.bullets.getIds(cx, cy, visibleDistance);
                case 2: return manager.blocks.getIds(cx, cy, visibleDistance);
            }
            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Загрузка... ждите");
            //ServiceHost host = new ServiceHost(typeof(MyObject), new Uri("http://19/"));
            manager.blocks.elements = generator.genAll();
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
            time.Elapsed += new ElapsedEventHandler(manager.work);
            time.Start();
            timerSlow.Elapsed += new ElapsedEventHandler(MyObject.setOffline);
            timerSlow.Start();
            Console.ReadLine();

            host.Close();
        }


    }
}
