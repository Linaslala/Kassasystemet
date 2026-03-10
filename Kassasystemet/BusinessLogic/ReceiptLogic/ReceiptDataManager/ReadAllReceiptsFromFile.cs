using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager
{
    /// <summary>
    /// Läser sparade kvitton från fil och återskapar ReceiptModel‑objekt.
    /// </summary>
    public class ReadAllReceiptsFromFile : IReadAllReceiptsFromFile
    {
        public List<IReceiptModel> ReadAll()
        {
            return ReadAllFromPath(ReceiptFilePath.TodayReceiptPath);
        }

        public List<IReceiptModel> ReadAllFromPath(string path)
        {
            var receiptsByNumber = new Dictionary<int, IReceiptModel>();

            if (!File.Exists(path))
                return new List<IReceiptModel>();

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line == "-----KVITTOSTART-----" || line == "-----KVITTOSLUT-----")
                    continue;

                var parts = line.Split(';');
                if (parts.Length < 6)
                    continue;

                if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int receiptNumber))
                    continue;

                if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int memberIdNumber))
                    memberIdNumber = 0;

                if (!DateTime.TryParseExact(
                        parts[2],
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime receiptCreatedAt))
                    continue;

                if (!int.TryParse(parts[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out int totalItems))
                    totalItems = 0;

                if (!decimal.TryParse(parts[4], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal totalAmount))
                    totalAmount = 0m;

                var receiptRows = ParseReceiptRows(parts[5]);

                var receipt = new ReceiptModel(
                    receiptNumber,
                    memberIdNumber,
                    receiptCreatedAt,
                    receiptRows,
                    totalItems,
                    totalAmount);

                if (receiptsByNumber.TryGetValue(receiptNumber, out var existing))
                {
                    if (receipt.ReceiptCreatedAt >= existing.ReceiptCreatedAt)
                        receiptsByNumber[receiptNumber] = receipt;
                }
                else
                {
                    receiptsByNumber[receiptNumber] = receipt;
                }
            }

            return receiptsByNumber.Values.ToList();
        }

        private static List<ReceiptRowModel> ParseReceiptRows(string serializedRows)
        {
            var receiptRows = new List<ReceiptRowModel>();
            if (string.IsNullOrWhiteSpace(serializedRows))
                return receiptRows;

            var rowParts = serializedRows.Split('§', StringSplitOptions.RemoveEmptyEntries);
            foreach (var rp in rowParts)
            {
                var two = rp.Split(new[] { "\\n" }, StringSplitOptions.None);
                if (two.Length != 2)
                    continue;

                string receiptText = Unescape(two[0]);
                if (!decimal.TryParse(two[1], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal receiptAmount))
                    continue;

                receiptRows.Add(new ReceiptRowModel(receiptText, receiptAmount));
            }
            return receiptRows;
        }

        private static string Unescape(string receiptText)
        {
            receiptText ??= "";
            return receiptText
                .Replace("%A7", "§")
                .Replace("%7C", "\\n")
                .Replace("%3B", ";")
                .Replace("%25", "%")
                .Trim();
        }
    }
}