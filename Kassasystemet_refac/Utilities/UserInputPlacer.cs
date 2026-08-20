using System;
using System.Collections.Generic;
using System.Text;

namespace Kassasystemet_refac
{
    public static class UserInputPlacer
    {
        public static string ReadCenteredText(string textPrompt)
        {
            textPrompt = textPrompt.Replace("\r", "").Replace("\n", "");

            int left = Math.Max(0, (Console.WindowWidth - textPrompt.Length) / 2);

            Console.Write(new string(' ', left) + textPrompt);
            return (Console.ReadLine() ?? "").Trim();
        }
    }
}
