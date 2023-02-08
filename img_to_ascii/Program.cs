using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

bool withColor = true;

Image image = Image.FromFile("D:\\testimages\\hitler2.png");
Bitmap imageBitmap = new Bitmap(image);
int cellSize = 3;

// Resize image for cells
while (image.Width % cellSize != 0 && image.Height % cellSize != 0)
{
    cellSize++;
    if (cellSize * (image.Height / cellSize) == 0 || cellSize * (image.Width / cellSize) == 0)
    {
        //Console.WriteLine("Not resized for cell aspect ratio");
        break;
    }
    if (cellSize >= 30)
    {
        imageBitmap = ResizeImage(image, cellSize * (image.Width / cellSize), cellSize * (image.Height / cellSize));
        //Console.WriteLine("Resized");
        break;
    }
}

// make image wider
imageBitmap = ResizeImage(image, cellSize * (image.Width * 175 / 100 / cellSize) * 15/10, cellSize * (image.Height / cellSize)*15/10);
image = imageBitmap as Image;

// symbol counters
int bricksX = image.Width / cellSize;
int bricksY = image.Height / cellSize;

char[,] charArr = new char[bricksX, bricksY];
string asciiTable = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
//string asciiTable = "@&#0Oo*^.  ";

asciiTable = Reverse(asciiTable);

// fill symbol array with chars
for (int i = 0; i < bricksX; ++i)
{
    for (int j = 0; j < bricksY; ++j)
    {
        charArr[i, j] = asciiTable[Convert.ToInt32(GetAverageBrightnessForCell(imageBitmap, cellSize, i, j) * (asciiTable.Length - 1))];
    }
}

WriteAsAscii(charArr, bricksX, bricksY, imageBitmap, cellSize);


//Console.ForegroundColor = (ConsoleColor)imageBitmap.GetPixel(0, 0).ToKnownColor();


string Reverse(string a)
{
    string toReturn = "";
    for (int i = 0; i < a.Length; ++i)
    {
        toReturn += a[a.Length - i - 1];
    }
    return toReturn;
}

void WriteAsAscii(char[,] arr, int sizeX, int sizeY, Bitmap? imageBitmap = null, int cellSize = 0)
{
    ConsoleColor color = ConsoleColor.Gray;
    for (int x = 0; x < sizeY; ++x)
    {
        for (int y = 0; y < sizeX; ++y)
        {
            if (imageBitmap != null && cellSize != 0 && withColor)
            {
                color = GetAverageColorForCell(imageBitmap, cellSize, y, x);
            }
            Console.ForegroundColor = color;
            Console.Write(arr[y, x]);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        Console.WriteLine();
    }
}

float GetAverageBrightnessForCell(Bitmap imageBitmap, int cellSize, int CellNumX, int CellNumY)
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

ConsoleColor GetAverageColorForCell(Bitmap imageBitmap, int cellSize, int CellNumX, int CellNumY)
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
    byte r = Convert.ToByte(red / (cellSize * cellSize));
    byte g = Convert.ToByte(green / (cellSize * cellSize));
    byte b = Convert.ToByte(blue / (cellSize * cellSize));
    Color color = Color.FromArgb(red, green, blue); 

    return GetConsoleColor(color);
}

ConsoleColor GetConsoleColor(Color color)
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

Bitmap ResizeImage(Image image, int new_width, int new_height)
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
