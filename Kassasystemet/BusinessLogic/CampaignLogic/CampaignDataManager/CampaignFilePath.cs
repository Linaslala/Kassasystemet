using System.IO;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager
{
    /// <summary>
    /// Innehåller central filväg för kampanjdata.
    /// Läser/Skriver från projektets TextFiles-mapp.
    /// </summary>
    internal static class CampaignFilePath
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

        public static string Path => System.IO.Path.Combine(TextFilesDir, "campaigns.txt");
    }
}