using System.Collections.Generic;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces
{
    public interface IReadAllCampaignsFromFile
    {
        List<ICampaignModel> ReadAll();
    }
}