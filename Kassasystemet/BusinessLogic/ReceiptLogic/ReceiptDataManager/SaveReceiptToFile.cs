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
    /// Ansvarar för att spara kvitton till fil.
    /// 
    /// Varje kvitto sparas som en rad i textfilen:
    /// ReceiptNumber;MemberId;CreatedAt;TotalItems;TotalAmount;ReceiptRows
    /// 
    /// ReceiptRows sparas med specialseparatorer (§ och radbrytning)
    /// 
    /// Design:
    /// - Samma filbaserade strategi som produkter, medlemmar och kampanjer.
    /// - Ingen affärslogik – endast spara till fil.
    /// </summary>
    public class SaveReceiptToFile : ISaveReceiptToFile
    {
        public void SaveAll(List<IReceiptModel> receipts)
        {
            receipts ??= new List<IReceiptModel>();

            var lines = receipts.Select(Serialize).ToList();
            File.WriteAllLines(ReceiptFilePath.ReceiptsPath, lines);
        }

        private static string Serialize(IReceiptModel receipt)
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
                    receipt.ReceiptRows.Select(receiptRow =>
                        $"{Escape(receiptRow.ReceiptProductText)}|{receiptRow.ReceiptProductAmount.ToString(CultureInfo.InvariantCulture)}"));
            }

            return $"{receipt.ReceiptNumber};{receipt.MemberIdNumber};{createdAt};{totalItems};{totalAmount};{receiptRows}";
        }


        // Escapar specialtecken som används som separatorer i kvittofilen.
        // Krävs för att serialisering/deserialisering ska fungera korrekt
        private static string Escape(string text)
        {
            text ??= "";
            return text
                .Replace("%", "%25")
                .Replace(";", "%3B")
                .Replace("|", "%7C")
                .Replace("§", "%A7")
                .Trim();
        }
    }
}