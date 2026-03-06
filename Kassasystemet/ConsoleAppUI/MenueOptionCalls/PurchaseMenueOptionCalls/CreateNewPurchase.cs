using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptModels;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.PurchaseMenueOptionCalls
{
    /// <summary>
    /// Ansvarar för hela köpfunktionen i kassasystemet.
    /// 
    /// Funktionalitet:
    /// - Registrera nytt köp med valfri kund (kan anges senare).
    /// - Lägga till flera produkter i följd utan att behöva gå tillbaka till huvudmenyn.
    /// - Visa varukorg med antal och totalsumma.
    /// - Möjlighet att pausa pågående köp (sparas som utkast).
    /// - Slutföra betalning:
    ///     Skapar och sparar kvitto till fil
    ///     Visar kvittot direkt i konsolen
    ///     Återgår till köpmenyn
    /// 
    /// Designprinciper:
    /// - Samma meny- och renderingsmönster som övriga delar av systemet.
    /// - renderAboveOptions används för att säkerställa att innehåll alltid syns
    ///   ovanför menyval (ingen scroll uppåt behövs).
    /// - Ingen affärslogik i UI.
    /// </summary>
    public class CreateNewPurchase
    {
        public void Run()
        {
            int memberIdNumber = ReadCustomerNumberOrSkip();
            var cart = new List<CartItemModel>();
            PurchaseLoop(memberIdNumber, cart);
        }

        public void Run(int memberIdNumber, List<(int productIdNumber, int productQuantity)> resumeItems)
        {
            var cart = LoadCartFromSavedItems(resumeItems);
            PurchaseLoop(memberIdNumber, cart);
        }

        private void PurchaseLoop(int memberIdNumber, List<CartItemModel> cart)
        {
            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Registrera nytt köp ==");
                Console.WriteLine();

                if (memberIdNumber > 0)
                {
                    CenterConsoleOutput.CenterTextToWindow($"Kundnummer: {memberIdNumber}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    CenterConsoleOutput.CenterTextToWindow("Kundnummer saknas (lägg till innan betalning).");
                    Console.ResetColor();
                }

                Console.WriteLine();
                PrintCart(cart);
                Console.WriteLine();

                CenterConsoleOutput.CenterTextToWindow($"Antal varor: {cart.Sum(x => x.ProductQuantity)}");
                CenterConsoleOutput.CenterTextToWindow($"Summa (utan rabatter): {cart.Sum(x => x.LineTotal).ToString("0.00", CultureInfo.InvariantCulture)} SEK");
                Console.WriteLine();

                var purchaseMenu = new ConsoleOptionsArrow();
                var purchaseOptions = new[]
                {
                    "Lägg till produkter (flera i följd)",
                    "Ta bort produkt från varukorgen",
                    "Ändra kundnummer",
                    "Pausa pågående köp (spara) och gå ut",
                    "Betala",
                    "Avbryt (utan att spara)"
                };

                int choice = purchaseMenu.ShowArrow("Välj:", purchaseOptions);

                if (choice == 0)
                {

                    // Inmatningsläge för flera produkter i följd.
                    // Användaren kan skriva "ProduktId Antal" flera gånger utan att lämna läget.
                    var action = AddProductsInLoop(memberIdNumber, cart);

                    if (action == AddFlowAction.PayNow)
                    {
                        if (TryPayAndShowReceipt(ref memberIdNumber, cart))
                            return;
                    }
                    else if (action == AddFlowAction.PauseDraft)
                    {
                        SavePurchaseDraft(memberIdNumber, cart);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        CenterConsoleOutput.CenterTextToWindow("Pågående köp sparat.");
                        Console.ResetColor();
                        ValidatedConsoleInput.PauseCentered();
                        return;
                    }

                }
                else if (choice == 1)
                {
                    RemoveProductFromCart(cart);
                }
                else if (choice == 2)
                {
                    memberIdNumber = ReadMemberIdNumber();
                }
                else if (choice == 3)
                {
                    SavePurchaseDraft(memberIdNumber, cart);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    CenterConsoleOutput.CenterTextToWindow("Pågående köp sparat. Du kan återuppta senare.");
                    Console.ResetColor();
                    ValidatedConsoleInput.PauseCentered();
                    return;
                }
                else if (choice == 4)
                {
                    if (TryPayAndShowReceipt(ref memberIdNumber, cart))
                        return;
                }
                else
                {
                    return;
                }
            }
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

            ReceiptModel receipt = CompletePayment(memberIdNumber, cart);
            ClearPurchaseDraft();

            Console.Clear();
            CenterConsoleOutput.CenterTextToWindow("== KVITTO ==");
            Console.WriteLine();
            ReceiptPrinter.PrintDetailed(receipt);
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att gå tillbaka till köpmenyn...");
            return true;
        }

        private AddFlowAction AddProductsInLoop(int memberIdNumber, List<CartItemModel> cart)
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
                return AddFlowAction.BackToMenu;
            }

            var productById = products
                .GroupBy(p => p.ProductIdNumber)
                .ToDictionary(g => g.Key, g => g.First());

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Lägg till produkter ==");
                Console.WriteLine();

                //Information till användaren
                string inputHeader = $"{"ProduktId",-12}{"Antal",-10}";
                CenterConsoleOutput.CenterTextToWindow(inputHeader);
                CenterConsoleOutput.CenterTextToWindow(new string('-', inputHeader.Length));
                CenterConsoleOutput.CenterTextToWindow("Exempel: 12 3   eller   12,3");
                CenterConsoleOutput.CenterTextToWindow("Skriv 'klar' eller lämna tomt för att avsluta och välja nästa steg.");
                Console.WriteLine();

                PrintCart(cart);
                Console.WriteLine();

                string input = UserInputPlacer.ReadCenteredText("Ange ProduktId + Antal: ").Trim();

                if (string.IsNullOrWhiteSpace(input) || input.Equals("klar", StringComparison.OrdinalIgnoreCase))
                {
                    return ShowDoneMenu(memberIdNumber, cart);
                }

                if (!TryParseIdAndQuantity(input, out int productIdNumber, out int productQuantity, out string error))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow(error);
                    Console.ResetColor();
                    ValidatedConsoleInput.PauseCentered();
                    continue;
                }

                if (!productById.TryGetValue(productIdNumber, out var product))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CenterConsoleOutput.CenterTextToWindow($"Ingen produkt hittades med ProduktId {productIdNumber}.");
                    Console.ResetColor();
                    ValidatedConsoleInput.PauseCentered();
                    continue;
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
        }

        private AddFlowAction ShowDoneMenu(int memberIdNumber, List<CartItemModel> cart)
        {
            var menu = new ConsoleOptionsArrow();
            var options = new[]
            {
                "Betala",
                "Pausa pågående köp (spara)",
                "Fortsätt lägga till produkter",
                "Tillbaka till köpmenyn"
            };

            int choice = menu.ShowArrow(
                "Du är klar. Vad vill du göra?",
                options,
                renderAboveOptions: () =>
                {
                    CenterConsoleOutput.CenterTextToWindow("== Lägg till produkter ==");
                    Console.WriteLine();
                    PrintCart(cart);
                    Console.WriteLine();

                    if (memberIdNumber > 0)
                        CenterConsoleOutput.CenterTextToWindow($"Kundnummer: {memberIdNumber}");
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        CenterConsoleOutput.CenterTextToWindow("Kundnummer saknas (anges vid betalning).");
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                });

            if (choice == 0) return AddFlowAction.PayNow;
            if (choice == 1) return AddFlowAction.PauseDraft;
            if (choice == 2) return AddFlowAction.ContinueAdding;
            return AddFlowAction.BackToMenu;
        }

        private enum AddFlowAction
        {
            ContinueAdding,
            PayNow,
            PauseDraft,
            BackToMenu
        }

        // Parser accepterar: "12 3", "12,3", "12;3", "12x3", "12*3"
        private static bool TryParseIdAndQuantity(string productInput, out int productIdNumber, out int productQuantity, out string error)
        {
            productIdNumber = 0;
            productQuantity = 0;
            error = "";

            string normalized = (productInput ?? "").Trim()
                .Replace("x", " ", StringComparison.OrdinalIgnoreCase)
                .Replace("*", " ")
                .Replace(",", " ")
                .Replace(";", " ");

            var parts = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                error = "Ogiltigt format. Skriv: ProduktId Antal (ex: 12 3) eller 12,3.";
                return false;
            }

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out productIdNumber) || productIdNumber <= 0)
            {
                error = "Ogiltigt ProduktId. Ange ett heltal större än 0.";
                return false;
            }

            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out productQuantity) || productQuantity <= 0)
            {
                error = "Ogiltigt antal. Ange ett heltal större än 0.";
                return false;
            }

            return true;
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

        // Skapar och sparar kvitto (ingen meny här)
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

                string productLine = item.ProductQuantity > 1
                    ? $"{item.ProductName} {item.ProductQuantity}st*{item.UnitPrice.ToString("0.00", CultureInfo.InvariantCulture)}"
                    : $"{item.ProductName}";

                receiptRows.Add(new ReceiptRowModel(productLine, lineTotal));

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
                            $"Rabatt: {bestCampaign.PercentOff.ToString("0.0", CultureInfo.InvariantCulture)}%",
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