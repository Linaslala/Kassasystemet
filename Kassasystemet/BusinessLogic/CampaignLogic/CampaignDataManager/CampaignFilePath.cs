using System.IO;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager
{
    /// <summary>
    /// Innehåller central filväg för kampanjdata.
    /// Läser/Skriver från projektets Data-mapp.
    /// </summary>
    internal static class CampaignFilePath
    {
        private static string EnsureDataDir()
        {
            var dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static readonly string DataDir = EnsureDataDir();

        public static string Path => System.IO.Path.Combine(DataDir, "campaigns.txt");
    }
}