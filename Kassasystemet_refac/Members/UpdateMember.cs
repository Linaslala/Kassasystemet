namespace Kassasystemet_refac
{
    public class UpdateMember
    {
        //Run() ska bara beskriva arbetsflödet.
        public void Run()
        {
            IReadAllMembersFromFile memberReader = new ReadAllMembersFromFile();
            ISaveMemberToFile memberWriter = new SaveMemberToFile();
            ISearchMember memberFinder = new MemberSearch(memberReader);


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
                        Console.WriteLine();
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

                    RenderSelectedMember(
                      memberId,
                      memberFirstName,
                      memberLastName);

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
                        RenderSelectedMember(
                             memberId,
                             memberFirstName,
                             memberLastName);
                    });


                    MemberEditResult result =
                        HandleEditChoice(
                            editChoice,
                            memberId,
                            ref memberFirstName,
                            ref memberLastName,
                            memberReader,
                            memberWriter);

                    if (result == MemberEditResult.Continue)
                    {
                        continue;
                    }

                    if (result == MemberEditResult.Exit)
                    {
                        return;
                    }

                    if (result == MemberEditResult.Saved)
                    {
                        NotificationService.ShowSuccessHeader(
                        "=== Medlemsinformation uppdaterad ===");

                        RenderSelectedMember(
                            memberId,
                            memberFirstName,
                            memberLastName);

                        ValidatedConsoleInput.PauseCentered();

                        if (ShowAfterSaveMenu())
                        {
                            break;
                        }

                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
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

        private static void RenderSelectedMember(
            int memberId,
            string firstName,
            string lastName)
        {
            CenterConsoleOutput.CenterTextToWindow(
                "Vald medlem:");

            Console.WriteLine();

            string header =
                $"{"Medlemsnummer",-20}" +
                $"{"Förnamn",-20}" +
                $"{"Efternamn",-20}";

            string row =
                $"{memberId,-20}" +
                $"{firstName,-20}" +
                $"{lastName,-20}";

            CenterConsoleOutput.CenterTextToWindow(header);

            CenterConsoleOutput.CenterTextToWindow(
                new string('-', header.Length));

            CenterConsoleOutput.CenterTextToWindow(
                row);

            Console.WriteLine();
        }

        //Metoden:
        //1. läser alla medlemmar
        //2. Hittar rätt medlem
        //3. Ersätter medlemmen
        //4.Sparar listan
        //5. Visar resultat
        //Returnerar true om sparningen lyckats
        //Returnerar false om medlemmen inte hittades
        private static bool SaveMemberChanges(
            int memberId,
            string memberFirstName,
            string memberLastName,
            IReadAllMembersFromFile memberReader,
            ISaveMemberToFile memberWriter)
        {
            var members = memberReader.ReadAll();
            int index = members.FindIndex(m => m.MemberIdNumber == memberId);

            if (index < 0)
            {
                NotificationService.ShowError(
                    "Kunde inte spara: Kunden finns inte");

                ValidatedConsoleInput
                    .PauseCentered();

                return false;
            }

            members[index] = new MemberModel(memberId, memberFirstName, memberLastName);
            memberWriter.SaveAll(members);

            return true;
        }

        //Extract Workflow Methods
        //Visar menyn efter att en medlem sparats.
        //Returnerar true om användaren vill uppdatera ytterligare en medlem
        private static bool ShowAfterSaveMenu()
        {
            var afterSaveMenu = new ConsoleOptionsArrow();
            var afterSaveOptions = new[]
            {
                "Uppdatera en till medlem",
                "Tillbaka till medlemssidan"
            };

            int choice =
                afterSaveMenu.ShowArrow(
                    "Välj:",
                    afterSaveOptions);

            return choice == 0;
        }

        //Hanterar användarens val i redigeringsmenyn
        //Metoden ansvarar för att uppdatera förnamn, efternamn och spara medlem
        //samt avbryta redigeringen.
        //Returnerar ett resultat som talar om vad användaren gjorde
        private static MemberEditResult HandleEditChoice(
            int editChoice,
            int memberId,
            ref string memberFirstName,
            ref string memberLastName,
            IReadAllMembersFromFile memberReader,
            ISaveMemberToFile memberWriter)
        {
            if (editChoice == 0)
            {
                memberFirstName = ValidatedConsoleInput
                    .ReadValidatedCenteredText(
                        "== Uppdatera medlem ==\n",
                        "Nytt förnamn: ",
                        MemberValidationService.ValidateMemberFirstName);

                return MemberEditResult.Continue;

            }

            if (editChoice == 1)
            {
                memberLastName = ValidatedConsoleInput
                    .ReadValidatedCenteredText(
                        "== Uppdatera medlem ==\n",
                        "Nytt efternamn: ",
                        MemberValidationService.ValidateMemberLastName);

                return MemberEditResult.Continue;
            }

            if (editChoice == 2)
            {
                bool saved =
                     SaveMemberChanges(
                         memberId,
                         memberFirstName,
                         memberLastName,
                         memberReader,
                         memberWriter);

                if (saved)
                {
                    return MemberEditResult.Saved;
                }

                return MemberEditResult.Exit;

            }

            return MemberEditResult.Exit;
        }

    }
}

