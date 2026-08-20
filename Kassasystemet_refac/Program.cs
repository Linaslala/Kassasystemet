using Kassasystemet_refac.Data;
using System.Globalization;

namespace Kassasystemet_refac
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Linas Klubb Livs – Kassasystem";

            Seeder.SeedAll();

            while (true)
            {
                Console.Clear();

                string headerLineOne = "========================================";
                string headerText = "VÄLKOMMEN TILL LINAS KLUBB-LIVS";
                string headerLineTwo = "========================================";
                string enterText = "Tryck ENTER för att logga in som kassör";
                string closingText = "Tryck ESC för att stänga";

                Console.ForegroundColor = ConsoleColor.Yellow;
                CenterConsoleOutput.CenterTextToWindow(headerLineOne);
                CenterConsoleOutput.CenterTextToWindow(headerText);
                CenterConsoleOutput.CenterTextToWindow(headerLineTwo);
                Console.ResetColor();

                Console.WriteLine();
                CenterConsoleOutput.CenterTextToWindow(enterText);
                Console.WriteLine();
                CenterConsoleOutput.CenterTextToWindow(closingText);

                while (true)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                        Environment.Exit(0);

                    if (key == ConsoleKey.Enter)
                        break;
                }

                var mainMenue = new MainMenu();
                mainMenue.Run();
            }
        }
    }
   
    public class CreateNewPurchase
    {
        public void Run()
        {
            int memberIdNumber = ReadCustomerNumberOrSkip();
            var cart = new List<CartItemModel>();
            PurchaseSplitViewLoop(ref memberIdNumber, cart);
        }

        public void Run(int memberIdNumber, List<(int productIdNumber, int productQuantity)> resumeItems)
        {
            var cart = LoadCartFromSavedItems(resumeItems);
            PurchaseSplitViewLoop(ref memberIdNumber, cart);
        }

        private void PurchaseSplitViewLoop(ref int memberIdNumber, List<CartItemModel> cart)
        {
            IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
            var products = productReader.ReadAll();

            if (products == null || products.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Det finns inga produkter registrerade. Skapa produkter först.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            var productById = products
                .GroupBy(p => p.ProductIdNumber)
                .ToDictionary(g => g.Key, g => g.First());

            var footerOptions = new[]
            {
                "Pay",
                "Sök produkt (hitta Produktnummer)",
                "Ta bort produkt från varukorgen",
                "Ändra kundnummer",
                "Pausa pågående köp (spara) och gå ut",
                "Avbryt (utan att spara)"
            };

            const string topAction = "Lägg till produkt (produktnummer + antal)";

            int selectedIndex = 0;

            while (true)
            {
                RenderSplitPurchaseView(memberIdNumber, cart, topAction, footerOptions, selectedIndex);

                // Läs tangent
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex <= 0 ? footerOptions.Length : selectedIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex >= footerOptions.Length ? 0 : selectedIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        if (selectedIndex == 0)
                        {
                            CartService.AddProductPrompt(cart, productById);
                            break;
                        }

                        int footerChoice = selectedIndex - 1;

                        if (footerChoice == 0)
                        {
                            if (ReceiptService.TryPayAndShowReceipt(ref memberIdNumber, cart))
                                return;
                            break;
                        }

                        if (footerChoice == 1)
                        {
                            PurchaseProductSearchService.ShowInlineProductSearchAndPresent();
                            break;
                        }

                        if (footerChoice == 2)
                        {
                            CartService.RemoveProductFromCart(cart);
                            break;
                        }

                        if (footerChoice == 3)
                        {
                            memberIdNumber = ReadMemberIdNumber();
                            break;
                        }

                        if (footerChoice == 4)
                        {
                            DraftPurchaseService.SavePurchaseDraft(memberIdNumber, cart);
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            CenterConsoleOutput.CenterTextToWindow("Pågående köp sparat. Du kan återuppta senare.");
                            Console.ResetColor();
                            ValidatedConsoleInput.PauseCentered();
                            return;
                        }

                        return;

                    default:
                        break;
                }
            }
        }

        private static void RenderSplitPurchaseView(
                    int memberIdNumber,
                    List<CartItemModel> cart,
                    string topAction,
                    IReadOnlyList<string> footerOptions,
                    int selectedIndex)
        {
            Console.Clear();

            CenterConsoleOutput.CenterTextToWindow("== Registrera nytt köp ==");
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow("Ange kundnummer (eller lämna tomt om du vill lägga till senare):");
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow($"Kundnummer: {(memberIdNumber > 0 ? memberIdNumber.ToString(CultureInfo.InvariantCulture) : "")}");
            Console.WriteLine();
            Console.WriteLine();

            bool topSelected = selectedIndex == 0;
            if (topSelected) Console.ForegroundColor = ConsoleColor.Green;
            CenterConsoleOutput.CenterTextToWindow($"{(topSelected ? ">" : " ")} {topAction}");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine();

            CartService.PrintCart(cart);
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow($"Antal varor: {cart.Sum(x => x.ProductQuantity)}");
            CenterConsoleOutput.CenterTextToWindow($"Summa (utan rabatter): {cart.Sum(x => x.LineTotal).ToString("0.00", CultureInfo.InvariantCulture)} SEK");

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < footerOptions.Count; i++)
            {
                bool isSelected = selectedIndex == (i + 1);
                string line = isSelected ? $"> {footerOptions[i]}" : $"  {footerOptions[i]}";

                if (isSelected) Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleOutput.CenterTextToWindow(line);
                Console.ResetColor();
            }
        }

        //private static void ShowInlineProductSearchAndPresent()
        //{
        //    IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
        //    ISearchProduct finder = new ProductSearch(reader);

        //    while (true)
        //    {
        //        Console.Clear();
        //        CenterConsoleOutput.CenterTextToWindow("== Hitta produkt ==");
        //        Console.WriteLine();

        //        string queryInput = UserInputPlacer
        //            .ReadCenteredText("Sök på produktnummer eller produktnamn (tomt = tillbaka): ")
        //            .Trim();

        //        if (string.IsNullOrWhiteSpace(queryInput))
        //            return;

        //        var results = finder.Search(queryInput);

        //        if (results.Count == 0)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            CenterConsoleOutput.CenterTextToWindow("Produkten du söker finns inte i systemet.");
        //            Console.ResetColor();
        //            ValidatedConsoleInput.PauseCentered();
        //            continue;
        //        }

        //        var selected = results.Count == 1
        //            ? results[0]
        //            : PurchaseProductSearchService.SelectProductFromList(results);

        //        Console.Clear();
        //        CenterConsoleOutput.CenterTextToWindow("== Produkt ==");
        //        Console.WriteLine();
        //        Console.WriteLine();

        //        string header = $"{"Produktnummer",-20}{"Produkt",-20}{"Pris (kr)",-20}{"Pristyp",-20}";
        //        string row =
        //            $"{selected.ProductIdNumber,-20}" +
        //            $"{selected.ProductName,-20}" +
        //            $"{selected.ProductPrice,-20}" +
        //            $"{selected.ProductPriceType,-20}";

        //        CenterConsoleOutput.CenterTextToWindow(header);
        //        CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
        //        CenterConsoleOutput.CenterTextToWindow(row);

        //        Console.WriteLine();
        //        CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent för att återgå...");
        //        Console.ResetColor();
        //        Console.ReadKey(true);
        //        return;
        //    }
        //}

        //private static IProductModel SelectProductFromList(List<IProductModel> products)
        //{
        //    var ordered = products
        //                  .OrderBy(p => p.ProductIdNumber)
        //                  .ToList();

        //    var rows = ordered
        //         .Select(p =>
        //             $"{p.ProductIdNumber,-20} {p.ProductName,-20} {p.ProductPrice,-20} {p.ProductPriceType,-20}")
        //         .ToArray();

        //    var arrow = new ConsoleOptionsArrow();

        //    int index = arrow.ShowArrow(
        //        "Välj produkt:",
        //        rows,
        //        renderAboveOptions: () =>
        //        {
        //            CenterConsoleOutput.CenterTextToWindow("== Produkt ==");
        //            Console.WriteLine();
        //            Console.WriteLine();

        //            string header = $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";
        //            CenterConsoleOutput.CenterTextToWindow(header);
        //            CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
        //        });

        //    return ordered[index];
        //}

        private static int ReadCustomerNumberOrSkip()
        {
            Console.Clear();
            CenterConsoleOutput.CenterTextToWindow("== Registrera nytt köp ==");
            Console.WriteLine();
            CenterConsoleOutput.CenterTextToWindow("Ange kundnummer (eller lämna tomt om du vill lägga till senare):");
            Console.WriteLine();

            string input = UserInputPlacer.ReadCenteredText("Kundnummer: ").Trim();

            if (string.IsNullOrWhiteSpace(input))
                return 0;

            if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) || value <= 0)
                return 0;

            return value;
        }

        public static int ReadMemberIdNumber()
        {
            string input = ValidatedConsoleInput.ReadValidatedCenteredText(
                "== Kundnummer ==",
                "Kundnummer: ",
                ValidatePositiveInt);

            return int.Parse(input.Trim(), CultureInfo.InvariantCulture);
        }

        private static List<CartItemModel> LoadCartFromSavedItems(List<(int productIdNumber, int productQuantity)> savedItems)
        {
            IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
            var products = productReader.ReadAll();

            var cart = new List<CartItemModel>();

            foreach (var (productIdNumber, productQuantity) in savedItems)
            {
                var product = products.FirstOrDefault(p => p.ProductIdNumber == productIdNumber);
                if (product == null) continue;

                cart.Add(new CartItemModel(
                    product.ProductIdNumber,
                    product.ProductName,
                    product.ProductPrice,
                    product.ProductPriceType,
                    productQuantity));
            }

            return cart;
        }

        private static void ValidatePositiveInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Får inte vara tomt.");

            if (!int.TryParse(input.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                throw new ArgumentException("Måste vara ett heltal.");

            if (value <= 0)
                throw new ArgumentException("Måste vara större än 0.");
        }
    }
}



