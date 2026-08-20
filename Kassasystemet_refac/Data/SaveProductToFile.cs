using System.Globalization;
using static Kassasystemet_refac.SearchMemberMenu;

namespace Kassasystemet_refac
{
    public class SaveProductToFile : ISaveProductToFile
    {
        public void SaveAll(List<IProductModel> products)
        {
            string filePath = ProductFilePath.Path;

            var productDirectory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(productDirectory) && !Directory.Exists(productDirectory))
                Directory.CreateDirectory(productDirectory);

            using var writer = new StreamWriter(filePath, false);

            foreach (var product in products)
            {

                writer.WriteLine(
                    $"{product.ProductIdNumber};{product.ProductName};" +
                    $"{product.ProductPrice.ToString(CultureInfo.InvariantCulture)};" +
                    $"{product.ProductPriceType}"
                );
            }
        }
    }
}
