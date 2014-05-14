using System;
using System.Collections.Generic;
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

            int px = -100;
            int py = -100;
            int distX = 20;
            int distY = 20;
            int sizeX = 10;
            int sizeY = 10;

            for (int i = 0; i < 10; i++)
            {
                px = 0;
                for (int j = 0; j < 10; j++)
                {
                    switch (rand.Next(0, 5))
                    {
                        case 0: 
                            res.Add(new MyObject.block(px, py, "house", 0, true, 5, sizeX, sizeY));
                            break;
                        case 1:
                            res.Add(new MyObject.block(px, py, "tree", 0, true, 5, sizeX / 2, sizeX / 2));
                            break;
                        default: 
                            res.Add(new MyObject.block(px, py, "road", 0, false, 0, sizeX, sizeY));
                            break;
                    }
                    px += distX;
                }
                py += distY;
            }

            return res;
        }
    }
}
