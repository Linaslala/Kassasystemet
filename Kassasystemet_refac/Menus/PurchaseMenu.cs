namespace Kassasystemet_refac
{
    public class PurchaseMenu
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
}
