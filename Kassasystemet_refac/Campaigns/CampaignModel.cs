//namespace Kassasystemet_refac
//{
//    public class CampaignModel : ICampaignModel
//    {
//        public string CampaignName { get; }
//        public CampaignType TypeOfCampaign { get; }
//        public DateTime CampaignStartDate { get; }
//        public DateTime CampaignEndDate { get; }
//        public IReadOnlyList<int> ProductIdNumbers { get; }

//        public CampaignModel(
//            string campaignName,
//            CampaignType typeOfCampaign,
//            DateTime campaignStartDate,
//            DateTime campaignEndDate,
//            IReadOnlyList<int> productIdNumbers)
//        {
//            CampaignName = campaignName;
//            TypeOfCampaign = typeOfCampaign;
//            CampaignStartDate = campaignStartDate;
//            CampaignEndDate = campaignEndDate;
//            ProductIdNumbers = productIdNumbers;
//        }

//        public bool IsActive(DateTime now)
//        {
//            return now >= CampaignStartDate && now <= CampaignEndDate;
//        }
//    }
//}
