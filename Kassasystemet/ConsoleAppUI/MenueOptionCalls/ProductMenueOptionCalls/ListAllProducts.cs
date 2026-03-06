using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using LinasKlubbLivs.BusinessLogic.ProductLogic;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.ProductMenueOptionCalls
{
    /// <summary>
    /// UI‑flöde för att uppdatera produktinformation.
    /// </summary>
    public class ListAllProducts
    {
        public void Run()
        {
            Console.Clear();

            string listAllProductsHeader = "== Alla produkter ==";

            CenterConsoleOutput.CenterTextToWindow(listAllProductsHeader);

            IReadAllProductsFromFile reader = new ReadAllProductsFromFile();
            ISearchProduct finder = new ProductSearch(reader);

            var products = reader.ReadAll()
            .OrderBy(p => p.ProductIdNumber)
            .ToList();

            if (!products.Any())
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Det finns inga produkter i lager");
                Console.ResetColor();

                Console.WriteLine();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            string productHeader =
                $"{"Produktnummer",-20}{"Produkt",-20}{"Pris",-20}{"Pristyp",-20}";
                CenterConsoleOutput.CenterTextToWindow(productHeader);
                CenterConsoleOutput.CenterTextToWindow(new string('-', productHeader.Length));

            foreach (var product in products)
            {
                CenterConsoleOutput.CenterTextToWindow(
                    $"{product.ProductIdNumber,-20} {product.ProductName,-20} {product.ProductPrice,-20} {product.ProductPriceType,-20}"
                );
            }

            Console.WriteLine();
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");
        }
    }
}
