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

                    case ConsoleKey.DownArrow:

                        selectedIndex =
                            HandleNavigationKey(
                                key,
                                selectedIndex,
                                footerOptions.Length);
                        break;

                    case ConsoleKey.Enter:

                        if (selectedIndex == 0)
                        {
                            CartService.AddProductPrompt(
                                cart,
                                productById);
                            break;
                        }

                        PurchaseMenuChoice footerChoice =
                            (PurchaseMenuChoice)(selectedIndex -1);

                        if (HandleFooterChoice(
                            footerChoice,
                            ref memberIdNumber,
                            cart,
                            productById))
                        {
                            return;
                        }
                        break;
                }
            }
        }

        private static int HandleNavigationKey(
            ConsoleKey key,
            int selectedIndex,
            int maxIndex)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return selectedIndex <= 0
                        ? maxIndex
                        : selectedIndex - 1;

                case ConsoleKey.DownArrow:
                    return selectedIndex >= maxIndex
                        ? 0
                        : selectedIndex + 1;

                default:
                    return selectedIndex;
            }
        }

        public bool HandleFooterChoice(
            PurchaseMenuChoice footerChoice,
            ref int memberIdNumber,
            List<CartItemModel> cart,
            Dictionary<int, IProductModel> productById)
        {
            if (footerChoice == PurchaseMenuChoice.Pay)
            {
                if (ReceiptService.TryPayAndShowReceipt(ref memberIdNumber, cart))
                {
                    return true;
                }
                return false;
            }

            if (footerChoice == PurchaseMenuChoice.SearchProduct)
            {
                PurchaseProductSearchService.ShowInlineProductSearchAndPresent();
                return false;
            }

            if (footerChoice == PurchaseMenuChoice.RemoveProduct)
            {
                CartService.RemoveProductFromCart(cart);
                return false;
            }

            if (footerChoice == PurchaseMenuChoice.ChangeCustomer)
            {
                memberIdNumber = PurchaseInputService.ReadMemberIdNumber();
                return false;
            }

            if (footerChoice == PurchaseMenuChoice.SaveAndExit)
            {
                PurchaseDraftService.SavePurchaseDraft(memberIdNumber, cart);
                Console.Clear();
                
                NotificationService.ShowSuccessHeader(
                    "Pågående köp sparat. Du kan återuppta senare.");

                //Console.ForegroundColor = ConsoleColor.Green;
                //CenterConsoleOutput.CenterTextToWindow("Pågående köp sparat. Du kan återuppta senare.");
                //Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();

                return true;
            }

            return true;
        }

        public enum PurchaseMenuChoice
        {
            Pay = 0,
            SearchProduct = 1,
            RemoveProduct = 2,
            ChangeCustomer = 3,
            SaveAndExit = 4
        }
    }
}
