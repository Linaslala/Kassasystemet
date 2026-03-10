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
    /// Läser kvitton från fil och återskapar ReceiptModel-objekt.
    ///
    /// Stödjer två format:
    /// 1) "Presentationsformat" (snyggt kvitto, flerradigt):
    ///
    /// 2) "Maskinformat" (en rad med separatorer och escapes)
    ///
    /// Dedupe: ReceiptNumber (senaste ReceiptCreatedAt prioriteras).
    /// </summary>
    public class ReadAllReceiptsFromFile : IReadAllReceiptsFromFile
    {
        public List<IReceiptModel> ReadAll()
            => ReadAllFromPath(ReceiptFilePath.TodayReceiptPath);

        public List<IReceiptModel> ReadAllFromPath(string path)
        {
            if (!File.Exists(path))
                return new List<IReceiptModel>();

            var firstNonEmpty = File.ReadLines(path).FirstOrDefault(l => !string.IsNullOrWhiteSpace(l)) ?? "";
            bool looksLegacy =
                firstNonEmpty.Contains("-----KVITTOSTART-----", StringComparison.OrdinalIgnoreCase)
                || firstNonEmpty.Count(c => c == ';') >= 5;

            return looksLegacy ? ReadLegacySerialized(path) : ReadReceiptPresentationFormat(path);
        }

        private static List<IReceiptModel> ReadReceiptPresentationFormat(string path)
        {
            var lines = File.ReadAllLines(path);
            var byNumber = new Dictionary<int, IReceiptModel>();

            int i = 0;
            while (i < lines.Length)
            {
                var line = (lines[i] ?? "").Trim();

                if (!line.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                {
                    i++;
                    continue;
                }

                if (!TryParseReceiptNumber(line, out int receiptNumber))
                {
                    i++;
                    continue;
                }

                i++;

                if (!TryGetNextNonEmpty(lines, ref i, out var dateLine))
                    break;

                if (!DateTime.TryParseExact(
                        dateLine,
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var createdAt))
                {
                    ResyncToNextReceipt(lines, ref i);
                    continue;
                }

                i++;
                if (!TryGetNextNonEmpty(lines, ref i, out var memberLine))
                    break;

                if (!TryParseMemberLine(memberLine, out int memberIdNumber))
                {
                    ResyncToNextReceipt(lines, ref i);
                    continue;
                }

                i++;
                while (i < lines.Length && (IsSeparator(lines[i]) || string.IsNullOrWhiteSpace(lines[i])))
                    i++;

                var rowModels = new List<ReceiptRowModel>();
                while (i < lines.Length)
                {
                    var t = (lines[i] ?? "").Trim();

                    if (string.IsNullOrWhiteSpace(t) || IsSeparator(t))
                    {
                        i++;
                        continue;
                    }

                    if (t.StartsWith("Totalt antal varor:", StringComparison.OrdinalIgnoreCase) ||
                        t.StartsWith("TOTALT:", StringComparison.OrdinalIgnoreCase) ||
                        t.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    if (TryParseRow(t, out var rowText, out var amount))
                        rowModels.Add(new ReceiptRowModel(rowText, amount));

                    i++;
                }

                int totalItems = 0;
                decimal totalAmount = 0m;

                while (i < lines.Length)
                {
                    var t = (lines[i] ?? "").Trim();

                    if (string.IsNullOrWhiteSpace(t) || IsSeparator(t))
                    {
                        i++;
                        continue;
                    }

                    if (t.StartsWith("Totalt antal varor:", StringComparison.OrdinalIgnoreCase))
                    {
                        var part = t.Substring("Totalt antal varor:".Length).Trim();
                        int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out totalItems);
                        i++;
                        continue;
                    }

                    if (t.StartsWith("TOTALT:", StringComparison.OrdinalIgnoreCase))
                    {
                        var part = t.Substring("TOTALT:".Length).Trim();
                        part = part.Replace("SEK", "", StringComparison.OrdinalIgnoreCase).Trim();
                        decimal.TryParse(part, NumberStyles.Number, CultureInfo.InvariantCulture, out totalAmount);
                        i++;
                        continue;
                    }

                    if (t.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                        break;

                    i++;
                }

                var receipt = new ReceiptModel(
                    receiptNumber,
                    memberIdNumber,
                    createdAt,
                    rowModels,
                    totalItems,
                    totalAmount);

                if (byNumber.TryGetValue(receiptNumber, out var existing))
                {
                    if (receipt.ReceiptCreatedAt >= existing.ReceiptCreatedAt)
                        byNumber[receiptNumber] = receipt;
                }
                else
                {
                    byNumber[receiptNumber] = receipt;
                }
            }

            return byNumber.Values
                .OrderBy(r => r.ReceiptNumber)
                .ToList();
        }

        private static void ResyncToNextReceipt(string[] lines, ref int i)
        {
            while (i < lines.Length && !(lines[i] ?? "").Trim().StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                i++;
        }

        private static bool TryParseMemberLine(string memberLine, out int memberIdNumber)
        {
            memberIdNumber = 0;
            var t = (memberLine ?? "").Trim();

            const string kund = "Kundnummer:";
            const string medlem = "Medlemsnummer:";

            if (t.StartsWith(kund, StringComparison.OrdinalIgnoreCase))
            {
                var part = t.Substring(kund.Length).Trim();
                return int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out memberIdNumber);
            }

            if (t.StartsWith(medlem, StringComparison.OrdinalIgnoreCase))
            {
                var part = t.Substring(medlem.Length).Trim();
                return int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out memberIdNumber);
            }

            return false;
        }

        private static bool TryGetNextNonEmpty(string[] lines, ref int i, out string value)
        {
            while (i < lines.Length)
            {
                var t = (lines[i] ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(t))
                {
                    value = t;
                    return true;
                }
                i++;
            }
            value = "";
            return false;
        }

        private static bool IsSeparator(string? line)
        {
            var t = (line ?? "").Trim();
            if (t.Length == 0) return false;

            bool allEq = t.All(c => c == '=');
            bool allDash = t.All(c => c == '-');

            return (allEq && t.Length >= 10) || (allDash && t.Length >= 10);
        }

        private static bool TryParseReceiptNumber(string line, out int receiptNumber)
        {
            receiptNumber = 0;
            int idx = line.IndexOf('#');
            if (idx < 0) return false;

            var part = line.Substring(idx + 1).Trim();
            return int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out receiptNumber);
        }

        private static bool TryParseRow(string rowLine, out string text, out decimal amount)
        {
            text = "";
            amount = 0m;

            // "<text> <amount>"
            int lastSpace = rowLine.LastIndexOf(' ');
            if (lastSpace <= 0) return false;

            var amountPart = rowLine.Substring(lastSpace + 1).Trim();
            if (!decimal.TryParse(amountPart, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
                return false;

            text = rowLine.Substring(0, lastSpace).TrimEnd();
            return !string.IsNullOrWhiteSpace(text);
        }

        private static List<IReceiptModel> ReadLegacySerialized(string path)
        {
            var receiptsByNumber = new Dictionary<int, IReceiptModel>();

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