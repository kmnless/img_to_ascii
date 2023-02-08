using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace img_to_ascii
{
    static class RenderHandler
    {
        public static RENDER_MODE renderMode { get; set; }

        public static string Reverse(string a)
        {
            string toReturn = "";
            for (int i = 0; i < a.Length; ++i)
            {
                toReturn += a[a.Length - i - 1];
            }
            return toReturn;
        }

        public static void WriteAsAscii(char[,] arr, int sizeX, int sizeY, Bitmap? imageBitmap = null, int cellSize = 0)
        {
            ConsoleColor color = ConsoleColor.Gray;
            for (int x = 0; x < sizeY; ++x)
            {
                for (int y = 0; y < sizeX; ++y)
                {
                    if (imageBitmap != null && cellSize != 0 && renderMode == RENDER_MODE.COLORED)
                    {
                        color = GetAverageColorForCell(imageBitmap, cellSize, y, x);
                    }
                    else if (imageBitmap != null && cellSize != 0 && renderMode == RENDER_MODE.MONOCHROME)
                    {
                        float brightness = GetAverageBrightnessForCell(imageBitmap, cellSize, y, x);
                        if (brightness <= 0.4)
                            color = ConsoleColor.DarkGray;
                        else if (brightness <= 0.75 && brightness > 0.4)
                            color = ConsoleColor.Gray;
                        else if (brightness > 0.75)
                            color = ConsoleColor.White;
                    }
                    Console.ForegroundColor = color;
                    Console.Write(arr[y, x]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }
        }

        public static float GetAverageBrightnessForCell(Bitmap imageBitmap, int cellSize, int CellNumX, int CellNumY)
        {
            float summBrightness = 0;
            float pixelBrightness = 0;

            for (int x = 0; x < cellSize; ++x)
            {
                for (int y = 0; y < cellSize; ++y)
                {
                    pixelBrightness = imageBitmap.GetPixel(x + CellNumX * cellSize, y + CellNumY * cellSize).GetBrightness();
                    summBrightness += pixelBrightness;
                }
            }
            return summBrightness / cellSize / cellSize;
        }

        public static ConsoleColor GetAverageColorForCell(Bitmap imageBitmap, int cellSize, int CellNumX, int CellNumY)
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            for (int x = 0; x < cellSize; ++x)
            {
                for (int y = 0; y < cellSize; ++y)
                {
                    red += imageBitmap.GetPixel(x + CellNumX * cellSize, y + CellNumY * cellSize).R;
                    green += imageBitmap.GetPixel(x + CellNumX * cellSize, y + CellNumY * cellSize).G;
                    blue += imageBitmap.GetPixel(x + CellNumX * cellSize, y + CellNumY * cellSize).B;
                }
            }
            red = red / cellSize / cellSize;
            green = green / cellSize / cellSize;
            blue = blue / cellSize / cellSize;

            Color color = Color.FromArgb(red, green, blue);

            return GetConsoleColor(color);
        }

        public static ConsoleColor GetConsoleColor(Color color)
        {
            if (color.GetSaturation() < 0.5)
            {
                // we have a grayish color
                switch ((int)(color.GetBrightness() * 3.5))
                {
                    case 0: return ConsoleColor.Black;
                    case 1: return ConsoleColor.DarkGray;
                    case 2: return ConsoleColor.Gray;
                    default: return ConsoleColor.White;
                }
            }
            int hue = (int)Math.Round(color.GetHue() / 60, MidpointRounding.AwayFromZero);
            if (color.GetBrightness() < 0.4)
            {
                // dark color
                switch (hue)
                {
                    case 1: return ConsoleColor.DarkYellow;
                    case 2: return ConsoleColor.DarkGreen;
                    case 3: return ConsoleColor.DarkCyan;
                    case 4: return ConsoleColor.DarkBlue;
                    case 5: return ConsoleColor.DarkMagenta;
                    default: return ConsoleColor.DarkRed;
                }
            }
            // bright color
            switch (hue)
            {
                case 1: return ConsoleColor.Yellow;
                case 2: return ConsoleColor.Green;
                case 3: return ConsoleColor.Cyan;
                case 4: return ConsoleColor.Blue;
                case 5: return ConsoleColor.Magenta;
                default: return ConsoleColor.Red;
            }
        }

        public static Bitmap ResizeImage(Image image, int new_width, int new_height)
        {
            Rectangle destRect = new Rectangle(0, 0, new_width, new_height);
            Bitmap? destImage = new Bitmap(new_width, new_height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics? graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
