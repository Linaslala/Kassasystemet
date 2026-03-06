using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic
{
    /// <summary>
    /// Sökmotor för produkter.
    /// 
    /// Stödjer sökning på produktnummer, namn och pristyp.
    /// </summary>
    public class ProductSearch : ISearchProduct
    {
        private readonly IReadAllProductsFromFile _reader;

        public ProductSearch(IReadAllProductsFromFile reader)
        {
            _reader = reader;
        }

        public List<IProductModel> Search(string searchProductText)
        {
            var all = _reader.ReadAll();

            if (string.IsNullOrWhiteSpace(searchProductText))
                return all;

            string query = searchProductText.Trim().ToLowerInvariant();

            return all
                .Where(p =>
                {
                    string name = (p.ProductName ?? "").ToLowerInvariant();
                    string type = (p.ProductPriceType ?? "").ToLowerInvariant();
                    string full = (p.ProductFullName ?? "").ToLowerInvariant();

                    return p.ProductIdNumber.ToString().Contains(query)
                           || name.Contains(query)
                           || type.Contains(query)
                           || full.Contains(query)
                           || p.ProductPrice.ToString().Contains(query);
                })
                .ToList();
        }
    }
}