namespace Kassasystemet_refac
{
    public class ConsoleOptionsArrow
    {
        public int ShowArrow(string title, IReadOnlyList<string> options)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J");

                CenterConsoleOutput.CenterTextToWindow(title);
                Console.WriteLine();
                Console.WriteLine();

                for (int i = 0; i < options.Count; i++)
                {
                    bool isSelected = (i == selectedIndex);
                    string line = isSelected
                        ? $"> {options[i]}"
                        : $" {options[i]}";

                    if (isSelected)
                        Console.ForegroundColor = ConsoleColor.Green;

                    CenterConsoleOutput.CenterTextToWindow(line);
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex <= 0
                            ? options.Count - 1
                            : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex >= options.Count - 1
                            ? 0
                            : selectedIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
            }
        }

        public int ShowArrow(string title, IReadOnlyList<string> options, Action? renderAboveOptions)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();

                if (renderAboveOptions != null)
                {
                    renderAboveOptions();
                }
                else
                {
                    CenterConsoleOutput.CenterTextToWindow(title);
                    Console.WriteLine();
                    Console.WriteLine();
                }

                if (renderAboveOptions != null)
                    Console.WriteLine();

                for (int i = 0; i < options.Count; i++)
                {
                    bool isSelected = (i == selectedIndex);
                    string line = isSelected
                        ? $"> {options[i]}"
                        : $"  {options[i]}";

                    if (isSelected)
                        Console.ForegroundColor = ConsoleColor.Green;

                    CenterConsoleOutput.CenterTextToWindow(line);
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex <= 0
                            ? options.Count - 1
                            : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex >= options.Count - 1
                            ? 0
                            : selectedIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
            }
        }
    }
}

