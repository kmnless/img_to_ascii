using System.Drawing;
using img_to_ascii;

Console.Title = "Image to ASCII";

Gui gui = new Gui();

RenderHandler.renderMode = RENDER_MODE.BLACK_WHITE;
bool success = false;

Image? image = null;

// Trying get image
while (!success)
{
    gui.Start();
    RenderHandler.renderMode = gui.renderMode;
    try
    {
        image = Image.FromFile(gui.path);
        success= true;
    }
    catch
    {
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Wrong path!");
        Console.ForegroundColor = ConsoleColor.Gray;
        success = false;
    }
}

// setting windowsize adjusted to image width
int asciiImageWidth = gui.imageWidth;
Console.SetWindowSize(Convert.ToInt32(asciiImageWidth/4.56), Console.LargestWindowHeight-10);

Bitmap imageBitmap = new Bitmap(image);
const int cellSize = 12;

double transmotionKoef = asciiImageWidth / Convert.ToDouble(image.Width);
imageBitmap = RenderHandler.ResizeImage(image, Convert.ToInt32(asciiImageWidth), Convert.ToInt32(imageBitmap.Height*transmotionKoef));
// make image wider
imageBitmap = RenderHandler.ResizeImage(image, cellSize * (imageBitmap.Width * 175 / 100 / cellSize) * 15/10, cellSize * (imageBitmap.Height / cellSize)*15/10);
image = imageBitmap as Image;

// symbol counters
int bricksX = image.Width / cellSize;
int bricksY = image.Height / cellSize;

char[,] charArr = new char[bricksX, bricksY];

const string asciiTablLeLong = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
const string asciiTableShort = "@&#0Oo*^.   ";
string asciiTable;

if(gui.isAsciiTableLong)
    asciiTable = asciiTablLeLong;
else
    asciiTable = asciiTableShort;

//asciiTable = RenderHandler.Reverse(asciiTable);

// fill symbol array with chars
for (int i = 0; i < bricksX; ++i)
{
    for (int j = 0; j < bricksY; ++j)
    {
        charArr[i, j] = asciiTable[Convert.ToInt32(RenderHandler.GetAverageBrightnessForCell(imageBitmap, cellSize, i, j) * (asciiTable.Length - 1))];
    }
}

RenderHandler.WriteAsAscii(charArr, bricksX, bricksY, imageBitmap, cellSize);
