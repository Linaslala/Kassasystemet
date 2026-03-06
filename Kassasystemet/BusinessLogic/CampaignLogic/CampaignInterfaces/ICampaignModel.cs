using System;
using System.Collections.Generic;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces
{
    public interface ICampaignModel
    {
        string CampaignName { get; }
        CampaignType TypeOfCampaign { get; }

        DateTime CampaignStartDate { get; }
        DateTime CampaignEndDate { get; }

        IReadOnlyList<int> ProductIdNumbers { get; }

        bool IsActive(DateTime now);
    }
}
