using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TextCopy;

namespace passwordking
{
    class Program
    {
        public static Config config;
        public static LangSystem langSystem;
        public static List<Entry> entries;
        public static byte screen = 0;
        public static int curY = 0;
        public static int offset = 0;
        public static int maxDraw = Console.WindowHeight - 4;
        public static int select = 0;
        public static string filePassword = "";
        public static bool Run = true;
        static void Main(string[] args)
        {
            config = new Config();
            try
            {
                if (File.Exists("config.cfg"))
                {
                    config.Load();
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
            langSystem = new LangSystem(config.Language);
            entries = new List<Entry>();
            Console.Clear();
            Console.CursorVisible = false;
            Console.Title = langSystem.Sets["title"];

            while (Run)
            {
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = false;
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

                if (screen == 0) // init Select
                {
                    MainMenu();
                }
                else if (screen == 1) // Main Screen
                {
                    MainWindow();
                }
                else if (screen == 2) // New Screen
                {
                    NewMenu();
                }
                else if (screen == 3) // Load Screen
                {
                    LoadMenu();
                }
                else if (screen == 4) // Add
                {
                    Add();
                }
                else if (screen == 5) // Edit
                {
                    Edit();
                }
                else if (screen == 255) // Hilfe
                {
                    Help();
                }
            }
        }
        static void NewMenu()
        {
            if (File.Exists("passwords"))
            {
                switch (Select(langSystem.Sets["textoverwrite"]))
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
        static void LoadMenu()
        {
            if (File.Exists("passwords"))
            {
                try
                {
                    Console.Write(langSystem.Sets["mainpassword"] + ": ");
                    Console.CursorVisible = true;
                    if (config.ShowPasswordInput == false) Console.ForegroundColor = Console.BackgroundColor;
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
            else screen = 2;
        }
        static void MainMenu()
        {
            Console.WriteLine("--- " + langSystem.Sets["title"] + " ---");
            if (curY == 0)
            {
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Green;
            }
            else { Console.ResetColor(); }
            Console.WriteLine(langSystem.Sets["new"]);

            if (curY == 1)
            {
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Green;
            }
            else { Console.ResetColor(); }
            Console.WriteLine(langSystem.Sets["load"]);

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
                if (curY == 0) curY = 1;
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

        static void MainWindow()
        {
            maxDraw = Console.WindowHeight - 4;
            select = curY + offset;

            // UI Create String
            string KeyBuffer = "";
            // Split Line
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                KeyBuffer = KeyBuffer + "-";
            }
            // Draw Control
            KeyBuffer = KeyBuffer + config.Keybind_Add + ": " + langSystem.Sets["ui_add"] + " | ";
            KeyBuffer = KeyBuffer + config.Keybind_Edit + ": " + langSystem.Sets["ui_edit"] + " | ";
            KeyBuffer = KeyBuffer + config.Keybind_Delete + ": " + langSystem.Sets["ui_delete"] + " | ";
            KeyBuffer = KeyBuffer + config.Keybind_Help + ": " + langSystem.Sets["ui_help"] + "\n";
            KeyBuffer = KeyBuffer + config.Keybind_GetPassword + ": " + langSystem.Sets["ui_copy"] + " | ";
            KeyBuffer = KeyBuffer + config.Keybind_Save + ": " + langSystem.Sets["ui_save"] + " | ";
            KeyBuffer = KeyBuffer + config.Keybind_Exit + ": " + langSystem.Sets["ui_exit"] + "\n";
            KeyBuffer = KeyBuffer + config.Keybind_Up + ": " + langSystem.Sets["ui_scrollup"] + " | ";
            KeyBuffer = KeyBuffer + config.Keybind_Down + ": " + langSystem.Sets["ui_scrolldown"] + " | ";
            KeyBuffer = KeyBuffer + config.Keybind_Reset + ": " + langSystem.Sets["ui_reset"];
            string ItemBuffer = "";
            // Draw Entries
            for (int i = 0; i < maxDraw; i++)
            {
                if (i + offset < entries.Count)
                    ItemBuffer = ItemBuffer + entries[i + offset] + "\n";
                else
                    ItemBuffer = ItemBuffer + "\n";
            }
            Console.Write(ItemBuffer);
            Console.Write(KeyBuffer);

            // Highlight Selected
            if (entries.Count > 0)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(0, curY);
                Console.Write(entries[curY + offset]);
                Console.ResetColor();
            }

            MainInput();
        }

        static void MainInput()
        {
            // Input
            ConsoleKey keyInput = Console.ReadKey(false).Key;

            if (keyInput == config.Keybind_Add) // Add
            {
                screen = 4;
            }
            else if (keyInput == config.Keybind_Edit) // Edit
            {
                screen = 5;
            }
            else if (keyInput == config.Keybind_Delete) // Delete
            {
                if (select < entries.Count)
                {
                    if (Select(langSystem.Sets["ask_sure"]) == true)
                    {
                        entries.RemoveAt(select);
                    }
                }
            }
            else if (keyInput == config.Keybind_Help) // Help
            {
                screen = 255;
            }
            else if (keyInput == config.Keybind_GetPassword) // Get Password
            {
                if (select < entries.Count)
                {
                    ClipboardService.SetText(entries[select].Password);
                }
            }
            else if (keyInput == config.Keybind_Save) // Save
            {
                if (filePassword == "")
                {
                    Console.Clear();
                    Console.Write(langSystem.Sets["mainpassword"] + ": ");
                    Console.CursorVisible = true;
                    if (config.ShowPasswordInput == false) Console.ForegroundColor = Console.BackgroundColor;
                    filePassword = Console.ReadLine();
                    Console.ResetColor();
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
            else if (keyInput == config.Keybind_Exit) // Exit
            {
                Run = false;
            }
            else if (keyInput == config.Keybind_Down) // Down
            {
                if (curY + offset != entries.Count - 1)
                {
                    if (curY == maxDraw - 1)
                    {
                        if (offset < entries.Count - 1)
                            offset++;
                    }
                    else
                    {
                        curY++;
                    }
                }
            }
            else if (keyInput == config.Keybind_Up) // Up
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
            else if (keyInput == config.Keybind_Reset) // Reset
            {
                if (Select(langSystem.Sets["textdelete"]))
                {
                    entries.Clear();
                    filePassword = "";
                }
            }
        }
        static void Add()
        {
            string name = "", psw = "";
            Console.CursorVisible = true;
            while (true)
            {
                Console.CursorVisible = true;
                Console.Clear();
                Console.WriteLine(langSystem.Sets["textadd"]);
                Console.Write(langSystem.Sets["name"] + ": ");
                name = Console.ReadLine();
                if (name.Contains("§%&%§") == true) name = "";
                else if (name != "") break;
            }
            while (true)
            {
                Console.CursorVisible = true;
                Console.Clear();
                Console.WriteLine(langSystem.Sets["textadd"]);
                Console.WriteLine(langSystem.Sets["name"] + ": " + name);
                Console.Write(langSystem.Sets["password"] + ": ");
                if (config.ShowPasswordInput == false) Console.ForegroundColor = Console.BackgroundColor;
                psw = Console.ReadLine();
                Console.ResetColor();
                if (psw.Contains("§%&%§") == true) { psw = ""; }
                else if (psw.StartsWith("random"))
                {
                    string[] pswenter = psw.Split(" ");
                    int lenpsw;
                    try
                    {
                        lenpsw = Convert.ToInt32(pswenter[1]);
                        psw = RandomPassword(lenpsw, config);
                        break;
                    }
                    catch { }
                }
                else if (psw != "") break;
            }
            entries.Add(new Entry(name, psw));
            name = ""; psw = "";
            Console.CursorVisible = false;
            screen = 1;
        }
        static void Edit()
        {
            if (select < entries.Count)
            {
                Console.CursorVisible = true;
                Console.WriteLine(langSystem.Sets["textedit1"] + ": " + entries[select].Name + "\n" + langSystem.Sets["textedit2"]);
                Console.Write(langSystem.Sets["name"] + ": ");
                string nin = Console.ReadLine();
                Console.Write(langSystem.Sets["password"] + ": ");
                string pin = Console.ReadLine();
                if (nin != "")
                {
                    entries[select].Name = nin;
                }
                if (pin != "")
                {
                    if (pin.Contains("random"))
                    {
                        try
                        {
                            string[] pinenter = pin.Split(" ");
                            int pinlen = Convert.ToInt32(pinenter[1]);
                            entries[select].Password = RandomPassword(pinlen, config);
                        }
                        catch { pin = entries[select].Password; }
                    }
                    else entries[select].Password = pin;
                }
                screen = 1;
            }
            else screen = 1;
        }


        static bool Select(string topText)
        {
            byte buffer = 0;
            while (true)
            {
                Console.Clear();
                Console.ResetColor();
                Console.WriteLine(topText);
                if (buffer == 0) Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(langSystem.Sets["yes"]);
                Console.ResetColor();
                if (buffer == 1) Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(langSystem.Sets["no"]);
                Console.ResetColor();
                ConsoleKey key = Console.ReadKey(false).Key;
                if (key == config.Keybind_Up && buffer == 1) buffer = 0;
                else if (key == config.Keybind_Down && buffer == 0) buffer = 1;
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

        static void Help()
        {
            byte selection = 0;
            while (true)
            {
                Console.Clear();
                Console.ResetColor();
                Console.WriteLine(langSystem.Sets["helpTitle"]);

                if (selection == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(langSystem.Sets["howtoUseTitle"]);
                Console.ResetColor();
                if (selection == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(langSystem.Sets["configTitle"]);
                Console.ResetColor();
                if (selection == 2)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(langSystem.Sets["creditTitle"]);
                Console.ResetColor();
                if (selection == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(langSystem.Sets["back"]);
                Console.ResetColor();

                ConsoleKey key = Console.ReadKey(false).Key;
                if (key == config.Keybind_Down && selection != 3) selection++;
                else if (key == config.Keybind_Up && selection != 0) selection--;
                else if (key == ConsoleKey.Enter)
                {
                    if (selection == 0) // How to Use
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(langSystem.Sets["howtoUseText"]);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.WriteLine(langSystem.Sets["back"]);
                            Console.ResetColor();
                            if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                            {
                                break;
                            }
                        }
                    }
                    else if (selection == 1) // Edit Config
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(langSystem.Sets["configText"]);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.WriteLine(langSystem.Sets["back"]);
                            Console.ResetColor();
                            if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                            {
                                break;
                            }
                        }
                    }
                    else if (selection == 2) // Credit
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(langSystem.Sets["creditText"]);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.WriteLine(langSystem.Sets["back"]);
                            Console.ResetColor();
                            if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                            {
                                break;
                            }
                        }
                    }
                    else if (selection == 3) // Back
                    {
                        screen = 1;
                        break;
                    }
                }
            }
        }

        static string RandomPassword(int len, Config config)
        {
            String res = "";
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (len-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res = res + (config.RandomCharSet[(int)(num % (uint)config.RandomCharSet.Length)]);
                }
            }

            return res.ToString();
        }
    }
}