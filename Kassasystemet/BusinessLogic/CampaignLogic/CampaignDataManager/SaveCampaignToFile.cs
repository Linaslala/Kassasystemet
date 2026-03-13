using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignInterfaces;
using LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LinasKlubbLivs.BusinessLogic.CampaignLogic.CampaignDataManager
{
    /// <summary>
    /// Ansvarar för att spara alla kampanjer till fil.
    /// 
    /// Om kampanj är null så skapas en ny lista
    /// 
    /// Varje kampanj kan i textformat 
    /// läsas tillbaka av ReadAllCampaignsFromFile.
    /// 
    /// Endast PercentOffCampaign stöds (För tillfället. Möjlighet att lägga till fler kampanjtyper finns!)
    /// </summary>
    public class SaveCampaignToFile : ISaveCampaignToFile
    {
        public void SaveAll(List<ICampaignModel> campaigns)
        {
            campaigns ??= new List<ICampaignModel>();

            var lines = campaigns.Select(SerializeCampaigns).ToList();

            File.WriteAllLines(CampaignFilePath.Path, lines);
        }

        private static string SerializeCampaigns(ICampaignModel campaign)
        {
            string typeOfCampaign = campaign.TypeOfCampaign.ToString();
            string campaignName = Escape(campaign.CampaignName);
            string campaignStartDate = campaign.CampaignStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string campaignEndDate = campaign.CampaignEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string productIdNumbers = string.Join(",", campaign.ProductIdNumbers);

            decimal percent = (campaign as PercentOffCampaign)?.PercentOff
                            ?? throw new InvalidOperationException("Endast PercentOffCampaign kan sparas.");

            return $"{typeOfCampaign};{campaignName};{campaignStartDate};{campaignEndDate};{productIdNumbers};{percent.ToString(CultureInfo.InvariantCulture)}";
        }

        private static string Escape(string text) =>
            (text ?? "").Replace(";", ",").Trim();
    }
}

