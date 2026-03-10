using LinasKlubbLivs.BusinessLogic.ReceiptLogic;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System.Linq;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.SalesReportOptionCalls
{
    /// <summary>
    /// Sökvy för kvitton.
    /// 
    /// Funktion:
    /// - Användaren kan söka på kvittonummer eller kundnummer.
    /// - Alla träffar visas med full kvittoutskrift.
    /// 
    /// UI-princip:
    /// - renderAboveOptions används konsekvent för att:
    /// visa resultat ovanför meny
    /// </summary>
    public class FindReceipt
    {
        public void Run()
        {
            Console.Clear();

            IReadAllReceiptsFromFile reader = new ReadAllReceiptsFromFile();
            IReceiptSearch finder = new ReceiptSearch(reader);

            while (true)
            {
                Console.Clear();

                CenterConsoleOutput.CenterTextToWindow("== SÖK KVITTO ==");
                Console.WriteLine();

                string query = UserInputPlacer.ReadCenteredText(
                    "Sök på kvittonummer eller kundnummer: ").Trim();

                var results = finder.Search(query)
                    .OrderByDescending(r => r.ReceiptNumber)
                    .ToList();

                var arrow = new ConsoleOptionsArrow();

                int choice = arrow.ShowArrow("Välj:", new[] { "Ny sökning", "Tillbaka till startmenyn" }, renderAboveOptions: () =>
                    {
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);

                        CenterConsoleOutput.CenterTextToWindow("== HITTADE KVITTON ==");
                        Console.WriteLine();


                        if (!results.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Inget kvitto hittades.");
                            Console.ResetColor();
                            Console.WriteLine();
                            return;
                        }

                        foreach (var receipt in results)
                        {
                            CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                            ReceiptPrinter.PrintDetailed(receipt);
                        }

                        CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                        Console.WriteLine();

                        CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                        Console.WriteLine();
                    });

                if (choice == 0)
                    continue;

                return;
            }
        }
    }
}