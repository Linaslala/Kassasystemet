using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using LinasKlubbLivs.BusinessLogic.ProductLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager
{
    /// <summary>
    /// Läser alla produkter från fil och återskapar produktobjekt.
    /// </summary>
    public class ReadAllProductsFromFile : IReadAllProductsFromFile
    {
        public List<IProductModel> ReadAll()
        {
            var products = new List<IProductModel>();

            string filePath = ProductFilePath.Path;

            if (!File.Exists(filePath))
                return products;

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');
                if (parts.Length != 4)
                    continue;

                if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productIdNumber))
                    continue;

                if (!decimal.TryParse(parts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal productPrice))
                    continue;

                products.Add(new ProductModel(
                    productIdNumber,
                    parts[1],
                    productPrice,
                    parts[3]));
            }
            return products;
        }
    }
}