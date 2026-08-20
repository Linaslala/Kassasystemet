namespace Kassasystemet_refac
{
    internal class PurchaseProductSearchService
    {
        public static IProductModel SelectProductFromList(List<IProductModel> products)
        {
            var ordered = products
                          .OrderBy(p => p.ProductIdNumber)
                          .ToList();

            var rows = ordered
                 .Select(p =>
                     $"{p.ProductIdNumber,-20} {p.ProductName,-20} {p.ProductPrice,-20} {p.ProductPriceType,-20}")
                 .ToArray();

            var arrow = new ConsoleOptionsArrow();

            int index = arrow.ShowArrow(
                "Välj produkt:",
                rows,
                renderAboveOptions: () =>
                {
                    CenterConsoleOutput.CenterTextToWindow("== Produkt ==");
                    Console.WriteLine();
                    Console.WriteLine();

                    string header = $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";
                    CenterConsoleOutput.CenterTextToWindow(header);
                    CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
                });

            return ordered[index];
        }

        public static void ShowInlineProductSearchAndPresent()
        {
            IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
            ISearchProduct finder = new ProductSearch(reader);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Hitta produkt ==");
                Console.WriteLine();

                string queryInput = UserInputPlacer
                    .ReadCenteredText("Sök på produktnummer eller produktnamn (tomt = tillbaka): ")
                    .Trim();

                if (string.IsNullOrWhiteSpace(queryInput))
                    return;

                var results = finder.Search(queryInput);

                if (results.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow("Produkten du söker finns inte i systemet.");
                    Console.ResetColor();
                    ValidatedConsoleInput.PauseCentered();
                    continue;
                }

                var selected = results.Count == 1
                    ? results[0]
                    : PurchaseProductSearchService.SelectProductFromList(results);

                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Produkt ==");
                Console.WriteLine();
                Console.WriteLine();

                string header = $"{"Produktnummer",-20}{"Produkt",-20}{"Pris (kr)",-20}{"Pristyp",-20}";
                string row =
                    $"{selected.ProductIdNumber,-20}" +
                    $"{selected.ProductName,-20}" +
                    $"{selected.ProductPrice,-20}" +
                    $"{selected.ProductPriceType,-20}";

                CenterConsoleOutput.CenterTextToWindow(header);
                CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
                CenterConsoleOutput.CenterTextToWindow(row);

                Console.WriteLine();
                CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent för att återgå...");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }
        }

    }
}
