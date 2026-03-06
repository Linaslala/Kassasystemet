using LinasKlubbLivs.BusinessLogic.CampaignLogic;
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
    /// UI‑flöde för att radera befintliga kampanjer.
    /// </summary>
    public class DeleteCampaign
    {
        public void Run()
        {
            IReadAllCampaignsFromFile reader = new ReadAllCampaignsFromFile();
            ISaveCampaignToFile writer = new SaveCampaignToFile();
            ISearchCampaign finder = new CampaignSearch(reader);

            IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
            var productLookup = productReader.ReadAll()
                .GroupBy(p => p.ProductIdNumber)
                .ToDictionary(g => g.Key, g => g.First().ProductName);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Ta bort kampanj ==");
                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText(
                    "Sök på kampanjnamn, datum eller berörda produkter: ");

                var searchResult = finder.Search(queryInput);

                if (searchResult.Count == 0)
                {
                    var arrowNoResult = new ConsoleOptionsArrow();
                    var noResultOptions = new[]
                    {
                        "Ny sökning",
                        "Tillbaka till kampanjsidan"
                    };

                    int choice = arrowNoResult.ShowArrow("Välj:", noResultOptions, renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Ta bort kampanj ==");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Kampanjen du söker finns inte i systemet.");
                            Console.ResetColor();
                            Console.WriteLine();
                        });

                    if (choice == 0)
                        continue;

                    return;
                }

                var selectedCampaign = searchResult.Count == 1
                    ? searchResult[0]
                    : SelectCampaign(searchResult);

                string campaignName = selectedCampaign.CampaignName;
                DateTime startDate = selectedCampaign.CampaignStartDate;
                DateTime endDate = selectedCampaign.CampaignEndDate;
                var productIds = selectedCampaign.ProductIdNumbers?.ToList() ?? new List<int>();

                decimal percentOff = selectedCampaign is PercentOffCampaign poc
                    ? poc.PercentOff
                    : 0m;

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Ta bort kampanj ==");
                    Console.WriteLine();
                    Console.WriteLine();

                    RenderSelectedCampaign(
                          campaignName,
                          startDate,
                          endDate,
                          productIds,
                          percentOff,
                          productLookup);

                    var confirmMenu = new ConsoleOptionsArrow();
                    var confirmOptions = new[]
                    {
                        "Ja, radera kampanj",
                        "Nej, tillbaka"
                    };

                    int deleteChoice = confirmMenu.ShowArrow(
                        "Är du säker?",
                        confirmOptions,
                        renderAboveOptions: () =>
                        {
                            RenderSelectedCampaign(
                                campaignName,
                                startDate,
                                endDate,
                                productIds,
                                percentOff,
                                productLookup);
                        });

                    if (deleteChoice != 0)
                        return;

                    var campaigns = reader.ReadAll();

                    int removed = campaigns.RemoveAll(c =>
                        string.Equals(c.CampaignName, campaignName, StringComparison.Ordinal) &&
                        c.CampaignStartDate == startDate &&
                        c.CampaignEndDate == endDate &&
                        SameIdNumbers(c.ProductIdNumbers, productIds));

                    if (removed == 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        CenterConsoleOutput.CenterTextToWindow(
                            "Kunde inte radera: kampanjen hittades inte längre i listan.");
                        Console.ResetColor();
                        ValidatedConsoleInput.PauseCentered();
                        return;
                    }

                    writer.SaveAll(campaigns);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    CenterConsoleOutput.CenterTextToWindow("Kampanj raderad");
                    Console.ResetColor();
                    Console.WriteLine();

                    var afterDeleteMenu = new ConsoleOptionsArrow();
                    var afterDeleteOptions = new[]
                    {
                        "Radera en till kampanj",
                        "Tillbaka till kampanjsidan"
                    };

                    int afterChoice = afterDeleteMenu.ShowArrow("Välj:", afterDeleteOptions);
                    if (afterChoice == 0)
                        break;

                    return;
                }
            }
        }

        private static void RenderSelectedCampaign(
                    string name,
                    DateTime start,
                    DateTime end,
                    List<int> productIds,
                    decimal percentOff,
                    Dictionary<int, string> productLookup)
        {
            CenterConsoleOutput.CenterTextToWindow("Vald kampanj:");
            Console.WriteLine();

            string productsInline = string.Join(", ",
                productIds.OrderBy(id => id)
                          .Select(id =>
                              productLookup.TryGetValue(id, out var pname)
                                  ? $"{id} {pname}"
                                  : $"{id} (okänd)"));

            string infoHeader =
                $"{"Kampanjnamn",-20}{"Startdatum",-15}{"Slutdatum",-15}{"Produkter",-35}{"Rabatt",-10}";

            string infoRow =
                $"{name,-20}{start,-15:yyyy-MM-dd}{end,-15:yyyy-MM-dd}{productsInline,-35}{percentOff.ToString("0.##", CultureInfo.InvariantCulture) + "%",-10}";

            CenterConsoleOutput.CenterTextToWindow(infoHeader);
            CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
            CenterConsoleOutput.CenterTextToWindow(infoRow);
            Console.WriteLine();
        }

        private static bool SameIdNumbers(IReadOnlyList<int> a, List<int> b) =>
            (a == null && b == null) ||
            (a != null && b != null &&
             a.Where(x => x > 0).Distinct().OrderBy(x => x)
              .SequenceEqual(b.Where(x => x > 0).Distinct().OrderBy(x => x)));

        private static ICampaignModel SelectCampaign(List<ICampaignModel> campaigns)
        {
            var display = campaigns
                .Select(c =>
                    $"{c.CampaignName} ({c.CampaignStartDate:yyyy-MM-dd} - {c.CampaignEndDate:yyyy-MM-dd}) " +
                    $"[{string.Join(",", c.ProductIdNumbers)}]")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj kampanj:", display);
            return campaigns[index];
        }
    }
}