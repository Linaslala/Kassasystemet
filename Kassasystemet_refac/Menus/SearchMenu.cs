namespace Kassasystemet_refac
{
    public class SearchMenu
    {
        private readonly string[] _searchMenuOptions =
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
                int selectedIndex = arrowSearchMenu.ShowArrow("=== Sök ===", _searchMenuOptions);

                if (HandleSearchMenuSelection(selectedIndex))
                    return;
            }
        }
        private bool HandleSearchMenuSelection(int index)
        {
            switch (index)
            {
                case 0:
                    var memberSearch = new SearchMemberMenu();
                    memberSearch.Run();
                    return false;

                case 1:
                    var productSearch = new SearchProductMenu();
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
