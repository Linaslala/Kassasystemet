namespace Kassasystemet_refac
{
    public class UpdateProduct
    {
        public void Run()
        {
            IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
            ISaveProductToFile productWriter = new SaveProductToFile();
            ISearchProduct productFinder = new ProductSearch(reader);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Uppdatera produktinformation ==");

                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText("Sök på produktnummer eller produktnamn: ");
                var searchProductResult = productFinder.Search(queryInput);

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
                        CenterConsoleOutput.CenterTextToWindow("== Uppdatera produktinformation ==");
                        NotificationService.ShowError(
                            "Produkten du söker finns inte i systemet");
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
                    CenterConsoleOutput.CenterTextToWindow("== Uppdatera produkt ==");
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

                    var arrowEdit = new ConsoleOptionsArrow();
                    var editOptions = new[]
                    {
                        "Ändra produktnamn",
                        "Ändra pris",
                        "Ändra pristyp",
                        "Spara",
                        "Avbryt"
                    };

                    int editChoice = arrowEdit.ShowArrow("Välj vad du vill ändra:", editOptions, renderAboveOptions: () =>
                    {
                        CenterConsoleOutput.CenterTextToWindow("Vald produkt:");
                        Console.WriteLine();

                        string infoHeader = $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";
                        string infoRow = $"{productId,-20} {productName,-20} {productPrice,-20} {productPriceType,-20}";

                        CenterConsoleOutput.CenterTextToWindow(infoHeader);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                        CenterConsoleOutput.CenterTextToWindow(infoRow);

                        Console.WriteLine();
                    });

                    if (editChoice == 0)
                    {
                        productName = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera produkt ==",
                            "Produktnamn: ",
                            ValidateProductName
                         );
                    }
                    else if (editChoice == 1)
                    {
                        string productPriceInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera produkt ==",
                            "Pris: ",
                            ValidateProductPrice
                        );

                        productPrice = decimal.Parse(productPriceInput);
                    }
                    else if (editChoice == 2)
                    {
                        productPriceType = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera produkt ==",
                            "Pristyp: ",
                            ValidateProductPriceType
                        );
                    }
                    else if (editChoice == 3)
                    {
                        var products = reader.ReadAll();
                        int index = products.FindIndex(p => p.ProductIdNumber == productId);

                        if (index < 0)
                        {
                            NotificationService.ShowError(
                                "Kunde inte spara: Produkten finns inte");
                        }

                        products[index] = new ProductModel(productId, productName, productPrice, productPriceType);
                        productWriter.SaveAll(products);


                        NotificationService.ShowSuccess(
                           "=== Produktinformation uppdaterad ===");


                        CenterConsoleOutput.CenterTextToWindow($"{productId} {productName} {productPrice} {productPriceType}");

                        ValidatedConsoleInput.PauseCentered();

                        var afterSaveProductMenu = new ConsoleOptionsArrow();
                        var afterSaveProductOptions = new[]
                        {
                            "Uppdatera en till produkt",
                            "Tillbaka till produktsidan"
                        };

                        int afterChoice = afterSaveProductMenu.ShowArrow("Välj:", afterSaveProductOptions);
                        if (afterChoice == 0)
                            break;

                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private static void ValidateProductName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Ogiltigt produktnamn: får inte vara tomt.");
        }

        private static void ValidateProductPrice(string productPriceInput)
        {
            if (string.IsNullOrWhiteSpace(productPriceInput))
                throw new ArgumentException("Ogiltigt pris: får inte vara tomt.");

            if (!decimal.TryParse(productPriceInput, out _))
                throw new ArgumentException("Ogiltigt pris: måste vara ett giltigt nummer.");
        }

        private static void ValidateProductPriceType(string productPriceType)
        {
            if (string.IsNullOrWhiteSpace(productPriceType))
                throw new ArgumentException("Ogiltigt produkttyp: får inte vara tomt.");

            if (productPriceType.Any(char.IsDigit))
                throw new ArgumentException("Ogiltig produktpristyp: måste ange styckpris eller kilopris");
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
