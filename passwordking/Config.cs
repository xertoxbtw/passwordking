using Newtonsoft.Json;
using System;
using System.IO;

namespace passwordking
{
    public class Config
    {
        public Language Lang;
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
            File.WriteAllText("config.cfg",JsonConvert.SerializeObject(this));
        }
        public Config Load()
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.cfg"));
        }
        public void Reset()
        {
            Lang = Language.EN;
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
        public enum Language
        {
            EN, DE
        }
    }
}
