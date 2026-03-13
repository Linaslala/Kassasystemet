using LinasKlubbLivs.BusinessLogic.CampaignLogic;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.CampaignMenueOptionCalls
{
    /// <summary>
    /// UI‑flöde för att uppdatera kampanjer.
    /// </summary>
    public class UpdateCampaign
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
                CenterConsoleOutput.CenterTextToWindow("== Uppdatera kampanj ==");
                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText("Sök på kampanjnamn, datum eller berörda produkter: ");
                var searchCampaignResult = finder.Search(queryInput);

                if (searchCampaignResult.Count == 0)
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
                            CenterConsoleOutput.CenterTextToWindow("== Uppdatera kampanj ==");
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

                var selectedCampaign = searchCampaignResult.Count == 1
                    ? searchCampaignResult[0]
                    : SelectCampaign(searchCampaignResult);

                string originalName = selectedCampaign.CampaignName;
                DateTime originalStart = selectedCampaign.CampaignStartDate;
                DateTime originalEnd = selectedCampaign.CampaignEndDate;
                var originalProductIds = selectedCampaign.ProductIdNumbers?.ToList() ?? new List<int>();

                decimal originalPercentOff = selectedCampaign is PercentOffCampaign percentOffCampaign ? percentOffCampaign.PercentOff : 0m;

                string campaignName = originalName;
                DateTime campaignStartDate = originalStart;
                DateTime campaignEndDate = originalEnd;
                var productIdNumbers = originalProductIds.ToList();
                decimal percentOff = originalPercentOff;

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Uppdatera kampanj ==");
                    Console.WriteLine();
                    Console.WriteLine();

                    RenderSelectedCampaign(campaignName, campaignStartDate, campaignEndDate, productIdNumbers, percentOff, productLookup);

                    var arrowEdit = new ConsoleOptionsArrow();
                    var editOptions = new[]
                    {
                        "Ändra kampanjnamn",
                        "Ändra startdatum",
                        "Ändra slutdatum",
                        "Ändra produkter",
                        "Ändra rabattprocent",
                        "Spara",
                        "Avbryt"
                    };

                    int editChoice = arrowEdit.ShowArrow("Välj vad du vill ändra:", editOptions, renderAboveOptions: () =>
                    {
                        RenderSelectedCampaign(campaignName, campaignStartDate, campaignEndDate, productIdNumbers, percentOff, productLookup);
                    });
                    ;

                    if (editChoice == 0)
                    {
                        campaignName = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera kampanj ==",
                            "Kampanjnamn: ",
                            ValidateCampaignName);
                    }
                    else if (editChoice == 1)
                    {
                        string campaignStartDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera kampanj ==",
                            "Startdatum (yyyy-MM-dd): ",
                            ValidateCampaignDate);

                        campaignStartDate = DateTime.ParseExact(campaignStartDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        if (campaignEndDate < campaignStartDate)
                            campaignEndDate = campaignStartDate;
                    }
                    else if (editChoice == 2)
                    {
                        string campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera kampanj ==",
                            "Slutdatum (yyyy-MM-dd): ",
                            ValidateCampaignDate);

                        campaignEndDate = DateTime.ParseExact(campaignEndDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        while (campaignEndDate < campaignStartDate)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Slutdatum kan inte vara före startdatum.");
                            Console.ResetColor();

                            campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                                "== Uppdatera kampanj ==",
                                "Slutdatum (yyyy-MM-dd): ",
                                ValidateCampaignDate,
                                clearConsoleEachAttempt: false);

                            campaignEndDate = DateTime.ParseExact(campaignEndDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                    }
                    else if (editChoice == 3)
                    {
                        string productIdNumbersInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera kampanj ==",
                            "Produktnummer (1,2,3): ",
                            ValidateProductIdNumbers);

                        productIdNumbers = ParseProductIdNumbers(productIdNumbersInput);
                    }
                    else if (editChoice == 4)
                    {
                        string percentOffInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera kampanj ==",
                            "Rabattprocent (1-100): ",
                            ValidatePercent);

                        percentOff = ParseDecimalInvariant(percentOffInput);
                    }
                    else if (editChoice == 5)
                    {
                        var campaigns = reader.ReadAll();

                        int index = campaigns.FindIndex(c =>
                            string.Equals(c.CampaignName, originalName, StringComparison.Ordinal) &&
                            c.CampaignStartDate == originalStart &&
                            c.CampaignEndDate == originalEnd &&
                            SameIdNumbers(c.ProductIdNumbers, originalProductIds) &&
                            (!(c is PercentOffCampaign existingPoc) || existingPoc.PercentOff == originalPercentOff)
                        );

                        if (index < 0)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Kunde inte spara: kampanjen hittades inte längre i listan.");
                            Console.ResetColor();
                            ValidatedConsoleInput.PauseCentered();
                            return;
                        }

                        campaigns[index] = new PercentOffCampaign(
                        campaignName,
                        campaignStartDate,
                        campaignEndDate,
                        productIdNumbers,
                        percentOff
                        );

                        writer.SaveAll(campaigns);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        CenterConsoleOutput.CenterTextToWindow("Kampanj uppdaterad!");
                        Console.ResetColor();
                        Console.WriteLine();

                        CenterConsoleOutput.CenterTextToWindow($"{campaignName} ({campaignStartDate:yyyy-MM-dd} - {campaignEndDate:yyyy-MM-dd})");
                        CenterConsoleOutput.CenterTextToWindow($"Produkter: {string.Join(",", productIdNumbers)}");
                        CenterConsoleOutput.CenterTextToWindow($"Rabatt: {percentOff.ToString(CultureInfo.InvariantCulture)}%");
                        ValidatedConsoleInput.PauseCentered();

                        var afterSaveMenu = new ConsoleOptionsArrow();
                        var afterSaveOptions = new[]
                        {
                            "Uppdatera en till kampanj",
                            "Tillbaka till kampanjsidan"
                        };

                        int afterChoice = afterSaveMenu.ShowArrow("Välj:", afterSaveOptions);
                        if (afterChoice == 0) break;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private static void RenderSelectedCampaign(
            string campaignName,
            DateTime start,
            DateTime end,
            List<int> productIdNumbers,
            decimal percentOff,
            Dictionary<int, string> productLookup)

        {
            CenterConsoleOutput.CenterTextToWindow("Vald kampanj:");
            Console.WriteLine();

            string productsInline = BuildProductsInline(productIdNumbers, productLookup);

            string infoHeader = $"{"Kampanjnamn",-20}{"Startdatum",-15}{"Slutdatum",-15}";

            string infoRow =$"{campaignName,-20}{start,-15:yyyy-MM-dd}{end,-15:yyyy-MM-dd}";

            CenterConsoleOutput.CenterTextToWindow(infoHeader);
            CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
            CenterConsoleOutput.CenterTextToWindow(infoRow);
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow("Produkter i kampanjen:");
            Console.WriteLine();

            string infoHeaderTwo = $"{"ProduktNummer",-12}{"Produktnamn",-30}{"Rabatt",-10}";
            CenterConsoleOutput.CenterTextToWindow(infoHeaderTwo);
            CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeaderTwo.Length));

            string percentOffText = percentOff.ToString("0.##", CultureInfo.InvariantCulture) + "%";

            foreach (var id in productIdNumbers.OrderBy(x => x))
            {
                string productName = productLookup.TryGetValue(id, out var n) ? n : "(okänd produkt)";
                CenterConsoleOutput.CenterTextToWindow($"{id,-12}{productName,-30}{percentOffText,-10}");
            }

            Console.WriteLine();
        }

        private static string BuildProductsInline(List<int> productIdNumbers, Dictionary<int, string> productLookup)
        {
            if (productIdNumbers == null || productIdNumbers.Count == 0) return "";

            return string.Join(", ",
                productIdNumbers
                    .Where(id => id > 0)
                    .Distinct()
                    .OrderBy(id => id)
                    .Select(id =>
                    {
                        string name = productLookup.TryGetValue(id, out var n) ? n : "okänd";
                        return $"{id} {name}";
                    })
            );
        }

        private static void ValidateCampaignName(string campaignNameInput)
        {
            if (string.IsNullOrWhiteSpace(campaignNameInput))
                throw new ArgumentException("Ogiltigt kampanjnamn: får inte vara tomt.");
        }

        private static void ValidateCampaignDate(string campaignDateInput)
        {
            if (string.IsNullOrWhiteSpace(campaignDateInput))
                throw new ArgumentException("Ogiltigt datum: får inte vara tomt.");

            if (!DateTime.TryParseExact(
                campaignDateInput.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
            {
                throw new ArgumentException("Fel format. Ex: 2026-03-03");
            }
        }

        private static void ValidateProductIdNumbers(string productIdNumbersInput)
        {
            if (string.IsNullOrWhiteSpace(productIdNumbersInput))
                throw new ArgumentException("Du måste ange minst ett produktnummer.");

            var parsed = ParseProductIdNumbers(productIdNumbersInput);
            if (parsed.Count == 0)
                throw new ArgumentException("Du måste ange minst ett giltigt produktnummer (ex: 1,2,3).");
        }

        private static void ValidatePercent(string percentOffInput)
        {
            decimal percentValue = ParseDecimalInvariant(percentOffInput);
            if (percentValue <= 0m || percentValue > 100m)
                throw new ArgumentException("Ogiltig procent: ange ett tal mellan 1 och 100.");
        }

        private static List<int> ParseProductIdNumbers(string productIdNumbersInput)
        {
            return (productIdNumbersInput ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => int.TryParse(p, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                .Select(p => int.Parse(p, CultureInfo.InvariantCulture))
                .Where(n => n > 0)
                .Distinct()
                .ToList();
        }

        private static decimal ParseDecimalInvariant(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                throw new ArgumentException("Ogiltigt tal: får inte vara tomt.");

            string normalized = userInput.Trim().Replace(',', '.');
            if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
                throw new ArgumentException("Ogiltigt tal: ange ett numeriskt värde.");

            return value;
        }

        private static bool SameIdNumbers(IReadOnlyList<int> a, List<int> b) =>
            (a == null && b == null) ||
            (a != null && b != null &&
             a.Where(x => x > 0).Distinct().OrderBy(x => x)
              .SequenceEqual(b.Where(x => x > 0).Distinct().OrderBy(x => x)));

        private static ICampaignModel SelectCampaign(List<ICampaignModel> campaigns)
        {
            var campaignDisplay = campaigns
                .Select(c =>
                    $"{c.CampaignName} ({c.CampaignStartDate:yyyy-MM-dd} - {c.CampaignEndDate:yyyy-MM-dd}) [{string.Join(",", c.ProductIdNumbers)}]")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj kampanj:", campaignDisplay);
            return campaigns[index];
        }
    }
}
