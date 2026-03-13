using System;
using System.Globalization;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces;

namespace LinasKlubbLivs.ConsoleAppUI.HelpMethods
{
    /// <summary>
    /// Ansvarar för utskrift av kvitton i konsolen.
    /// 
    /// PrintDetailed:
    /// Skriver ut fullständig kvittostruktur:
    /// Kvittonummer
    /// Datum och tid
    /// Kundnummer (om finns)
    /// Alla kvittorader (produkter + rabatter)
    /// Totalt antal varor
    /// Totalsumma
    /// 
    /// UI:
    /// Alla rader och avdelare centreras via CenterConsoleOutput
    /// för konsekvent och läsbar presentation.
    /// Används av:
    /// Köpfunktionen
    /// Sök kvitto
    /// </summary>

    public static class ReceiptPrinter
    {
        public static void PrintDetailed(IReceiptModel receipt)
        {
            CenterConsoleOutput.CenterTextToWindow(new string('=', 41));

            CenterConsoleOutput.CenterTextToWindow($"KVITTO #{receipt.ReceiptNumber}");

            CenterConsoleOutput.CenterTextToWindow(receipt.ReceiptCreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

            if (receipt.MemberIdNumber != 0)
                CenterConsoleOutput.CenterTextToWindow($"Medlemsnummer: {receipt.MemberIdNumber}");

            CenterConsoleOutput.CenterTextToWindow(new string('-', 41));

            if (receipt.ReceiptRows != null)
            {
                foreach (var row in receipt.ReceiptRows)
                {
                    if (row.ReceiptProductQuantity > 0)
                    {
                        var unitPrice = row.ReceiptProductAmount / row.ReceiptProductQuantity;

                        CenterConsoleOutput.CenterTextToWindow(
                            $"{row.ReceiptProductText} {row.ReceiptProductQuantity}st*{unitPrice.ToString("0.00", CultureInfo.InvariantCulture)} " +
                            $"{row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}"
                        );
                    }
                    else
                    {
                        CenterConsoleOutput.CenterTextToWindow(
                            $"{row.ReceiptProductText} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}"
                        );
                    }
                }
            }

            CenterConsoleOutput.CenterTextToWindow(new string('-', 41));

            CenterConsoleOutput.CenterTextToWindow($"Totalt antal varor: {receipt.TotalItems}");
            CenterConsoleOutput.CenterTextToWindow($"TOTALT: {receipt.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");

            CenterConsoleOutput.CenterTextToWindow(new string('=', 41));
            Console.WriteLine();
        }
    }
}