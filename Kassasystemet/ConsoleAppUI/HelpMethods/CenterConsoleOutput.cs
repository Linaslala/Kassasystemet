using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.HelpMethods
{

    /// <summary>
    /// Hjälpklass för att skriva centrerad text i konsolfönstret.
    /// 
    /// Används för konsekvent layout i hela applikationen.
    /// </summary>
    public static class CenterConsoleOutput
    {
        public static void CenterTextToWindow(string text)
        {
            foreach (var textLine in text.Split('\n'))
            {
                var line = textLine.TrimEnd('\r');

                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine();
                    continue;
                }

                int padding = (Console.WindowWidth / 2) + (line.Length / 2);
                Console.WriteLine(string.Format("{0," + padding + "}", line));
            }
        }
    }
}
