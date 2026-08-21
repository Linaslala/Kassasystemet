using Kassasystemet_refac.Data;
using System.Globalization;
using System.Text;
using static Kassasystemet_refac.SearchMemberMenu;

namespace Kassasystemet_refac
{
    public class SaveReceiptToFile : ISaveReceiptToFile
    {
        private const int Width = 41;
        private static readonly string equalsDivider = new string('=', Width);
        private static readonly string Dash = new string('-', Width);

        public void SaveAll(List<IReceiptModel> receipts)
        {
            receipts ??= new List<IReceiptModel>();

            var today = DateTime.Now.Date;

            var onlyTodaysReceipts = receipts
                .Where(r => r.ReceiptCreatedAt.Date == today)
                .GroupBy(r => r.ReceiptNumber)
                .Select(g => g.OrderByDescending(x => x.ReceiptCreatedAt).First())
                .OrderBy(r => r.ReceiptNumber)
                .ToList();

            var receiptPath = ReceiptFilePath.TodayReceiptPath;
            var receiptDirectory = Path.GetDirectoryName(receiptPath);
            if (!string.IsNullOrWhiteSpace(receiptDirectory) && !Directory.Exists(receiptDirectory))
                Directory.CreateDirectory(receiptDirectory);

            using var receiptWriter = new StreamWriter(receiptPath, append: false, Encoding.UTF8);

            foreach (var receipt in onlyTodaysReceipts)
            {
                WriteReceipt(receiptWriter, receipt);
                receiptWriter.WriteLine();
            }
        }

        private static void WriteReceipt(StreamWriter receiptWriter, IReceiptModel receipt)
        {
            receiptWriter.WriteLine(equalsDivider);
            receiptWriter.WriteLine($"KVITTO #{receipt.ReceiptNumber}");
            receiptWriter.WriteLine(receipt.ReceiptCreatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            if (receipt.MemberIdNumber != 0)
                receiptWriter.WriteLine($"Medlemsnummer: {receipt.MemberIdNumber}");

            receiptWriter.WriteLine(Dash);

            if (receipt.ReceiptRows != null && receipt.ReceiptRows.Any())
            {
                foreach (var row in receipt.ReceiptRows)
                {
                    if (row.ReceiptProductQuantity > 0)
                    {
                        var unitPrice = row.ReceiptProductAmount / row.ReceiptProductQuantity;

                        receiptWriter.WriteLine(
                            $"{row.ReceiptProductText} {row.ReceiptProductQuantity}st*{unitPrice.ToString("0.00", CultureInfo.InvariantCulture)} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        receiptWriter.WriteLine(
                            $"{row.ReceiptProductText} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}");
                    }
                }
            }

            receiptWriter.WriteLine(Dash);
            receiptWriter.WriteLine($"Totalt antal varor: {receipt.TotalItems}");
            receiptWriter.WriteLine($"TOTALT: {receipt.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");
            receiptWriter.WriteLine(equalsDivider);
        }
    }
}
