using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using System.Globalization;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager
{
    /// <summary>
    /// Sparar alla produkter till fil.
    /// </summary>
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
