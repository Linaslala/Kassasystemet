using System;
using System.IO;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager
{
    /// <summary>
    /// Innehåller filväg för kvitton och pågående köp (utkast).
    /// </summary>
    internal static class ReceiptFilePath
    {
        private static readonly string BaseDir =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LinasKlubbLivs"
            );

        public static string ReceiptsPath =>
            Path.Combine(BaseDir, "receipts.txt");

        public static string ReceiptDraftPath =>
            Path.Combine(BaseDir, "receiptDraft.txt");
    }
}