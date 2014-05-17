using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using structureClasses;

namespace server
{
    public class generator
    {
        static Random rand = new Random();

        public static List<gameObjBlock> genAll()
        {
            List<gameObjBlock> res = new List<gameObjBlock>();

            int px = -500;
            int py = -500;
            int distX = 20;
            int distY = 20;
            int sizeX = 10;
            int sizeY = 10;

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    switch (rand.Next(0, 5))
                    {
                        case 0:
                            res.Add(add(px, py, "house", 0, true, 0, sizeX, sizeY));
                            break;
                        case 1:
                            res.Add(add(px, py, "tree", 0, true, 0, sizeX / 2, sizeX / 2));
                            break;
                        default:
                            res.Add(add(px, py, "road", 0, false, 0, sizeX, sizeY));
                            if (rand.Next(0, 3) == 0)
                            {
                                var a = add(px, py, "car", rand.Next(0, 360), true, 0, sizeX / 2, sizeY / 2);
                                a.color = Color.FromArgb(rand.Next(100, 256), rand.Next(100, 256), rand.Next(100, 256));
                                res.Add(a);
                            }
                            break;
                    }
                    px += distX;
                }
                py += distY;
                px = -500;
            }

            //res.Add(add(-20, -20, "house", 0, true, 0, sizeX, sizeY));

            return res;
        }

        public static gameObjBlock add(double px, double py, string type, int lifes, bool isBlockable, int dir, double sizeX, double  sizeY)
        {
            gameObjBlock b = new gameObjBlock();
            b.x = px;
            b.y = py;
            b.type = type;
            b.lifes = lifes;
            b.isBlocakble = isBlockable;
            b.direction = dir;
            b.sizeX = sizeX;
            b.sizeY = sizeY;
            b.color = Color.White;
            return b;
        }
    }
}
