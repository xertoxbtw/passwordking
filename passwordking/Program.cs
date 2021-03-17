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
        static void Main(string[] args)
        {
            Config config = new Config();
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

            LangSystem langSystem = new LangSystem(config.Language);
            List<Entry> entries = new List<Entry>();
            byte screen = 0;
            int curY = 0;
            int offset = 0;
            int maxDraw = Console.WindowHeight - 4;
            int select = 0;
            string filePassword = "";
            Console.Clear();
            Console.CursorVisible = false;
            Console.Title = langSystem.Sets["title"];
            bool Run = true;
            while (Run)
            {
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = false;
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

                if (screen == 0) // init Select
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
                    Buffer1 = Buffer1 + config.Keybind_Add + ": " + langSystem.Sets["ui_add"] + " | ";
                    Buffer1 = Buffer1 + config.Keybind_Edit + ": " + langSystem.Sets["ui_edit"] + " | ";
                    Buffer1 = Buffer1 + config.Keybind_Delete + ": " + langSystem.Sets["ui_delete"] + " | ";
                    Buffer1 = Buffer1 + config.Keybind_Help + ": " + langSystem.Sets["ui_help"] + "\n";
                    Buffer1 = Buffer1 + config.Keybind_GetPassword + ": " + langSystem.Sets["ui_copy"] + " | ";
                    Buffer1 = Buffer1 + config.Keybind_Save + ": " + langSystem.Sets["ui_save"] + " | ";
                    Buffer1 = Buffer1 + config.Keybind_Exit + ": " + langSystem.Sets["ui_exit"] + "\n";
                    Buffer1 = Buffer1 + config.Keybind_Up + ": " + langSystem.Sets["ui_scrollup"] + " | ";
                    Buffer1 = Buffer1 + config.Keybind_Down + ": " + langSystem.Sets["ui_scrolldown"] + " | ";
                    Buffer1 = Buffer1 + config.Keybind_Reset + ": " + langSystem.Sets["ui_reset"];
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
                            if (Select(langSystem.Sets["ask_sure"], config, langSystem) == true)
                            {
                                entries.RemoveAt(select);
                            }
                        }
                    }
                    else if (keyInput == config.Keybind_Help)
                    {
                        screen = 255;
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
                        if (Select(langSystem.Sets["textdelete"], config, langSystem))
                        {
                            entries.Clear();
                            filePassword = "";
                        }
                    }
                }
                else if (screen == 2) // New Screen
                {
                    if (File.Exists("passwords"))
                    {
                        switch (Select(langSystem.Sets["textoverwrite"], config, langSystem))
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
                    else { screen = 2; }

                }
                else if (screen == 4) // Add
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
                else if (screen == 5) // Edit
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
                            else
                            {
                                entries[select].Password = pin;
                            }
                        }
                        screen = 1;
                    }
                    else
                    {
                        screen = 1;
                    }
                }
                else if (screen == 255) // Hilfe
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
                        if (key == config.Keybind_Down && selection != 3)
                            selection++;

                        else if (key == config.Keybind_Up && selection != 0)
                            selection--;

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
            }
        }

        static bool Select(string topText, Config config, LangSystem langSystem)
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
                Console.WriteLine(langSystem.Sets["yes"]);
                Console.ResetColor();
                if (buffer == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(langSystem.Sets["no"]);
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

