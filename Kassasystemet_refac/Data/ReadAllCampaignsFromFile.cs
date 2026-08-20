using System.Globalization;
using static Kassasystemet_refac.SearchMember;

namespace Kassasystemet_refac
{
    public class ReadAllCampaignsFromFile : IReadAllCampaignsFromFile
    {
        public List<ICampaignModel> ReadAll()
        {
            var campaigns = new List<ICampaignModel>();

            if (!File.Exists(CampaignFilePath.Path))
                return campaigns;

            var lines = File.ReadAllLines(CampaignFilePath.Path);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');
                if (parts.Length < 6)
                    continue;

                if (!Enum.TryParse(parts[0], out CampaignType campaignType))
                    continue;

                if (campaignType != CampaignType.PercentOffCampaign)
                    continue;

                string campaignName = parts[1].Trim();

                if (!DateTime.TryParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var campaignStartDate))
                    continue;

                if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var campaignEndDate))
                    continue;

                var productIdNumbers = ParseIdNumbers(parts[4]);
                if (productIdNumbers.Count == 0)
                    continue;

                if (!decimal.TryParse(parts[5], NumberStyles.Number, CultureInfo.InvariantCulture, out var percentOff))
                    continue;

                try
                {
                    campaigns.Add(new PercentOffCampaign(campaignName, campaignStartDate, campaignEndDate, productIdNumbers, percentOff));
                }
                catch
                {
                    continue;
                }

            }
            return campaigns;
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
