using LinasKlubbLivs.BusinessLogic.ProductLogic;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.SearchMenueOptionCalls
{
    /// <summary>
    /// UI‑flöde för att söka efter medlemmar.
    /// </summary>
    public class SearchProduct
    {
        public void Run()
        {
            IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
            ISearchProduct finder = new ProductSearch(reader);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Hitta produkt ==");
                Console.WriteLine();

                string queryInput = UserInputPlacer
                    .ReadCenteredText("Sök på produktnummer eller produktnamn: ")
                    .Trim();

                var results = finder.Search(queryInput);

                if (results.Count == 0)
                {
                    var arrowNoResult = new ConsoleOptionsArrow();
                    var noResultOptions = new[]
                    {
                        "Ny sökning",
                        "Tillbaka till huvudmenyn"
                    };

                    int choice = arrowNoResult.ShowArrow(
                        "Välj:",
                        noResultOptions,
                        renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Hitta produkt ==");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow(
                                "Produkten du söker finns inte i systemet.");
                            Console.ResetColor();
                            Console.WriteLine();
                        });

                    if (choice == 0)
                        continue;

                    return;
                }

                var selected = results.Count == 1
                    ? results[0]
                    : SelectProduct(results);

                var arrowAfterFound = new ConsoleOptionsArrow();
                var afterFoundOptions = new[]
                {
                    "Ny sökning",
                    "Tillbaka till huvudmenyn"
                };

                int afterChoice = arrowAfterFound.ShowArrow(
                    "Välj:",
                    afterFoundOptions,
                    renderAboveOptions: () =>
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Produkt ==");
                        Console.WriteLine();
                        Console.WriteLine();

                        string header =
                            $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";

                        string row =
                            $"{selected.ProductIdNumber,-20}" +
                            $"{selected.ProductName,-20}" +
                            $"{selected.ProductPrice,-20}" +
                            $"{selected.ProductPriceType,-20}";

                        CenterConsoleOutput.CenterTextToWindow(header);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
                        CenterConsoleOutput.CenterTextToWindow(row);

                        Console.WriteLine();
                    });

                if (afterChoice == 0)
                    continue;

                return;
            }
        }

        private static IProductModel SelectProduct(List<IProductModel> products)
        {
            var productDisplay = products
                .OrderBy(p => p.ProductIdNumber)
                .Select(p => $"{p.ProductIdNumber,-6} {p.ProductFullName}")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj produkt:", productDisplay);
            return products[index];
        }
    }
}