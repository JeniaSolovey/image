using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageConsole
{
    public class Pixel
    {
        public Color color { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
    public class PixelImage : Pixel
    {
        public Image image;

        public PixelImage(string imagePath , int x, int y)
        {
            image = Bitmap.FromFile(imagePath);
            this.x = x;
            this.y = y;
        }
    }
}
