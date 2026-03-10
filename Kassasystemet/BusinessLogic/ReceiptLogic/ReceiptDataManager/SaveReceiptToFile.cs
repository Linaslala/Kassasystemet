using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager
{
    /// <summary>
    /// Ansvarar för att spara kvitton till fil.
    ///
    /// Format:
    /// -----KVITTOSTART-----
    /// ReceiptNumber;MemberId;CreatedAt;TotalItems;TotalAmount;ReceiptRows
    /// -----KVITTOSLUT-----
    ///
    /// </summary>
    public class SaveReceiptToFile : ISaveReceiptToFile
    {
        private const string StartMarker = "-----KVITTOSTART-----";
        private const string EndMarker = "-----KVITTOSLUT-----";

        public void SaveAll(List<IReceiptModel> receipts)
        {
            receipts ??= new List<IReceiptModel>();

            var unique = receipts
                            .Where(r => r != null)
                            .GroupBy(r => r.ReceiptNumber)

                            .Select(g => g.OrderByDescending(x => x.ReceiptCreatedAt).First())
                            .OrderBy(r => r.ReceiptNumber)
                            .ToList();

            var path = ReceiptFilePath.TodayReceiptPath;
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // 2) Skriv till tempfil först
            var tempPath = path + ".tmp";

            using (var writer = new StreamWriter(tempPath, append: false, Encoding.UTF8))
            {
                foreach (var receipt in unique)
                {
                    writer.WriteLine(StartMarker);
                    writer.WriteLine(SerializeOneLine(receipt));
                    writer.WriteLine(EndMarker);
                }
            }

            File.Copy(tempPath, path, overwrite: true);
            File.Delete(tempPath);
        }

        private static string SerializeOneLine(IReceiptModel receipt)
        {
            string createdAt =
                receipt.ReceiptCreatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            string totalItems =
                receipt.TotalItems.ToString(CultureInfo.InvariantCulture);

            string totalAmount =
                receipt.TotalAmount.ToString(CultureInfo.InvariantCulture);

            string receiptRows = "";
            if (receipt.ReceiptRows != null && receipt.ReceiptRows.Count > 0)
            {
                receiptRows = string.Join("§",
                    receipt.ReceiptRows.Select(rr =>
                        $"{Escape(rr.ReceiptProductText)}\\n{rr.ReceiptProductAmount.ToString(CultureInfo.InvariantCulture)}"));
            }

            return $"{receipt.ReceiptNumber};{receipt.MemberIdNumber};{createdAt};{totalItems};{totalAmount};{receiptRows}";
        }

        private static string Escape(string text)
        {
            text ??= "";
            return text
                .Replace("%", "%25")
                .Replace(";", "%3B")
                .Replace("\\n", "%7C")
                .Replace("§", "%A7")
                .Trim();
        }
    }
}