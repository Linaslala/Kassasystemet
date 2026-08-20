namespace Kassasystemet_refac
{
    public class ProductMenu
    {
        private readonly string[] _productMenuOptions =
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
                int selectedIndex = arrowProductMenu.ShowArrow("=== Produktsida ===", _productMenuOptions);

                if (HandleProductMenuSelection(selectedIndex))
                    return;
            }
        }
        private static bool HandleProductMenuSelection(int index)
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
