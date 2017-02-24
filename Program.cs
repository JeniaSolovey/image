using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;

namespace ImageConsole
{
    class Program
    {
        static Color GetAVGColor(Bitmap bm)
        {
            int r = 0, g = 0, b = 0;
            Color color = new Color();
            for (int x = 0; x < bm.Width; x += 1)
            {
                for (int y = 0; y < bm.Height; y += 1)
                {
                    color = bm.GetPixel(x, y);
                    r += color.R;
                    g += color.G;
                    b += color.B;
                }
            }
            int size = bm.Width * bm.Height;
            r = r / size;
            g = g / size;
            b = b / size;
            return Color.FromArgb(r,g,b);
        }

        static void Main(string[] args)
        {
            string templatePath = @"1.png";
            Bitmap template  = new Bitmap(templatePath);

            int quality = 1;
            int pixselSize = quality * 10;
            int templateWidth = template.Width;
            int templateHeight = template.Height;
            int resultImageWidth = templateWidth * pixselSize / quality;
            int resultImageHeight = templateHeight * pixselSize/ quality;

            Bitmap resultImage = new Bitmap(resultImageWidth, resultImageHeight);

            List<SourseImage> sourseImages = new List<SourseImage>();

            DirectoryInfo dir = new DirectoryInfo(@"img");
            if (!dir.Exists) return;
            var files = dir.GetFiles();

            Parallel.ForEach(files, item =>
            {
                Color color = GetAVGColor(new Bitmap(item.FullName));
                sourseImages.Add(new SourseImage()
                {
                    Name = item.FullName,
                    R = color.R,
                    G = color.G,
                    B = color.B,
                });
            });

            Console.WriteLine("avg");

            List<Pixel> pixels = new List<Pixel>();
            for (int x = 0; x < templateWidth; x += quality){
                for (int y = 0; y < templateHeight; y += quality)
                {
                    pixels.Add(new Pixel() {x = x,  y = y, color = template.GetPixel(x, y)});
                }
            }

            List<PixelImage> rightImages = new List<PixelImage>();
            foreach (var pixel in pixels)
            {
                int r = 0, g = 0, b = 0;
                r += pixel.color.R; g += pixel.color.G; b += pixel.color.B;

                var rightImg = sourseImages.Select(n =>
                {
                    int nearR = Math.Abs(n.R - r);
                    int nearG = Math.Abs(n.G - g);
                    int nearB = Math.Abs(n.B - b);
                    return new { near = nearR + nearG + nearB, item = n };
                })
                                            .OrderBy(s => s.near)
                                            .First().item.Name;
                rightImages.Add(new PixelImage(rightImg, pixel.x*10, pixel.y*10));
            }

            Console.WriteLine("rItem");

            Graphics graphics = Graphics.FromImage(resultImage);
            foreach (var item in rightImages)
            {
                graphics.DrawImage(item.image, item.x, item.y, pixselSize, pixselSize);
            }


            Console.WriteLine("draw");

            Console.WriteLine("end");
            resultImage.Save("result.png");
            Console.ReadLine();
            
        }
    }
}
