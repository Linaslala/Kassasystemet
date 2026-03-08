using System.IO;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager
{
    /// <summary>
    /// Innehåller filväg för kvitton och pågående köp (utkast).
    /// Läser/Skriver från projektets Data-mapp.
    /// </summary>
    internal static class ReceiptFilePath
    {
        private static string EnsureDataDir()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static readonly string DataDir = EnsureDataDir();

        public static string ReceiptsPath => Path.Combine(DataDir, "receipts.txt");
        public static string ReceiptDraftPath => Path.Combine(DataDir, "receiptDraft.txt");
    }
}