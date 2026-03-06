using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.UserInterface.Menues;

namespace LinasKlubbLivs
{
    public class Program
    {
        /// <summary>
        /// Applikationens startpunkt.
        /// 
        /// Initierar huvudmenyn och startar kassasystemet.
        /// </summary>
        static void Main(string[] args)
        {
            Console.Title = "Linas Klubb Livs – Kassasystem";

            while (true)
            {
                Console.Clear();

                string headerLineOne = "========================================";
                string headerText = "VÄLKOMMEN TILL LINAS KLUBB-LIVS";
                string headerLineTwo = "========================================";
                string enterText = "Tryck ENTER för att logga in som kassör";
                string closingText = "Tryck ESC för att stänga";

                Console.ForegroundColor = ConsoleColor.Yellow;
                CenterConsoleOutput.CenterTextToWindow(headerLineOne);
                CenterConsoleOutput.CenterTextToWindow(headerText);
                CenterConsoleOutput.CenterTextToWindow(headerLineTwo);
                Console.ResetColor();

                Console.WriteLine();
                CenterConsoleOutput.CenterTextToWindow(enterText);
                Console.WriteLine();
                CenterConsoleOutput.CenterTextToWindow(closingText);

                while (true)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                        Environment.Exit(0);

                    if (key == ConsoleKey.Enter)
                        break;
                }

                var mainMenue = new MainMenue();
                mainMenue.Run();
            }
        }
    }
}