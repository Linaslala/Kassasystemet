namespace Kassasystemet_refac
{
    public class DeleteProduct
    {
        public void Run()
        {
            IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
            ISaveProductToFile writer = new SaveProductToFile();
            ISearchProduct finder = new ProductSearch(reader);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Ta bort produkt ==");

                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText("Sök på produktnummer eller produktnamn: ");
                var searchProductResult = finder.Search(queryInput);

                if (searchProductResult.Count == 0)
                {
                    var arrowNoResult = new ConsoleOptionsArrow();
                    var noResultOptions = new[]
                    {
                        "Ny sökning",
                        "Tillbaka till produktsidan"
                    };

                    int choice = arrowNoResult.ShowArrow("Välj:", noResultOptions, renderAboveOptions: () =>
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Ta bort produkt ==");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        CenterConsoleOutput.CenterTextToWindow("Produkten du söker finns inte i systemet.");
                        Console.ResetColor();
                        Console.WriteLine();
                    });

                    if (choice == 0)
                        continue;

                    return;
                }

                var selectedProduct = searchProductResult.Count == 1
                ? searchProductResult[0]
                : SelectProduct(searchProductResult);

                int productId = selectedProduct.ProductIdNumber;
                string productName = selectedProduct.ProductName;
                decimal productPrice = selectedProduct.ProductPrice;
                string productPriceType = selectedProduct.ProductPriceType;

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Ta bort produkt ==");
                    Console.WriteLine();
                    Console.WriteLine();

                    CenterConsoleOutput.CenterTextToWindow("Vald produkt:");
                    Console.WriteLine();

                    string infoHeader = $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";
                    string infoRow = $"{productId,-20} {productName,-20} {productPrice,-20} {productPriceType,-20}";

                    CenterConsoleOutput.CenterTextToWindow(infoHeader);
                    CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                    CenterConsoleOutput.CenterTextToWindow(infoRow);

                    Console.WriteLine();

                    var arrowConfirm = new ConsoleOptionsArrow();
                    var confirmOptions = new[]
                    {
                        "Ja, radera produkt",
                        "Nej, tillbaka"
                    };

                    int deleteChoice = arrowConfirm.ShowArrow("Är du säker?", confirmOptions, renderAboveOptions: () =>
                    {
                        CenterConsoleOutput.CenterTextToWindow("Radera produkt:");
                        Console.WriteLine();

                        string infoHeader = $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";
                        string infoRow = $"{productId,-20} {productName,-20} {productPrice,-20} {productPriceType,-20}";

                        CenterConsoleOutput.CenterTextToWindow(infoHeader);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                        CenterConsoleOutput.CenterTextToWindow(infoRow);

                        Console.WriteLine();
                    });

                    if (deleteChoice != 0)
                    {
                        return;
                    }

                    var products = reader.ReadAll();
                    int removed = products.RemoveAll(p => p.ProductIdNumber == productId);

                    if (removed == 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        CenterConsoleOutput.CenterTextToWindow("Kunde inte radera: produkten hittades inte längre i listan.");
                        Console.ResetColor();
                        ValidatedConsoleInput.PauseCentered();
                        return;
                    }

                    writer.SaveAll(products);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    CenterConsoleOutput.CenterTextToWindow("Produkt raderad");
                    Console.ResetColor();

                    Console.WriteLine();

                    var afterDeleteProductMenu = new ConsoleOptionsArrow();
                    var afterDeleteProductOptions = new[]
                    {
                        "Radera en till produkt",
                        "Tillbaka till produktsidan"
                    };

                    int afterDeleteProductChoice = afterDeleteProductMenu.ShowArrow("Välj:", afterDeleteProductOptions);
                    if (afterDeleteProductChoice == 0)
                        continue;

                    return;
                }
            }
        }

        private static IProductModel SelectProduct(List<IProductModel> products)
        {
            var productDisplay = products
                .Select(p => $"{p.ProductIdNumber,-6} {p.ProductFullName}")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj produkt:", productDisplay);
            return products[index];
        }
    }
}
