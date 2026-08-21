namespace Kassasystemet_refac
{
    public interface ICampaignModel
    {
        string CampaignName { get; }
        CampaignType TypeOfCampaign { get; }

        DateTime CampaignStartDate { get; }
        DateTime CampaignEndDate { get; }

        IReadOnlyList<int> ProductIdNumbers { get; }

        bool IsActive(DateTime now);

        string Serialize();
    }
}
