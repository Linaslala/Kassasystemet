
using LinasKlubbLivs.BusinessLogic.MemberLogic;
using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberDataManager;
using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.MemberMenueOptionCalls
{
    /// <summary>
    /// UI‑flöde för att uppdatera medlemsinformation.
    /// </summary>
    public class UpdateMember
    {
        public void Run()
        {
            IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
            ISaveMemberToFile writer = new SaveMemberToFile();
            ISearchMember finder = new MemberSearch(reader);


            while (true)
            {
                Console.Clear();
                CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlemsinformation ==");

                Console.WriteLine();

                string queryInput = UserInputPlacer.ReadCenteredText("Sök på medlemsnummer eller namn: ");
                var searchMemberResult = finder.Search(queryInput);


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
                            CenterConsoleOutput.CenterTextToWindow("== Uppdatera medlemsinformation ==");
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
                            "== Uppdatera medlem ==",
                            "Nytt förnamn: ",
                            ValidateMemberFirstName
                        );
                    }
                    else if (editChoice == 1)
                    {
                        memberLastName = ValidatedConsoleInput.ReadValidatedCenteredText(
                            "== Uppdatera medlem ==",
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
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            CenterConsoleOutput.CenterTextToWindow("Kunde inte spara: medlem hittades inte längre i listan.");
                            Console.ResetColor();
                            ValidatedConsoleInput.PauseCentered();
                            return;
                        }

                        members[index] = new MemberModel(memberId, memberFirstName, memberLastName);
                        writer.SaveAll(members);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;

                        CenterConsoleOutput.CenterTextToWindow("== Medlemsinformation uppdaterad ==");
                        Console.WriteLine();

                        string headerRow = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
                        string dataRow = $"{memberId,-20}{memberFirstName,-20}{memberLastName,-20}";

                        CenterConsoleOutput.CenterTextToWindow(headerRow);
                        CenterConsoleOutput.CenterTextToWindow(new string('-', headerRow.Length));
                        CenterConsoleOutput.CenterTextToWindow(dataRow);

                        Console.ResetColor();
                        ValidatedConsoleInput.PauseCentered();

                        var afterSaveMemberMenue = new ConsoleOptionsArrow();
                        var afterSaveMemberOptions = new[]
                        {
                            "Uppdatera en till medlem",
                            "Tillbaka till medlemssidan"
                        };

                        int afterChoice = afterSaveMemberMenue.ShowArrow("Välj:", afterSaveMemberOptions);
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
