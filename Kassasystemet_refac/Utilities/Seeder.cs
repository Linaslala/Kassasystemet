using static Kassasystemet_refac.SearchMemberMenu;

namespace Kassasystemet_refac
{
    public static class Seeder
    {
        public static void SeedAll()
        {
            SeedProducts();
            SeedMembers();
            SeedCampaigns();
        }

        private static void SeedProducts()
        {
            var path = ProductFilePath.Path;

            if (File.Exists(path) && new FileInfo(path).Length > 0)
                return;

            File.WriteAllLines(path, new[]
            {
                "1; Banan; 123; kilopris",
                "2; Ananas; 60; kilopris",
                "3; Grovt grus; 1049.95; kilopris",
                "4; Hushållspapper; 19.99; styckpris"
            });
        }

        private static void SeedMembers()
        {
            var path = MemberFilePath.MembersPath;

            if (File.Exists(path) && new FileInfo(path).Length > 0)
                return;

            File.WriteAllLines(path, new[]
            {
                "1; Lina; Samuelsson",
                "2; Tomas; Wejskog",
                "3; Pelle; Jönsson",
                "4; Franz; Kafka"
            });
        }

        private static void SeedCampaigns()
        {
            var path = CampaignFilePath.Path;
            if (File.Exists(path) && new FileInfo(path).Length > 0)
                return;

            File.WriteAllLines(path, new[]
            {
                "PercentOffCampaign;25 % Rabatt!;2026-03-06;2026-03-10;1;25",
                "PercentOffCampaign;Vårrea;2026-03-08;2026-04-30;1;50",
                "PercentOffCampaign;HusKampanj;2026-03-18;2026-03-31;2;5",
                "PercentOffCampaign;SuperMegaRea;2026-04-01;2026-04-30;3,4;70"
            });
        }
    }
}
