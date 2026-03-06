using System;
using System.IO;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic.MemberDataManager
{
    /// <summary>
    /// Innehåller filväg för medlemsdata.
    /// 
    /// Säkerställer att samma datakälla används överallt i prodrammet.
    /// </summary>
    internal static class MemberFilePath
    {
        private static readonly string BaseDir =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LinasKlubbLivs"
            );

        public static string MembersPath =>
            Path.Combine(BaseDir, "members.txt");
    }
}