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
                ValidateProductName
            );

            string productPriceInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                productHeader,
                productPricePrompt,
                ValidateProductPrice,
                clearConsoleEachAttempt: false
            );

            string productPriceTypeInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                productHeader,
                productPriceTypePrompt,
                ValidateProductPriceType,
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

            products.Add(new ProductModel(newProductId, productNameInput, productPriceDecimalInput, productPriceTypeInput));
            productWriter.SaveAll(products);


            Console.Clear();

            NotificationService.ShowSuccessHeader(
                 "=== Ny produkt sparad ===");

            //Console.ForegroundColor = ConsoleColor.Green;

            //CenterConsoleOutput.CenterTextToWindow("== Ny produkt sparad ==");
            //Console.WriteLine();

            string infoHeader =
                $"{"Produktnummer",-12}{"Produktnamn",-25}{"Pris",-12}{"Pristyp",-15}";

            string infoRow =
                $"{newProductId,-12}{productNameInput,-25}{productPriceDecimalInput.ToString("0.00", CultureInfo.CurrentCulture) + " kr",-12}" +
                $"{productPriceTypeInput,-15}";

            // Skriv ut header, avskiljare och data
            CenterConsoleOutput.CenterTextToWindow(infoHeader);
            CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
            CenterConsoleOutput.CenterTextToWindow(infoRow);

            Console.ResetColor();
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");


            var afterSaveMenu = new ConsoleOptionsArrow();
            var afterSaveOptions = new[]
            {
                "Registrera ny produkt",
                "Tillbaka till produktsidan"
            };

            int choice = afterSaveMenu.ShowArrow("Välj:", afterSaveOptions);
            if (choice == 0)
            {
                Run();
                return;
            }
            return;
        }

        private static void ValidateProductName(string productNameInput)
        {
            if (string.IsNullOrWhiteSpace(productNameInput))
                throw new ArgumentException("Ogiltigt produktnamn: får inte vara tomt.");
        }

        private static void ValidateProductPrice(string productPriceInput)
        {
            if (string.IsNullOrWhiteSpace(productPriceInput))
                throw new ArgumentException("Ogiltigt produktpris: får inte vara tomt.");

            if (productPriceInput.Any(char.IsLetter))
                throw new ArgumentException("Ogiltigt produktpris: får inte innehålla bokstäver.");
        }
        private static void ValidateProductPriceType(string productPriceTypeInput)
        {
            if (string.IsNullOrWhiteSpace(productPriceTypeInput))
                throw new ArgumentException("Ogiltigt produkttyp: får inte vara tomt.");

            if (productPriceTypeInput.Any(char.IsDigit))
                throw new ArgumentException("Ogiltig produktpristyp: måste ange styckpris eller kilopris");
        }
    }
}
