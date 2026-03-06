using System;
using System.Collections.Generic;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic
{
    /// <summary>
    /// Basmodell för kampanjer.
    /// </summary>
    public class CampaignModel : ICampaignModel
    {
        public string CampaignName { get; }
        public CampaignType TypeOfCampaign { get; }
        public DateTime CampaignStartDate { get; }
        public DateTime CampaignEndDate { get; }
        public IReadOnlyList<int> ProductIdNumbers { get; }

        public CampaignModel(
            string campaignName,
            CampaignType typeOfCampaign,
            DateTime campaignStartDate,
            DateTime campaignEndDate,
            IReadOnlyList<int> productIdNumbers)
        {
            CampaignName = campaignName;
            TypeOfCampaign = typeOfCampaign;
            CampaignStartDate = campaignStartDate;
            CampaignEndDate = campaignEndDate;
            ProductIdNumbers = productIdNumbers;
        }

        public bool IsActive(DateTime now)
        {
            return now >= CampaignStartDate && now <= CampaignEndDate;
        }
    }
}