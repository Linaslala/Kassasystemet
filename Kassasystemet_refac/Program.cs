using System.Globalization;
using System.Text;
using static Kassasystemet_refac.Program;
using static Kassasystemet_refac.SearchMember;

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

                var mainMenue = new MainMenue();
                mainMenue.Run();
            }
        }
        public class MainMenue
        {
            private readonly string[] _mainMenueOptions =
            {
            "Registrera nytt köp\n",
            "Sök",
            "Hantera kunder",
            "Hantera produkter",
            "Hantera kampanjer",
            "Redovisa försäljning\n",
            "Logga ut"
        };

            public void Run()
            {
                var arrowMainMenu = new ConsoleOptionsArrow();

                while (true)
                {
                    int selectedIndex = arrowMainMenu.ShowArrow("Välj funktion", _mainMenueOptions);

                    if (HandleMainMenueSelection(selectedIndex))
                        return;
                }
            }

            private bool HandleMainMenueSelection(int index)
            {
                switch (index)
                {
                    case 0:
                        var newPurchase = new PurchaseMenue();
                        newPurchase.Run();
                        return false;

                    case 1:
                        var searchMenue = new SearchMenue();
                        searchMenue.Run();
                        return false;

                    case 2:
                        var memberMenue = new MemberMenue();
                        memberMenue.Run();
                        return false;

                    case 3:
                        var productMenue = new ProductMenue();
                        productMenue.Run();
                        return false;

                    case 4:
                        var campaignMenue = new CampaignMenue();
                        campaignMenue.Run();
                        return false;

                    case 5:
                        var salesReportMenue = new SalesReportMenue();
                        salesReportMenue.Run();
                        return false;

                    case 6:
                        return true;

                    default:
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent...");
                        Console.ReadKey(true);
                        return false;
                }
            }
        }
        public class MemberMenue
        {
            private readonly string[] _memberMenueOptions =
        {
            "Redistrera ny medlem\n",
            "Uppdatera klubbmedlem",
            "Lista alla klubbmedlemmar",
            "Avsluta medlemsskap\n",
            "Tillbaka till huvudmenyn"
        };

            public void Run()
            {
                var arrowMemberMenu = new ConsoleOptionsArrow();

                while (true)
                {
                    int selectedIndex = arrowMemberMenu.ShowArrow("=== Medlemssida ===", _memberMenueOptions);

                    if (HandleMemberMenueSelection(selectedIndex))
                        return;
                }
            }
            private static bool HandleMemberMenueSelection(int index)
            {
                switch (index)
                {
                    case 0:
                        new CreateNewMember().Run();
                        return false;

                    case 1:
                        new UpdateMember().Run();
                        return false;

                    case 2:
                        new ListAllMembers().Run();
                        return false;

                    case 3:
                        new DeleteMember().Run();
                        return false;

                    case 4:
                        return true;

                    default:
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent...");
                        Console.ReadKey(true);
                        return false;
                }

            }
        }
        public class ProductMenue
        {
            private readonly string[] _productMenueOptions =
        {
            "Redistrera ny produkt\n",
            "Uppdatera produkt",
            "Lista alla produkter",
            "Ta bort produkt\n",
            "Tillbaka till huvudmenyn"
        };

            public void Run()
            {
                var arrowProductMenu = new ConsoleOptionsArrow();

                while (true)
                {
                    int selectedIndex = arrowProductMenu.ShowArrow("=== Produktsida ===", _productMenueOptions);

                    if (HandleProductMenueSelection(selectedIndex))
                        return;
                }
            }
            private static bool HandleProductMenueSelection(int index)
            {
                switch (index)
                {
                    case 0:
                        new CreateNewProduct().Run();
                        return false;

                    case 1:
                        new UpdateProduct().Run();
                        return false;

                    case 2:
                        new ListAllProducts().Run();
                        return false;

                    case 3:
                        new DeleteProduct().Run();
                        return false;

                    case 4:
                        return true;

                    default:
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent...");
                        Console.ReadKey(true);
                        return false;
                }
            }
        }

        public class PurchaseMenue
        {
            private readonly string[] _purchaseMenuOptions =
            {
            "Starta nytt köp\n",
            "Återuppta pågående köp\n",
            "Tillbaka till huvudmenyn"
        };

            public void Run()
            {
                var arrowPurchaseMenu = new ConsoleOptionsArrow();

                while (true)
                {
                    int selectedIndex = arrowPurchaseMenu.ShowArrow("=== Köp ===", _purchaseMenuOptions);

                    if (HandlePurchaseSelection(selectedIndex))
                        return;
                }
            }
            private bool HandlePurchaseSelection(int index)
            {
                switch (index)
                {
                    case 0:
                        new CreateNewPurchase().Run();
                        return false;

                    case 1:
                        new ResumePurchase().Run();
                        return false;

                    case 2:
                        return true;

                    default:
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent...");
                        Console.ReadKey(true);
                        return false;
                }
            }
        }
        public class SalesReportMenue
        {
            private readonly string[] _salesReportMenuOptions =
            {
            "Försäljningsrapport",
            "Sök kvitto\n",
            "Tillbaka till huvudmenyn"
        };

            public void Run()
            {
                var arrowMenu = new ConsoleOptionsArrow();

                while (true)
                {
                    int selectedIndex = arrowMenu.ShowArrow("=== Försäljningsrapport ===", _salesReportMenuOptions);

                    if (HandleReportSelection(selectedIndex))
                        return;
                }
            }

            private bool HandleReportSelection(int index)
            {
                switch (index)
                {
                    case 0:
                        new ListAllReceipts().Run();
                        return false;

                    case 1:
                        new FindReceipt().Run();
                        return false;

                    case 2:
                        return true;

                    default:
                        return false;
                }
            }
        }
        public class SearchMenue
        {
            private readonly string[] _searchMenueOptions =
            {
            "Sök klubbmedlem",
            "Sök produkt\n",
            "Tillbaka till huvudmenyn"
        };

            public void Run()
            {
                var arrowSearchMenu = new ConsoleOptionsArrow();

                while (true)
                {
                    int selectedIndex = arrowSearchMenu.ShowArrow("=== Sök ===", _searchMenueOptions);

                    if (HandleSearchMenueSelection(selectedIndex))
                        return;
                }
            }
            private bool HandleSearchMenueSelection(int index)
            {
                switch (index)
                {
                    case 0:
                        var memberSearch = new SearchMember();
                        memberSearch.Run();
                        return false;

                    case 1:
                        var productSearch = new SearchProduct();
                        productSearch.Run();
                        return false;

                    case 2:
                        return true;

                    default:
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent...");
                        Console.ReadKey(true);
                        return false;
                }
            }
        }
        public class SearchProduct
        {
            public void Run()
            {
                IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
                ISearchProduct finder = new ProductSearch(reader);

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Hitta produkt ==");
                    Console.WriteLine();

                    string queryInput = UserInputPlacer
                        .ReadCenteredText("Sök på produktnummer eller produktnamn (tryck enter för alla): ")
                        .Trim();

                    var results = finder.Search(queryInput);

                    if (results.Count == 0)
                    {
                        var arrowNoResult = new ConsoleOptionsArrow();
                        var noResultOptions = new[]
                        {
                        "Ny sökning",
                        "Tillbaka till huvudmenyn"
                    };

                        int choice = arrowNoResult.ShowArrow(
                            "Välj:",
                            noResultOptions,
                            renderAboveOptions: () =>
                            {
                                Console.Clear();
                                CenterConsoleOutput.CenterTextToWindow("== Hitta produkt ==");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Red;
                                CenterConsoleOutput.CenterTextToWindow(
                                    "Produkten du söker finns inte i systemet.");
                                Console.ResetColor();
                                Console.WriteLine();
                            });

                        if (choice == 0)
                            continue;

                        return;
                    }

                    var selected = results.Count == 1
                        ? results[0]
                        : SelectProduct(results);

                    var arrowAfterFound = new ConsoleOptionsArrow();
                    var afterFoundOptions = new[]
                    {
                    "Ny sökning",
                    "Tillbaka till huvudmenyn"
                };

                    int afterChoice = arrowAfterFound.ShowArrow(
                        "Välj:",
                        afterFoundOptions,
                        renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Produkt ==");
                            Console.WriteLine();
                            Console.WriteLine();

                            string header =
                                $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";

                            string row =
                                $"{selected.ProductIdNumber,-20}" +
                                $"{selected.ProductName,-20}" +
                                $"{selected.ProductPrice,-20}" +
                                $"{selected.ProductPriceType,-20}";

                            CenterConsoleOutput.CenterTextToWindow(header);
                            CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
                            CenterConsoleOutput.CenterTextToWindow(row);

                            Console.WriteLine();
                        });

                    if (afterChoice == 0)
                        continue;

                    return;
                }
            }

            private static IProductModel SelectProduct(List<IProductModel> products)
            {
                var productDisplay = products
                    .OrderBy(p => p.ProductIdNumber)
                    .Select(p => $"{p.ProductFullName}")
                    .ToArray();

                var arrow = new ConsoleOptionsArrow();
                int index = arrow.ShowArrow("Välj produkt:", productDisplay);
                return products[index];
            }
        }
    }
    public class SearchMember
    {
        public void Run()
        {
            IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
            ISearchMember finder = new MemberSearch(reader);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Hitta medlem ==");
                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText("Sök på medlemsnummer eller namn (tryck enter för alla): ").Trim();

                var results = finder.Search(queryInput);

                if (results.Count == 0)
                {
                    var arrowNoResult = new ConsoleOptionsArrow();
                    var noResultOptions = new[]
                    {
                        "Ny sökning",
                        "Tillbaka till huvudmenyn"
                    };

                    int choice = arrowNoResult.ShowArrow(
                        "Välj:",
                        noResultOptions,
                        renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Hitta medlem ==");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Medlemmen du söker finns inte i systemet.");
                            Console.ResetColor();
                            Console.WriteLine();
                        });

                    if (choice == 0)
                        continue;

                    return;
                }

                var selected = results.Count == 1
                         ? results[0]
                         : SelectMember(results);

                var arrowAfterFound = new ConsoleOptionsArrow();
                var afterFoundOptions = new[]
                {
                    "Ny sökning",
                    "Tillbaka till huvudmenyn"
                };

                int afterChoice = arrowAfterFound.ShowArrow(
                    "Välj:",
                    afterFoundOptions,
                    renderAboveOptions: () =>
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Klubbmedlem ==");
                        Console.WriteLine();
                        Console.WriteLine();

                        string header = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string row = $"{selected.MemberIdNumber,-20}{selected.MemberFirstName,-20}{selected.MemberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(header);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
                        CenterConsoleOutput.CenterTextToWindow(row);

                        Console.WriteLine();
                    });

                if (afterChoice == 0)
                    continue;

                return;
            }
        }

        private static IMemberModel SelectMember(List<IMemberModel> members)
        {
            var memberDisplay = members
                .OrderBy(m => m.MemberIdNumber)
                .Select(m => $"{m.MemberIdNumber,-6} {m.MemberFullName}")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj medlem:", memberDisplay);
            return members[index];
        }
        public class ListAllReceipts
        {
            public void Run()
            {
                Console.Clear();

                var reader = new ReadAllReceiptsFromFile();

                var baseDir = AppContext.BaseDirectory;
                var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
                var textFilesDir = Path.Combine(projectDir, "TextFiles");

                var allReceiptFiles = Directory
                    .EnumerateFiles(textFilesDir, "RECEIPT_*.txt")
                    .OrderBy(f => f)
                    .ToList();

                var allReceipts = new List<IReceiptModel>();

                foreach (var file in allReceiptFiles)
                {
                    var receiptsFromFile = reader.ReadAllFromPath(file);
                    allReceipts.AddRange(receiptsFromFile);
                }

                var receipts = allReceipts
                     .GroupBy(r => r.ReceiptNumber)
                     .Select(g => g.OrderByDescending(x => x.ReceiptCreatedAt).First())
                     .OrderByDescending(r => r.ReceiptCreatedAt)
                     .ToList();

                var arrow = new ConsoleOptionsArrow();

                arrow.ShowArrow("Välj:", new[] { "Tillbaka till huvudmenyn" }, renderAboveOptions: () =>
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);

                    CenterConsoleOutput.CenterTextToWindow("== FÖRSÄLJNINGSRAPPORT ==");
                    Console.WriteLine();

                    if (!receipts.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        CenterConsoleOutput.CenterTextToWindow("Det finns inga registrerade köp.");
                        Console.ResetColor();
                        Console.WriteLine();
                        return;
                    }

                    var byDay = receipts.GroupBy(r => r.ReceiptCreatedAt.Date).OrderByDescending(g => g.Key);

                    foreach (var dayGroup in byDay)
                    {
                        CenterConsoleOutput.CenterTextToWindow($"DATUM: {dayGroup.Key:yyyy-MM-dd}");
                        CenterConsoleOutput.CenterTextToWindow(new string('-', 50));

                        var products = dayGroup
                            .SelectMany(r => r.ReceiptRows)
                            .Where(x => x.ReceiptProductQuantity > 0)
                            .GroupBy(x => x.ReceiptProductText)
                            .Select(g => new
                            {
                                Product = g.Key,
                                Quantity = g.Sum(x => x.ReceiptProductQuantity),
                                Amount = g.Sum(x => x.ReceiptProductAmount)
                            })
                            .OrderByDescending(x => x.Quantity)
                            .ThenBy(x => x.Product);

                        foreach (var p in products)
                            CenterConsoleOutput.CenterTextToWindow(
                                $"{p.Product} x{p.Quantity} {p.Amount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");

                        var discountTotal = dayGroup
                            .SelectMany(r => r.ReceiptRows)
                            .Where(x =>
                                x.ReceiptProductQuantity == 0 &&
                                x.ReceiptProductAmount < 0 &&
                                x.ReceiptProductText.StartsWith("Rabatt", StringComparison.OrdinalIgnoreCase))
                            .Sum(x => x.ReceiptProductAmount);

                        if (discountTotal < 0)
                        {
                            CenterConsoleOutput.CenterTextToWindow(
                                $"Dagens rabatter: {discountTotal.ToString("0.00", CultureInfo.InvariantCulture)} SEK");
                        }

                        CenterConsoleOutput.CenterTextToWindow(new string('-', 50));

                        var total = dayGroup.Sum(r => r.TotalAmount);
                        CenterConsoleOutput.CenterTextToWindow(
                            $"TOTALT FÖRSÄLJNINGSPRIS: {total.ToString("0.00", CultureInfo.InvariantCulture)} SEK");

                        CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                });
            }
        }
        public class FindReceipt
        {
            public void Run()
            {
                Console.Clear();

                IReadAllReceiptsFromFile reader = new ReadAllReceiptsFromFile();
                IReceiptSearch finder = new ReceiptSearch(reader);

                while (true)
                {
                    Console.Clear();

                    CenterConsoleOutput.CenterTextToWindow("== SÖK KVITTO ==");
                    Console.WriteLine();

                    string query = UserInputPlacer.ReadCenteredText(
                        "Sök på kvittonummer eller kundnummer: ").Trim();

                    var results = finder.Search(query)
                        .OrderByDescending(r => r.ReceiptNumber)
                        .ToList();

                    var arrow = new ConsoleOptionsArrow();

                    int choice = arrow.ShowArrow("Välj:", new[] { "Ny sökning", "Tillbaka till startmenyn" }, renderAboveOptions: () =>
                    {
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);

                        CenterConsoleOutput.CenterTextToWindow("== HITTADE KVITTON ==");
                        Console.WriteLine();


                        if (!results.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Inget kvitto hittades.");
                            Console.ResetColor();
                            Console.WriteLine();
                            return;
                        }

                        foreach (var receipt in results)
                        {
                            CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                            ReceiptPrinter.PrintDetailed(receipt);
                        }

                        CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                        Console.WriteLine();

                        CenterConsoleOutput.CenterTextToWindow(new string('=', 50));
                        Console.WriteLine();
                    });

                    if (choice == 0)
                        continue;

                    return;
                }
            }
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
        public class UpdateProduct
        {
            public void Run()
            {
                IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
                ISaveProductToFile writer = new SaveProductToFile();
                ISearchProduct finder = new ProductSearch(reader);

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Uppdatera produktinformation ==");

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
                            CenterConsoleOutput.CenterTextToWindow("== Uppdatera produktinformation ==");
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
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                CenterConsoleOutput.CenterTextToWindow("Kunde inte spara: produkten hittades inte längre i listan.");
                                Console.ResetColor();
                                ValidatedConsoleInput.PauseCentered();
                                return;
                            }

                            products[index] = new ProductModel(productId, productName, productPrice, productPriceType);
                            writer.SaveAll(products);

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            CenterConsoleOutput.CenterTextToWindow("Produktinformation uppdaterad!");
                            Console.ResetColor();
                            Console.WriteLine();

                            CenterConsoleOutput.CenterTextToWindow($"{productId} {productName} {productPrice} {productPriceType}");

                            ValidatedConsoleInput.PauseCentered();

                            var afterSaveProductMenue = new ConsoleOptionsArrow();
                            var afterSaveProductOptions = new[]
                            {
                            "Uppdatera en till produkt",
                            "Tillbaka till produktsidan"
                        };

                            int afterChoice = afterSaveProductMenue.ShowArrow("Välj:", afterSaveProductOptions);
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
        public class ListAllProducts
        {
            public void Run()
            {
                Console.Clear();

                string listAllProductsHeader = "== Alla produkter ==";

                CenterConsoleOutput.CenterTextToWindow(listAllProductsHeader);

                IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
                ISearchProduct finder = new ProductSearch(reader);

                var products = reader.ReadAll()
                .OrderBy(p => p.ProductIdNumber)
                .ToList();

                if (!products.Any())
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow("Det finns inga produkter i lager");
                    Console.ResetColor();

                    Console.WriteLine();
                    ValidatedConsoleInput.PauseCentered();
                    return;
                }

                string productHeader =
                    $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";
                CenterConsoleOutput.CenterTextToWindow(productHeader);
                CenterConsoleOutput.CenterTextToWindow(new string('-', productHeader.Length));

                foreach (var product in products)
                {
                    CenterConsoleOutput.CenterTextToWindow(
                        $"{product.ProductIdNumber,-20} {product.ProductName,-20} {product.ProductPrice,-20} {product.ProductPriceType,-20}"
                    );
                }

                Console.WriteLine();
                ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");
            }
        }
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

                IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
                ISaveProductToFile writer = new SaveProductToFile();

                var products = reader.ReadAll();

                int newProductId = products.Any()
                    ? products.Max(p => p.ProductIdNumber) + 1
                    : 1;


                decimal productPriceDecimalInput =
                    decimal.Parse(productPriceInput.Replace(',', '.'),
                    CultureInfo.InvariantCulture);

                products.Add(new ProductModel(newProductId, productNameInput, productPriceDecimalInput, productPriceTypeInput));
                writer.SaveAll(products);


                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;

                // Rubrik
                CenterConsoleOutput.CenterTextToWindow("== Ny produkt sparad ==");
                Console.WriteLine();

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
        public class CreateNewMember
        {
            public void Run()
            {
                string memberHeader = "== Registrera ny medlem ==";
                string memberFirstNamePrompt = "Förnamn: ";
                string memberLastNamePrompt = "Efternamn: ";

                string memberFirstNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    memberHeader,
                    memberFirstNamePrompt,
                    ValidateMemberFirstName
                );

                string memberLastNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    memberHeader,
                    memberLastNamePrompt,
                    ValidateMemberLastName,
                    clearConsoleEachAttempt: false
                );

                IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
                ISaveMemberToFile writer = new SaveMemberToFile();

                var members = reader.ReadAll();

                int newMemberIdNumber = members.Any()
                    ? members.Max(m => m.MemberIdNumber) + 1
                    : 1;

                members.Add(new MemberModel(newMemberIdNumber, memberFirstNameInput, memberLastNameInput));
                writer.SaveAll(members);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;

                CenterConsoleOutput.CenterTextToWindow("Ny medlem sparad:");
                Console.WriteLine();

                string memberHeaderRow = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                string memberDataRow = $"{newMemberIdNumber,-20}{memberFirstNameInput,-20}{memberLastNameInput,-20}";

                CenterConsoleOutput.CenterTextToWindow(memberHeaderRow);
                CenterConsoleOutput.CenterTextToWindow(new string('-', memberHeaderRow.Length));
                CenterConsoleOutput.CenterTextToWindow(memberDataRow);

                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");


                var afterSaveMemberMenu = new ConsoleOptionsArrow();
                var afterSaveMemberOptions = new[]
                {
                "Registrera ny medlem",
                "Tillbaka till medlemssidan"
            };

                int choice = afterSaveMemberMenu.ShowArrow("Välj:", afterSaveMemberOptions);
                if (choice == 0)
                {
                    Run();
                    return;
                }
                return;
            }

            private static void ValidateMemberFirstName(string memberFirstNameInput)
            {
                if (string.IsNullOrWhiteSpace(memberFirstNameInput))
                    throw new ArgumentException("Ogiltigt förnamn: får inte vara tomt.");

                if (memberFirstNameInput.Any(char.IsDigit))
                    throw new ArgumentException("Ogiltigt förnamn: får inte innehålla siffror.");
            }

            private static void ValidateMemberLastName(string memberLastNameInput)
            {
                if (string.IsNullOrWhiteSpace(memberLastNameInput))
                    throw new ArgumentException("Ogiltigt efternamn: får inte vara tomt.");

                if (memberLastNameInput.Any(char.IsDigit))
                    throw new ArgumentException("Ogiltigt efternamn: får inte innehålla siffror.");
            }
        }
        public class DeleteMember
        {
            public void Run()
            {
                IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
                ISaveMemberToFile writer = new SaveMemberToFile();
                ISearchMember finder = new MemberSearch(reader);

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Avsluta medlemsskap ==");

                    Console.WriteLine();

                    string queryInput = UserInputPlacer.ReadCenteredText("Sök på medlemsnummer eller namn: ");
                    var searchMemberResult = finder.Search(queryInput);

                    if (searchMemberResult.Count == 0)
                    {
                        var arrowNoResult = new ConsoleOptionsArrow();
                        var noResultOptions = new[]
                        {
                        "Ny sökning",
                        "Tillbaka till medlemssidan"
                    };

                        int choice = arrowNoResult.ShowArrow("Välj:", noResultOptions, renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Avsluta medlemskap ==");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Medlemmen du söker finns inte i systemet.");
                            Console.ResetColor();
                            Console.WriteLine();
                        });

                        if (choice == 0)
                            continue;

                        return;
                    }

                    var selectedMember = searchMemberResult.Count == 1
                    ? searchMemberResult[0]
                    : SelectMember(searchMemberResult);

                    int memberId = selectedMember.MemberIdNumber;
                    string memberFirstName = selectedMember.MemberFirstName;
                    string memberLastName = selectedMember.MemberLastName;

                    while (true)
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Avsluta medlemsskap ==");
                        Console.WriteLine();
                        Console.WriteLine();

                        CenterConsoleOutput.CenterTextToWindow("Vald medlem:");
                        Console.WriteLine();

                        string infoHeader = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string infoRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(infoHeader);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                        CenterConsoleOutput.CenterTextToWindow(infoRow);

                        Console.WriteLine();

                        var arrowConfirm = new ConsoleOptionsArrow();
                        var confirmOptions = new[]
                        {
                        "Ja, radera medlem",
                        "Nej, tillbaka"
                    };

                        int deleteChoice = arrowConfirm.ShowArrow("Är du säker?", confirmOptions, renderAboveOptions: () =>
                        {
                            CenterConsoleOutput.CenterTextToWindow("Avsluta medelmskap:");
                            Console.WriteLine();

                            string infoHeader = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                            string infoRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                            CenterConsoleOutput.CenterTextToWindow(infoHeader);
                            CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                            CenterConsoleOutput.CenterTextToWindow(infoRow);

                            Console.WriteLine();
                        });

                        if (deleteChoice != 0)
                        {
                            return;
                        }

                        var members = reader.ReadAll();
                        int removed = members.RemoveAll(m => m.MemberIdNumber == memberId);

                        if (removed == 0)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Kunde inte radera: medlem hittades inte längre i listan.");
                            Console.ResetColor();
                            ValidatedConsoleInput.PauseCentered();
                            return;
                        }

                        writer.SaveAll(members);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        CenterConsoleOutput.CenterTextToWindow("Medlem raderad");
                        Console.ResetColor();

                        Console.WriteLine();

                        var afterDeleteMemberMenu = new ConsoleOptionsArrow();
                        var afterDeleteMemberOptions = new[]
                        {
                        "Radera en till medlem",
                        "Tillbaka till medlemssidan"
                    };

                        int afterDeleteMemberChoice = afterDeleteMemberMenu.ShowArrow("Välj:", afterDeleteMemberOptions);
                        if (afterDeleteMemberChoice == 0)
                            continue;

                        return;
                    }
                }
            }

            private static IMemberModel SelectMember(List<IMemberModel> members)
            {
                var memberDisplay = members
                    .Select(m => $"{m.MemberIdNumber,-6} {m.MemberFullName}")
                    .ToArray();

                var arrow = new ConsoleOptionsArrow();
                int index = arrow.ShowArrow("Välj medlem:", memberDisplay);
                return members[index];
            }
        }
        public class ListAllMembers
        {
            public void Run()
            {
                Console.Clear();

                CenterConsoleOutput.CenterTextToWindow("== Alla medlemmar ==");
                Console.WriteLine();

                IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
                var members = reader.ReadAll()
                    .OrderBy(m => m.MemberIdNumber)
                    .ToList();

                if (!members.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow("Inga medlemmar finns registrerade.");
                    Console.ResetColor();

                    Console.WriteLine();
                    ValidatedConsoleInput.PauseCentered();
                    return;
                }

                string memberHeader =
                    $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                CenterConsoleOutput.CenterTextToWindow(memberHeader);
                CenterConsoleOutput.CenterTextToWindow(new string('-', memberHeader.Length));

                foreach (var member in members)
                {
                    string row =
                        $"{member.MemberIdNumber,-20}{member.MemberFirstName,-20}{member.MemberLastName,-20}";
                    CenterConsoleOutput.CenterTextToWindow(row);
                }

                Console.WriteLine();
                ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");
            }
        }
        public class UpdateMember
        {
            public void Run()
            {
                IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
                ISaveMemberToFile writer = new SaveMemberToFile();
                ISearchMember finder = new MemberSearch(reader);


                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlemsinformation ==");

                    Console.WriteLine();

                    string queryInput = UserInputPlacer.ReadCenteredText("Sök på medlemsnummer eller namn: ");
                    var searchMemberResult = finder.Search(queryInput);


                    if (searchMemberResult.Count == 0)
                    {
                        var arrowNoResult = new ConsoleOptionsArrow();
                        var noResultOptions = new[]
                        {
                        "Ny sökning",
                        "Tillbaka till medlemssidan"
                    };

                        int choice = arrowNoResult.ShowArrow("Välj:", noResultOptions, renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlemsinformation ==");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Medlemmen du söker finns inte i systemet.");
                            Console.ResetColor();
                            Console.WriteLine();
                        });

                        if (choice == 0)
                            continue;

                        return;
                    }

                    var selectedMember = searchMemberResult.Count == 1
                    ? searchMemberResult[0]
                    : SelectMember(searchMemberResult);

                    int memberId = selectedMember.MemberIdNumber;
                    string memberFirstName = selectedMember.MemberFirstName;
                    string memberLastName = selectedMember.MemberLastName;

                    while (true)
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlem ==");
                        Console.WriteLine();
                        Console.WriteLine();

                        CenterConsoleOutput.CenterTextToWindow("Vald medlem:");
                        Console.WriteLine();

                        string infoHeader = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string infoRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(infoHeader);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                        CenterConsoleOutput.CenterTextToWindow(infoRow);

                        Console.WriteLine();

                        var arrowEdit = new ConsoleOptionsArrow();
                        var editOptions = new[]
                        {
                        "Ändra förnamn",
                        "Ändra efternamn",
                        "Spara\n",
                        "Avbryt"
                    };

                        int editChoice = arrowEdit.ShowArrow("Välj vad du vill ändra:", editOptions, renderAboveOptions: () =>
                        {
                            CenterConsoleOutput.CenterTextToWindow("Vald medlem:");
                            Console.WriteLine();

                            string infoHeader = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                            string infoRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                            CenterConsoleOutput.CenterTextToWindow(infoHeader);
                            CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                            CenterConsoleOutput.CenterTextToWindow(infoRow);

                            Console.WriteLine();
                        });

                        if (editChoice == 0)
                        {
                            memberFirstName = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera medlem ==\n",
                                "Nytt förnamn: ",
                                ValidateMemberFirstName
                            );
                        }
                        else if (editChoice == 1)
                        {
                            memberLastName = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera medlem ==\n",
                                "Nytt efternamn: ",
                                ValidateMemberLastName
                            );
                        }
                        else if (editChoice == 2)
                        {
                            var members = reader.ReadAll();
                            int index = members.FindIndex(m => m.MemberIdNumber == memberId);

                            if (index < 0)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                CenterConsoleOutput.CenterTextToWindow("Kunde inte spara: medlem hittades inte längre i listan.");
                                Console.ResetColor();
                                ValidatedConsoleInput.PauseCentered();
                                return;
                            }

                            members[index] = new MemberModel(memberId, memberFirstName, memberLastName);
                            writer.SaveAll(members);

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;

                            CenterConsoleOutput.CenterTextToWindow("== Medlemsinformation uppdaterad ==");
                            Console.WriteLine();

                            string headerRow = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                            string dataRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                            CenterConsoleOutput.CenterTextToWindow(headerRow);
                            CenterConsoleOutput.CenterTextToWindow(new string('-', headerRow.Length));
                            CenterConsoleOutput.CenterTextToWindow(dataRow);

                            Console.ResetColor();
                            ValidatedConsoleInput.PauseCentered();

                            var afterSaveMemberMenue = new ConsoleOptionsArrow();
                            var afterSaveMemberOptions = new[]
                            {
                            "Uppdatera en till medlem",
                            "Tillbaka till medlemssidan"
                        };

                            int afterChoice = afterSaveMemberMenue.ShowArrow("Välj:", afterSaveMemberOptions);
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

            private static void ValidateMemberFirstName(string memberFirstNameInput)
            {
                if (string.IsNullOrWhiteSpace(memberFirstNameInput))
                    throw new ArgumentException("Ogiltigt förnamn: får inte vara tomt.");

                if (memberFirstNameInput.Any(char.IsDigit))
                    throw new ArgumentException("Ogiltigt förnamn: får inte innehålla siffror.");
            }

            private static void ValidateMemberLastName(string memberLastNameInput)
            {
                if (string.IsNullOrWhiteSpace(memberLastNameInput))
                    throw new ArgumentException("Ogiltigt efternamn: får inte vara tomt.");

                if (memberLastNameInput.Any(char.IsDigit))
                    throw new ArgumentException("Ogiltigt efternamn: får inte innehålla siffror.");
            }
            private static IMemberModel SelectMember(List<IMemberModel> members)
            {
                var memberDisplay = members
                        .Select(m => $"{m.MemberIdNumber,-6} {m.MemberFullName}")
                        .ToArray();

                var arrow = new ConsoleOptionsArrow();
                int index = arrow.ShowArrow("Välj Medlem:", memberDisplay);

                return members[index];
            }
        }
        public class CreateNewCampaign
        {
            public void Run()
            {
                string campaignHeader = "== Skapa kampanj ==";

                string campaignNamePrompt = "Kampanjnamn: ";
                string campaignStartDatePrompt = "Startdatum (yyyy-MM-dd): ";
                string campaignEndDatePrompt = "Slutdatum (yyyy-MM-dd): ";
                string productIdNumbersPrompt = "Produktnummer (om flera - separera med kommatecken): ";
                string percentOffPrompt = "Rabattprocent (1-100): ";

                string campaignNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    campaignHeader, campaignNamePrompt, ValidateCampaignName);

                string campaignStartDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    campaignHeader, campaignStartDatePrompt, ValidateCampaignDate, clearConsoleEachAttempt: false);

                DateTime campaignStartDate = DateTime.ParseExact(campaignStartDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    campaignHeader, campaignEndDatePrompt, ValidateCampaignDate, clearConsoleEachAttempt: false);

                DateTime campaignEndDate = DateTime.ParseExact(campaignEndDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                while (campaignEndDate < campaignStartDate)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow("Slutdatum kan inte vara före startdatum.");
                    Console.ResetColor();

                    campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                        campaignHeader, campaignEndDatePrompt, ValidateCampaignDate, clearConsoleEachAttempt: false);

                    campaignEndDate = DateTime.ParseExact(campaignEndDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                string productIdNumbersInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    campaignHeader, productIdNumbersPrompt, ValidateProductIdNumbers, clearConsoleEachAttempt: false);

                List<int> productIdNumbers = ParseProductIdNumbers(productIdNumbersInput);

                string percentOffInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                                campaignHeader, percentOffPrompt, ValidatePercent, clearConsoleEachAttempt: false);

                decimal percentOff = ParseDecimalInvariant(percentOffInput);

                ICampaignModel newCampaign = new PercentOffCampaign(
                    campaignNameInput,
                    campaignStartDate,
                    campaignEndDate,
                    productIdNumbers,
                    percentOff);

                IReadAllCampaignsFromFile reader = new ReadAllCampaignsFromFile();
                ISaveCampaignToFile writer = new SaveCampaignToFile();

                var campaigns = reader.ReadAll();
                campaigns.Add(newCampaign);
                writer.SaveAll(campaigns);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleOutput.CenterTextToWindow("== Ny kampanj skapad ==");
                Console.WriteLine();

                CenterConsoleOutput.CenterTextToWindow($"Kampanj: {newCampaign.CampaignName} ({newCampaign.TypeOfCampaign})");

                CenterConsoleOutput.CenterTextToWindow(
                    $"Gäller: {campaignStartDate:yyyy-MM-dd} till {campaignEndDate:yyyy-MM-dd}"
                );

                Console.WriteLine();

                var productReader = new ReadAllProductsFromFile();
                var allProducts = productReader.ReadAll();

                var campaignProducts = allProducts
                    .Where(p => productIdNumbers.Contains(p.ProductIdNumber))
                    .OrderBy(p => p.ProductIdNumber)
                    .ToList();

                var percentCampaign = newCampaign as PercentOffCampaign;

                if (campaignProducts.Any())
                {
                    CenterConsoleOutput.CenterTextToWindow("Produkter i kampanjen:");
                    Console.WriteLine();

                    int tableWidth = 60;
                    int leftPadding = (Console.WindowWidth - tableWidth) / 2;
                    string indent = new string(' ', Math.Max(0, leftPadding));

                    Console.WriteLine(
                        indent + $"{"Produktnummer",-15}{"Produktnamn",-30}{"Rabattprocent",-15}"
                    );

                    Console.WriteLine(indent + new string('-', 60));

                    foreach (var product in campaignProducts)
                    {
                        Console.WriteLine(indent + $"{product.ProductIdNumber,-15}{product.ProductName,-30}{percentOff + " %",-15}");
                    }
                }

                else
                {
                    CenterConsoleOutput.CenterTextToWindow("OBS: Inga matchande produkter hittades för angivna produktnummer.");
                }

                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");

                var afterSavedCampaignMenu = new ConsoleOptionsArrow();
                var afterSavedCampaignOptions = new[]
                {
                "Skapa ny kampanj",
                "Tillbaka till kampanjmenyn"
            };

                int choice = afterSavedCampaignMenu.ShowArrow("Välj:", afterSavedCampaignOptions);

                if (choice == 0)
                {
                    Run();
                    return;
                }
            }

            private static void ValidateCampaignName(string campaignNameInput)
            {
                if (string.IsNullOrWhiteSpace(campaignNameInput))
                    throw new ArgumentException("Ogiltigt namn: får inte vara tomt.");
            }

            private static void ValidateCampaignDate(string campaignDateInput)
            {
                if (string.IsNullOrWhiteSpace(campaignDateInput))
                    throw new ArgumentException("Ogiltigt datum: får inte vara tomt.");

                if (!DateTime.TryParseExact(campaignDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out _))
                {
                    throw new ArgumentException("Fel format. Ex: 2026-03-03");
                }
            }

            private static void ValidateProductIdNumbers(string productIdNumbersInput)
            {
                if (string.IsNullOrWhiteSpace(productIdNumbersInput))
                    throw new ArgumentException("Du måste ange minst ett produktnummer.");

                var productIdNumbers = ParseProductIdNumbers(productIdNumbersInput);
                if (productIdNumbers.Count == 0)
                    throw new ArgumentException("Du måste ange minst ett giltigt produktnummer (ex: 1,2,3).");
            }

            private static void ValidatePercent(string precentOffInput)
            {
                decimal value = ParseDecimalInvariant(precentOffInput);
                if (value <= 0m || value > 100m)
                    throw new ArgumentException("Ogiltig procent: ange ett tal mellan 1 och 100.");
            }

            private static List<int> ParseProductIdNumbers(string productIdNumbersInput)
            {
                return (productIdNumbersInput ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => int.TryParse(p, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                    .Select(p => int.Parse(p, CultureInfo.InvariantCulture))
                    .Where(n => n > 0)
                    .Distinct()
                    .ToList();
            }

            private static decimal ParseDecimalInvariant(string percentOffInput)
            {
                if (string.IsNullOrWhiteSpace(percentOffInput))
                    throw new ArgumentException("Ogiltigt tal: får inte vara tomt.");

                string normalized = percentOffInput.Trim().Replace(',', '.');

                if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
                    throw new ArgumentException("Ogiltigt tal: ange ett numeriskt värde.");

                return value;
            }
        }
        public class DeleteCampaign
        {
            public void Run()
            {
                IReadAllCampaignsFromFile reader = new ReadAllCampaignsFromFile();
                ISaveCampaignToFile writer = new SaveCampaignToFile();
                ISearchCampaign finder = new CampaignSearch(reader);

                IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
                var productLookup = productReader.ReadAll()
                    .GroupBy(p => p.ProductIdNumber)
                    .ToDictionary(g => g.Key, g => g.First().ProductName);

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Ta bort kampanj ==");
                    Console.WriteLine();

                    string queryInput = UserInputPlacer.ReadCenteredText(
                        "Sök på kampanjnamn, datum eller berörda produkter: ");

                    var searchResult = finder.Search(queryInput);

                    if (searchResult.Count == 0)
                    {
                        var arrowNoResult = new ConsoleOptionsArrow();
                        var noResultOptions = new[]
                        {
                        "Ny sökning",
                        "Tillbaka till kampanjsidan"
                    };

                        int choice = arrowNoResult.ShowArrow("Välj:", noResultOptions, renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Ta bort kampanj ==");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Kampanjen du söker finns inte i systemet.");
                            Console.ResetColor();
                            Console.WriteLine();
                        });

                        if (choice == 0)
                            continue;

                        return;
                    }

                    var selectedCampaign = searchResult.Count == 1
                        ? searchResult[0]
                        : SelectCampaign(searchResult);

                    string campaignName = selectedCampaign.CampaignName;
                    DateTime startDate = selectedCampaign.CampaignStartDate;
                    DateTime endDate = selectedCampaign.CampaignEndDate;
                    var productIds = selectedCampaign.ProductIdNumbers?.ToList() ?? new List<int>();

                    decimal percentOff = selectedCampaign is PercentOffCampaign poc
                        ? poc.PercentOff
                        : 0m;

                    while (true)
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Ta bort kampanj ==");
                        Console.WriteLine();
                        Console.WriteLine();

                        RenderSelectedCampaign(
                              campaignName,
                              startDate,
                              endDate,
                              productIds,
                              percentOff,
                              productLookup);

                        var confirmMenu = new ConsoleOptionsArrow();
                        var confirmOptions = new[]
                        {
                        "Ja, radera kampanj",
                        "Nej, tillbaka"
                    };

                        int deleteChoice = confirmMenu.ShowArrow("Är du säker?",
                            confirmOptions,
                            renderAboveOptions: () =>
                            {
                                RenderSelectedCampaign(
                                    campaignName,
                                    startDate,
                                    endDate,
                                    productIds,
                                    percentOff,
                                    productLookup);
                            });

                        if (deleteChoice != 0)
                            return;

                        var campaigns = reader.ReadAll();

                        int removed = campaigns.RemoveAll(c =>
                            string.Equals(c.CampaignName, campaignName, StringComparison.Ordinal) &&
                            c.CampaignStartDate == startDate &&
                            c.CampaignEndDate == endDate &&
                            SameIdNumbers(c.ProductIdNumbers, productIds));

                        if (removed == 0)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow(
                                "Kunde inte radera: kampanjen hittades inte längre i listan.");
                            Console.ResetColor();
                            ValidatedConsoleInput.PauseCentered();
                            return;
                        }

                        writer.SaveAll(campaigns);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        CenterConsoleOutput.CenterTextToWindow("Kampanj raderad");
                        Console.ResetColor();
                        Console.WriteLine();

                        var afterDeleteMenu = new ConsoleOptionsArrow();
                        var afterDeleteOptions = new[]
                        {
                        "Radera en till kampanj",
                        "Tillbaka till kampanjsidan"
                    };

                        int afterChoice = afterDeleteMenu.ShowArrow("Välj:", afterDeleteOptions);
                        if (afterChoice == 0)
                            break;

                        return;
                    }
                }
            }

            private static void RenderSelectedCampaign(
                        string name,
                        DateTime start,
                        DateTime end,
                        List<int> productIdNumber,
                        decimal percentOff,
                        Dictionary<int, string> productLookup)
            {
                CenterConsoleOutput.CenterTextToWindow("Vald kampanj:");
                Console.WriteLine();

                string productsInline = string.Join(", ",
                    productIdNumber.OrderBy(id => id)
                              .Select(id =>
                                  productLookup.TryGetValue(id, out var productName)
                                      ? $"{id} {productName}"
                                      : $"{id} (okänd)"));

                string infoHeader =
                    $"{"Kampanjnamn",-20}{"Startdatum",-15}{"Slutdatum",-15}{"Produkter",-35}{"Rabatt",-10}";

                string infoRow =
                    $"{name,-20}{start,-15:yyyy-MM-dd}{end,-15:yyyy-MM-dd}{productsInline,-35}{percentOff.ToString("0.##", CultureInfo.InvariantCulture) + "%",-10}";

                CenterConsoleOutput.CenterTextToWindow(infoHeader);
                CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                CenterConsoleOutput.CenterTextToWindow(infoRow);
                Console.WriteLine();
            }

            private static bool SameIdNumbers(IReadOnlyList<int> a, List<int> b) =>
                (a == null && b == null) ||
                (a != null && b != null &&
                 a.Where(x => x > 0).Distinct().OrderBy(x => x)
                  .SequenceEqual(b.Where(x => x > 0).Distinct().OrderBy(x => x)));

            private static ICampaignModel SelectCampaign(List<ICampaignModel> campaigns)
            {
                var display = campaigns
                    .Select(c =>
                        $"{c.CampaignName} ({c.CampaignStartDate:yyyy-MM-dd} - {c.CampaignEndDate:yyyy-MM-dd}) " +
                        $"[{string.Join(",", c.ProductIdNumbers)}]")
                    .ToArray();

                var arrow = new ConsoleOptionsArrow();
                int index = arrow.ShowArrow("Välj kampanj:", display);
                return campaigns[index];
            }
        }
        public class ListAllCampaigns
        {
            public void Run()
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Alla kampanjer ==");
                Console.WriteLine();

                IReadAllCampaignsFromFile campaignReader = new ReadAllCampaignsFromFile();
                IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();

                var campaigns = campaignReader.ReadAll()
                    .OrderBy(c => c.CampaignStartDate)
                    .ThenBy(c => c.CampaignName)
                    .ToList();

                if (!campaigns.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow("Inga kampanjer finns registrerade.");
                    Console.ResetColor();
                    Console.WriteLine();
                    ValidatedConsoleInput.PauseCentered();
                    return;
                }

                var productLookup = productReader.ReadAll()
                    .GroupBy(p => p.ProductIdNumber)
                    .ToDictionary(g => g.Key, g => g.First().ProductName);

                string header =
                       $"{"Kampanjnamn",-20}{"Startdatum",-15}{"Slutdatum",-15}{"Produkter",-35}{"Rabatt",-10}";

                CenterConsoleOutput.CenterTextToWindow(header);
                CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));

                foreach (var campaign in campaigns)
                {
                    string productsText = string.Join(", ",
                        campaign.ProductIdNumbers
                            .OrderBy(id => id)
                            .Select(id =>
                                productLookup.TryGetValue(id, out var productName)
                                    ? $"{id} {productName}"
                                    : $"{id} (okänd)")
                    );

                    string discountText = campaign is PercentOffCampaign percentOffCampaign
                        ? percentOffCampaign.PercentOff.ToString("0.##", CultureInfo.InvariantCulture) + "%"
                        : "-";

                    string campaingListRows =
                        $"{campaign.CampaignName,-20}" +
                        $"{campaign.CampaignStartDate,-15:yyyy-MM-dd}" +
                        $"{campaign.CampaignEndDate,-15:yyyy-MM-dd}" +
                        $"{productsText,-35}" +
                        $"{discountText,-10}";

                    CenterConsoleOutput.CenterTextToWindow(campaingListRows);
                }

                Console.WriteLine();
                ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");
            }
        }
        public class UpdateCampaign
        {
            public void Run()
            {
                IReadAllCampaignsFromFile reader = new ReadAllCampaignsFromFile();
                ISaveCampaignToFile writer = new SaveCampaignToFile();
                ISearchCampaign finder = new CampaignSearch(reader);

                IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
                var productLookup = productReader.ReadAll()
                        .GroupBy(p => p.ProductIdNumber)
                        .ToDictionary(g => g.Key, g => g.First().ProductName);

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Uppdatera kampanj ==");
                    Console.WriteLine();

                    string queryInput = UserInputPlacer.ReadCenteredText("Sök på kampanjnamn, datum eller berörda produkter: ");
                    var searchCampaignResult = finder.Search(queryInput);

                    if (searchCampaignResult.Count == 0)
                    {
                        var arrowNoResult = new ConsoleOptionsArrow();
                        var noResultOptions = new[]
                        {
                        "Ny sökning",
                        "Tillbaka till kampanjsidan"
                    };

                        int choice = arrowNoResult.ShowArrow("Välj:", noResultOptions, renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Uppdatera kampanj ==");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Kampanjen du söker finns inte i systemet.");
                            Console.ResetColor();
                            Console.WriteLine();
                        });

                        if (choice == 0)
                            continue;

                        return;
                    }

                    var selectedCampaign = searchCampaignResult.Count == 1
                        ? searchCampaignResult[0]
                        : SelectCampaign(searchCampaignResult);

                    string originalName = selectedCampaign.CampaignName;
                    DateTime originalStart = selectedCampaign.CampaignStartDate;
                    DateTime originalEnd = selectedCampaign.CampaignEndDate;
                    var originalProductIds = selectedCampaign.ProductIdNumbers?.ToList() ?? new List<int>();

                    decimal originalPercentOff = selectedCampaign is PercentOffCampaign percentOffCampaign ? percentOffCampaign.PercentOff : 0m;

                    string campaignName = originalName;
                    DateTime campaignStartDate = originalStart;
                    DateTime campaignEndDate = originalEnd;
                    var productIdNumbers = originalProductIds.ToList();
                    decimal percentOff = originalPercentOff;

                    while (true)
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Uppdatera kampanj ==");
                        Console.WriteLine();
                        Console.WriteLine();

                        RenderSelectedCampaign(campaignName, campaignStartDate, campaignEndDate, productIdNumbers, percentOff, productLookup);

                        var arrowEdit = new ConsoleOptionsArrow();
                        var editOptions = new[]
                        {
                        "Ändra kampanjnamn",
                        "Ändra startdatum",
                        "Ändra slutdatum",
                        "Ändra produkter",
                        "Ändra rabattprocent",
                        "Spara",
                        "Avbryt"
                    };

                        int editChoice = arrowEdit.ShowArrow("Välj vad du vill ändra:", editOptions, renderAboveOptions: () =>
                        {
                            RenderSelectedCampaign(campaignName, campaignStartDate, campaignEndDate, productIdNumbers, percentOff, productLookup);
                        });
                        ;

                        if (editChoice == 0)
                        {
                            campaignName = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera kampanj ==",
                                "Kampanjnamn: ",
                                ValidateCampaignName);
                        }
                        else if (editChoice == 1)
                        {
                            string campaignStartDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera kampanj ==",
                                "Startdatum (yyyy-MM-dd): ",
                                ValidateCampaignDate);

                            campaignStartDate = DateTime.ParseExact(campaignStartDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            if (campaignEndDate < campaignStartDate)
                                campaignEndDate = campaignStartDate;
                        }
                        else if (editChoice == 2)
                        {
                            string campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera kampanj ==",
                                "Slutdatum (yyyy-MM-dd): ",
                                ValidateCampaignDate);

                            campaignEndDate = DateTime.ParseExact(campaignEndDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            while (campaignEndDate < campaignStartDate)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                CenterConsoleOutput.CenterTextToWindow("Slutdatum kan inte vara före startdatum.");
                                Console.ResetColor();

                                campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                                    "== Uppdatera kampanj ==",
                                    "Slutdatum (yyyy-MM-dd): ",
                                    ValidateCampaignDate,
                                    clearConsoleEachAttempt: false);

                                campaignEndDate = DateTime.ParseExact(campaignEndDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            }
                        }
                        else if (editChoice == 3)
                        {
                            string productIdNumbersInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera kampanj ==",
                                "Produktnummer (1,2,3): ",
                                ValidateProductIdNumbers);

                            productIdNumbers = ParseProductIdNumbers(productIdNumbersInput);
                        }
                        else if (editChoice == 4)
                        {
                            string percentOffInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera kampanj ==",
                                "Rabattprocent (1-100): ",
                                ValidatePercent);

                            percentOff = ParseDecimalInvariant(percentOffInput);
                        }
                        else if (editChoice == 5)
                        {
                            var campaigns = reader.ReadAll();

                            int index = campaigns.FindIndex(c =>
                                string.Equals(c.CampaignName, originalName, StringComparison.Ordinal) &&
                                c.CampaignStartDate == originalStart &&
                                c.CampaignEndDate == originalEnd &&
                                SameIdNumbers(c.ProductIdNumbers, originalProductIds) &&
                                (!(c is PercentOffCampaign existingPoc) || existingPoc.PercentOff == originalPercentOff)
                            );

                            if (index < 0)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                CenterConsoleOutput.CenterTextToWindow("Kunde inte spara: kampanjen hittades inte längre i listan.");
                                Console.ResetColor();
                                ValidatedConsoleInput.PauseCentered();
                                return;
                            }

                            campaigns[index] = new PercentOffCampaign(
                            campaignName,
                            campaignStartDate,
                            campaignEndDate,
                            productIdNumbers,
                            percentOff
                            );

                            writer.SaveAll(campaigns);

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            CenterConsoleOutput.CenterTextToWindow("Kampanj uppdaterad!");
                            Console.ResetColor();
                            Console.WriteLine();

                            CenterConsoleOutput.CenterTextToWindow($"{campaignName} ({campaignStartDate:yyyy-MM-dd} - {campaignEndDate:yyyy-MM-dd})");
                            CenterConsoleOutput.CenterTextToWindow($"Produkter: {string.Join(",", productIdNumbers)}");
                            CenterConsoleOutput.CenterTextToWindow($"Rabatt: {percentOff.ToString(CultureInfo.InvariantCulture)}%");
                            ValidatedConsoleInput.PauseCentered();

                            var afterSaveMenu = new ConsoleOptionsArrow();
                            var afterSaveOptions = new[]
                            {
                            "Uppdatera en till kampanj",
                            "Tillbaka till kampanjsidan"
                        };

                            int afterChoice = afterSaveMenu.ShowArrow("Välj:", afterSaveOptions);
                            if (afterChoice == 0) break;
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }

            private static void RenderSelectedCampaign(
                string campaignName,
                DateTime start,
                DateTime end,
                List<int> productIdNumbers,
                decimal percentOff,
                Dictionary<int, string> productLookup)

            {
                CenterConsoleOutput.CenterTextToWindow("Vald kampanj:");
                Console.WriteLine();

                string productsInline = BuildProductsInline(productIdNumbers, productLookup);

                string infoHeader = $"{"Kampanjnamn",-20}{"Startdatum",-15}{"Slutdatum",-15}";

                string infoRow = $"{campaignName,-20}{start,-15:yyyy-MM-dd}{end,-15:yyyy-MM-dd}";

                CenterConsoleOutput.CenterTextToWindow(infoHeader);
                CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                CenterConsoleOutput.CenterTextToWindow(infoRow);
                Console.WriteLine();

                CenterConsoleOutput.CenterTextToWindow("Produkter i kampanjen:");
                Console.WriteLine();

                string infoHeaderTwo = $"{"ProduktNummer",-12}{"Produktnamn",-30}{"Rabatt",-10}";
                CenterConsoleOutput.CenterTextToWindow(infoHeaderTwo);
                CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeaderTwo.Length));

                string percentOffText = percentOff.ToString("0.##", CultureInfo.InvariantCulture) + "%";

                foreach (var id in productIdNumbers.OrderBy(x => x))
                {
                    string productName = productLookup.TryGetValue(id, out var n) ? n : "(okänd produkt)";
                    CenterConsoleOutput.CenterTextToWindow($"{id,-12}{productName,-30}{percentOffText,-10}");
                }

                Console.WriteLine();
            }

            private static string BuildProductsInline(List<int> productIdNumbers, Dictionary<int, string> productLookup)
            {
                if (productIdNumbers == null || productIdNumbers.Count == 0) return "";

                return string.Join(", ",
                    productIdNumbers
                        .Where(id => id > 0)
                        .Distinct()
                        .OrderBy(id => id)
                        .Select(id =>
                        {
                            string name = productLookup.TryGetValue(id, out var n) ? n : "okänd";
                            return $"{id} {name}";
                        })
                );
            }

            private static void ValidateCampaignName(string campaignNameInput)
            {
                if (string.IsNullOrWhiteSpace(campaignNameInput))
                    throw new ArgumentException("Ogiltigt kampanjnamn: får inte vara tomt.");
            }

            private static void ValidateCampaignDate(string campaignDateInput)
            {
                if (string.IsNullOrWhiteSpace(campaignDateInput))
                    throw new ArgumentException("Ogiltigt datum: får inte vara tomt.");

                if (!DateTime.TryParseExact(
                    campaignDateInput.Trim(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _))
                {
                    throw new ArgumentException("Fel format. Ex: 2026-03-03");
                }
            }

            private static void ValidateProductIdNumbers(string productIdNumbersInput)
            {
                if (string.IsNullOrWhiteSpace(productIdNumbersInput))
                    throw new ArgumentException("Du måste ange minst ett produktnummer.");

                var parsed = ParseProductIdNumbers(productIdNumbersInput);
                if (parsed.Count == 0)
                    throw new ArgumentException("Du måste ange minst ett giltigt produktnummer (ex: 1,2,3).");
            }

            private static void ValidatePercent(string percentOffInput)
            {
                decimal percentValue = ParseDecimalInvariant(percentOffInput);
                if (percentValue <= 0m || percentValue > 100m)
                    throw new ArgumentException("Ogiltig procent: ange ett tal mellan 1 och 100.");
            }

            private static List<int> ParseProductIdNumbers(string productIdNumbersInput)
            {
                return (productIdNumbersInput ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => int.TryParse(p, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                    .Select(p => int.Parse(p, CultureInfo.InvariantCulture))
                    .Where(n => n > 0)
                    .Distinct()
                    .ToList();
            }

            private static decimal ParseDecimalInvariant(string userInput)
            {
                if (string.IsNullOrWhiteSpace(userInput))
                    throw new ArgumentException("Ogiltigt tal: får inte vara tomt.");

                string normalized = userInput.Trim().Replace(',', '.');
                if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
                    throw new ArgumentException("Ogiltigt tal: ange ett numeriskt värde.");

                return value;
            }

            private static bool SameIdNumbers(IReadOnlyList<int> a, List<int> b) =>
                (a == null && b == null) ||
                (a != null && b != null &&
                 a.Where(x => x > 0).Distinct().OrderBy(x => x)
                  .SequenceEqual(b.Where(x => x > 0).Distinct().OrderBy(x => x)));

            private static ICampaignModel SelectCampaign(List<ICampaignModel> campaigns)
            {
                var campaignDisplay = campaigns
                    .Select(c =>
                        $"{c.CampaignName} ({c.CampaignStartDate:yyyy-MM-dd} - {c.CampaignEndDate:yyyy-MM-dd}) [{string.Join(",", c.ProductIdNumbers)}]")
                    .ToArray();

                var arrow = new ConsoleOptionsArrow();
                int index = arrow.ShowArrow("Välj kampanj:", campaignDisplay);
                return campaigns[index];
            }
        }

        
        public static class ReceiptPrinter
        {
            public static void PrintDetailed(IReceiptModel receipt)
            {
                CenterConsoleOutput.CenterTextToWindow(new string('=', 41));

                CenterConsoleOutput.CenterTextToWindow($"KVITTO #{receipt.ReceiptNumber}");

                CenterConsoleOutput.CenterTextToWindow(receipt.ReceiptCreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

                if (receipt.MemberIdNumber != 0)
                    CenterConsoleOutput.CenterTextToWindow($"Medlemsnummer: {receipt.MemberIdNumber}");

                CenterConsoleOutput.CenterTextToWindow(new string('-', 41));

                if (receipt.ReceiptRows != null)
                {
                    foreach (var row in receipt.ReceiptRows)
                    {
                        if (row.ReceiptProductQuantity > 0)
                        {
                            var unitPrice = row.ReceiptProductAmount / row.ReceiptProductQuantity;

                            CenterConsoleOutput.CenterTextToWindow(
                                $"{row.ReceiptProductText} {row.ReceiptProductQuantity}st*{unitPrice.ToString("0.00", CultureInfo.InvariantCulture)} " +
                                $"{row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}"
                            );
                        }
                        else
                        {
                            CenterConsoleOutput.CenterTextToWindow(
                                $"{row.ReceiptProductText} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}"
                            );
                        }
                    }
                }

                CenterConsoleOutput.CenterTextToWindow(new string('-', 41));

                CenterConsoleOutput.CenterTextToWindow($"Totalt antal varor: {receipt.TotalItems}");
                CenterConsoleOutput.CenterTextToWindow($"TOTALT: {receipt.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");

                CenterConsoleOutput.CenterTextToWindow(new string('=', 41));
                Console.WriteLine();
            }
        }

        
        
        public class ReceiptSearch : IReceiptSearch
        {
            private readonly IReadAllReceiptsFromFile _reader;

            public ReceiptSearch(IReadAllReceiptsFromFile reader)
            {
                _reader = reader;
            }

            public List<IReceiptModel> Search(string searchText)
            {
                var all = _reader.ReadAll();

                if (string.IsNullOrWhiteSpace(searchText))
                    return all;

                string userReceiptQuery = searchText.Trim();

                return all.Where(r =>
                       r.ReceiptNumber.ToString().Contains(userReceiptQuery)
                    || r.MemberIdNumber.ToString().Contains(userReceiptQuery)
                ).ToList();
            }
        }
              
        
        public interface ISaveReceiptToFile
        {
            void SaveAll(List<IReceiptModel> receipts);
        }

        public interface IReceiptSearch
        {
            List<IReceiptModel> Search(string searchReceiptText);
        }

        public interface IReceiptModel
        {
            int ReceiptNumber { get; }
            int MemberIdNumber { get; }
            DateTime ReceiptCreatedAt { get; }
            IReadOnlyList<ReceiptRowModel> ReceiptRows { get; }
            int TotalItems { get; }
            decimal TotalAmount { get; }
        }

        public interface IReadAllReceiptsFromFile
        {
            List<IReceiptModel> ReadAll();
        }
        public class SaveReceiptToFile : ISaveReceiptToFile
        {
            private const int Width = 41;
            private static readonly string equalsDivider = new string('=', Width);
            private static readonly string Dash = new string('-', Width);

            public void SaveAll(List<IReceiptModel> receipts)
            {
                receipts ??= new List<IReceiptModel>();

                var today = DateTime.Now.Date;

                var onlyTodaysReceipts = receipts
                    .Where(r => r.ReceiptCreatedAt.Date == today)
                    .GroupBy(r => r.ReceiptNumber)
                    .Select(g => g.OrderByDescending(x => x.ReceiptCreatedAt).First())
                    .OrderBy(r => r.ReceiptNumber)
                    .ToList();

                var receiptPath = ReceiptFilePath.TodayReceiptPath;
                var receiptDirectory = Path.GetDirectoryName(receiptPath);
                if (!string.IsNullOrWhiteSpace(receiptDirectory) && !Directory.Exists(receiptDirectory))
                    Directory.CreateDirectory(receiptDirectory);

                using var writer = new StreamWriter(receiptPath, append: false, Encoding.UTF8);

                foreach (var receipt in onlyTodaysReceipts)
                {
                    WriteReceipt(writer, receipt);
                    writer.WriteLine();
                }
            }

            private static void WriteReceipt(StreamWriter writer, IReceiptModel receipt)
            {
                writer.WriteLine(equalsDivider);
                writer.WriteLine($"KVITTO #{receipt.ReceiptNumber}");
                writer.WriteLine(receipt.ReceiptCreatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

                if (receipt.MemberIdNumber != 0)
                    writer.WriteLine($"Medlemsnummer: {receipt.MemberIdNumber}");

                writer.WriteLine(Dash);

                if (receipt.ReceiptRows != null && receipt.ReceiptRows.Any())
                {
                    foreach (var row in receipt.ReceiptRows)
                    {
                        if (row.ReceiptProductQuantity > 0)
                        {
                            var unitPrice = row.ReceiptProductAmount / row.ReceiptProductQuantity;

                            writer.WriteLine(
                                $"{row.ReceiptProductText} {row.ReceiptProductQuantity}st*{unitPrice.ToString("0.00", CultureInfo.InvariantCulture)} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}");
                        }
                        else
                        {
                            writer.WriteLine(
                                $"{row.ReceiptProductText} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}");
                        }
                    }
                }

                writer.WriteLine(Dash);
                writer.WriteLine($"Totalt antal varor: {receipt.TotalItems}");
                writer.WriteLine($"TOTALT: {receipt.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");
                writer.WriteLine(equalsDivider);
            }
        }
        internal static class ReceiptFilePath
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

            public static string TodayReceiptPath =>
                Path.Combine(TextFilesDir, $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

            public static string ReceiptDraftPath =>
                Path.Combine(TextFilesDir, "receiptDraft.txt");
        }
        public class ReadAllReceiptsFromFile : IReadAllReceiptsFromFile
        {
            public List<IReceiptModel> ReadAll()
            {
                var baseDir = AppContext.BaseDirectory;
                var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
                var textFilesDir = Path.Combine(projectDir, "TextFiles");

                if (!Directory.Exists(textFilesDir))
                    return new List<IReceiptModel>();

                var allReceipts = new List<IReceiptModel>();

                foreach (var file in Directory.EnumerateFiles(textFilesDir, "RECEIPT_*.txt"))
                {
                    allReceipts.AddRange(ReadAllFromPath(file));
                }
                return allReceipts;
            }

            public List<IReceiptModel> ReadAllFromPath(string path)
            {
                if (!File.Exists(path))
                    return new List<IReceiptModel>();

                return ReadReceiptPresentationFormat(path);
            }

            private static List<IReceiptModel> ReadReceiptPresentationFormat(string path)
            {
                var lines = File.ReadAllLines(path);
                var byNumber = new Dictionary<int, IReceiptModel>();

                int t = 0;
                while (t < lines.Length)
                {
                    var line = (lines[t] ?? "").Trim();

                    if (!line.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                    {
                        t++;
                        continue;
                    }

                    if (!TryParseReceiptNumber(line, out int receiptNumber))
                    {
                        t++;
                        continue;
                    }

                    t++;

                    if (!TryGetNextNonEmpty(lines, ref t, out var dateTextLine))
                        break;

                    if (!DateTime.TryParseExact(
                            dateTextLine,
                            "yyyy-MM-dd HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var createdAt))
                    {
                        SkipToNextReceipt(lines, ref t);
                        continue;
                    }

                    t++;

                    if (!TryGetNextNonEmpty(lines, ref t, out var memberLine))
                        break;

                    if (!TryParseMemberLine(memberLine, out int memberIdNumber))
                    {
                        SkipToNextReceipt(lines, ref t);
                        continue;
                    }

                    t++;

                    while (t < lines.Length && (IsSeparator(lines[t]) || string.IsNullOrWhiteSpace(lines[t])))
                        t++;

                    var rowModels = new List<ReceiptRowModel>();

                    while (t < lines.Length)
                    {
                        var rowLine = (lines[t] ?? "").Trim();

                        if (string.IsNullOrWhiteSpace((string)rowLine) || IsSeparator((string)rowLine))
                        {
                            t++;
                            continue;
                        }

                        if (rowLine.StartsWith("Totalt antal varor:", StringComparison.OrdinalIgnoreCase) ||
                            rowLine.StartsWith("TOTALT:", StringComparison.OrdinalIgnoreCase) ||
                            rowLine.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        if (TryParseRow(rowLine, out var rowText, out var rowQuantity, out var rowAmount))
                            rowModels.Add(new ReceiptRowModel(rowText, rowQuantity, rowAmount));

                        t++;
                    }

                    int totalItems = 0;
                    decimal totalAmount = 0m;

                    while (t < lines.Length)
                    {
                        var totalsLine = (lines[t] ?? "").Trim();

                        if (string.IsNullOrWhiteSpace(totalsLine) || IsSeparator(totalsLine))
                        {
                            t++;
                            continue;
                        }

                        if (totalsLine.StartsWith("Totalt antal varor:", StringComparison.OrdinalIgnoreCase))
                        {
                            var part = totalsLine.Substring("Totalt antal varor:".Length).Trim();
                            int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out totalItems);
                            t++;
                            continue;
                        }

                        if (totalsLine.StartsWith("TOTALT:", StringComparison.OrdinalIgnoreCase))
                        {
                            var part = totalsLine.Substring("TOTALT:".Length).Trim();
                            part = part.Replace("SEK", "", StringComparison.OrdinalIgnoreCase).Trim();
                            decimal.TryParse(part, NumberStyles.Number, CultureInfo.InvariantCulture, out totalAmount);
                            t++;
                            continue;
                        }

                        if (totalsLine.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                            break;

                        t++;
                    }

                    var receipt = new ReceiptModel(
                        receiptNumber,
                        memberIdNumber,
                        createdAt,
                        rowModels,
                        totalItems,
                        totalAmount);

                    if (byNumber.TryGetValue(receiptNumber, out var receiptExists))
                    {
                        if (receipt.ReceiptCreatedAt >= receiptExists.ReceiptCreatedAt)
                            byNumber[receiptNumber] = receipt;
                    }
                    else
                    {
                        byNumber[receiptNumber] = receipt;
                    }
                }

                return byNumber.Values
                    .OrderBy(r => r.ReceiptNumber)
                    .ToList();
            }

            private static void SkipToNextReceipt(string[] lines, ref int i)
            {
                while (i < lines.Length && !(lines[i] ?? "").Trim().StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                    i++;
            }

            private static bool TryParseMemberLine(string memberLine, out int memberIdNumber)
            {
                memberIdNumber = 0;

                var m = (memberLine ?? "").Trim();

                const string member = "Medlemsnummer:";

                if (!m.StartsWith(member, StringComparison.OrdinalIgnoreCase))
                    return false;

                var part = m.Substring(member.Length).Trim();
                return int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out memberIdNumber);
            }

            private static bool TryGetNextNonEmpty(string[] lines, ref int i, out string value)
            {
                while (i < lines.Length)
                {
                    var t = (lines[i] ?? "").Trim();
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        value = t;
                        return true;
                    }
                    i++;
                }
                value = "";
                return false;
            }

            private static bool IsSeparator(string? line)
            {
                var t = (line ?? "").Trim();
                if (t.Length == 0) return false;

                bool allEq = t.All(c => c == '=');
                bool allDash = t.All(c => c == '-');

                return (allEq && t.Length >= 10) || (allDash && t.Length >= 10);
            }

            private static bool TryParseReceiptNumber(string line, out int receiptNumber)
            {
                receiptNumber = 0;
                int receiptIdIndex = line.IndexOf('#');
                if (receiptIdIndex < 0) return false;

                var part = line.Substring(receiptIdIndex + 1).Trim();
                return int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out receiptNumber);
            }

            private static bool TryParseRow(string rowLine, out string rowText, out int rowQuantity, out decimal rowAmount)
            {
                rowText = "";
                rowQuantity = 0;
                rowAmount = 0m;

                int lastSpace = rowLine.LastIndexOf(' ');
                if (lastSpace <= 0) return false;

                var amountPart = rowLine.Substring(lastSpace + 1).Trim();
                if (!decimal.TryParse(amountPart, NumberStyles.Number, CultureInfo.InvariantCulture, out rowAmount))
                    return false;

                var left = rowLine.Substring(0, lastSpace).TrimEnd();

                int lastSpaceIndex = left.LastIndexOf(' ');

                if (lastSpaceIndex > 0)
                {
                    var mabyeQuantityToken = left.Substring(lastSpaceIndex + 1).Trim();
                    int stIndex = mabyeQuantityToken.IndexOf("st*", StringComparison.OrdinalIgnoreCase);
                    if (stIndex > 0 && int.TryParse(mabyeQuantityToken.Substring(0, stIndex), NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedQuantity))
                    {
                        rowQuantity = parsedQuantity;
                        rowText = left.Substring(0, lastSpaceIndex).TrimEnd();
                        return !string.IsNullOrWhiteSpace(rowText);
                    }
                }

                rowText = left.Trim();
                rowQuantity = 0;
                return !string.IsNullOrWhiteSpace(rowText);
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
        
        public interface ISearchProduct
        {
            List<IProductModel> Search(string searchProductText);
        }

        public interface ISaveProductToFile
        {
            void SaveAll(List<IProductModel> products);
        }

        public interface IReadAllProductsFromFile
        {
            List<IProductModel> ReadAll();
        }

        public interface IProductModel
        {
            int ProductIdNumber { get; }
            string ProductName { get; }
            decimal ProductPrice { get; }
            string ProductPriceType { get; }
            string ProductFullName { get; }
        }

        public class SaveProductToFile : ISaveProductToFile
        {
            public void SaveAll(List<IProductModel> products)
            {
                string filePath = ProductFilePath.Path;

                var productDirectory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(productDirectory) && !Directory.Exists(productDirectory))
                    Directory.CreateDirectory(productDirectory);

                using var writer = new StreamWriter(filePath, false);

                foreach (var product in products)
                {

                    writer.WriteLine(
                        $"{product.ProductIdNumber};{product.ProductName};" +
                        $"{product.ProductPrice.ToString(CultureInfo.InvariantCulture)};" +
                        $"{product.ProductPriceType}"
                    );
                }
            }
        }
        public class ReadAllProductsFromFile : IReadAllProductsFromFile
        {
            public List<IProductModel> ReadAll()
            {
                var products = new List<IProductModel>();

                string filePath = ProductFilePath.Path;

                if (!File.Exists(filePath))
                    return products;

                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parts = line.Split(';');
                    if (parts.Length != 4)
                        continue;

                    if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productIdNumber))
                        continue;

                    if (!decimal.TryParse(parts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal productPrice))
                        continue;

                    products.Add(new ProductModel(
                        productIdNumber,
                        parts[1],
                        productPrice,
                        parts[3]));
                }
                return products;
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

        public class MemberSearch : ISearchMember
        {
            private readonly IReadAllMembersFromFile _reader;

            public MemberSearch(IReadAllMembersFromFile reader)
            {
                _reader = reader;
            }

            public List<IMemberModel> Search(string searchMemberText)
            {
                var allMembers = _reader.ReadAll();

                if (string.IsNullOrWhiteSpace(searchMemberText))
                    return allMembers;

                string userQuery = searchMemberText.Trim().ToLowerInvariant();

                return allMembers
                    .Where(m =>
                    {
                        string firstName = (m.MemberFirstName ?? "").ToLowerInvariant();
                        string lastName = (m.MemberLastName ?? "").ToLowerInvariant();
                        string fullName = (m.MemberFullName ?? "").ToLowerInvariant();

                        return m.MemberIdNumber.ToString().Contains(userQuery)
                               || firstName.Contains(userQuery)
                               || lastName.Contains(userQuery)
                               || fullName.Contains(userQuery);
                    })
                    .ToList();
            }
        }
        
        public interface ISearchMember
        {
            List<IMemberModel> Search(string searchMemberText);
        }
        public interface ISaveMemberToFile
        {
            void SaveAll(List<IMemberModel> members);
        }

        public interface IReadAllMembersFromFile
        {
            List<IMemberModel> ReadAll();
        }

        public interface IMemberModel
        {
            int MemberIdNumber { get; }
            string MemberFirstName { get; }
            string MemberLastName { get; }
            string MemberFullName { get; }
        }

        public class SaveMemberToFile : ISaveMemberToFile
        {
            public void SaveAll(List<IMemberModel> members)
            {
                string filePath = MemberFilePath.MembersPath;

                var memberDirectory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(memberDirectory) && !Directory.Exists(memberDirectory))
                    Directory.CreateDirectory(memberDirectory);

                using var writer = new StreamWriter(filePath, false);

                foreach (var member in members)
                {
                    writer.WriteLine($"{member.MemberIdNumber};{member.MemberFirstName};{member.MemberLastName}");
                }
            }
        }
        public class ReadAllMembersFromFile : IReadAllMembersFromFile
        {
            public List<IMemberModel> ReadAll()
            {
                var members = new List<IMemberModel>();

                string filePath = MemberFilePath.MembersPath;

                if (!File.Exists(filePath))
                    return members;

                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length != 3) continue;

                    if (int.TryParse(parts[0], out int memberId))
                    {
                        members.Add(new MemberModel(memberId, parts[1], parts[2]));
                    }
                }
                return members;
            }
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

        public interface ISaveCampaignToFile
        {
            void SaveAll(List<ICampaignModel> campaigns);
        }
        public interface IReadAllCampaignsFromFile
        {
            List<ICampaignModel> ReadAll();
        }
        public interface ICampaignModel
        {
            string CampaignName { get; }
            CampaignType TypeOfCampaign { get; }

            DateTime CampaignStartDate { get; }
            DateTime CampaignEndDate { get; }

            IReadOnlyList<int> ProductIdNumbers { get; }

            bool IsActive(DateTime now);
        }
        public class SaveCampaignToFile : ISaveCampaignToFile
        {
            public void SaveAll(List<ICampaignModel> campaigns)
            {
                campaigns ??= new List<ICampaignModel>();

                var lines = campaigns.Select(SerializeCampaigns).ToList();

                File.WriteAllLines(CampaignFilePath.Path, lines);
            }

            private static string SerializeCampaigns(ICampaignModel campaign)
            {
                string typeOfCampaign = campaign.TypeOfCampaign.ToString();
                string campaignName = Escape(campaign.CampaignName);
                string campaignStartDate = campaign.CampaignStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string campaignEndDate = campaign.CampaignEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string productIdNumbers = string.Join(",", campaign.ProductIdNumbers);

                decimal percent = (campaign as PercentOffCampaign)?.PercentOff
                                ?? throw new InvalidOperationException("Endast PercentOffCampaign kan sparas.");

                return $"{typeOfCampaign};{campaignName};{campaignStartDate};{campaignEndDate};{productIdNumbers};{percent.ToString(CultureInfo.InvariantCulture)}";
            }

            private static string Escape(string text) =>
                (text ?? "").Replace(";", ",").Trim();
        }

        public class ReadAllCampaignsFromFile : IReadAllCampaignsFromFile
        {
            public List<ICampaignModel> ReadAll()
            {
                var campaigns = new List<ICampaignModel>();

                if (!File.Exists(CampaignFilePath.Path))
                    return campaigns;

                var lines = File.ReadAllLines(CampaignFilePath.Path);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parts = line.Split(';');
                    if (parts.Length < 6)
                        continue;

                    if (!Enum.TryParse(parts[0], out CampaignType campaignType))
                        continue;

                    if (campaignType != CampaignType.PercentOffCampaign)
                        continue;

                    string campaignName = parts[1].Trim();

                    if (!DateTime.TryParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var campaignStartDate))
                        continue;

                    if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var campaignEndDate))
                        continue;

                    var productIdNumbers = ParseIdNumbers(parts[4]);
                    if (productIdNumbers.Count == 0)
                        continue;

                    if (!decimal.TryParse(parts[5], NumberStyles.Number, CultureInfo.InvariantCulture, out var percentOff))
                        continue;

                    try
                    {
                        campaigns.Add(new PercentOffCampaign(campaignName, campaignStartDate, campaignEndDate, productIdNumbers, percentOff));
                    }
                    catch
                    {
                        continue;
                    }

                }
                return campaigns;
            }

            private static List<int> ParseIdNumbers(string idNumbersText) =>
                        (idNumbersText ?? "")
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(i => i.Trim())
                            .Where(i => int.TryParse(i, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                            .Select(i => int.Parse(i, CultureInfo.InvariantCulture))
                            .Where(n => n > 0)
                            .Distinct()
                            .ToList();
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

        public interface ISearchCampaign
        {
            List<ICampaignModel> Search(string searchCampaignText);
        }

        public class CampaignMenue
        {
            private readonly string[] _campaignMenueOptions =
            {
            "Skapa ny kampanj\n",
            "Uppdatera kampanj",
            "Lista kampanjer",
            "Ta bort kampanj\n",
            "Tillbaka till huvudmenyn"
        };

            public void Run()
            {
                var arrowCampaignMenu = new ConsoleOptionsArrow();

                while (true)
                {
                    int selectedIndex = arrowCampaignMenu.ShowArrow("=== Kampanjsida ===", _campaignMenueOptions);

                    if (HandleCampaignMenueSelection(selectedIndex))
                        return;
                }
            }
            private bool HandleCampaignMenueSelection(int index)
            {
                switch (index)
                {
                    case 0:
                        new CreateNewCampaign().Run();
                        return false;

                    case 1:
                        new UpdateCampaign().Run();
                        return false;

                    case 2:
                        new ListAllCampaigns().Run();
                        return false;

                    case 3:
                        new DeleteCampaign().Run();
                        return false;

                    case 4:
                        return true;

                    default:
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent...");
                        Console.ReadKey(true);
                        return false;
                }
            }
        }

    }

}
        
    
