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
   
   

   
    
   
    






    

    
   
   
    


    



   






   

   

    

    public class ProductSearch : ISearchProduct
    {
        private readonly IReadAllProductsFromFile _reader;

        public ProductSearch(IReadAllProductsFromFile reader)
        {
            _reader = reader;
        }

        public List<IProductModel> Search(string searchProductText)
        {
            var all = _reader.ReadAll();

            if (string.IsNullOrWhiteSpace(searchProductText))
                return all;

            string userProductQuery = searchProductText.Trim().ToLowerInvariant();

            return all
                .Where(p =>
                {
                    string productName = (p.ProductName ?? "").ToLowerInvariant();
                    string productType = (p.ProductPriceType ?? "").ToLowerInvariant();
                    string fullProductName = (p.ProductFullName ?? "").ToLowerInvariant();

                    return p.ProductIdNumber.ToString().Contains(userProductQuery)
                           || productName.Contains(userProductQuery)
                           || productType.Contains(userProductQuery)
                           || fullProductName.Contains(userProductQuery)
                           || p.ProductPrice.ToString().Contains(userProductQuery);
                })
                .ToList();
        }
    }





   




    internal static class ProductFilePath
    {
        private static string EnsureTextFilesDir()
        {
            var baseDir = AppContext.BaseDirectory;

            var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

            var textFilesDir = System.IO.Path.Combine(projectDir, "TextFiles");

            Directory.CreateDirectory(textFilesDir);
            return textFilesDir;
        }

        private static readonly string TextFilesDir = EnsureTextFilesDir();

        public static string Path => System.IO.Path.Combine(TextFilesDir, "products.txt");
    }






    





    internal static class MemberFilePath
    {
        private static string EnsureTextFilesDir()
        {
            var baseDir = AppContext.BaseDirectory;

            var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

            var textFilesDir = Path.Combine(projectDir, "TextFiles");
            Directory.CreateDirectory(textFilesDir);

            return textFilesDir;
        }

        private static readonly string TextFilesDir = EnsureTextFilesDir();

        public static string MembersPath => Path.Combine(TextFilesDir, "members.txt");
    }
    public class CampaignSearch : ISearchCampaign
    {
        private readonly IReadAllCampaignsFromFile _reader;

        public CampaignSearch(IReadAllCampaignsFromFile reader)
        {
            _reader = reader;
        }

        public List<ICampaignModel> Search(string searchCampaignText)
        {
            if (string.IsNullOrWhiteSpace(searchCampaignText))
                return new List<ICampaignModel>();

            searchCampaignText = searchCampaignText.Trim().ToLowerInvariant();

            bool isProductIdInt = int.TryParse(
                searchCampaignText,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int searchedProductId);

            bool isDateFormatSearch = DateTime.TryParseExact(
                searchCampaignText,
                new[] { "yyyy-MM-dd", "yyyy-MM", "yyyyMMdd", "yy-MM-dd", "dd-MM-yyyy", "dd/MM/yyyy" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime searchedDate);

            return _reader.ReadAll()
                .Where(c =>
                {
                    if (!string.IsNullOrWhiteSpace(c.CampaignName) &&
                         c.CampaignName.ToLowerInvariant().Contains(searchCampaignText))
                        return true;

                    string campaignStartText = c.CampaignStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).ToLowerInvariant();
                    string campaignEndText = c.CampaignEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).ToLowerInvariant();

                    if (campaignStartText.Contains(searchCampaignText) || campaignEndText.Contains(searchCampaignText))
                        return true;

                    if (isDateFormatSearch &&
                        (c.CampaignStartDate.Date == searchedDate.Date || c.CampaignEndDate.Date == searchedDate.Date))
                        return true;

                    if (c.ProductIdNumbers != null)
                    {
                        if (isProductIdInt && c.ProductIdNumbers.Contains(searchedProductId))
                            return true;

                        string productIdNumbersText = string.Join(",", c.ProductIdNumbers).ToLowerInvariant();
                        if (productIdNumbersText.Contains(searchCampaignText))
                            return true;
                    }

                    return false;
                })
                .ToList();
        }
    }
    public class CampaignModel : ICampaignModel
    {
        public string CampaignName { get; }
        public CampaignType TypeOfCampaign { get; }
        public DateTime CampaignStartDate { get; }
        public DateTime CampaignEndDate { get; }
        public IReadOnlyList<int> ProductIdNumbers { get; }

        public CampaignModel(
            string campaignName,
            CampaignType typeOfCampaign,
            DateTime campaignStartDate,
            DateTime campaignEndDate,
            IReadOnlyList<int> productIdNumbers)
        {
            CampaignName = campaignName;
            TypeOfCampaign = typeOfCampaign;
            CampaignStartDate = campaignStartDate;
            CampaignEndDate = campaignEndDate;
            ProductIdNumbers = productIdNumbers;
        }

        public bool IsActive(DateTime now)
        {
            return now >= CampaignStartDate && now <= CampaignEndDate;
        }
    }
    public class PercentOffCampaign : ICampaignModel
    {
        public string CampaignName { get; }
        public CampaignType TypeOfCampaign => CampaignType.PercentOffCampaign;

        public DateTime CampaignStartDate { get; }
        public DateTime CampaignEndDate { get; }

        public IReadOnlyList<int> ProductIdNumbers { get; }

        public decimal PercentOff { get; }

        public PercentOffCampaign(string campaignName, DateTime campaignStartDate, DateTime campaignEndDate, IEnumerable<int> productIdNumbers, decimal percentOff)
        {
            ValidateCampaignParts(campaignName, campaignStartDate, campaignEndDate, productIdNumbers);

            if (percentOff <= 0m || percentOff > 100m)
                throw new ArgumentException("Rabattprocenten måste vara > 0 och <= 100.", nameof(percentOff));

            CampaignName = campaignName.Trim();
            CampaignStartDate = campaignStartDate;
            CampaignEndDate = campaignEndDate;
            ProductIdNumbers = productIdNumbers.Distinct().Where(n => n > 0).ToList();
            PercentOff = percentOff;
        }

        public bool IsActive(DateTime now) => now >= CampaignStartDate && now <= CampaignEndDate;

        private static void ValidateCampaignParts(string campaignName, DateTime campaignStartDate, DateTime campaignEndDate, IEnumerable<int> productIdNumbers)
        {
            if (string.IsNullOrWhiteSpace(campaignName))
                throw new ArgumentException("Namn får inte vara tomt.", nameof(campaignName));

            if (campaignEndDate < campaignStartDate)
                throw new ArgumentException("Slutdatum kan inte vara före startdatum.");

            if (productIdNumbers == null)
                throw new ArgumentNullException(nameof(productIdNumbers));

            if (!productIdNumbers.Any(i => i > 0))
                throw new ArgumentException("Minst ett giltigt produkt-id krävs.", nameof(productIdNumbers));
        }
    }
    public enum CampaignType
    {
        PercentOffCampaign
    }







    internal static class CampaignFilePath
    {
        private static string EnsureTextFilesDir()
        {
            var baseDir = AppContext.BaseDirectory;

            var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

            var textFilesDir = System.IO.Path.Combine(projectDir, "TextFiles");
            Directory.CreateDirectory(textFilesDir);

            return textFilesDir;
        }

        private static readonly string TextFilesDir = EnsureTextFilesDir();

        public static string Path => System.IO.Path.Combine(TextFilesDir, "campaigns.txt");
    }



    



   

   

    public class ResumePurchase
    {
        public void Run()
        {
            if (!File.Exists(ReceiptFilePath.ReceiptDraftPath))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Det finns inget sparat pågående köp att återuppta.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            if (!TryLoadReceiptDraft(out int memberIdNUmber, out List<(int productIdNumber, int productQuantity)> items))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Det sparade köpet är skadat och kan inte återupptas.");
                Console.ResetColor();

                try { File.Delete(ReceiptFilePath.ReceiptDraftPath); } catch { }
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            new CreateNewPurchase().Run(memberIdNUmber, items);
        }

        private static bool TryLoadReceiptDraft(out int memberIdNumber, out List<(int productIdNumber, int productQuantity)> items)
        {
            memberIdNumber = 0;
            items = new List<(int, int)>();

            string content = File.ReadAllText(ReceiptFilePath.ReceiptDraftPath);
            if (string.IsNullOrWhiteSpace(content))
                return false;

            var receiptParts = content.Split(';');
            if (receiptParts.Length < 2)
                return false;

            int.TryParse(receiptParts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out memberIdNumber);

            var receiptRows = receiptParts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in receiptRows)
            {
                var two = row.Split(',');
                if (two.Length != 2) continue;

                if (int.TryParse(two[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productIdNumber) &&
                    int.TryParse(two[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productQuantity) &&
                    productIdNumber > 0 && productQuantity > 0)
                {
                    items.Add((productIdNumber, productQuantity));
                }
            }

            return true;
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
                            AddProductPrompt(cart, productById);
                            break;
                        }

                        int footerChoice = selectedIndex - 1;

                        if (footerChoice == 0)
                        {
                            if (TryPayAndShowReceipt(ref memberIdNumber, cart))
                                return;
                            break;
                        }

                        if (footerChoice == 1)
                        {
                            ShowInlineProductSearchAndPresent();
                            break;
                        }

                        if (footerChoice == 2)
                        {
                            RemoveProductFromCart(cart);
                            break;
                        }

                        if (footerChoice == 3)
                        {
                            memberIdNumber = ReadMemberIdNumber();
                            break;
                        }

                        if (footerChoice == 4)
                        {
                            SavePurchaseDraft(memberIdNumber, cart);
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

            PrintCart(cart);
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

        private static void AddProductPrompt(List<CartItemModel> cart, Dictionary<int, IProductModel> productByIdNumber)
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
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Ogiltig inmatning. Skriv exakt: Produktnummer Antal (ex: 1 2).");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            if (!productByIdNumber.TryGetValue(productIdNumber, out var product))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow($"Ingen produkt hittades med Produktnummer {productIdNumber}.");
                Console.ResetColor();
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

            Console.ForegroundColor = ConsoleColor.Green;
            CenterConsoleOutput.CenterTextToWindow($"Tillagd: {product.ProductName} x{productQuantity}");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        private bool TryPayAndShowReceipt(ref int memberIdNumber, List<CartItemModel> cart)
        {
            if (cart.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Du kan inte betala ett tomt köp.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return false;
            }

            if (memberIdNumber <= 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Du måste ange kundnummer innan betalning.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                memberIdNumber = ReadMemberIdNumber();
            }

            var memberReader = new ReadAllMembersFromFile();
            var members = memberReader.ReadAll();

            int memberIdSnapshot = memberIdNumber;

            bool customerExists = members.Any(m => m.MemberIdNumber == memberIdSnapshot);

            if (!customerExists)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow(
                    $"Ingen kund hittades med kundnummer {memberIdNumber}.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();

                memberIdNumber = 0;
                return false;
            }

            ReceiptModel receipt = CompletePayment(memberIdNumber, cart);
            ClearPurchaseDraft();

            Console.Clear();
            CenterConsoleOutput.CenterTextToWindow("== KVITTO ==");
            Console.WriteLine();
            ReceiptPrinter.PrintDetailed(receipt);
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att gå tillbaka...");
            return true;
        }

        private static void ShowInlineProductSearchAndPresent()
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
                    : SelectProductFromList(results);

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

        private static IProductModel SelectProductFromList(List<IProductModel> products)
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

        private void RemoveProductFromCart(List<CartItemModel> cart)
        {
            if (cart.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Varukorgen är tom.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            var purchaseDisplay = cart
                .Select(c => $"{c.ProductIdNumber,-6} {c.ProductName} Antal: {c.ProductQuantity}")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj produkt att ta bort:", purchaseDisplay);
            cart.RemoveAt(index);
        }

        private static void PrintCart(List<CartItemModel> cart)
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

        private ReceiptModel CompletePayment(int memberIdNumber, List<CartItemModel> cart)
        {
            var campaigns = new ReadAllCampaignsFromFile()
                .ReadAll()
                .OfType<PercentOffCampaign>()
                .Where(c => c.IsActive(DateTime.Now))
                .ToList();

            var receiptRows = new List<ReceiptRowModel>();

            foreach (var item in cart)
            {
                decimal lineTotal = item.LineTotal;
                receiptRows.Add(new ReceiptRowModel(item.ProductName, item.ProductQuantity, lineTotal));

                var bestCampaign = campaigns
                    .Where(c => c.ProductIdNumbers != null && c.ProductIdNumbers.Contains(item.ProductIdNumber))
                    .OrderByDescending(c => c.PercentOff)
                    .FirstOrDefault();

                if (bestCampaign != null && bestCampaign.PercentOff > 0m)
                {
                    decimal discount = Math.Round(lineTotal * (bestCampaign.PercentOff / 100m), 2);
                    if (discount > 0m)
                    {

                        receiptRows.Add(new ReceiptRowModel(
                            $"Rabatt: {bestCampaign.PercentOff.ToString("0.0", CultureInfo.InvariantCulture)}%", 0,
                            -discount));
                    }
                }
            }

            decimal totalAmount = receiptRows.Sum(r => r.ReceiptProductAmount);
            int totalItems = cart.Sum(x => x.ProductQuantity);

            var receiptReader = new ReadAllReceiptsFromFile();
            var receiptWriter = new SaveReceiptToFile();
            var receipts = receiptReader.ReadAll();

            int nextReceiptNumber = receipts.Any() ? receipts.Max(r => r.ReceiptNumber) + 1 : 1;

            var receipt = new ReceiptModel(
                nextReceiptNumber,
                memberIdNumber,
                DateTime.Now,
                receiptRows,
                totalItems,
                totalAmount);

            receipts.Add(receipt);
            receiptWriter.SaveAll(receipts);
            return receipt;
        }

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

        private static int ReadMemberIdNumber()
        {
            string input = ValidatedConsoleInput.ReadValidatedCenteredText(
                "== Kundnummer ==",
                "Kundnummer: ",
                ValidatePositiveInt);

            return int.Parse(input.Trim(), CultureInfo.InvariantCulture);
        }

        private static void SavePurchaseDraft(int memberIdNumber, List<CartItemModel> cart)
        {
            string items = string.Join("\n", cart.Select(c => $"{c.ProductIdNumber},{c.ProductQuantity}"));
            string line = $"{memberIdNumber};{items}";
            File.WriteAllText(ReceiptFilePath.ReceiptDraftPath, line);
        }

        private static void ClearPurchaseDraft()
        {
            if (File.Exists(ReceiptFilePath.ReceiptDraftPath))
                File.Delete(ReceiptFilePath.ReceiptDraftPath);
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



