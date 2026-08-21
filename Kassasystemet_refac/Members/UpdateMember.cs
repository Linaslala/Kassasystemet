namespace Kassasystemet_refac
{
    public class UpdateMember
    {
        public void Run()
        {
            IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
            ISaveMemberToFile memberWriter = new SaveMemberToFile();
            ISearchMember memberFinder = new MemberSearch(reader);


            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlemsinformation ==");

                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText("Sök på medlemsnummer eller namn: ");
                var searchMemberResult = memberFinder.Search(queryInput);


                if (searchMemberResult.Count == 0)
                {
                    var arrowNoResult = new ConsoleOptionsArrow();
                    var noResultOptions = new[]
                    {
                        "Ny sökning",
                        "Tillbaka till medlemssidan"
                    };

                    int choice = arrowNoResult.ShowArrow("Välj:", noResultOptions, renderAboveOptions: () =>
                    {
                        CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlemsinformation ==");
                        NotificationService.ShowError(
                        "Medlemmen du söker finns inte i systemet.");
                    });

                    if (choice == 0)
                        continue;

                    return;
                }

                var selectedMember = searchMemberResult.Count == 1
                ? searchMemberResult[0]
                : SelectMember(searchMemberResult);

                int memberId = selectedMember.MemberIdNumber;
                string memberFirstName = selectedMember.MemberFirstName;
                string memberLastName = selectedMember.MemberLastName;

                while (true)
                {
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlem ==");
                    Console.WriteLine();
                    Console.WriteLine();

                    CenterConsoleOutput.CenterTextToWindow("Vald medlem:");
                    Console.WriteLine();

                    string infoHeader = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                    string infoRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                    CenterConsoleOutput.CenterTextToWindow(infoHeader);
                    CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                    CenterConsoleOutput.CenterTextToWindow(infoRow);

                    Console.WriteLine();

                    var arrowEdit = new ConsoleOptionsArrow();
                    var editOptions = new[]
                    {
                        "Ändra förnamn",
                        "Ändra efternamn",
                        "Spara\n",
                        "Avbryt"
                    };

                    int editChoice = arrowEdit.ShowArrow("Välj vad du vill ändra:", editOptions, renderAboveOptions: () =>
                    {
                        CenterConsoleOutput.CenterTextToWindow("Vald medlem:");
                        Console.WriteLine();

                        string infoHeader = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string infoRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(infoHeader);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                        CenterConsoleOutput.CenterTextToWindow(infoRow);

                        Console.WriteLine();
                    });

                    if (editChoice == 0)
                    {
                        memberFirstName = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera medlem ==\n",
                            "Nytt förnamn: ",
                            ValidateMemberFirstName
                        );
                    }
                    else if (editChoice == 1)
                    {
                        memberLastName = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera medlem ==\n",
                            "Nytt efternamn: ",
                            ValidateMemberLastName
                        );
                    }
                    else if (editChoice == 2)
                    {
                        var members = reader.ReadAll();
                        int index = members.FindIndex(m => m.MemberIdNumber == memberId);

                        if (index < 0)
                        {
                            NotificationService.ShowError(
                                "Kunde inte spara: Kunden finns inte");

                            ValidatedConsoleInput
                                .PauseCentered();

                            return;
                        }

                        members[index] = new MemberModel(memberId, memberFirstName, memberLastName);
                        memberWriter.SaveAll(members);

                        NotificationService.ShowSuccessHeader(
                           "=== Medlemsinformation uppdaterad ===");

                        string headerRow = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string dataRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(headerRow);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', headerRow.Length));
                        CenterConsoleOutput.CenterTextToWindow(dataRow);

                        Console.ResetColor();
                        ValidatedConsoleInput.PauseCentered();

                        var afterSaveMemberMenu = new ConsoleOptionsArrow();
                        var afterSaveMemberOptions = new[]
                        {
                            "Uppdatera en till medlem",
                            "Tillbaka till medlemssidan"
                        };

                        int afterChoice = afterSaveMemberMenu.ShowArrow("Välj:", afterSaveMemberOptions);
                        if (afterChoice == 0)
                            break;

                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private static void ValidateMemberFirstName(string memberFirstNameInput)
        {
            if (string.IsNullOrWhiteSpace(memberFirstNameInput))
                throw new ArgumentException("Ogiltigt förnamn: får inte vara tomt.");

            if (memberFirstNameInput.Any(char.IsDigit))
                throw new ArgumentException("Ogiltigt förnamn: får inte innehålla siffror.");
        }

        private static void ValidateMemberLastName(string memberLastNameInput)
        {
            if (string.IsNullOrWhiteSpace(memberLastNameInput))
                throw new ArgumentException("Ogiltigt efternamn: får inte vara tomt.");

            if (memberLastNameInput.Any(char.IsDigit))
                throw new ArgumentException("Ogiltigt efternamn: får inte innehålla siffror.");
        }
        private static IMemberModel SelectMember(List<IMemberModel> members)
        {
            var memberDisplay = members
                    .Select(m => $"{m.MemberIdNumber,-6} {m.MemberFullName}")
                    .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj Medlem:", memberDisplay);

            return members[index];
        }
    }
}
