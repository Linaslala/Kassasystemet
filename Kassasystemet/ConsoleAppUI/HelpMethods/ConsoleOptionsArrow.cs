using System;
using System.Collections.Generic;

namespace LinasKlubbLivs.ConsoleAppUI.HelpMethods
{
    /// <summary>
    /// Hanterar menyval i konsolen med piltangenter.
    /// 
    /// Stödjer renderAboveOptions för korrekt scroll‑beteende.
    /// </summary>
    public class ConsoleOptionsArrow
    {
        public int ShowArrow(string title, IReadOnlyList<string> options, Action? renderAboveOptions = null)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();

                CenterConsoleOutput.CenterTextToWindow(title);
                Console.WriteLine();
                Console.WriteLine();

                renderAboveOptions?.Invoke();

                for (int i = 0; i < options.Count; i++)
                {
                    bool isSelected = (i == selectedIndex);
                    string line = isSelected ? $"> {options[i]}" : $"  {options[i]}";

                    if (isSelected)
                        Console.ForegroundColor = ConsoleColor.Green;

                    CenterConsoleOutput.CenterTextToWindow(line);
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex <= 0 ? options.Count - 1 : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex >= options.Count - 1 ? 0 : selectedIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
            }
        }
    }
}