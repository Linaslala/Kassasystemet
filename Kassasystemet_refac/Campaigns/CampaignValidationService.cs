namespace Kassasystemet_refac
{
    internal class CampaignValidationService
    {
        public static void ValidateCampaignParts(string campaignName, DateTime campaignStartDate, DateTime campaignEndDate, IEnumerable<int> productIdNumbers)
        {
            if (string.IsNullOrWhiteSpace(campaignName))
                throw new ArgumentException("Namn får inte vara tomt.", nameof(campaignName));

            if (campaignEndDate < campaignStartDate)
                throw new ArgumentException("Slutdatum kan inte vara före startdatum.");

            if (productIdNumbers == null)
                throw new ArgumentNullException(nameof(productIdNumbers));

            if (!productIdNumbers.Any(i => i > 0))
                throw new ArgumentException("Minst ett giltigt produkt-id krävs.", nameof(productIdNumbers));
        }
    }
}
