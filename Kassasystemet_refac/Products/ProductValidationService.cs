namespace Kassasystemet_refac
{
    public static class ProductValidationService
    {
        public static void ValidateProductName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Ogiltigt produktnamn: får inte vara tomt.");
        }

        public static void ValidateProductPrice(string productPriceInput)
        {
            if (string.IsNullOrWhiteSpace(productPriceInput))
                throw new ArgumentException("Ogiltigt pris: får inte vara tomt.");

            if (!decimal.TryParse(productPriceInput, out _))
                throw new ArgumentException("Ogiltigt pris: måste vara ett giltigt nummer.");
        }

        public static void ValidateProductPriceType(string productPriceType)
        {
            if (string.IsNullOrWhiteSpace(productPriceType))
                throw new ArgumentException("Ogiltigt produkttyp: får inte vara tomt.");

            if (productPriceType.Any(char.IsDigit))
                throw new ArgumentException("Ogiltig produktpristyp: måste ange styckpris eller kilopris");
        }
    }
}
