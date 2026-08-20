namespace Kassasystemet_refac.Data
{
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

        public static string TodayReceiptPath =>
            Path.Combine(TextFilesDir, $"RECEIPT_{DateTime.Now:yyyyMMdd}.txt");

        public static string ReceiptDraftPath =>
            Path.Combine(TextFilesDir, "receiptDraft.txt");
    }
}
