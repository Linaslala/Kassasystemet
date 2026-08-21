using System.Globalization;

namespace Kassasystemet_refac
{
    public class CartService
    {
        public static void PrintCart(List<CartItemModel> cart)
        {
            if (cart.Count == 0)
            {
                CenterConsoleOutput.CenterTextToWindow("Varukorg: (tom)");
                return;
            }

            CenterConsoleOutput.CenterTextToWindow("Varukorg:");
            Console.WriteLine();

            foreach (var item in cart.OrderBy(x => x.ProductIdNumber))
            {
                CenterConsoleOutput.CenterTextToWindow(
                    $"{item.ProductIdNumber} {item.ProductName} {item.ProductQuantity} st {item.LineTotal.ToString("0.00", CultureInfo.InvariantCulture)} SEK");
            }
        }

        public static void RemoveProductFromCart(List<CartItemModel> cart)
        {
            if (cart.Count == 0)
            {
                Console.Clear();
                NotificationService.ShowError(
                    "Varukorgen är tom");
                //Console.ForegroundColor = ConsoleColor.Red;
                //CenterConsoleOutput.CenterTextToWindow("Varukorgen är tom.");
                //Console.ResetColor();
                //ValidatedConsoleInput.PauseCentered();
                return;
            }

            var purchaseDisplay = cart
                .Select(c => $"{c.ProductIdNumber,-6} {c.ProductName} Antal: {c.ProductQuantity}")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj produkt att ta bort:", purchaseDisplay);
            cart.RemoveAt(index);
        }
        public static void AddProductPrompt(List<CartItemModel> cart, Dictionary<int, IProductModel> productByIdNumber)
        {
            Console.Clear();
            CenterConsoleOutput.CenterTextToWindow("== Lägg till produkt ==");
            Console.WriteLine();

            string lineInput = UserInputPlacer.ReadCenteredText("Ange: Produktnummer Antal (ex: 1 2): ").Trim();

            var tokens = lineInput.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2
                || !int.TryParse(tokens[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productIdNumber)
                || productIdNumber <= 0
                || !int.TryParse(tokens[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productQuantity)
                || productQuantity <= 0)
            {
                NotificationService.ShowError(
                             "Ogiltig inmatning. Skriv exakt: Produktnummer Antal (ex: 1 2).");

                //Console.ForegroundColor = ConsoleColor.Red;
                //CenterConsoleOutput.CenterTextToWindow("Ogiltig inmatning. Skriv exakt: Produktnummer Antal (ex: 1 2).");
                //Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            if (!productByIdNumber.TryGetValue(productIdNumber, out var product))
            {
                NotificationService.ShowError(
                             $"Ingen produkt hittades med Produktnummer {productIdNumber}.");
                //Console.ForegroundColor = ConsoleColor.Red;
                //CenterConsoleOutput.CenterTextToWindow($"Ingen produkt hittades med Produktnummer {productIdNumber}.");
                //Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            var existing = cart.FirstOrDefault(x => x.ProductIdNumber == productIdNumber);
            if (existing != null)
            {
                cart.Remove(existing);
                cart.Add(existing.WithQuantity(existing.ProductQuantity + productQuantity));
            }
            else
            {
                cart.Add(new CartItemModel(
                    product.ProductIdNumber,
                    product.ProductName,
                    product.ProductPrice,
                    product.ProductPriceType,
                    productQuantity));
            }

            NotificationService.ShowSuccessHeader(
                         $"Tillagd: {product.ProductName} x{productQuantity}");

            //Console.ForegroundColor = ConsoleColor.Green;
            //CenterConsoleOutput.CenterTextToWindow($"Tillagd: {product.ProductName} x{productQuantity}");
            //Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}
