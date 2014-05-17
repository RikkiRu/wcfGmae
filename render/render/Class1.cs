
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using structureClasses;

namespace render
{
    public class Render
    {
        public static gameList<gameObjPlayer> playerslist = new gameList<gameObjPlayer>();
        public static gameList<gameObjBullet> bulletList = new gameList<gameObjBullet>();
        public static gameList<gameObjBlock> blockList = new gameList<gameObjBlock>();

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
        public static Textures texTankHead;
        public static Dictionary<string, Textures> texBlocks = new Dictionary<string, Textures>();
        public static float ortoX;
        public static float ortoY;

        public static void RenderMain()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            var player = playerslist.elements.Where(c => c.name == name).FirstOrDefault();
            double tX = -player.x + ortoX / 2;
            double tY = -player.y + ortoY / 2;
            GL.Translate(tX, tY, 0);
            //coordPlayer = player.x.ToString() + " " + player.y.ToString();
            coordPlayer = "x: "+((int)player.x).ToString() +"   y: "+ ((int)player.y).ToString();

            foreach (var a in blockList.elements)
            {
                GL.Color3(a.color);
                GL.PushMatrix();
                GL.Translate(a.x, a.y, 0);
                GL.Rotate(a.direction, 0, 0, 1);
                drawQuad(texBlocks[a.type], - a.sizeX, - a.sizeY, a.sizeX, a.sizeY); 
                GL.PopMatrix(); 
            }

            foreach (var a in playerslist.elements)
            {
                if (a.state != 1)
                {
                    GL.PushMatrix();
                    Color t = a.color;
                    GL.Color3(t);
                    float angle = a.direction * 45.0f;
                    GL.Translate(a.x, a.y, 0);
                    GL.Rotate(angle, 0, 0, 1);
                    drawQuad(texTankBroke, -a.sizeX, -a.sizeY, a.sizeX, a.sizeY);
                    GL.PopMatrix();
                }
            }

            foreach (var a in playerslist.elements)
            {
                if (a.state == 1)
                {
                    GL.PushMatrix();
                    Color t = a.color;
                    GL.Color3(t);
                    float angle = a.direction * 45.0f;
                    GL.Translate(a.x, a.y, 0);
                    GL.Rotate(angle, 0, 0, 1);
                    drawQuad(texTank, -a.sizeX, -a.sizeY, a.sizeX, a.sizeY);
                    GL.PopMatrix();

                    GL.PushMatrix();
                    GL.Translate(a.x, a.y, 0);
                    GL.Rotate(a.headDir, 0, 0, 1);
                    drawQuad(texTankHead, -a.sizeX * 1.5, -a.sizeY * 1.5f, a.sizeX * 1.5f, a.sizeY * 1.5f);
                    GL.PopMatrix();
                }
            }

            GL.Color3(Color.White);
            foreach (var a in bulletList.elements)
            {
                drawQuad(texBullet, a.x - 2, a.y - 2, a.x + 2, a.y + 2);
            }
        }

        static public void drawQuad(Textures t, double p1x, double p1y, double p2x, double p2y)
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
