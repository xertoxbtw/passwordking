using System;
using System.Collections.Generic;
using System.IO;
using TextCopy;

namespace passwordking
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = new Config();
            try
            {
                if (File.Exists("config.cfg"))
                {
                    config = config.Load();
                }
                else
                {
                    config.Reset();
                    config.Save();
                }
            }
            catch
            {
                config.Reset();
                config.Save();
            }


            List<Entry> entries = new List<Entry>();
            byte screen = 0;
            int curY = 0;
            int offset = 0;
            int maxDraw = Console.WindowHeight - 4;
            int select = 0;
            string filePassword = "";
            Console.Clear();
            Console.CursorVisible = false;
            Console.Title = "passwordKing by xertox";
            bool Run = true;
            while (Run)
            {
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = false;
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

                if (screen == 0) // init Select
                {
                    Console.WriteLine("--- passwordKing ---");
                    if (curY == 0)
                    {
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else { Console.ResetColor(); }
                    Console.WriteLine("New");

                    if (curY == 1)
                    {
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else { Console.ResetColor(); }
                    Console.WriteLine("Load");

                    Console.ResetColor();
                    ConsoleKey input = Console.ReadKey(false).Key;
                    if (input == config.Keybind_Up)
                    {
                        if (curY == 1)
                        {
                            curY = 0;
                        }
                    }
                    else if (input == config.Keybind_Down)
                    {
                        if (curY == 0)
                        {
                            curY = 1;
                        }
                    }
                    else if (input == ConsoleKey.Enter)
                    {
                        if (curY == 0)
                        {
                            screen = 2;
                            curY = 0;
                        }
                        else if (curY == 1)
                        {
                            screen = 3;
                            curY = 0;
                        }
                    }
                }
                else if (screen == 1) // Main Screen
                {
                    maxDraw = Console.WindowHeight - 4;
                    select = curY + offset;
                    string Buffer1 = "";
                    for (int i = 0; i < Console.WindowWidth; i++)
                    {
                        Buffer1 = Buffer1 + "-";
                    }
                    Buffer1 = Buffer1 + config.Keybind_Add + ": Add | ";
                    Buffer1 = Buffer1 + config.Keybind_Edit + ": Edit | ";
                    Buffer1 = Buffer1 + config.Keybind_Delete + ": Delete\n";
                    Buffer1 = Buffer1 + config.Keybind_GetPassword + ": Copy Password | ";
                    Buffer1 = Buffer1 + config.Keybind_Save + ": Save | ";
                    Buffer1 = Buffer1 + config.Keybind_Exit + ": Exit\n";
                    Buffer1 = Buffer1 + config.Keybind_Up + ": Scroll Up | ";
                    Buffer1 = Buffer1 + config.Keybind_Down + ": Scroll Down | ";
                    Buffer1 = Buffer1 + config.Keybind_Reset + ": Reset";
                    string Buffer2 = "";
                    for (int i = 0; i < maxDraw; i++)
                    {
                        if (i + offset < entries.Count)
                        {

                            Buffer2 = Buffer2 + entries[i + offset] + "\n";
                        }
                        else
                        {
                            Buffer2 = Buffer2 + "\n";
                        }
                    }
                    Console.Write(Buffer2);
                    Console.Write(Buffer1);
                    if (entries.Count > 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(0, curY);
                        Console.Write(entries[curY + offset]);
                        Console.ResetColor();
                    }

                    ConsoleKey keyInput = Console.ReadKey(false).Key;

                    if (keyInput == config.Keybind_Add)
                    {
                        screen = 4;
                    }
                    else if (keyInput == config.Keybind_Edit)
                    {
                        screen = 5;
                    }
                    else if (keyInput == config.Keybind_Delete)
                    {
                        if (select < entries.Count)
                        {
                            if (Select("Are you sure?", config) == true)
                            {
                                entries.RemoveAt(select);
                            }
                            else
                            {

                            }
                        }
                    }
                    else if (keyInput == config.Keybind_GetPassword)
                    {
                        if (select < entries.Count)
                        {
                            ClipboardService.SetText(entries[select].Password);
                        }
                    }
                    else if (keyInput == config.Keybind_Save)
                    {
                        if (filePassword == "")
                        {
                            Console.Clear();
                            Console.WriteLine("Enter a Main Password: ");
                            Console.CursorVisible = true;
                            filePassword = Console.ReadLine();
                            Console.CursorVisible = false;
                        }
                        string Buffer = "";
                        for (int i = 0; i < entries.Count; i++)
                        {
                            Buffer = Buffer + entries[i].Name + "§%&%§" + entries[i].Password + "\n";
                        }
                        Buffer = StringCipher.Encrypt(Buffer, filePassword);
                        File.WriteAllText("passwords", Buffer);
                    }
                    else if (keyInput == config.Keybind_Exit)
                    {
                        Run = false;
                    }
                    else if (keyInput == config.Keybind_Down)
                    {

                        if (curY + offset != entries.Count - 1)
                        {
                            if (curY == maxDraw - 1)
                            {
                                if (offset < entries.Count - 1)
                                {

                                    offset++;
                                }
                            }
                            else
                            {
                                curY++;
                            }
                        }
                    }
                    else if (keyInput == config.Keybind_Up)
                    {

                        if (curY == 0)
                        {
                            if (offset != 0)
                            {
                                offset--;
                            }
                        }
                        else
                        {
                            curY--;
                        }
                    }
                    else if (keyInput == config.Keybind_Reset)
                    {
                        config.Reset();
                        config.Save();
                    }

                }
                else if (screen == 2) // New Screen
                {
                    if (File.Exists("passwords"))
                    {
                        switch (Select("Do you want to overwrite your Existing Passwords?", config))
                        {
                            case true:
                                entries.Clear();
                                screen = 1;
                                break;
                            case false:
                                screen = 0;
                                break;
                        }
                    }
                    else
                    {
                        entries.Clear();
                        screen = 1;
                    }
                }
                else if (screen == 3) // Load Screen
                {
                    if (File.Exists("passwords"))
                    {
                        try
                        {
                            Console.Write("Please enter the Password: ");
                            Console.CursorVisible = true;
                            Console.ForegroundColor = Console.BackgroundColor;
                            filePassword = Console.ReadLine();
                            Console.CursorVisible = false;
                            Console.ResetColor();
                            string def = StringCipher.Decrypt(File.ReadAllText("passwords"), filePassword);
                            entries.Clear();

                            string[] raw = def.Split("\n");
                            for (int i = 0; i < raw.Length - 1; i++)
                            {
                                string[] sc = raw[i].Split("§%&%§");
                                entries.Add(new Entry(sc[0], sc[1]));
                            }
                            screen = 1;
                        }
                        catch { }

                    }
                    else { screen = 2; }

                }
                else if (screen == 4) // Add
                {
                    string name = "", psw = "";
                    Console.CursorVisible = true;
                    while (name == "")
                    {
                        Console.CursorVisible = true;
                        Console.Clear();
                        Console.WriteLine("Add a new Entry");
                        Console.Write("Name: ");
                        name = Console.ReadLine();
                    }
                    while (psw == "")
                    {
                        Console.CursorVisible = true;
                        Console.Clear();
                        Console.WriteLine("Add a new Entry");
                        Console.WriteLine("Name: " + name);
                        Console.Write("Password: ");
                        Console.ForegroundColor = Console.BackgroundColor;
                        psw = Console.ReadLine();
                        Console.ResetColor();
                    }
                    entries.Add(new Entry(name, psw));
                    name = ""; psw = "";
                    Console.CursorVisible = false;
                    screen = 1;
                }
                else if (screen == 5) // Edit
                {
                    if (select < entries.Count)
                    {
                        Console.CursorVisible = true;
                        Console.WriteLine("Editing: " + entries[select].Name + "\n(Leave Empty to Edit nothing)");
                        Console.Write("Name: ");
                        string nin = Console.ReadLine();
                        Console.Write("Password: ");
                        string pin = Console.ReadLine();
                        if (nin != "")
                        {
                            entries[select].Name = nin;
                        }
                        if (pin != "")
                        {
                            entries[select].Password = pin;
                        }
                        screen = 1;
                    }
                    else
                    {
                        screen = 1;
                    }
                }
            }
        }

        static bool Select(string topText, Config config)
        {
            byte buffer = 0;
            while (true)
            {

                Console.Clear();
                Console.ResetColor();
                Console.WriteLine(topText);
                if (buffer == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine("Yes");
                Console.ResetColor();
                if (buffer == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine("No");
                Console.ResetColor();
                ConsoleKey key = Console.ReadKey(false).Key;
                if (key == config.Keybind_Up)
                {
                    if (buffer == 1)
                    {
                        buffer = 0;
                    }
                }
                else if (key == config.Keybind_Down)
                {
                    if (buffer == 0)
                    {
                        buffer = 1;
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (buffer == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }
    }
}

