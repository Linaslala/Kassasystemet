using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.SalesReportOptionCalls;

namespace LinasKlubbLivs.ConsoleAppUI.Menues
{
    /// <summary>
    /// Meny för försäljningsrapport och kvitton.
    /// </summary>
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
}