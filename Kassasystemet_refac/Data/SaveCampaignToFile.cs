using System.Globalization;

namespace Kassasystemet_refac
{
    public class SaveCampaignToFile : ISaveCampaignToFile
    {
        public void SaveAll(List<ICampaignModel> campaigns)
        {
            campaigns ??= new List<ICampaignModel>();

            var lines = campaigns
                .Select(c => c.Serialize())
                .ToList();

            File.WriteAllLines(CampaignFilePath.Path, lines);
        }
    }
}
