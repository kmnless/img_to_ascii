using System.Runtime.InteropServices;

namespace img_to_ascii
{
    class Gui
    {
        public string? path;

        public RENDER_MODE renderMode = RENDER_MODE.MONOCHROME;

        public bool Exit = false;

        public bool isAsciiTableLong = false;

        public int imageWidth = 550; 

        public void Start()
        {
            byte choice = 1;
            ConsoleKey key;
            byte keyCount = 3;
            bool isWorking = true;

            while (isWorking)
            {
                Console.CursorVisible = false;
                Console.WriteLine("\t\tImage to ASCII");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                if (choice == 1)
                    SelectButton();
                Console.Write("\tLoad image");
                UnselectButton();
                if (choice == 2)
                    SelectButton();
                Console.Write("\t\tSettings");
                UnselectButton();
                if (choice == 3)
                    SelectButton();
                Console.Write("\t\tExit");
                UnselectButton();
                Console.WriteLine();

                key = Console.ReadKey().Key;

                if (key == ConsoleKey.RightArrow)
                {
                    if (choice >= keyCount)
                    {
                        choice = 1;
                    }
                    else
                    {
                        choice++;
                    }
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    if (choice <= 1)
                    {
                        choice = keyCount;
                    }
                    else
                    {
                        choice--;
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            LoadImage();
                            isWorking = false;
                            break;
                        case 2:
                            Console.Clear();
                            ShowSettings();
                            Console.Clear();
                            break;
                        case 3:
                            isWorking = false;
                            Exit = true;
                            Console.Clear();
                            break;
                        default:
                            break;
                    }
                }
                Console.Clear();
            }
        }   

        private void LoadImage()
        {
            bool success = false;
            Console.CursorVisible = true;
            while (!success)
            {
                Console.WriteLine("Input file path");
                path = Console.ReadLine();
                if (path == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error");
                }
                else
                {
                    success = true;
                    Console.Clear();
                    Console.CursorVisible = false;
                }    
            }
        }

        private void ShowSettings()
        {
            bool isWorking = true;
            byte choice = 1;
            byte keyCount = 4;
            ConsoleKey key;

            while (isWorking)
            {
                Console.WriteLine();
                if(choice == 1)
                    SelectButton();
                Console.Write("Render type:");
                UnselectButton();                
                if (renderMode == RENDER_MODE.COLORED)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("  Colored");
                }
                else if(renderMode == RENDER_MODE.MONOCHROME)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  Monochrome");
                }
                else if(renderMode == RENDER_MODE.BLACK_WHITE)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("  Black and white");
                }
                UnselectButton();

                if (choice == 2)
                    SelectButton();
                Console.Write("ASCII table select:");
                UnselectButton();
                if (isAsciiTableLong)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  Long");
                }
                else if (!isAsciiTableLong)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("  Short");
                }
                UnselectButton();

                if (choice == 3)
                    SelectButton();
                Console.Write("Image width(length is proportional)");
                UnselectButton();
                if(imageWidth<=400 || imageWidth >= 750)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  {imageWidth}");
                UnselectButton();

                if (choice == 4)
                    SelectButton();
                Console.Write("Exit");
                UnselectButton();


                key = Console.ReadKey().Key;

                if (key == ConsoleKey.DownArrow)
                {
                    if (choice >= keyCount)
                    {
                        choice = 1;
                    }
                    else
                    {
                        choice++;
                    }
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    if (choice <= 1)
                    {
                        choice = keyCount;
                    }
                    else
                    {
                        choice--;
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            if ((int)renderMode < 2)
                                renderMode++;
                            else
                                renderMode = RENDER_MODE.BLACK_WHITE;
                            break;
                        case 2:
                            Console.Clear();
                            if(isAsciiTableLong)
                                isAsciiTableLong = false;
                            else
                                isAsciiTableLong = true;
                            break;
                        case 3:
                            key = ConsoleKey.A;
                            while(key != ConsoleKey.Enter)
                            {
                                key = Console.ReadKey().Key;
                                if(key == ConsoleKey.UpArrow)
                                    imageWidth += 5;
                                else if(key == ConsoleKey.DownArrow)
                                    imageWidth -= 5;
                            }
                            break;
                        case 4:
                            Console.Clear();
                            isWorking = false;
                            break;
                        default:
                            break;
                    }
                }
                Console.Clear();
            }
        }

        private void SelectButton()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        private void UnselectButton()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
   
}
