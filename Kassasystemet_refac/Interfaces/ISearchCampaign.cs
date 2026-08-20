namespace Kassasystemet_refac
{
    public interface ISearchCampaign
    {
        List<ICampaignModel> Search(string searchCampaignText);
    }
}
