using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Text;

namespace esufhkehfksdfkjceshk
{
    public class font
    {
        public Textures labels;
        public float label_lengh;
        public float label_height;
        public float label_x;
        public float label_y;
        public bool visible;

        public font(string s, float x, float y, Color color, int size)
        {
            Bitmap bm = new Bitmap(100 * s.Length, 100);
            Graphics gr = Graphics.FromImage(bm);

            var font = new Font(new FontFamily(GenericFontFamilies.SansSerif), size, GraphicsUnit.Pixel);
            var meashures = gr.MeasureString(s, font);
            gr.Clear(Color.Transparent);

            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);

            Brush br = new SolidBrush(color);
            gr.DrawString(s, font, br, rect);

            labels = new Textures(bm);
            label_lengh = (meashures.Width) + x;
            label_x = x;
            label_y = y;
            label_height = (meashures.Height) / 2 + y;
            visible = false;
        }

        public void move(float nx, float ny)
        {
            float dw = Math.Abs(label_x) - Math.Abs(label_lengh);
            float dh = Math.Abs(label_y) - Math.Abs(label_height);
            label_x = nx;
            label_y = ny;
        }
    }
}
