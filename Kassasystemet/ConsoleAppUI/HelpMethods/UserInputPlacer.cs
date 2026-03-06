using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.HelpMethods
{
    /// <summary>
    /// Hjälpklass för att placera användarinput på rätt plats 
    /// i programfönstret och konsekvent genom programmet.
    /// </summary>
    public static class UserInputPlacer
    {
        /// <summary>Places user input after prompt line</summary>
        public static string ReadCenteredText(string textPrompt)
        {
            textPrompt = textPrompt.Replace("\r", "").Replace("\n", "");

            int left = Math.Max(0, (Console.WindowWidth - textPrompt.Length) / 2);

            Console.Write(new string(' ', left) + textPrompt);
            return (Console.ReadLine() ?? "").Trim();
        }
    }
}
