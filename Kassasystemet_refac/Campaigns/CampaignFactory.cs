using System.Globalization;

namespace Kassasystemet_refac
{

    //Factoryns jobb är att ta information
    //Skapa rätt objekt
    //Returnera objektet
    public static class CampaignFactory
    {

        public static ICampaignModel Create(
            string[] parts)
        {
            CampaignType campaignType =
                Enum.Parse<CampaignType>(parts[0]);

            if (campaignType == CampaignType.PercentOffCampaign)
            {
                string campaignName = parts[1];

                DateTime campaignStartDate =
                    DateTime.Parse(parts[2]);

                DateTime campaignEndDate =
                    DateTime.Parse(parts[3]);

                List<int> productIdNumbers =
                    ParseIdNumbers(parts[4]);

                decimal percentOff =
                    decimal.Parse(parts[5]);

                return new PercentOffCampaign(
                    campaignName,
                    campaignStartDate,
                    campaignEndDate,
                    productIdNumbers,
                    percentOff);
            }

            throw new InvalidOperationException(
                "Okänd kampanjtyp");
        }

        private static List<int> ParseIdNumbers(string idNumbersText) =>
                    (idNumbersText ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => i.Trim())
                        .Where(i => int.TryParse(i, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                        .Select(i => int.Parse(i, CultureInfo.InvariantCulture))
                        .Where(n => n > 0)
                        .Distinct()
                        .ToList();
    }
}
