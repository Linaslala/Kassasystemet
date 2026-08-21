namespace Kassasystemet_refac
{
    //En controller ska ofta ta emot input
    //Bestämma vad som ska ske
    //Anropa rätt service
    public class PurchaseController
    {
        public void Run()
        {
            int memberIdNumber = PurchaseInputService.ReadCustomerNumberOrSkip();
            var cart = new List<CartItemModel>();
            PurchaseSplitViewLoop(ref memberIdNumber, cart);
        }

        public void Run(int memberIdNumber, List<(int productIdNumber, int productQuantity)> resumeItems)
        {
            var cart = PurchaseDraftService.LoadCartFromSavedItems(resumeItems);
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
                PurchaseConsoleView.RenderSplitPurchaseView(memberIdNumber, cart, topAction, footerOptions, selectedIndex);

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

                        if (HandleFooterChoice(
                            footerChoice,
                            ref memberIdNumber,
                            cart,
                            productById))
                        {
                            return;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public bool HandleFooterChoice(
            int footerChoice,
            ref int memberIdNumber,
            List<CartItemModel> cart,
            Dictionary<int, IProductModel> productById)
        {
            if (footerChoice == 0)
            {
                if (ReceiptService.TryPayAndShowReceipt(ref memberIdNumber, cart))
                {
                    return true;
                }
                return false;
            }

            if (footerChoice == 1)
            {
                PurchaseProductSearchService.ShowInlineProductSearchAndPresent();
                return false;
            }

            if (footerChoice == 2)
            {
                CartService.RemoveProductFromCart(cart);
                return false;
            }

            if (footerChoice == 3)
            {
                memberIdNumber = PurchaseInputService.ReadMemberIdNumber();
                return false;
            }

            if (footerChoice == 4)
            {
                PurchaseDraftService.SavePurchaseDraft(memberIdNumber, cart);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleOutput.CenterTextToWindow("Pågående köp sparat. Du kan återuppta senare.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();

                return true;
            }

            return true;
        }
    }
}
