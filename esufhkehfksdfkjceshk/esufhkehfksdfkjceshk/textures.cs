using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace esufhkehfksdfkjceshk
{
    public class Textures : IDisposable
    {
        int text;
        int width;
        int height;

        public Textures(string way)
        {
            Bitmap Ibm = new Bitmap(way);
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
}
