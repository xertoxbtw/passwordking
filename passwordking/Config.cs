using System;
using System.Collections.Generic;
using System.IO;

namespace passwordking
{
    public class Config
    {
        public LangSystem.Language Language;
        public ConsoleKey Keybind_Add;
        public ConsoleKey Keybind_Save;
        public ConsoleKey Keybind_Delete;
        public ConsoleKey Keybind_Edit;
        public ConsoleKey Keybind_GetPassword;
        public ConsoleKey Keybind_Exit;
        public ConsoleKey Keybind_Up;
        public ConsoleKey Keybind_Down;
        public ConsoleKey Keybind_Reset;
        public void Save()
        {
            string langstring = "";
            if (Language == LangSystem.Language.DE) langstring = "DE";
            else if (Language == LangSystem.Language.EN) langstring = "EN";
            string cfg = "Config File for passwordKing\nGoto https://docs.microsoft.com/en-us/dotnet/api/system.consolekey?view=net-5.0 for Keycode List\n";
            cfg = cfg + "Language:" + langstring+"\n";
            cfg = cfg + "Key_Add:" + Convert.ToByte(Keybind_Add) + "\n";
            cfg = cfg + "Key_Save:" + Convert.ToByte(Keybind_Save) + "\n";
            cfg = cfg + "Key_Delete:" + Convert.ToByte(Keybind_Delete) + "\n";
            cfg = cfg + "Key_Edit:" + Convert.ToByte(Keybind_Edit) + "\n";
            cfg = cfg + "Key_GetPassword:" + Convert.ToByte(Keybind_GetPassword) + "\n";
            cfg = cfg + "Key_Exit:" + Convert.ToByte(Keybind_Exit) + "\n";
            cfg = cfg + "Key_Up:" + Convert.ToByte(Keybind_Up) + "\n";
            cfg = cfg + "Key_Down:" + Convert.ToByte(Keybind_Down) + "\n";
            cfg = cfg + "Key_Reset:" + Convert.ToByte(Keybind_Reset);

            File.WriteAllText("config.cfg", cfg);
        }
        public void Load()
        {
            string[] rawLines = File.ReadAllLines("config.cfg");
            for (int i = 0; i < rawLines.Length; i++)
            {
                string[] thisLine = rawLines[i].Split(":", StringSplitOptions.RemoveEmptyEntries);
                if (thisLine[0] == "Language")
                {
                    if (thisLine[1].ToLower() == "de")
                    {
                        Language = LangSystem.Language.DE;
                    }
                    else if (thisLine[1].ToLower() == "en")
                    {
                        Language = LangSystem.Language.EN;
                    }
                }
                else if (thisLine[0] == "Key_Add")
                {
                    Keybind_Add = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_Save")
                {
                    Keybind_Save = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_Delete")
                {
                    Keybind_Delete = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_Edit")
                {
                    Keybind_Edit = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_GetPassword")
                {
                    Keybind_GetPassword = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_Exit")
                {
                    Keybind_Exit = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_Up")
                {
                    Keybind_Up = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_Down")
                {
                    Keybind_Down = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
                else if (thisLine[0] == "Key_Reset")
                {
                    Keybind_Reset = (ConsoleKey)Convert.ToByte(thisLine[1]);
                }
            }
        }
        public void Reset()
        {
            Language = LangSystem.Language.EN;
            Keybind_Add = ConsoleKey.F1;
            Keybind_Edit = ConsoleKey.F2;
            Keybind_Delete = ConsoleKey.F3;
            Keybind_GetPassword = ConsoleKey.Enter;
            Keybind_Save = ConsoleKey.S;
            Keybind_Exit = ConsoleKey.X;
            Keybind_Up = ConsoleKey.UpArrow;
            Keybind_Down = ConsoleKey.DownArrow;
            Keybind_Reset = ConsoleKey.F12;
        }
    }
}
