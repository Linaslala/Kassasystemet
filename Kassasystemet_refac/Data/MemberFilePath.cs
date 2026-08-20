namespace Kassasystemet_refac
{
    internal static class MemberFilePath
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

        public static string MembersPath => Path.Combine(TextFilesDir, "members.txt");
    }
}
