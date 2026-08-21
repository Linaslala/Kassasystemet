namespace Kassasystemet_refac
{
    public class FindReceipt
    {
        public void Run()
        {
            Console.Clear();

            IReadAllReceiptsFromFile receiptReader = new ReadAllReceiptsFromFile();
            IReceiptSearch receiptFinder = new ReceiptSearch(receiptReader);

            while (true)
            {
                Console.Clear();

                CenterConsoleOutput.CenterTextToWindow("== SÖK KVITTO ==");
                Console.WriteLine();

                string query = UserInputPlacer.ReadCenteredText(
                    "Sök på kvittonummer eller kundnummer: ").Trim();

                var results = receiptFinder.Search(query)
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
                        NotificationService.ShowError(
                            "Inget kvitto hittades.");

                        //Console.ForegroundColor = ConsoleColor.Red;
                        //CenterConsoleOutput.CenterTextToWindow("Inget kvitto hittades.");
                        //Console.ResetColor();
                        //Console.WriteLine();
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
