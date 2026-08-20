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

            using var writer = new StreamWriter(receiptPath, append: false, Encoding.UTF8);

            foreach (var receipt in onlyTodaysReceipts)
            {
                WriteReceipt(writer, receipt);
                writer.WriteLine();
            }
        }

        private static void WriteReceipt(StreamWriter writer, IReceiptModel receipt)
        {
            writer.WriteLine(equalsDivider);
            writer.WriteLine($"KVITTO #{receipt.ReceiptNumber}");
            writer.WriteLine(receipt.ReceiptCreatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

            if (receipt.MemberIdNumber != 0)
                writer.WriteLine($"Medlemsnummer: {receipt.MemberIdNumber}");

            writer.WriteLine(Dash);

            if (receipt.ReceiptRows != null && receipt.ReceiptRows.Any())
            {
                foreach (var row in receipt.ReceiptRows)
                {
                    if (row.ReceiptProductQuantity > 0)
                    {
                        var unitPrice = row.ReceiptProductAmount / row.ReceiptProductQuantity;

                        writer.WriteLine(
                            $"{row.ReceiptProductText} {row.ReceiptProductQuantity}st*{unitPrice.ToString("0.00", CultureInfo.InvariantCulture)} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        writer.WriteLine(
                            $"{row.ReceiptProductText} {row.ReceiptProductAmount.ToString("0.00", CultureInfo.InvariantCulture)}");
                    }
                }
            }

            writer.WriteLine(Dash);
            writer.WriteLine($"Totalt antal varor: {receipt.TotalItems}");
            writer.WriteLine($"TOTALT: {receipt.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)} SEK");
            writer.WriteLine(equalsDivider);
        }
    }
}
