using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using structureClasses;
using System.Timers;

namespace server
{
    class manager
    {
        public static playerList players = new playerList();
        public static blockList blocks = new blockList();
        public static bulletList bullets = new bulletList();

        public static int boxBullet = 2;

        public class playerList : gameList<gameObjPlayer>
        {
            public bool tryMove(int id, double x, double y)
            {
                gameObjPlayer p = elements.Where(c => c.id == id).FirstOrDefault();
                if (p == null) return false;

                double dx = p.x + x;
                double dy = p.y + y;

                foreach (var a in elements)
                {
                    if (a == p || a.state == 0 || a.isOnline == false) continue;

                    if (IsInDistance(dx, dy, a.x, a.y, a.sizeX + p.sizeX / 2, a.sizeY + p.sizeY / 2)) return false;
                }

                foreach (var a in blocks.elements)
                {
                    if (a.isBlocakble && IsInDistance(dx, dy, a.x, a.y, a.sizeX + p.sizeX / 2, a.sizeY + p.sizeY / 2))
                    {
                        switch (a.type)
                        {
                            case "tree": a.type = "treeB"; a.isBlocakble = false; p.nonUpdatable = true; return true;
                            case "houseD": a.type = "houseB"; a.isBlocakble = false; p.nonUpdatable = true; return true;
                            case "car": a.type = "carB"; a.isBlocakble = false; p.nonUpdatable = true; return true;
                        }
                        return false;
                    }
                }

                p.x = dx;
                p.y = dy;
                return true;
            }
        }

        public class blockList : gameList<gameObjBlock>
        {
            public void hit(int id)
            {
                gameObjBlock p = elements.Where(c => c.id == id).First();
                if (p == null) return;

                p.lifes--;

                if (p.lifes < 0)
                {
                    switch (p.type)
                    {
                        case "house": p.type = "houseD"; p.lifes = 0; break;
                        case "houseD": p.type = "houseB"; p.isBlocakble = false; p.nonUpdatable = true; break;
                        case "car": p.type = "carB"; p.isBlocakble = false; p.nonUpdatable = true; break;
                        case "tree": p.type = "treeB"; p.isBlocakble = false; p.nonUpdatable = true; break;
                        default: p.forDelete = true; break;
                    }
                }
            }
        }

        public class bulletList : gameList<gameObjBullet>
        {
            public void move(int id)
            {
                gameObjBullet p = elements.Where(c => c.id == id).FirstOrDefault();
                if (p == null) return;

                p.lifetime--;
                if (p.lifetime < 0) p.forDelete = true;
                p.x += p.speedX;
                p.y += p.speedY;
                //Console.WriteLine(p.x.ToString() + " " + p.y.ToString());
            }

            public bool explosion(int id, double px, double py, double sizeX, double sizeY)
            {
                gameObjBullet p = elements.Where(c => c.id == id).First();
                if (p == null) return false;

                double dx = 0;
                double dy = 0;

                if (px > p.x) dx = px - p.x;
                else dx = p.x - px;
                if (py > p.y) dy = py - p.y;
                else dy = p.y - py;

                if (dx < sizeX && dy < sizeY) return true;

                return false;
            }
        }

        public static bool IsInDistance(double x, double y, double ax, double ay, double distX, double distY)
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

        static public void work(object source, ElapsedEventArgs e)
        {
            //Console.WriteLine(playerslist[0].direction.ToString());
            try
            {
                foreach (var a in players.elements)
                {
                    if (a.timeToShot > 0) a.timeToShot--;
                }

                foreach (var a in bullets.elements)
                {
                    bullets.move(a.id);
                    foreach (var b in players.elements)
                    {
                        if (a.nameplayer != b.name && b.isOnline && b.state != 0 && bullets.explosion(a.id, b.x, b.y, b.sizeX, b.sizeY))
                        {
                            b.state = 0;
                            Console.WriteLine("убит " + b.name);
                            var player = players.elements.Where(c => c.name == a.nameplayer).FirstOrDefault();
                            player.frags++;
                            a.forDelete = true;
                        }
                    }

                    foreach (var b in blocks.elements)
                    {
                        if (b.isBlocakble && bullets.explosion(a.id, b.x, b.y, b.sizeX, b.sizeY))
                        {
                            blocks.hit(b.id);
                            a.forDelete = true;
                        }
                    }

                    foreach (var b in bullets.elements)
                    {
                        if (a != b && bullets.explosion(a.id, b.x, b.y, boxBullet, boxBullet))
                        {
                            a.forDelete = true;
                            b.forDelete = true;
                        }
                    }

                    bullets.clearDeleted();
                    blocks.clearDeleted();
                    players.clearDeleted();
                }
            }
            catch { }
        }
    }
}
