using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.MemberMenueOptionCalls;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.ConsoleAppUI.Menues
{
    public class MemberMenue
    {
        private readonly string[] _memberMenueOptions =
    {
            "Redistrera ny medlem\n",
            "Uppdatera klubbmedlem",
            "Lista alla klubbmedlemmar",
            "Avsluta medlemsskap\n",
            "Tillbaka till huvudmenyn"
        };

        public void Run()
        {
            var arrowMemberMenu = new ConsoleOptionsArrow();

            while (true)
            {
                // ShowArrow sköter render + upp/ner + enter och returnerar valt index
                int selectedIndex = arrowMemberMenu.ShowArrow("=== Medlemssida ===", _memberMenueOptions);

                // HandleSelection avgör om vi ska avsluta (Logga ut = true)
                if (HandleMemberMenueSelection(selectedIndex))
                    return;
            }
        }
        private static bool HandleMemberMenueSelection(int index)
        {
            switch (index)
            {
                case 0:
                    new CreateNewMember().Run();
                    return false;

                case 1:
                    new UpdateMember().Run();
                    return false;

                case 2:
                    new ListAllMembers().Run();
                    return false;

                case 3:
                    new DeleteMember().Run();
                    return false;

                case 4:
                    return true;

                default:
                    Console.Clear();
                    CenterConsoleOutput.CenterTextToWindow("Tryck valfri tangent...");
                    Console.ReadKey(true);
                    return false;
            }
        }
    }
}

