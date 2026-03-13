using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager;
using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.CampaignMenueOptionCalls
{
    /// <summary>
    /// UI‑flöde för att skapa nya kampanjer.
    /// 
    /// Guidar användaren genom hela skapandeprocessen.
    /// </summary>
    public class CreateNewCampaign
    {
        public void Run()
        {
            string campaignHeader = "== Skapa kampanj ==";

            string campaignNamePrompt = "Kampanjnamn: ";
            string campaignStartDatePrompt = "Startdatum (yyyy-MM-dd): ";
            string campaignEndDatePrompt = "Slutdatum (yyyy-MM-dd): ";
            string productIdNumbersPrompt = "Produktnummer (om flera - separera med kommatecken): ";
            string percentOffPrompt = "Rabattprocent (1-100): ";

            string campaignNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                campaignHeader, campaignNamePrompt, ValidateCampaignName);

            string campaignStartDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                campaignHeader, campaignStartDatePrompt, ValidateCampaignDate, clearConsoleEachAttempt: false);

            DateTime campaignStartDate = DateTime.ParseExact(campaignStartDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                campaignHeader, campaignEndDatePrompt, ValidateCampaignDate, clearConsoleEachAttempt: false);

            DateTime campaignEndDate = DateTime.ParseExact(campaignEndDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            while (campaignEndDate < campaignStartDate)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Slutdatum kan inte vara före startdatum.");
                Console.ResetColor();

                campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    campaignHeader, campaignEndDatePrompt, ValidateCampaignDate, clearConsoleEachAttempt: false);

                campaignEndDate = DateTime.ParseExact(campaignEndDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            string productIdNumbersInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                campaignHeader, productIdNumbersPrompt, ValidateProductIdNumbers, clearConsoleEachAttempt: false);

            List<int> productIdNumbers = ParseProductIdNumbers(productIdNumbersInput);

            string percentOffInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            campaignHeader, percentOffPrompt, ValidatePercent, clearConsoleEachAttempt: false);

            decimal percentOff = ParseDecimalInvariant(percentOffInput);

            ICampaignModel newCampaign = new PercentOffCampaign(
                campaignNameInput,
                campaignStartDate,
                campaignEndDate,
                productIdNumbers,
                percentOff);

            IReadAllCampaignsFromFile reader = new ReadAllCampaignsFromFile();
            ISaveCampaignToFile writer = new SaveCampaignToFile();

            var campaigns = reader.ReadAll();
            campaigns.Add(newCampaign);
            writer.SaveAll(campaigns);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            CenterConsoleOutput.CenterTextToWindow("== Ny kampanj skapad ==");
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow($"Kampanj: {newCampaign.CampaignName} ({newCampaign.TypeOfCampaign})");

            CenterConsoleOutput.CenterTextToWindow(
                $"Gäller: {campaignStartDate:yyyy-MM-dd} till {campaignEndDate:yyyy-MM-dd}"
            );

            Console.WriteLine();

            var productReader = new ReadAllProductsFromFile();
            var allProducts = productReader.ReadAll();

            var campaignProducts = allProducts
                .Where(p => productIdNumbers.Contains(p.ProductIdNumber))
                .OrderBy(p => p.ProductIdNumber)
                .ToList();

            var percentCampaign = newCampaign as PercentOffCampaign;

            if (campaignProducts.Any())
            {
                CenterConsoleOutput.CenterTextToWindow("Produkter i kampanjen:");
                Console.WriteLine();

                int tableWidth = 60; 
                int leftPadding = (Console.WindowWidth - tableWidth) / 2;
                string indent = new string(' ', Math.Max(0, leftPadding));

                Console.WriteLine(
                    indent + $"{"Produktnummer",-15}{"Produktnamn",-30}{"Rabattprocent",-15}"
                );

                Console.WriteLine(indent + new string('-', 60));

                foreach (var product in campaignProducts)
                {
                    Console.WriteLine(indent + $"{product.ProductIdNumber,-15}{product.ProductName,-30}{percentOff + " %",-15}");
                }
            }

            else
            {
                CenterConsoleOutput.CenterTextToWindow("OBS: Inga matchande produkter hittades för angivna produktnummer.");
            }

            Console.ResetColor();
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");

            var afterSavedCampaignMenu = new ConsoleOptionsArrow();
            var afterSavedCampaignOptions = new[]
            {
                "Skapa ny kampanj",
                "Tillbaka till kampanjmenyn"
            };

            int choice = afterSavedCampaignMenu.ShowArrow("Välj:", afterSavedCampaignOptions);

            if (choice == 0)
            {
                Run();
                return;
            }
        }

        private static void ValidateCampaignName(string campaignNameInput)
        {
            if (string.IsNullOrWhiteSpace(campaignNameInput))
                throw new ArgumentException("Ogiltigt namn: får inte vara tomt.");
        }

        private static void ValidateCampaignDate(string campaignDateInput)
        {
            if (string.IsNullOrWhiteSpace(campaignDateInput))
                throw new ArgumentException("Ogiltigt datum: får inte vara tomt.");

            if (!DateTime.TryParseExact(campaignDateInput.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Fel format. Ex: 2026-03-03");
            }
        }

        private static void ValidateProductIdNumbers(string productIdNumbersInput)
        {
            if (string.IsNullOrWhiteSpace(productIdNumbersInput))
                throw new ArgumentException("Du måste ange minst ett produktnummer.");

            var productIdNumbers = ParseProductIdNumbers(productIdNumbersInput);
            if (productIdNumbers.Count == 0)
                throw new ArgumentException("Du måste ange minst ett giltigt produktnummer (ex: 1,2,3).");
        }

        private static void ValidatePercent(string precentOffInput)
        {
            decimal value = ParseDecimalInvariant(precentOffInput);
            if (value <= 0m || value > 100m)
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

        private static decimal ParseDecimalInvariant(string percentOffInput)
        {
            if (string.IsNullOrWhiteSpace(percentOffInput))
                throw new ArgumentException("Ogiltigt tal: får inte vara tomt.");

            string normalized = percentOffInput.Trim().Replace(',', '.');

            if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
                throw new ArgumentException("Ogiltigt tal: ange ett numeriskt värde.");

            return value;
        }
    }
}
