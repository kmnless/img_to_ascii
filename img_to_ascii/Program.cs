using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

Image image = Image.FromFile("D:\\testimages\\otval.jpg");
Bitmap image_bitmap = new Bitmap(image);
int cell_size = 31;

while(image.Width%cell_size!=0 && image.Height %cell_size!= 0)
{
    Console.WriteLine(cell_size);
    cell_size++;
    if(cell_size * (image.Height / cell_size) == 0 || cell_size * (image.Width / cell_size) == 0)
    {
        Console.WriteLine("Image is in bad res");
        break;
    }
    if (cell_size >= 50)
    {
        image_bitmap = ResizeImage(image, cell_size * (image.Width / cell_size), cell_size * (image.Height / cell_size));
        Console.WriteLine("Success");
        break;
    }
}

int bricksX = image.Width / cell_size;
int bricksY = image.Height / cell_size;
Image newImage = image_bitmap as Image;
newImage.Save("D:\\testimages\\resized\\oisya1.png");
int GetAverageBrightness(Bitmap imageBitmap, int cellSize, int CellNumX, int CellNumY) 
{
    int summBrightness = 0;
    for(int x = 0; x < cellSize; ++x)
    {
        for (int y = 0; y < cellSize; ++y)
        {
            imageBitmap.GetPixel(x+CellNumX)
        }
    }
}
static Bitmap ResizeImage(Image image, int new_width, int new_height)
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
