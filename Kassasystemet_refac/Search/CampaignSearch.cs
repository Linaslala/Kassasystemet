using System.Globalization;

namespace Kassasystemet_refac
{
    public class CampaignSearch : ISearchCampaign
    {
        private readonly IReadAllCampaignsFromFile _reader;

        public CampaignSearch(IReadAllCampaignsFromFile reader)
        {
            _reader = reader;
        }

        public List<ICampaignModel> Search(string searchCampaignText)
        {
            if (string.IsNullOrWhiteSpace(searchCampaignText))
                return new List<ICampaignModel>();

            searchCampaignText = searchCampaignText.Trim().ToLowerInvariant();

            bool isProductIdInt = int.TryParse(
                searchCampaignText,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int searchedProductId);

            bool isDateFormatSearch = DateTime.TryParseExact(
                searchCampaignText,
                new[] { "yyyy-MM-dd", "yyyy-MM", "yyyyMMdd", "yy-MM-dd", "dd-MM-yyyy", "dd/MM/yyyy" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime searchedDate);

            return _reader.ReadAll()
                .Where(c =>
                {
                    if (!string.IsNullOrWhiteSpace(c.CampaignName) &&
                         c.CampaignName.ToLowerInvariant().Contains(searchCampaignText))
                        return true;

                    string campaignStartText = c.CampaignStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).ToLowerInvariant();
                    string campaignEndText = c.CampaignEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).ToLowerInvariant();

                    if (campaignStartText.Contains(searchCampaignText) || campaignEndText.Contains(searchCampaignText))
                        return true;

                    if (isDateFormatSearch &&
                        (c.CampaignStartDate.Date == searchedDate.Date || c.CampaignEndDate.Date == searchedDate.Date))
                        return true;

                    if (c.ProductIdNumbers != null)
                    {
                        if (isProductIdInt && c.ProductIdNumbers.Contains(searchedProductId))
                            return true;

                        string productIdNumbersText = string.Join(",", c.ProductIdNumbers).ToLowerInvariant();
                        if (productIdNumbersText.Contains(searchCampaignText))
                            return true;
                    }

                    return false;
                })
                .ToList();
        }
    }
}
