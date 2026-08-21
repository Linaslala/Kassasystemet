using System.Globalization;

namespace Kassasystemet_refac
{
    public class PercentOffCampaign : ICampaignModel
    {
        public string CampaignName { get; }
        public CampaignType TypeOfCampaign => CampaignType.PercentOffCampaign;

        public DateTime CampaignStartDate { get; }
        public DateTime CampaignEndDate { get; }

        public IReadOnlyList<int> ProductIdNumbers { get; }

        public decimal PercentOff { get; }

        public PercentOffCampaign(string campaignName, DateTime campaignStartDate, DateTime campaignEndDate, IEnumerable<int> productIdNumbers, decimal percentOff)
        {
            CampaignValidationService.ValidateCampaignParts(campaignName, campaignStartDate, campaignEndDate, productIdNumbers);

            if (percentOff <= 0m || percentOff > 100m)
                throw new ArgumentException("Rabattprocenten måste vara > 0 och <= 100.", nameof(percentOff));

            CampaignName = campaignName.Trim();
            CampaignStartDate = campaignStartDate;
            CampaignEndDate = campaignEndDate;
            ProductIdNumbers = productIdNumbers.Distinct().Where(n => n > 0).ToList();
            PercentOff = percentOff;
        }

        public bool IsActive(DateTime now) => now >= CampaignStartDate && now <= CampaignEndDate;

        public string Serialize()
        {
            string typeOfCampaign = TypeOfCampaign.ToString();
            string campaignName = Escape(CampaignName);
            string campaignStartDate = CampaignStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string campaignEndDate = CampaignEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string productIdNumbers = string.Join(",", ProductIdNumbers);

            return $"" +
                $"{typeOfCampaign};" +
                $"{campaignName};" +
                $"{campaignStartDate};" +
                $"{campaignEndDate};" +
                $"{productIdNumbers};{PercentOff.ToString(CultureInfo.InvariantCulture)}";

        }
        private static string Escape(string text) =>
        (text ?? "").Replace(";", ",").Trim();
    }
}
