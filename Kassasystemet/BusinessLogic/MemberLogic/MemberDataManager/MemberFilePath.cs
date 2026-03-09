using System.IO;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic.MemberDataManager
{
    /// <summary>
    /// Innehåller filväg för medlemsdata.
    /// Säkerställer att samma datakälla används överallt i programmet.
    /// Läser/Skriver från projektets TextFiles-mapp.
    /// </summary>
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