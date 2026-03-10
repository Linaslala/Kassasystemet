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
            Console.Clear();

            var reader = new ReadAllReceiptsFromFile();

            var baseDir = AppContext.BaseDirectory;
            var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
            var textFilesDir = Path.Combine(projectDir, "TextFiles");

            var allReceiptFiles = Directory
                .EnumerateFiles(textFilesDir, "RECEIPT_*.txt")
                .OrderBy(f => f)
                .ToList();

            var allReceipts = new List<IReceiptModel>();

            foreach (var file in allReceiptFiles)
            {
                var receiptsFromFile = reader.ReadAllFromPath(file);
                allReceipts.AddRange(receiptsFromFile);
            }

            var receipts = allReceipts
                 .GroupBy(r => r.ReceiptNumber)
                 .Select(g => g.OrderByDescending(x => x.ReceiptCreatedAt).First())
                 .OrderByDescending(r => r.ReceiptCreatedAt)
                 .ToList();

            Console.Clear();

            var arrow = new ConsoleOptionsArrow();

            arrow.ShowArrow("Välj:", new[] { "Tillbaka till startmenyn" }, renderAboveOptions: () =>

            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

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

                CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                Console.WriteLine();
            });
        }
    }
}