using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.ProductMenueOptionCalls;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.Menues
{
    /// <summary>
    /// Meny för produktrelaterade funktioner.
    /// </summary>
    public class ProductMenue
    {
        private readonly string[] _productMenueOptions =
    {
            "Redistrera ny produkt\n",
            "Uppdatera produkt",
            "Lista alla produkter",
            "Ta bort produkt\n",
            "Tillbaka till huvudmenyn"
        };

        public void Run()
        {
            var arrowProductMenu = new ConsoleOptionsArrow();

            while (true)
            {
                // ShowArrow sköter render + upp/ner + enter och returnerar valt index
                int selectedIndex = arrowProductMenu.ShowArrow("=== Produktsida ===", _productMenueOptions);

                // HandleSelection avgör om vi ska avsluta (Logga ut = true)
                if (HandleProductMenueSelection(selectedIndex))
                    return;
            }
        }
        private static bool HandleProductMenueSelection(int index)
        {
            switch (index)
            {
                case 0:
                    new CreateNewProduct().Run();
                    return false;

                case 1:
                    new UpdateProduct().Run();
                    return false;

                case 2:
                    new ListAllProducts().Run();
                    return false;

                case 3:
                    new DeleteProduct().Run();
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
