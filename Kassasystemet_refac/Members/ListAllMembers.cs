namespace Kassasystemet_refac
{
    public class ListAllMembers
    {
        public void Run()
        {
            Console.Clear();

            CenterConsoleOutput.CenterTextToWindow("== Alla medlemmar ==");
            Console.WriteLine();

            IReadAllMembersFromFile memberReader = new ReadAllMembersFromFile();
            var members = memberReader.ReadAll()
                .OrderBy(m => m.MemberIdNumber)
                .ToList();

            if (!members.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Inga medlemmar finns registrerade.");
                Console.ResetColor();

                Console.WriteLine();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            string memberHeader =
                $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
            CenterConsoleOutput.CenterTextToWindow(memberHeader);
            CenterConsoleOutput.CenterTextToWindow(new string('-', memberHeader.Length));

            foreach (var member in members)
            {
                string row =
                    $"{member.MemberIdNumber,-20}{member.MemberFirstName,-20}{member.MemberLastName,-20}";
                CenterConsoleOutput.CenterTextToWindow(row);
            }

            Console.WriteLine();
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");
        }
    }
}
