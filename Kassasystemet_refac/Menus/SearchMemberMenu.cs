namespace Kassasystemet_refac
{
    public class SearchMemberMenu
    {
        public void Run()
        {
            IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
            ISearchMember memberFinder = new MemberSearch(reader);

            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Hitta medlem ==");
                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText("Sök på medlemsnummer eller namn (tryck enter för alla): ").Trim();

                var results = memberFinder.Search(queryInput);

                if (results.Count == 0)
                {
                    var arrowNoResult = new ConsoleOptionsArrow();
                    var noResultOptions = new[]
                    {
                        "Ny sökning",
                        "Tillbaka till huvudmenyn"
                    };

                    int choice = arrowNoResult.ShowArrow(
                        "Välj:",
                        noResultOptions,
                        renderAboveOptions: () =>
                        {
                            Console.Clear();
                            CenterConsoleOutput.CenterTextToWindow("== Hitta medlem ==");
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

                var selected = results.Count == 1
                         ? results[0]
                         : SelectMember(results);

                var arrowAfterFound = new ConsoleOptionsArrow();
                var afterFoundOptions = new[]
                {
                    "Ny sökning",
                    "Tillbaka till huvudmenyn"
                };

                int afterChoice = arrowAfterFound.ShowArrow(
                    "Välj:",
                    afterFoundOptions,
                    renderAboveOptions: () =>
                    {
                        Console.Clear();
                        CenterConsoleOutput.CenterTextToWindow("== Klubbmedlem ==");
                        Console.WriteLine();
                        Console.WriteLine();

                        string header = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string row = $"{selected.MemberIdNumber,-20}{selected.MemberFirstName,-20}{selected.MemberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(header);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', header.Length));
                        CenterConsoleOutput.CenterTextToWindow(row);

                        Console.WriteLine();
                    });

                if (afterChoice == 0)
                    continue;

                return;
            }
        }

        private static IMemberModel SelectMember(List<IMemberModel> members)
        {
            var memberDisplay = members
                .OrderBy(m => m.MemberIdNumber)
                .Select(m => $"{m.MemberIdNumber,-6} {m.MemberFullName}")
                .ToArray();

            var arrow = new ConsoleOptionsArrow();
            int index = arrow.ShowArrow("Välj medlem:", memberDisplay);
            return members[index];
        }
    }
}
