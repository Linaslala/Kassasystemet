using System.Globalization;

namespace Kassasystemet_refac
{
    internal class PurchaseInputService
    {
        public static int ReadCustomerNumberOrSkip()
        {
            Console.Clear();
            CenterConsoleOutput.CenterTextToWindow("== Registrera nytt köp ==");
            Console.WriteLine();
            CenterConsoleOutput.CenterTextToWindow("Ange kundnummer (eller lämna tomt om du vill lägga till senare):");
            Console.WriteLine();

            string input = UserInputPlacer.ReadCenteredText("Kundnummer: ").Trim();

            if (string.IsNullOrWhiteSpace(input))
                return 0;

            if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) || value <= 0)
                return 0;

            return value;
        }

        public static int ReadMemberIdNumber()
        {
            string input = ValidatedConsoleInput.ReadValidatedCenteredText(
                "== Kundnummer ==",
                "Kundnummer: ",
                ValidatePositiveInt);

            return int.Parse(input.Trim(), CultureInfo.InvariantCulture);
        }

        public static void ValidatePositiveInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Får inte vara tomt.");

            if (!int.TryParse(input.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                throw new ArgumentException("Måste vara ett heltal.");

            if (value <= 0)
                throw new ArgumentException("Måste vara större än 0.");
        }
    }
}
