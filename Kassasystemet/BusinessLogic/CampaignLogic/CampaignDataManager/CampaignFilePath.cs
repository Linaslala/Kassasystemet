using System;
using System.IO;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager
{
    /// <summary>
    /// Innehåller central filväg för kampanjdata.
    /// 
    /// Ansvarar för att definiera var kampanjer sparas och läses ifrån,
    /// så att hela applikationen använder samma sökväg.
    /// </summary>
    internal static class CampaignFilePath
    {
        private const string CampaignFileName = "campaigns.txt";

        public static string Path
        {
            get
            {
                return System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    CampaignFileName
                );
            }
        }
    }
}