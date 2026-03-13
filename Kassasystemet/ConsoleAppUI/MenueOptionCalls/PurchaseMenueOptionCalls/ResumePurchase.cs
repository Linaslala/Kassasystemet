using LinasKlubbLivs.ConsoleAppUI.HelpMethods;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager; 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LinasKlubbLivs.ConsoleAppUI.MenueOptionCalls.PurchaseMenueOptionCalls
{
    /// <summary>
    /// UI‑flöde för att uppdatera produktinformation
    /// från ett sparat kvitto (cart).
    /// </summary>
    public class ResumePurchase
    {
        public void Run()
        {
            if (!File.Exists(ReceiptFilePath.ReceiptDraftPath))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Det finns inget sparat pågående köp att återuppta.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            if (!TryLoadReceiptDraft(out int memberIdNUmber, out List<(int productIdNumber, int productQuantity)> items))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Det sparade köpet är skadat och kan inte återupptas.");
                Console.ResetColor();

                try { File.Delete(ReceiptFilePath.ReceiptDraftPath); } catch { }
                ValidatedConsoleInput.PauseCentered();
                return;
            }

            new CreateNewPurchase().Run(memberIdNUmber, items);
        }

        private static bool TryLoadReceiptDraft(out int memberIdNumber, out List<(int productIdNumber, int productQuantity)> items)
        {
            memberIdNumber = 0;
            items = new List<(int, int)>();

            string content = File.ReadAllText(ReceiptFilePath.ReceiptDraftPath);
            if (string.IsNullOrWhiteSpace(content))
                return false;

            var receiptParts = content.Split(';');
            if (receiptParts.Length < 2)
                return false;

            int.TryParse(receiptParts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out memberIdNumber);

            var receiptRows = receiptParts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in receiptRows)
            {
                var two = row.Split(',');
                if (two.Length != 2) continue;

                if (int.TryParse(two[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productIdNumber) &&
                    int.TryParse(two[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productQuantity) &&
                    productIdNumber > 0 && productQuantity > 0)
                {
                    items.Add((productIdNumber, productQuantity));
                }
            }

            return true;
        }
    }
}