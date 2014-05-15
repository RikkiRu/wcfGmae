using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace structureClasses
{
    public class gameObj
    {
        public int id;
        public double x;
        public double y;
        public double sizeX;
        public double sizeY;

        virtual string getFull()
        {
            string res = "b&"; //значит полный объект
            res += id.ToString() + "&";
            res += x.ToString() + "&";
            res += y.ToString() + "&";
            res += sizeX.ToString() + "&";
            res += sizeY.ToString();
            return res;
        }

        virtual string getFast()
        {
            string res = "s&"; //значит не полный
            res += id.ToString() + "&";
            res += x.ToString() + "&";
            res += y.ToString();
            return res;
        }
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
        public int timeToShot;
    }

    public class bullet
    {
        const int distToExpl = 5;
        const int lifetime = 80;

        public string nameplayer;
        public double x;
        public double y;
        double speedX;
        double speedY;
        public bool forDelele = false;
    }

    public class block
    {
        public double x;
        public double y;
        public int sizeX;
        public int sizeY;
        public string type;
        public int lifes;
        public bool forDelete = false;
        public int dir;
        public bool isBlocakble;
        public Color color;
    }
}
