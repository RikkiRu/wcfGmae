using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace structureClasses
{
    public class gameObj
    {
        public const int fields = 7;
        public int id;
        public double x;
        public double y;
        public double sizeX;
        public double sizeY;
        public bool forDelete = false;
        public bool nonUpdatable = false;

        static int autoincrement=0;
        public gameObj()
        {
            this.id = autoincrement;
            autoincrement++;
        }

        protected const char separator = '&';
        protected string[] temp; //тут храняться результаты сплита
  
        protected string addField(string my, string x)
        {
            if (x.Contains(separator)) return my;
            return my + x + separator.ToString();
        }

        public string getStr(bool isFast)
        {
            string res = "";
            res = addField(res, isFast.ToString());
            string temp;
            if (isFast) temp = getFast();
            else temp = getFull();
            res += temp;
            return res;
        }

        protected void makeStrings(string x)
        {
            char[] delit = new char[1];
            delit[0] = separator;
            temp = x.Split(delit);
        }

        public void setObj(string x)
        {
            if (x == "") return;
            makeStrings(x);
            bool isF = Convert.ToBoolean(temp[0]);
            if (isF)
            {
                updateObj();
            }
            else
            {
                makeObj();
            }
        }

        protected virtual string getFast()
        {
            string res = "";
            res = addField(res, id.ToString());
            res = addField(res, x.ToString());
            res = addField(res, y.ToString());
            res = addField(res, "");
            res = addField(res, "");
            res = addField(res, "");
            return res;
        }

        protected virtual string getFull()
        {
            string res = "";
            res = addField(res, id.ToString());
            res = addField(res, x.ToString());
            res = addField(res, y.ToString());
            res = addField(res, sizeX.ToString());
            res = addField(res, sizeY.ToString());
            res = addField(res, nonUpdatable.ToString());
            return res;
        }

        protected virtual void updateObj()
        {
            this.id = Convert.ToInt32(temp[1]);
            this.x = Convert.ToDouble(temp[2]);
            this.y = Convert.ToDouble(temp[3]);
        }

        protected virtual void makeObj()
        {
            this.id = Convert.ToInt32(temp[1]);
            this.x = Convert.ToDouble(temp[2]);
            this.y = Convert.ToDouble(temp[3]);
            this.sizeX = Convert.ToDouble(temp[4]);
            this.sizeY = Convert.ToDouble(temp[5]);
            this.nonUpdatable = Convert.ToBoolean(temp[6]);
        }
    }

    public class gameObjColoredDirected:gameObj
    {
        new public const int fields = gameObj.fields + 2;

        public int direction;
        public Color color;

        protected override string getFast()
        {
            string res = base.getFast();
            res = addField(res, direction.ToString());
            res = addField(res, "");
            return res;
        }

        protected override string getFull()
        {
            string res = base.getFull();
            res = addField(res, direction.ToString());
            res = addField(res, color.R.ToString() + "_" + color.G.ToString() + "_" + color.B.ToString());
            return res;
        }

        protected override void updateObj()
        {
            base.updateObj();
            this.direction = Convert.ToInt32(temp[gameObj.fields]);
        }

        protected override void makeObj()
        {
            base.makeObj();
            this.direction = Convert.ToInt32(temp[gameObj.fields]);
            string[] col = temp[gameObj.fields + 1].Split('_');
            this.color = Color.FromArgb(Convert.ToInt32(col[0]), Convert.ToInt32(col[1]), Convert.ToInt32(col[2]));
        }
    }

    public class gameObjPlayer : gameObjColoredDirected
    {
        new public const int fields = gameObjColoredDirected.fields + 5;

        public string name;
        public int state;
        public int headDir;
        public int frags;
        public string password;
        public bool isOnline;
        public int timeToShot;

        protected override string getFast()
        {
            string res = base.getFast();
            res = addField(res, state.ToString());
            res = addField(res, headDir.ToString());
            res = addField(res, frags.ToString());
            res = addField(res, isOnline.ToString());
            res = addField(res, "");
            return res;
        }

        protected override string getFull()
        {
            string res = base.getFull();
            res = addField(res, state.ToString());
            res = addField(res, headDir.ToString());
            res = addField(res, frags.ToString());
            res = addField(res, isOnline.ToString());
            res = addField(res, name);
            return res;
        }

        protected override void updateObj()
        {
            base.updateObj();
            this.state = Convert.ToInt32(temp[gameObjColoredDirected.fields]);
            this.headDir = Convert.ToInt32(temp[gameObjColoredDirected.fields + 1]);
            this.frags = Convert.ToInt32(temp[gameObjColoredDirected.fields + 2]);
            this.isOnline = Convert.ToBoolean(temp[gameObjColoredDirected.fields + 3]);
        }

        protected override void makeObj()
        {
            base.makeObj();
            this.state = Convert.ToInt32(temp[gameObjColoredDirected.fields]);
            this.headDir = Convert.ToInt32(temp[gameObjColoredDirected.fields + 1]);
            this.frags = Convert.ToInt32(temp[gameObjColoredDirected.fields + 2]);
            this.isOnline = Convert.ToBoolean(temp[gameObjColoredDirected.fields + 3]);
            this.name = temp[gameObjColoredDirected.fields + 4];
        }
    }

    public class gameObjBullet : gameObj
    {
        public string nameplayer;
        public double speedX;
        public double speedY;
        public int lifetime;
    }

    public class gameObjBlock : gameObjColoredDirected
    {
        new public const int fields = gameObjColoredDirected.fields + 3;

        public bool isBlocakble;
        public string type;
        public int lifes;

        protected override string getFast()
        {
            string res = base.getFast();
            res = addField(res, type);
            res = addField(res, lifes.ToString());
            res = addField(res, "");
            return res;
        }

        protected override string getFull()
        {
            string res = base.getFull();
            res = addField(res, type);
            res = addField(res, lifes.ToString());
            res = addField(res, isBlocakble.ToString());
            return res;
        }

        protected override void updateObj()
        {
            base.updateObj();
            this.type = temp[gameObjColoredDirected.fields];
            this.lifes = Convert.ToInt32(temp[gameObjColoredDirected.fields + 1]);
        }

        protected override void makeObj()
        {
            base.makeObj();
            this.type = temp[gameObjColoredDirected.fields];
            this.lifes = Convert.ToInt32(temp[gameObjColoredDirected.fields + 1]);
            this.isBlocakble = Convert.ToBoolean(temp[gameObjColoredDirected.fields + 2]);
        }
    }

    public class gameList<typeOfgameObj> where typeOfgameObj:gameObj
    {
        public delegate string delegatUpdate(int id);
        public delegate typeOfgameObj delCreate(int id);

        public List<typeOfgameObj> elements;
        public delegatUpdate updElem;
        public delCreate createElem;

        public gameList()
        {
            elements = new List<typeOfgameObj>();
        }

        public void update(int[] x)
        {
            if (x == null) return;

            foreach (var a in elements)
            {
                if (a.nonUpdatable) continue;
                if (!x.Contains(a.id)) { a.forDelete = true; continue; }
                else
                {
                    a.setObj(updElem(a.id));
                }
            }

            for (int i = 0; i < x.GetLength(0); i++)
            {
                typeOfgameObj n = elements.Where(c => c.id == x[i]).FirstOrDefault();
                if ( n == null)
                {
                    typeOfgameObj n2 = createElem(x[i]);
                    elements.Add(n2);
                }
            }
        }

        public int[] getIds(double x, double y, double radius)
        {
            List<int> reList = new List<int>();

            foreach (var a in elements)
            {
                if (IsInDistance(x, y, a.x, a.y, radius, radius))
                {
                    if (a is gameObjPlayer) if ((a as gameObjPlayer).isOnline == false) continue;
                    reList.Add(a.id);
                }
            }

            return reList.ToArray();
        }

        public void clearDeleted()
        {
            for (int i = elements.Count - 1; i >= 0; i--)
            {
                if (elements[i] == null) continue;
                if (elements[i].forDelete)
                {
                    elements.Remove(elements[i]);
                }
            }
        }

        static bool IsInDistance(double x, double y, double ax, double ay, double distX, double distY)
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
    }
}
