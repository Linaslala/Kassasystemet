using System;

namespace LinasKlubbLivs.ConsoleAppUI.HelpMethods
{
    /// <summary>
    /// Hjälpklass för validerad användarinmatning.
    /// 
    /// Används för att säkerställa korrekt inmatad data innan vidare hantering.
    /// </summary>
    public static class ValidatedConsoleInput
    {
        public static string ReadValidatedCenteredText(
            string header,
            string prompt,
            Action<string> validate,
            bool clearConsoleEachAttempt = true)
        {

            while (true)
            {
                if (clearConsoleEachAttempt)
                {
                    Console.Clear();
                    if (!string.IsNullOrWhiteSpace(header))
                    {
                        CenterConsoleOutput.CenterTextToWindow(header);
                        Console.WriteLine();
                    }
                }

                string input = UserInputPlacer.ReadCenteredText(prompt);

                try
                {
                    validate(input);
                    return input;
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow(ex.Message);
                    Console.ResetColor();
                    Console.WriteLine();
                    CenterConsoleOutput.CenterTextToWindow("Försök igen...");
                    Console.ReadKey(true);
                }
            }
        }

        public static void PauseCentered(string message = "Tryck valfri tangent för att fortsätta...")
        {
            Console.WriteLine();
            CenterConsoleOutput.CenterTextToWindow(message);
            Console.ReadKey(true);
        }
    }
}