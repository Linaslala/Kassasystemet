namespace Kassasystemet_refac
{
    internal static class ProductFilePath
    {
        private static string EnsureTextFilesDir()
        {
            var baseDir = AppContext.BaseDirectory;

            var projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

            var textFilesDir = System.IO.Path.Combine(projectDir, "TextFiles");

            Directory.CreateDirectory(textFilesDir);
            return textFilesDir;
        }

        private static readonly string TextFilesDir = EnsureTextFilesDir();

        public static string Path => System.IO.Path.Combine(TextFilesDir, "products.txt");
    }
}
