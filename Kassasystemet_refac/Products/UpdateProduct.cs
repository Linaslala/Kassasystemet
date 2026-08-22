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

                    RenderSelectedProduct(
                        productId,
                        productName,
                        productPrice,
                        productPriceType);

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
                        RenderSelectedProduct(
                            productId,
                            productName,
                            productPrice,
                            productPriceType);

                    });

                    if (editChoice == 0)
                    {
                        productName = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera produkt ==",
                            "Produktnamn: ",
                            ProductValidationService.ValidateProductName
                         );
                    }
                    else if (editChoice == 1)
                    {
                        string productPriceInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera produkt ==",
                            "Pris: ",
                            ProductValidationService.ValidateProductPrice
                        );

                        productPrice = decimal.Parse(productPriceInput);
                    }
                    else if (editChoice == 2)
                    {
                        productPriceType = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera produkt ==",
                            "Pristyp: ",
                            ProductValidationService.ValidateProductPriceType
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

                            ValidatedConsoleInput
                                .PauseCentered();

                            return;
                        }

                        products[index] = new ProductModel(productId, productName, productPrice, productPriceType);
                        productWriter.SaveAll(products);


                        NotificationService.ShowSuccessHeader(
                           "=== Produktinformation uppdaterad ===");

                        RenderSelectedProduct(
                            productId,
                            productName,
                            productPrice,
                            productPriceType);

                        ValidatedConsoleInput.PauseCentered();

                        if (ShowAfterSaveMenu())
                        {
                            break;
                        }

                        return;
                    }
                    else
                    {
                        return;
                    }
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

        private static void RenderSelectedProduct(
           int productId,
           string productName,
           decimal productPrice,
           string productPriceType)
        {
            CenterConsoleOutput.CenterTextToWindow(
                "Vald produkt:");

            Console.WriteLine();

            string header =
                $"{"Produktnummer",-20}" +
                $"{"Produkt",-20}" +
                $"{"Pris",-20}" +
                $"{"Pristyp",-20}";

            string row =
                $"{productId,-20}" +
                $"{productName,-20}" +
                $"{productPrice,-20}" +
                $"{productPriceType,-20}";

            CenterConsoleOutput.CenterTextToWindow(header);

            CenterConsoleOutput.CenterTextToWindow(
                new string('-', header.Length));

            CenterConsoleOutput.CenterTextToWindow(
                row);

            Console.WriteLine();
        }

        //Extract Workflow Methods
        //Visar menyn efter att en medlem sparats.
        //Returnerar true om användaren vill uppdatera ytterligare en produkt
        private static bool ShowAfterSaveMenu()
        {
            var afterSaveMenu = new ConsoleOptionsArrow();
            var afterSaveOptions = new[]
            {
                "Uppdatera en till produkt",
                "Tillbaka till produktsidan"
            };

            int choice =
                afterSaveMenu.ShowArrow(
                    "Välj:",
                    afterSaveOptions);

            return choice == 0;
        }
    }
}
