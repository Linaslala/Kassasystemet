using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Kassasystemet_refac
{
    internal class PurchaseViewRenderer
    {
        public static void RenderSplitPurchaseView(
                    int memberIdNumber,
                    List<CartItemModel> cart,
                    string topAction,
                    IReadOnlyList<string> footerOptions,
                    int selectedIndex)
        {
            Console.Clear();

            CenterConsoleOutput.CenterTextToWindow("== Registrera nytt köp ==");
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow("Ange kundnummer (eller lämna tomt om du vill lägga till senare):");
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow($"Kundnummer: {(memberIdNumber > 0 ? memberIdNumber.ToString(CultureInfo.InvariantCulture) : "")}");
            Console.WriteLine();
            Console.WriteLine();

            bool topSelected = selectedIndex == 0;
            if (topSelected) Console.ForegroundColor = ConsoleColor.Green;
            CenterConsoleOutput.CenterTextToWindow($"{(topSelected ? ">" : " ")} {topAction}");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine();

            CartService.PrintCart(cart);
            Console.WriteLine();

            CenterConsoleOutput.CenterTextToWindow($"Antal varor: {cart.Sum(x => x.ProductQuantity)}");
            CenterConsoleOutput.CenterTextToWindow($"Summa (utan rabatter): {cart.Sum(x => x.LineTotal).ToString("0.00", CultureInfo.InvariantCulture)} SEK");

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < footerOptions.Count; i++)
            {
                bool isSelected = selectedIndex == (i + 1);
                string line = isSelected ? $"> {footerOptions[i]}" : $"  {footerOptions[i]}";

                if (isSelected) Console.ForegroundColor = ConsoleColor.Green;
                CenterConsoleOutput.CenterTextToWindow(line);
                Console.ResetColor();
            }
        }
    }
}
