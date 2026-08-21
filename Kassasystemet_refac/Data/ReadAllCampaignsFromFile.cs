namespace Kassasystemet_refac
{
    public class ReadAllCampaignsFromFile : IReadAllCampaignsFromFile
    {
        public List<ICampaignModel> ReadAll()
        {
            var campaigns = new List<ICampaignModel>();

            if (!File.Exists(CampaignFilePath.Path))
                return campaigns;

            var lines = File.ReadAllLines(CampaignFilePath.Path);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');

                try
                {
                    campaigns.Add(
                        CampaignFactory.Create(parts));
                }
                catch
                {
                    continue;
                }
            }

            return campaigns;

        }
    }
}

