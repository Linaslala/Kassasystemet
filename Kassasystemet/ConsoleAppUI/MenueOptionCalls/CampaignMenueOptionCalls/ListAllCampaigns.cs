using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.CampaignMenueOptionCalls
{
    /// <summary>
    /// Visar en lista med alla kampanjer i systemet.
    /// </summary>
    public class ListAllCampaigns
    {
        public void Run()
        {
            Console.Clear();
            CenterConsoleOutput.CenterTextToWindow("== Alla kampanjer ==");
            Console.WriteLine();

            IReadAllCampaignsFromFile campaignReader = new ReadAllCampaignsFromFile();
            IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();

            var campaigns = campaignReader.ReadAll()
                .OrderBy(c => c.CampaignStartDate)
                .ThenBy(c => c.CampaignName)
                .ToList();

            if (!campaigns.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Inga kampanjer finns registrerade.");
                Console.ResetColor();
                Console.WriteLine();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            var productLookup = productReader.ReadAll()
                .GroupBy(p => p.ProductIdNumber)
                .ToDictionary(g => g.Key, g => g.First().ProductName);

            string header =
                   $"{"Kampanjnamn",-20}{"Startdatum",-15}{"Slutdatum",-15}{"Produkter",-35}{"Rabatt",-10}";

            CenterConsoleOutput.CenterTextToWindow(header);
            CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));

            foreach (var campaign in campaigns)
            {
                // Produkter som "1 Mjölk, 2 Bröd"
                string productsText = string.Join(", ",
                    campaign.ProductIdNumbers
                        .OrderBy(id => id)
                        .Select(id =>
                            productLookup.TryGetValue(id, out var name)
                                ? $"{id} {name}"
                                : $"{id} (okänd)")
                );

                string discountText = campaign is PercentOffCampaign poc
                    ? poc.PercentOff.ToString("0.##", CultureInfo.InvariantCulture) + "%"
                    : "-";

                string row =
                    $"{campaign.CampaignName,-20}" +
                    $"{campaign.CampaignStartDate,-15:yyyy-MM-dd}" +
                    $"{campaign.CampaignEndDate,-15:yyyy-MM-dd}" +
                    $"{productsText,-35}" +
                    $"{discountText,-10}";

                CenterConsoleOutput.CenterTextToWindow(row);
            }

            Console.WriteLine();
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");
        }
    }
}