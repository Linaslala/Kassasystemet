using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces
{
    public interface ISearchCampaign
    {
        List<ICampaignModel> Search(string searchCampaignText);
    }
}
