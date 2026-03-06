using System.Collections.Generic;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces
{
    /// <summary>
    /// Sparar alla kampanjer till lagring (t.ex. textfil).
    /// </summary>
    public interface ISaveCampaignToFile
    {
        void SaveAll(List<ICampaignModel> campaigns);
    }
}
