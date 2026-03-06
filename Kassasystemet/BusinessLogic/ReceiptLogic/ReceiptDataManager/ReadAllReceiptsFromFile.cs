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
    /// Läser alla sparade kvitton från fil och återskapar ReceiptModel‑objekt.
    /// 
    /// Funktion:
    /// - Läser en kvittorad per rad från fil.
    /// - Parserar grunddata (nummer, kund, datum, totaler).
    /// - Återskapar alla kvittorader (produkter, rabatter).
    /// 
    /// - Felaktiga eller skadade rader hoppas över.
    /// - Tom eller saknad fil ger tom lista (ingen exception).
    /// 
    /// Används av:
    /// - Försäljningsrapport
    /// - Sök kvitto
    /// - Kvittohistorik
    /// </summary>
    public class ReadAllReceiptsFromFile : IReadAllReceiptsFromFile
    {
        public List<IReceiptModel> ReadAll()
        {
            var receipts = new List<IReceiptModel>();

            if (!File.Exists(ReceiptFilePath.ReceiptsPath))
                return receipts;

            foreach (var line in File.ReadAllLines(ReceiptFilePath.ReceiptsPath))
            {
                if (string.IsNullOrWhiteSpace(line))
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

                receipts.Add(new ReceiptModel(
                    receiptNumber,
                    memberIdNumber,
                    receiptCreatedAt,
                    receiptRows,
                    totalItems,
                    totalAmount));
            }

            return receipts;
        }

        private static List<ReceiptRowModel> ParseReceiptRows(string serializedRows)
        {
            var receiptRows = new List<ReceiptRowModel>();

            if (string.IsNullOrWhiteSpace(serializedRows))
                return receiptRows;

            // Rows = Text|Amount§Text|Amount§...
            var rowParts = serializedRows.Split('§', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rp in rowParts)
            {
                var two = rp.Split('|');
                if (two.Length != 2)
                    continue;

                string receiptText = Unescape(two[0]);

                if (!decimal.TryParse(two[1], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal receiptAmount))
                    continue;

                receiptRows.Add(new ReceiptRowModel(receiptText, receiptAmount));
            }

            return receiptRows;
        }
        // "Escapar tillbaka" specialtecken som används som separatorer i kvittofilen.
        // För att texten ska läsas tillbaka så som den ser ut.
        private static string Unescape(string receiptText)
        {
            receiptText ??= "";
            return receiptText
                .Replace("%A7", "§")
                .Replace("%7C", "|")
                .Replace("%3B", ";")
                .Replace("%25", "%")
                .Trim();
        }
    }
}