namespace Kassasystemet_refac
{
    public class CreateNewMember
    {
        public void Run()
        {
            string memberHeader = "== Registrera ny medlem ==";
            string memberFirstNamePrompt = "Förnamn: ";
            string memberLastNamePrompt = "Efternamn: ";

            string memberFirstNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                memberHeader,
                memberFirstNamePrompt,
                MemberValidationService.ValidateMemberFirstName
            );

            string memberLastNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                memberHeader,
                memberLastNamePrompt,
                MemberValidationService.ValidateMemberLastName,
                clearConsoleEachAttempt: false
            );

            IReadAllMembersFromFile memberReader = new ReadAllMembersFromFile();
            ISaveMemberToFile memberWriter = new SaveMemberToFile();

            var members = memberReader.ReadAll();

            int newMemberIdNumber = members.Any()
                ? members.Max(m => m.MemberIdNumber) + 1
                : 1;

            members.Add(new MemberModel(
                newMemberIdNumber,
                memberFirstNameInput,
                memberLastNameInput));
            memberWriter.SaveAll(members);

            Console.Clear();

            NotificationService.ShowSuccessHeader(
                "=== Ny medlem sparad ===");

            //Run() vet bara "Visa den skapade medlemmen"
            //Jag skickar in den data metoden behöver
            //Metoden får datan via sina parametrar
            //newMemberIdNumber, memberFirstNameInput, memberLastNameInput är ARGUMENT
            //Vid metodanrop: ARGUMENT skickas in här ("ARGUMENT SKICKAS IN TILL PARAMETRAR")
            RenderCreatedMember(
                newMemberIdNumber,
                memberFirstNameInput,
                memberLastNameInput);

            //string memberHeaderRow =
            //    $"{"Medlemsnummer",-20}" +
            //    $"{"Förnamn",-20}" +
            //    $"{"Efternamn",-20}";

            //string memberDataRow =
            //    $"{newMemberIdNumber,-20}" +
            //    $"{memberFirstNameInput,-20}" +
            //    $"{memberLastNameInput,-20}";

            //CenterConsoleOutput.CenterTextToWindow(memberHeaderRow);
            //CenterConsoleOutput.CenterTextToWindow(new string('-', memberHeaderRow.Length));
            //CenterConsoleOutput.CenterTextToWindow(memberDataRow);

            Console.ResetColor();
            ValidatedConsoleInput.PauseCentered(
                "Tryck valfri tangent för att fortsätta...");


            if (ShowAfterCreateMenu())
            {
                Run();
                return;
            }
        }

        //RenderCreatedMember() vet "Hur medlemmen visas"
        //memberId, firstName, lastName är PARAMETRAR
        //Metoden får datan från argumenten i metodanropet.
        //Metoden använder parametrarna för att visa medlemmen
        public static void RenderCreatedMember(
            int memberId,
            string firstName,
            string lastName)
        {
            string header =
                $"{"Medlemsnummer",-20}" +
                $"{"Förnamn",-20}" +
                $"{"Efternamn",-20}";

            string row =
                $"{memberId,-20}" +
                $"{firstName,-20}" +
                $"{lastName,-20}";

            CenterConsoleOutput.CenterTextToWindow(header);
            CenterConsoleOutput.CenterTextToWindow(new string('-', row.Length));
            CenterConsoleOutput.CenterTextToWindow(row);
        }

        //Returnerar true om användaren vill registrera ytterligare en medlem.
        private static bool ShowAfterCreateMenu()
        {
            var menu = new ConsoleOptionsArrow();

            var options = new[]
            {
                "Registrera ny medlem",
                "Tillbaka till medlemssidan"
            };

            int choice = menu.ShowArrow(
                "Välj:",
                options);

            return choice == 0;

        }
    }
}
