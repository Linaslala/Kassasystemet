using System.IO;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic.MemberDataManager
{
    /// <summary>
    /// Innehåller filväg för medlemsdata.
    /// Säkerställer att samma datakälla används överallt i programmet.
    /// Läser/Skriver från projektets Data-mapp.
    /// </summary>
    internal static class MemberFilePath
    {
        private static string EnsureDataDir()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static readonly string DataDir = EnsureDataDir();

        public static string MembersPath => Path.Combine(DataDir, "members.txt");
    }
}
