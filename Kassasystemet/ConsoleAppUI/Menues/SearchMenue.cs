using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.SearchMenueOptionCalls;
using LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.Menues
{
    /// <summary>
    /// Samlingsmeny för sökfunktioner.
    /// </summary>
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
}
