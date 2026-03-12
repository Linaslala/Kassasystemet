using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System.Globalization;
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

            //Console.Clear();

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

            var byDay = receipts.GroupBy(r => r.ReceiptCreatedAt.Date).OrderByDescending(g => g.Key);


            foreach (var dayGroup in byDay)
            {
                CenterConsoleOutput.CenterTextToWindow($"DATUM: {dayGroup.Key:yyyy-MM-dd}");
                CenterConsoleOutput.CenterTextToWindow(new string('-', 50));

                var products = dayGroup
                    .SelectMany(r => r.ReceiptRows)
                    .Where(x => x.ReceiptProductQuantity > 0) 
                    .GroupBy(x => x.ReceiptProductText)
                    .Select(g => new
                    {
                        Product = g.Key,
                        Quantity = g.Sum(x => x.ReceiptProductQuantity),
                        Amount = g.Sum(x => x.ReceiptProductAmount)
                    })
                    .OrderByDescending(x => x.Quantity)
                    .ThenBy(x => x.Product);

                    foreach (var p in products)
                        CenterConsoleOutput.CenterTextToWindow(
                            $"{p.Product} x{p.Quantity} {p.Amount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");

                    var discountTotal = dayGroup
                        .SelectMany(r => r.ReceiptRows)
                        .Where(x =>
                            x.ReceiptProductQuantity == 0 &&
                            x.ReceiptProductAmount < 0 &&
                            x.ReceiptProductText.StartsWith("Rabatt", StringComparison.OrdinalIgnoreCase))
                        .Sum(x => x.ReceiptProductAmount);

                    if (discountTotal < 0)
                    {
                        CenterConsoleOutput.CenterTextToWindow(
                            $"Dagens rabatter: {discountTotal.ToString("0.00", CultureInfo.InvariantCulture)} SEK");
                    }

                    CenterConsoleOutput.CenterTextToWindow(new string('-', 50));

                    var total = dayGroup.Sum(r => r.TotalAmount);
                    CenterConsoleOutput.CenterTextToWindow(
                        $"TOTALT FÖRSÄLJNINGSPRIS: {total.ToString("0.00", CultureInfo.InvariantCulture)} SEK");

                    CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                    Console.WriteLine();
                }
                Console.WriteLine();
            });
        }
    }
}