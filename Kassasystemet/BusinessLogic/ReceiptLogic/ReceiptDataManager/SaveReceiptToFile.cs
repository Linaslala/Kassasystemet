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
    /// Sparar kvitton i MÄNSKLIGT LÄSBART format
    /// (samma layout som i försäljningsrapporten).
    /// Varje dag -> en fil: RECEIPT_yyyyMMdd.txt
    /// </summary>
    public class SaveReceiptToFile : ISaveReceiptToFile
    {
        private const int Width = 41;
        private static readonly string Equals = new string('=', Width);
        private static readonly string Dash = new string('-', Width);

        public void SaveAll(List<IReceiptModel> receipts)
        {
            receipts ??= new List<IReceiptModel>();

            var today = DateTime.Now.Date;

            var onlyToday = receipts
                .Where(r => r.ReceiptCreatedAt.Date == today)
                .GroupBy(r => r.ReceiptNumber)
                .Select(g => g.OrderByDescending(x => x.ReceiptCreatedAt).First())
                .OrderBy(r => r.ReceiptNumber)
                .ToList();

            var path = ReceiptFilePath.TodayReceiptPath;
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using var writer = new StreamWriter(path, append: false, Encoding.UTF8);

            foreach (var receipt in onlyToday)
            {
                WriteReceipt(writer, receipt);
                writer.WriteLine(); 
            }
        }

        private static void WriteReceipt(StreamWriter writer, IReceiptModel receipt)
        {
            writer.WriteLine(Equals);
            writer.WriteLine($"KVITTO #{receipt.ReceiptNumber}");
            writer.WriteLine(receipt.ReceiptCreatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            if (receipt.MemberIdNumber != 0)
                writer.WriteLine($"Medlemsnummer: {receipt.MemberIdNumber}");

            writer.WriteLine(Dash);

            if (receipt.ReceiptRows != null && receipt.ReceiptRows.Any())
            {
                foreach (var row in receipt.ReceiptRows)
                {
                    writer.WriteLine(
                        $"{row.ReceiptProductText} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}");
                }
            }

            writer.WriteLine(Dash);
            writer.WriteLine($"Totalt antal varor: {receipt.TotalItems}");
            writer.WriteLine($"TOTALT: {receipt.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");
            writer.WriteLine(Equals);
        }
    }
}