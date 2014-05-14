using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace server
{
    public class generator
    {
        static Random rand = new Random();

        public static List<MyObject.block> genAll()
        {
            List<MyObject.block> res = new List<MyObject.block>();

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
                            res.Add(new MyObject.block(px, py, "house", 0, true, 0, sizeX, sizeY));
                            break;
                        case 1:
                            res.Add(new MyObject.block(px, py, "tree", 0, true, 0, sizeX / 2, sizeX / 2));
                            break;
                        default:
                            res.Add(new MyObject.block(px, py, "road", 0, false, 0, sizeX, sizeY));
                            if (rand.Next(0, 10) == 0)
                            {
                                var a = new MyObject.block(px, py, "car", rand.Next(0, 360), true, 0, sizeX / 2, sizeY / 2);
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

            return res;
        }
    }
}
