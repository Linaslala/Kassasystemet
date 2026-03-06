using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.CampaignMenueOptionCalls;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.Menues
{
    /// <summary>
    /// Huvudmeny för kampanjhantering.
    /// </summary>
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
                // ShowArrow sköter render + upp/ner + enter och returnerar valt index
                int selectedIndex = arrowCampaignMenu.ShowArrow("=== Kampanjsida ===", _campaignMenueOptions);

                // HandleSelection avgör om vi ska avsluta (Logga ut = true)
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
