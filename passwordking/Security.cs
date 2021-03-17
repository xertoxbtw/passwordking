using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace passwordking
{
    public static class Security
    {
        public static string Encrypt(string input, string key)
        {
            char[] tostring = input.ToCharArray();
            List<char> Key = new List<char>();
            Key.AddRange(key.ToCharArray());
            for (int i = 0; Key.Count < input.Length; i++)
            {
                Key.Add(Key[i]);
            }

            for (int i = 0; i < input.Length; i++)
            {
                tostring[i] = (char)(tostring[i] + Key[i]);
            }
            return new string(tostring);
        }


        public static string Decrypt(string input, string key)
        {
            char[] tostring = input.ToCharArray();
            List<char> Key = new List<char>();
            Key.AddRange(key.ToCharArray());
            for (int i = 0; Key.Count < input.Length; i++)
            {
                Key.Add(Key[i]);
            }

            for (int i = 0; i < input.Length; i++)
            {
                tostring[i] = (char)(tostring[i] - Key[i]);
            }
            return new string(tostring);
        }
    }
}
