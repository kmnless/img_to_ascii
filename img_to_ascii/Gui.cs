using System.Runtime.InteropServices;

namespace img_to_ascii
{
    class Gui
    {
        public string? path;

        public bool withColor = false;

        public bool Exit = false;

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
            byte keyCount = 2;
            ConsoleKey key;

            while (isWorking)
            {
                Console.WriteLine();
                if(choice == 1)
                    SelectButton();
                Console.Write("Render with color:");
                UnselectButton();                
                if (withColor)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  Yes");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  No");
                }
                UnselectButton();

                if (choice == 2)
                    SelectButton();
                Console.Write("Back");
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
                            if (withColor)
                                withColor = false;
                            else
                                withColor = true;
                            break;
                        case 2:
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
