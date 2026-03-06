using LinasKlubbLivs.BusinessLogic.MemberLogic;
using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberDataManager;
using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces;
using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using System.Linq;
using System;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.MemberMenueOptionCalls
{
    /// <summary>
    /// UI‑flöde för att skapa nya medlemmar.
    /// </summary>
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
                ValidateMemberFirstName
            );

            string memberLastNameInput = ValidatedConsoleInput.ReadValidatedCenteredText(
                memberHeader,
                memberLastNamePrompt,
                ValidateMemberLastName,
                clearConsoleEachAttempt: false
            );

            IReadAllMembersFromFile reader = new ReadAllMembersFromFile();
            ISaveMemberToFile writer = new SaveMemberToFile();

            var members = reader.ReadAll();

            int newMemberIdNumber = members.Any()
                ? members.Max(m => m.MemberIdNumber) + 1
                : 1;

            members.Add(new MemberModel(newMemberIdNumber, memberFirstNameInput, memberLastNameInput));
            writer.SaveAll(members);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            CenterConsoleOutput.CenterTextToWindow("Ny medlem sparad:");
            Console.WriteLine();

            string memberHeaderRow = $"{"Medlemsnummer",-20}{"Förnamn",-20}{"Efternamn",-20}";
            string memberDataRow = $"{newMemberIdNumber,-20}{memberFirstNameInput,-20}{memberLastNameInput,-20}";

            CenterConsoleOutput.CenterTextToWindow(memberHeaderRow);
            CenterConsoleOutput.CenterTextToWindow(new string('-', memberHeaderRow.Length));
            CenterConsoleOutput.CenterTextToWindow(memberDataRow);

            Console.ResetColor();
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att fortsätta...");


            var afterSaveMemberMenu = new ConsoleOptionsArrow();
            var afterSaveMemberOptions = new[]
            {
                "Registrera ny medlem",
                "Tillbaka till medlemssidan"
            };

            int choice = afterSaveMemberMenu.ShowArrow("Välj:", afterSaveMemberOptions);
            if (choice == 0)
            {
                Run();
                return;
            }
            return;
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
    }
}