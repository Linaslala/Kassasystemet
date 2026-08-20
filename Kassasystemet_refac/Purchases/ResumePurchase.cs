using Kassasystemet_refac.Data;
using System.Globalization;

namespace Kassasystemet_refac
{
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

            if (!DraftPurchaseService.TryLoadReceiptDraft(out int memberIdNUmber, out List<(int productIdNumber, int productQuantity)> items))
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
    }
}
