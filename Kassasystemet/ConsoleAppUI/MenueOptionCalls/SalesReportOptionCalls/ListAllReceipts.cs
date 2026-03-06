using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System.Linq;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.SalesReportOptionCalls
{
    /// <summary>
    /// Visar en alla existerande kvitton (inte summering) i konsolen.
    /// 
    /// Funktion:
    /// - Listar alla kvitton i fallande ordning (senaste först).
    /// - Varje kvitto visas med tydlig avdelare.
    /// </summary>
    public class ListAllReceipts
    {
        public void Run()
        {
            IReadAllReceiptsFromFile reader = new ReadAllReceiptsFromFile();

            var receipts = reader.ReadAll()
                .OrderByDescending(r => r.ReceiptNumber)
                .ToList();

            var arrow = new ConsoleOptionsArrow();
            arrow.ShowArrow(
                "Välj:",
                new[] { "Tillbaka till startmenyn" },
                renderAboveOptions: () =>
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== FÖRSÄLJNINGSRAPPORT ==");
                    Console.WriteLine();

                    if (!receipts.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        CenterConsoleOutput.CenterTextToWindow("Det finns inga registrerade köp.");
                        Console.ResetColor();
                        Console.WriteLine();
                        return;
                    }

                    foreach (var receipt in receipts)
                    {
                        CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                        ReceiptPrinter.PrintDetailed(receipt);
                    }

                    //Avdelare mellan sista kvittot och menyn
                    CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                    Console.WriteLine();
                });
        }
    }
}