
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace render
{
    public class Render
    {
        public static List<playerclass> playerslist = new List<playerclass>();
        public static List<bullet> bulletList = new List<bullet>();
        public static List<block> blockList = new List<block>();
        public static string name;
        public static string coordPlayer = "";
        public static int zoom=1;

        public class Textures : IDisposable
        {
            int text;
            int width;
            int height;

            public Textures(string way)
            {
                Bitmap Ibm = new Bitmap(way);
                genTex(Ibm);
            }

            public Textures(Bitmap bitmap)
            {
                genTex(bitmap);
            }

            void genTex(Bitmap bitmap)
            {
                Bitmap Ibm = bitmap;
                text = GL.GenTexture();
                bind();
                width = Ibm.Width;
                height = Ibm.Height;
                var bm = Ibm.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bm.Scan0);

                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)SgisTextureEdgeClamp.ClampToEdgeSgis); // при фильтрации игнорируются тексели, выходящие за границу текстуры для s координаты
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)SgisTextureEdgeClamp.ClampToEdgeSgis); // при фильтрации игнорируются тексели, выходящие за границу текстуры для t координаты
            }

            public virtual void Dispose()
            {
                GL.DeleteTexture(text);
            }

            public void bind()
            {
                GL.BindTexture(TextureTarget.Texture2D, text);
            }
        }

        public static void matrix(int w, int h)
        {
            ortoX = 100;
            ortoY = 100;
            if (h == 0) h = 1;
            float aspect = (float)w / (float)h;
            GL.Viewport(0, 0, w, h);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            if (w < h) { GL.Ortho(0, ortoX, ortoY / aspect, 0, -2, 2); ortoY = ortoY / aspect; }
            else { GL.Ortho(0, ortoX * aspect, ortoY, 0, -2, 2); ortoX = ortoX * aspect; }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        public static Textures texBullet;
        public static Textures texTank;
        public static Textures texTankBroke;
        public static List<Textures> texBlocks = new List<Textures>();
        public static float ortoX;
        public static float ortoY;

        public class playerclass
        {
            public string name;
            public int x;
            public int y;
            public Color color;
            public int direction;
            public int state;
            public int sizeX;
            public int sizeY;

            public playerclass(string _name, int sizex, int sizey)
            {
                name = _name;
                x = 50;
                y = 50;
                color = Color.White;
                state = 1;
                sizeX = sizex;
                sizeY = sizey;
            }
        }

        public class bullet
        {
            public int x;
            public int y;


            public bullet(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public class block
        {
            public int x;
            public int y;
            public int sizeX;
            public int sizeY;
            public string type;
            public int lifes = 5;
            public int dir;
            public bool forDelete = false;

            public block(int x, int y, string type, int dir, int sizeX, int sizeY)
            {
                this.x = x;
                this.y = y;
                this.type = type;
                this.dir = dir;
                this.sizeX = sizeX;
                this.sizeY = sizeY;
            }
        }

        public static void RenderMain()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            var player = playerslist.Where(c => c.name == name).FirstOrDefault();
            int tX = -player.x + (int)ortoX / 2;
            int tY = -player.y + (int)ortoY / 2;
            GL.Translate(tX, tY, 0);
            //GL.Scale(kamera, kamera, 1);
            coordPlayer = player.x.ToString() + " " + player.y.ToString();

            foreach (var a in blockList)
            {
                switch (a.type)
                {
                    case "brick": drawQuad(texBlocks[0], a.x - a.sizeX, a.y - a.sizeY, a.x + a.sizeX, a.y + a.sizeY); break;

                }

            }

            foreach (var a in playerslist)
            {
                Color t = a.color;
                GL.Color3(t);
                float angle = a.direction * 90.0f;

                GL.LoadIdentity();
                GL.Translate(tX, tY, 0);
                GL.Translate(a.x, a.y, 0);
                //GL.Scale(kamera, kamera, 1);
                GL.Rotate(angle, 0, 0, 1);

                if (a.state == 1) drawQuad(texTank, -a.sizeX, -a.sizeY, a.sizeX, a.sizeY);
                else drawQuad(texTankBroke, -a.sizeX, -a.sizeY, a.sizeX, a.sizeY);
            }

            GL.LoadIdentity();

            GL.Translate(tX, tY, 0);
            //GL.Scale(kamera, kamera, 1);
            GL.Color3(Color.White);

            foreach (var a in bulletList)
            {
                drawQuad(texBullet, a.x - 2, a.y - 2, a.x + 2, a.y + 2);
            }
        }

        static public void drawQuad(Textures t, float p1x, float p1y, float p2x, float p2y)
        {
            t.bind();
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(p1x, p1y);
            GL.TexCoord2(0, 1); GL.Vertex2(p1x, p2y);
            GL.TexCoord2(1, 1); GL.Vertex2(p2x, p2y);
            GL.TexCoord2(1, 0); GL.Vertex2(p2x, p1y);
            GL.End();
        }
    }
}
