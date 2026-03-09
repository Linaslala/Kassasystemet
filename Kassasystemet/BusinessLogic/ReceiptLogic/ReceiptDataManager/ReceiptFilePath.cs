using System.IO;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptDataManager
{
    /// <summary>
    /// Innehåller filväg för kvitton och pågående köp (utkast).
    /// Läser/Skriver från projektets TextFiles-mapp.
    /// </summary>
    internal static class ReceiptFilePath
    {
        private static string EnsureTextFilesDir()
        {
            var baseDir = AppContext.BaseDirectory;

            var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

            var textFilesDir = Path.Combine(projectDir, "TextFiles");
            Directory.CreateDirectory(textFilesDir);

            return textFilesDir;
        }

        private static readonly string TextFilesDir = EnsureTextFilesDir();

        public static string ReceiptsPath => Path.Combine(TextFilesDir, "receipts.txt");
        public static string ReceiptDraftPath => Path.Combine(TextFilesDir, "receiptDraft.txt");
    }
}