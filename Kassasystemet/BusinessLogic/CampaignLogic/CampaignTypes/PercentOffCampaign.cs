using System;
using System.Collections.Generic;
using System.Linq;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes
{
    /// <summary>
    /// Representerar en kampanj med procentuell rabatt.
    /// 
    /// Gäller under ett datumintervall och för specifika produkter.
    /// </summary>
    public class PercentOffCampaign : ICampaignModel
    {
        public string CampaignName { get; }
        public CampaignType TypeOfCampaign => CampaignType.PercentOffCampaign;

        public DateTime CampaignStartDate { get; }
        public DateTime CampaignEndDate { get; }

        public IReadOnlyList<int> ProductIdNumbers { get; }

        public decimal PercentOff { get; }

        public PercentOffCampaign(string campaignName, DateTime campaignStartDate, DateTime campaignEndDate, IEnumerable<int> productIdNumbers, decimal percentOff)
        {
            ValidateCampaignParts(campaignName, campaignStartDate, campaignEndDate, productIdNumbers);

            if (percentOff <= 0m || percentOff > 100m)
                throw new ArgumentException("Rabattprocenten måste vara > 0 och <= 100.", nameof(percentOff));

            CampaignName = campaignName.Trim();
            CampaignStartDate = campaignStartDate;
            CampaignEndDate = campaignEndDate;
            ProductIdNumbers = productIdNumbers.Distinct().Where(n => n > 0).ToList();
            PercentOff = percentOff;
        }
       
        public bool IsActive(DateTime now) => now >= CampaignStartDate && now <= CampaignEndDate;

        private static void ValidateCampaignParts(string campaignName, DateTime campaignStartDate, DateTime campaignEndDate, IEnumerable<int> productIdNumbers)
        {
            if (string.IsNullOrWhiteSpace(campaignName))
                throw new ArgumentException("Namn får inte vara tomt.", nameof(campaignName));

            if (campaignEndDate < campaignStartDate)
                throw new ArgumentException("Slutdatum kan inte vara före startdatum.");

            if (productIdNumbers == null)
                throw new ArgumentNullException(nameof(productIdNumbers));

            if (!productIdNumbers.Any(i => i > 0))
                throw new ArgumentException("Minst ett giltigt produkt-id krävs.", nameof(productIdNumbers));
        }
    }
}