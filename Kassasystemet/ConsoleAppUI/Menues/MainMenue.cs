using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.ConsoleAppUI.Menues;
using System;

namespace LinasKlubbLivs.UserInterface.Menues
{
    /// <summary>
    /// Applikationens huvudmeny.
    /// </summary>
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
}