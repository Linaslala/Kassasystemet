namespace Kassasystemet_refac
{
    //Samlar all affärslogik som avgör om medlemsdata är giltlig
    //Dessa regler ska bara finnas på ett ställe i systemet
    public static class MemberValidationService
    {
        public static void ValidateMemberFirstName(string memberFirstNameInput)
        {
            if (string.IsNullOrWhiteSpace(memberFirstNameInput))
                throw new ArgumentException("Ogiltigt förnamn: får inte vara tomt.");

            if (memberFirstNameInput.Any(char.IsDigit))
                throw new ArgumentException("Ogiltigt förnamn: får inte innehålla siffror.");
        }

        public static void ValidateMemberLastName(string memberLastNameInput)
        {
            if (string.IsNullOrWhiteSpace(memberLastNameInput))
                throw new ArgumentException("Ogiltigt efternamn: får inte vara tomt.");

            if (memberLastNameInput.Any(char.IsDigit))
                throw new ArgumentException("Ogiltigt efternamn: får inte innehålla siffror.");
        }
    }
}
