using System.Globalization;
using static Kassasystemet_refac.SearchMember;

namespace Kassasystemet_refac
{
    public class ReadAllReceiptsFromFile : IReadAllReceiptsFromFile
    {
        public List<IReceiptModel> ReadAll()
        {
            var baseDir = AppContext.BaseDirectory;
            var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
            var textFilesDir = Path.Combine(projectDir, "TextFiles");

            if (!Directory.Exists(textFilesDir))
                return new List<IReceiptModel>();

            var allReceipts = new List<IReceiptModel>();

            foreach (var file in Directory.EnumerateFiles(textFilesDir, "RECEIPT_*.txt"))
            {
                allReceipts.AddRange(ReadAllFromPath(file));
            }
            return allReceipts;
        }

        public List<IReceiptModel> ReadAllFromPath(string path)
        {
            if (!File.Exists(path))
                return new List<IReceiptModel>();

            return ReadReceiptPresentationFormat(path);
        }

        private static List<IReceiptModel> ReadReceiptPresentationFormat(string path)
        {
            var lines = File.ReadAllLines(path);
            var byNumber = new Dictionary<int, IReceiptModel>();

            int t = 0;
            while (t < lines.Length)
            {
                var line = (lines[t] ?? "").Trim();

                if (!line.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                {
                    t++;
                    continue;
                }

                if (!TryParseReceiptNumber(line, out int receiptNumber))
                {
                    t++;
                    continue;
                }

                t++;

                if (!TryGetNextNonEmpty(lines, ref t, out var dateTextLine))
                    break;

                if (!DateTime.TryParseExact(
                        dateTextLine,
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var createdAt))
                {
                    SkipToNextReceipt(lines, ref t);
                    continue;
                }

                t++;

                if (!TryGetNextNonEmpty(lines, ref t, out var memberLine))
                    break;

                if (!TryParseMemberLine(memberLine, out int memberIdNumber))
                {
                    SkipToNextReceipt(lines, ref t);
                    continue;
                }

                t++;

                while (t < lines.Length && (IsSeparator(lines[t]) || string.IsNullOrWhiteSpace(lines[t])))
                    t++;

                var rowModels = new List<ReceiptRowModel>();

                while (t < lines.Length)
                {
                    var rowLine = (lines[t] ?? "").Trim();

                    if (string.IsNullOrWhiteSpace((string)rowLine) || IsSeparator((string)rowLine))
                    {
                        t++;
                        continue;
                    }

                    if (rowLine.StartsWith("Totalt antal varor:", StringComparison.OrdinalIgnoreCase) ||
                        rowLine.StartsWith("TOTALT:", StringComparison.OrdinalIgnoreCase) ||
                        rowLine.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    if (TryParseRow(rowLine, out var rowText, out var rowQuantity, out var rowAmount))
                        rowModels.Add(new ReceiptRowModel(rowText, rowQuantity, rowAmount));

                    t++;
                }

                int totalItems = 0;
                decimal totalAmount = 0m;

                while (t < lines.Length)
                {
                    var totalsLine = (lines[t] ?? "").Trim();

                    if (string.IsNullOrWhiteSpace(totalsLine) || IsSeparator(totalsLine))
                    {
                        t++;
                        continue;
                    }

                    if (totalsLine.StartsWith("Totalt antal varor:", StringComparison.OrdinalIgnoreCase))
                    {
                        var part = totalsLine.Substring("Totalt antal varor:".Length).Trim();
                        int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out totalItems);
                        t++;
                        continue;
                    }

                    if (totalsLine.StartsWith("TOTALT:", StringComparison.OrdinalIgnoreCase))
                    {
                        var part = totalsLine.Substring("TOTALT:".Length).Trim();
                        part = part.Replace("SEK", "", StringComparison.OrdinalIgnoreCase).Trim();
                        decimal.TryParse(part, NumberStyles.Number, CultureInfo.InvariantCulture, out totalAmount);
                        t++;
                        continue;
                    }

                    if (totalsLine.StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                        break;

                    t++;
                }

                var receipt = new ReceiptModel(
                    receiptNumber,
                    memberIdNumber,
                    createdAt,
                    rowModels,
                    totalItems,
                    totalAmount);

                if (byNumber.TryGetValue(receiptNumber, out var receiptExists))
                {
                    if (receipt.ReceiptCreatedAt >= receiptExists.ReceiptCreatedAt)
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

        private static void SkipToNextReceipt(string[] lines, ref int i)
        {
            while (i < lines.Length && !(lines[i] ?? "").Trim().StartsWith("KVITTO #", StringComparison.OrdinalIgnoreCase))
                i++;
        }

        private static bool TryParseMemberLine(string memberLine, out int memberIdNumber)
        {
            memberIdNumber = 0;

            var m = (memberLine ?? "").Trim();

            const string member = "Medlemsnummer:";

            if (!m.StartsWith(member, StringComparison.OrdinalIgnoreCase))
                return false;

            var part = m.Substring(member.Length).Trim();
            return int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out memberIdNumber);
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
            int receiptIdIndex = line.IndexOf('#');
            if (receiptIdIndex < 0) return false;

            var part = line.Substring(receiptIdIndex + 1).Trim();
            return int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out receiptNumber);
        }

        private static bool TryParseRow(string rowLine, out string rowText, out int rowQuantity, out decimal rowAmount)
        {
            rowText = "";
            rowQuantity = 0;
            rowAmount = 0m;

            int lastSpace = rowLine.LastIndexOf(' ');
            if (lastSpace <= 0) return false;

            var amountPart = rowLine.Substring(lastSpace + 1).Trim();
            if (!decimal.TryParse(amountPart, NumberStyles.Number, CultureInfo.InvariantCulture, out rowAmount))
                return false;

            var left = rowLine.Substring(0, lastSpace).TrimEnd();

            int lastSpaceIndex = left.LastIndexOf(' ');

            if (lastSpaceIndex > 0)
            {
                var mabyeQuantityToken = left.Substring(lastSpaceIndex + 1).Trim();
                int stIndex = mabyeQuantityToken.IndexOf("st*", StringComparison.OrdinalIgnoreCase);
                if (stIndex > 0 && int.TryParse(mabyeQuantityToken.Substring(0, stIndex), NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedQuantity))
                {
                    rowQuantity = parsedQuantity;
                    rowText = left.Substring(0, lastSpaceIndex).TrimEnd();
                    return !string.IsNullOrWhiteSpace(rowText);
                }
            }

            rowText = left.Trim();
            rowQuantity = 0;
            return !string.IsNullOrWhiteSpace(rowText);
        }
    }
}
