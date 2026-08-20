namespace Kassasystemet_refac
{
    public class MainMenu
    {
        private readonly string[] _mainMenuOptions =
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
                int selectedIndex = arrowMainMenu.ShowArrow("Välj funktion", _mainMenuOptions);

                if (HandleMainMenuSelection(selectedIndex))
                    return;
            }
        }

        private bool HandleMainMenuSelection(int index)
        {
            switch (index)
            {
                case 0:
                    var newPurchase = new PurchaseMenu();
                    newPurchase.Run();
                    return false;

                case 1:
                    var searchMenu = new SearchMenu();
                    searchMenu.Run();
                    return false;

                case 2:
                    var memberMenu = new MemberMenu();
                    memberMenu.Run();
                    return false;

                case 3:
                    var productMenu = new ProductMenu();
                    productMenu.Run();
                    return false;

                case 4:
                    var campaignMenu = new CampaignMenu();
                    campaignMenu.Run();
                    return false;

                case 5:
                    var salesReportMenu = new SalesReportMenu();
                    salesReportMenu.Run();
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
}
