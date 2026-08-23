namespace Kassasystemet_refac
{
    public class UpdateProduct
    {
        public void Run()
        {
            IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
            ISaveProductToFile productWriter = new SaveProductToFile();
            ISearchProduct productFinder = new ProductSearch(productReader);

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

                    int editChoice = arrowEdit
                        .ShowArrow(
                            "Välj vad du vill ändra:", 
                            editOptions, 
                            renderAboveOptions: () =>
                    {
                        RenderSelectedProduct(
                            productId,
                            productName,
                            productPrice,
                            productPriceType);

                    });

                    EditResult result =
                    HandleEditChoice(
                        editChoice,
                        productId,
                        ref productName,
                        ref productPrice,
                        ref productPriceType,
                        productReader,
                        productWriter);

                    if (result == EditResult.Continue)
                    {
                        continue;
                    }

                    if (result == EditResult.Exit)
                    {
                        return;
                    }

                    if (result == EditResult.Saved)
                    {
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

        //Hanterar användarens val i redigeringsmenyn
        //Metoden ansvarar för att uppdatera produktnamn, pris och pristyp
        //samt avbryta redigeringen.
        //Returnerar ett resultat som talar om vad användaren gjorde
        private static EditResult HandleEditChoice(
            int editChoice,
            int productId,
            ref string productName,
            ref decimal productPrice,
            ref string productPriceType,
            IReadAllProductsFromFile productReader,
            ISaveProductToFile productWriter)
        {
            if (editChoice == 0)
            {
                productName = ValidatedConsoleInput
                    .ReadValidatedCenteredText(
                        "== Uppdatera produkt ==",
                        "Produktnamn: ",
                        ProductValidationService.ValidateProductName);

                return EditResult.Continue;
            }

            if (editChoice == 1)
            {
                string productPriceInput = ValidatedConsoleInput
                    .ReadValidatedCenteredText(
                        "== Uppdatera produkt ==",
                        "Pris: ",
                        ProductValidationService.ValidateProductPrice);

                productPrice = decimal.Parse(productPriceInput);

                return EditResult.Continue;
            }

            if (editChoice == 2)
            {
                productPriceType = ValidatedConsoleInput
                    .ReadValidatedCenteredText(
                        "== Uppdatera produkt ==",
                        "Pristyp: ",
                        ProductValidationService.ValidateProductPriceType);

                return EditResult.Continue;

            }

            if (editChoice == 3)
            {
                bool saved =
                     SaveProductChanges(
                         productId,
                         productName,
                         productPrice,
                         productPriceType,
                         productReader,
                         productWriter);

                if (saved)
                {
                    return EditResult.Saved;
                }

                return EditResult.Exit;

            }

            return EditResult.Exit;
        }

        //Metoden:
        //1. läser alla medlemmar
        //2. Hittar rätt medlem
        //3. Ersätter medlemmen
        //4.Sparar listan
        //5. Visar resultat
        //Returnerar true om sparningen lyckats
        //Returnerar false om medlemmen inte hittades
        private static bool SaveProductChanges(
            int productId,
            string productName,
            decimal productPrice,
            string productPriceType,
            IReadAllProductsFromFile productReader,
            ISaveProductToFile productWriter)
        {
            var products = productReader.ReadAll();
            int index = products.FindIndex(p => 
            p.ProductIdNumber == productId);

            if (index < 0)
            {
                NotificationService.ShowError(
                    "Kunde inte spara: Produkten finns inte");

                ValidatedConsoleInput
                    .PauseCentered();

                return false;
            }

            products[index] = new ProductModel(
                productId, 
                productName, 
                productPrice,
                productPriceType);

            productWriter.SaveAll(products);

            return true;
        }
    }
}
