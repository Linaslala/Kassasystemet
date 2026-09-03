using System.Globalization;

namespace Kassasystemet_refac
{
    public class CreateNewProduct
    {
        public void Run()
        {
            string productHeader = "== Registrera ny Produkt ==";
            string productNamePrompt = "Produktnamn: ";
            string productPricePrompt = "Pris (kr): ";
            string productPriceTypePrompt = "Pristyp ( skriv styckpris eller kilopris): ";

            string productNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                productHeader,
                productNamePrompt,
                ProductValidationService.ValidateProductName
            );

            string productPriceInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                productHeader,
                productPricePrompt,
                ProductValidationService.ValidateProductPrice,
                clearConsoleEachAttempt: false
            );

            string productPriceTypeInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                productHeader,
                productPriceTypePrompt,
                ProductValidationService.ValidateProductPriceType,
                clearConsoleEachAttempt: false
            );

            IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
            ISaveProductToFile productWriter = new SaveProductToFile();

            var products = productReader.ReadAll();

            int newProductId = products.Any()
                ? products.Max(p => p.ProductIdNumber) + 1
                : 1;


            decimal productPriceDecimalInput =
                decimal.Parse(productPriceInput.Replace(',', '.'),
                CultureInfo.InvariantCulture);

            products.Add(new ProductModel(
                newProductId, 
                productNameInput, 
                productPriceDecimalInput, 
                productPriceTypeInput));
                productWriter.SaveAll(products);


            Console.Clear();

            NotificationService.ShowSuccessHeader(
                 "=== Ny produkt sparad ===");

            RenderCreatedProduct(
                newProductId, 
                productNameInput, 
                productPriceDecimalInput, 
                productPriceTypeInput);
                    
            Console.ResetColor();
            ValidatedConsoleInput.PauseCentered(
                "Tryck valfri tangent för att fortsätta...");

            if (ShowAfterCreateMenu())
            {
                Run();
                return;
            }
        }

        public static void RenderCreatedProduct(
            int productId,
            string productName,
            decimal productPrice,
            string productPriceType)
        {

            string header =
               $"{"Produktnummer",-12}" +
               $"{"Produktnamn",-25}" +
               $"{"Pris",-12}" +
               $"{"Pristyp",-15}";

            string row =
                $"{productId,-12}" +
                $"{productName,-25}" +
                $"{productPrice.ToString(
                        "0.00", CultureInfo.CurrentCulture) + " kr",-12}" +
                $"{productPriceType,-15}";

            // Skriv ut header, avskiljare och data
            CenterConsoleOutput.CenterTextToWindow(header);
            CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
            CenterConsoleOutput.CenterTextToWindow(row);
                  
        }

        //Returnerar true om användaren vill skapa ytterligare en produkt.
        private static bool ShowAfterCreateMenu()
        {
            var menu = new ConsoleOptionsArrow();

            var options = new[]
            {
                "Skapa ny produkt",
                "Tillbaka till produktsidan"
            };

            int choice = menu.ShowArrow(
                "Välj:",
                options);

            return choice == 0;

        }
    }

}
