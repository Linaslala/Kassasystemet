using System.Globalization;

namespace Kassasystemet_refac
{
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
                campaignHeader,
                campaignNamePrompt,
                CampaignValidationService.ValidateCampaignName);

            string campaignStartDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                campaignHeader,
                campaignStartDatePrompt,
                CampaignValidationService.ValidateCampaignDate,
                clearConsoleEachAttempt: false);

            DateTime campaignStartDate =
                DateTime.ParseExact(campaignStartDateInput,
                "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                campaignHeader,
                campaignEndDatePrompt,
                CampaignValidationService.ValidateCampaignDate,
                clearConsoleEachAttempt: false);

            DateTime campaignEndDate =
                DateTime.ParseExact(campaignEndDateInput,
                "yyyy-MM-dd", CultureInfo.InvariantCulture);

            while (campaignEndDate < campaignStartDate)
            {
                Console.Clear();

                NotificationService.ShowError(
                    "Slutdatum kan inte vara före startdatum.");

                campaignEndDateInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                    campaignHeader,
                    campaignEndDatePrompt,
                    CampaignValidationService.ValidateCampaignDate,
                    clearConsoleEachAttempt: false);

                campaignEndDate = DateTime.ParseExact(
                    campaignEndDateInput,
                    "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            string productIdNumbersInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                campaignHeader,
                productIdNumbersPrompt,
                CampaignValidationService.ValidateProductIdNumbers,
                clearConsoleEachAttempt: false);

            List<int> productIdNumbers = CampaignValidationService.ParseProductIdNumbers(productIdNumbersInput);

            string percentOffInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                            campaignHeader,
                            percentOffPrompt,
                            CampaignValidationService.ValidatePercent,
                            clearConsoleEachAttempt: false);

            decimal percentOff = CampaignValidationService.ParseDecimalInvariant(percentOffInput);

            ICampaignModel newCampaign = new PercentOffCampaign(
                campaignNameInput,
                campaignStartDate,
                campaignEndDate,
                productIdNumbers,
                percentOff);

            IReadAllCampaignsFromFile campaignReader = new ReadAllCampaignsFromFile();
            ISaveCampaignToFile campaignWriter = new SaveCampaignToFile();

            var campaigns = campaignReader.ReadAll();
            campaigns.Add(newCampaign);
            campaignWriter.SaveAll(campaigns);

            Console.Clear();

            NotificationService.ShowSuccessHeader(
             "=== Ny kampanj skapad ===");



            //CenterConsoleOutput.CenterTextToWindow(
            //    $"Kampanj: " +
            //    $"{newCampaign.CampaignName} " +
            //    $"({newCampaign.TypeOfCampaign})");

            //CenterConsoleOutput.CenterTextToWindow(
            //    $"Gäller: {campaignStartDate:yyyy-MM-dd} till {campaignEndDate:yyyy-MM-dd}"
            //);

            //Console.WriteLine();

            var productReader = new ReadAllProductsFromFile();
            var allProducts = productReader.ReadAll();

            var campaignProducts = allProducts
                .Where(p => productIdNumbers.Contains(p.ProductIdNumber))
                .OrderBy(p => p.ProductIdNumber)
                .ToList();

            RenderCreatedCampaign(
            newCampaign
            /*campaignProducts*/);

            //var percentCampaign = newCampaign as PercentOffCampaign;

            //if (campaignProducts.Any())
            //{
            //    CenterConsoleOutput.CenterTextToWindow("Produkter i kampanjen:");
            //    Console.WriteLine();

            //    int tableWidth = 60;
            //    int leftPadding = (Console.WindowWidth - tableWidth) / 2;
            //    string indent = new string(' ', Math.Max(0, leftPadding));

            //    Console.WriteLine(
            //        indent + $"{"Produktnummer",-15}" +
            //        $"{"Produktnamn",-30}" +
            //        $"{"Rabattprocent",-15}"
            //    );

            //    Console.WriteLine(indent + new string('-', 60));

            //    foreach (var product in campaignProducts)
            //    {
            //        Console.WriteLine(indent + 
            //            $"{product.ProductIdNumber,-15}" +
            //            $"{product.ProductName,-30}" +
            //            $"{percentOff + " %",-15}");
            //    }
            //}

            ////Syns detta i konsolfönstret?
            //else
            //{
            //    CenterConsoleOutput.CenterTextToWindow("OBS: Inga matchande produkter hittades för angivna produktnummer.");
            //}

            Console.ResetColor();
            ValidatedConsoleInput.PauseCentered(
                "Tryck valfri tangent för att fortsätta...");

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

        public static void RenderCreatedCampaign(
            ICampaignModel campaign
            /*IEnumerable<IProductModel> campaignProducts*/)
        {

            CenterConsoleOutput.CenterTextToWindow(
                    $"Kampanj: " +
                    $"{campaign.CampaignName} " +
                    $"({campaign.TypeOfCampaign})");

            CenterConsoleOutput.CenterTextToWindow(
                $"Gäller: {campaign.CampaignStartDate:yyyy-MM-dd} " +
                $"till {campaign.CampaignEndDate:yyyy-MM-dd}"
            );

            //Console.WriteLine();

            //var productReader = new ReadAllProductsFromFile();
            //var allProducts = productReader.ReadAll();

            //var campaignProducts = allProducts
            //    .Where(p => productIdNumbers.Contains(p.ProductIdNumber))
            //    .OrderBy(p => p.ProductIdNumber)
            //    .ToList();

            //var percentCampaign = newCampaign as PercentOffCampaign;

            //if (campaignProducts.Any())
            //{
            //    CenterConsoleOutput.CenterTextToWindow("Produkter i kampanjen:");
            //    Console.WriteLine();
            //}


            //    int tableWidth = 60;
            //    int leftPadding = (Console.WindowWidth - tableWidth) / 2;
            //    string indent = new string(' ', Math.Max(0, leftPadding));

            //    Console.WriteLine(
            //        indent + $"{"Produktnummer",-15}" +
            //                $"{"Produktnamn",-30}" +
            //                $"{"Rabattprocent",-15}"
            //            );

            //    Console.WriteLine(indent + new string('-', 60));

            //    foreach (var product in campaignProducts)
            //    {
            //        Console.WriteLine(indent +
            //            $"{product.ProductIdNumber,-15}" +
            //            $"{product.ProductName,-30}" +
            //            $"{percentOff + " %",-15}");
            //    }
            //}

            //Syns detta i konsolfönstret?
            //else
            //{
            //    CenterConsoleOutput.CenterTextToWindow("OBS: Inga matchande produkter hittades för angivna produktnummer.");
            //}
        }
    }
}
