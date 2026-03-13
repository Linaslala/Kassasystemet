using System.Collections.Generic;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces
{
    public interface ISaveCampaignToFile
    {
        void SaveAll(List<ICampaignModel> campaigns);
    }
}
