namespace Kassasystemet_refac
{
    public class DeleteMember
    {
        public void Run()
        {
            IReadAllMembersFromFile memberReader = new ReadAllMembersFromFile();
            ISaveMemberToFile memberWriter = new SaveMemberToFile();
            ISearchMember memberFinder = new MemberSearch(memberReader);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Avsluta medlemsskap ==");

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
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Avsluta medlemskap ==");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        CenterConsoleOutput.CenterTextToWindow("Medlemmen du söker finns inte i systemet.");
                        Console.ResetColor();
                        Console.WriteLine();
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
                    CenterConsoleOutput.CenterTextToWindow("== Avsluta medlemsskap ==");
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

                    var arrowConfirm = new ConsoleOptionsArrow();
                    var confirmOptions = new[]
                    {
                        "Ja, radera medlem",
                        "Nej, tillbaka"
                    };

                    int deleteChoice = arrowConfirm.ShowArrow("Är du säker?", confirmOptions, renderAboveOptions: () =>
                    {
                        CenterConsoleOutput.CenterTextToWindow("Avsluta medelmskap:");
                        Console.WriteLine();

                        string infoHeader = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string infoRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(infoHeader);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', infoHeader.Length));
                        CenterConsoleOutput.CenterTextToWindow(infoRow);

                        Console.WriteLine();
                    });

                    if (deleteChoice != 0)
                    {
                        return;
                    }

                    var members = memberReader.ReadAll();
                    int removed = members.RemoveAll(m => m.MemberIdNumber == memberId);

                    if (removed == 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        CenterConsoleOutput.CenterTextToWindow("Kunde inte radera: medlem hittades inte längre i listan.");
                        Console.ResetColor();
                        ValidatedConsoleInput.PauseCentered();
                        return;
                    }

                    memberWriter.SaveAll(members);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    CenterConsoleOutput.CenterTextToWindow("Medlem raderad");
                    Console.ResetColor();

                    Console.WriteLine();

                    var afterDeleteMemberMenu = new ConsoleOptionsArrow();
                    var afterDeleteMemberOptions = new[]
                    {
                        "Radera en till medlem",
                        "Tillbaka till medlemssidan"
                    };

                    int afterDeleteMemberChoice = afterDeleteMemberMenu.ShowArrow("Välj:", afterDeleteMemberOptions);
                    if (afterDeleteMemberChoice == 0)
                        continue;

                    return;
                }
            }
        }

        private static IMemberModel SelectMember(List<IMemberModel> members)
        {
            var memberDisplay = members
                .Select(m => $"{m.MemberIdNumber,-6} {m.MemberFullName}")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj medlem:", memberDisplay);
            return members[index];
        }
    }
}
