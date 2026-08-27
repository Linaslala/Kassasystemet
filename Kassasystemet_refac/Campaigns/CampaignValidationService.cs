using System.Globalization;

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

        public static void ValidateCampaignName(string campaignNameInput)
        {
            if (string.IsNullOrWhiteSpace(campaignNameInput))
                throw new ArgumentException("Namn får inte vara tomt.", nameof(campaignNameInput));
        }

        public static void ValidateCampaignDate(
            string campaignDateInput)
           
        {            

            if (string.IsNullOrWhiteSpace(campaignDateInput))
                throw new ArgumentException(
                    "Ogiltigt datum: får inte vara tomt.");

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
        public static void ValidateProductIdNumbers(string productIdNumbersInput)
        {
            if (productIdNumbersInput == null)
                throw new ArgumentNullException(nameof(productIdNumbersInput));

            if (!productIdNumbersInput.Any(i => i > 0))
                throw new ArgumentException("Minst ett giltigt produkt-id krävs.", nameof(productIdNumbersInput));
        }

        public static void ValidatePercent(string precentOffInput)
        {
            decimal value = ParseDecimalInvariant(precentOffInput);
            if (value <= 0m || value > 100m)
                throw new ArgumentException("Ogiltig procent: ange ett tal mellan 1 och 100.");
        }

        public static List<int> ParseProductIdNumbers(string productIdNumbersInput)
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

        public static decimal ParseDecimalInvariant(string percentOffInput)
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
