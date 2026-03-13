using LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic
{
    /// <summary>
    /// Representerar en produkt i sortimentet.
    /// 
    /// Innehåller produktnummer, produktnamn, pris och pristyp (kilopris eller styckpris)
    /// </summary>
    public class ProductModel : IProductModel
    {
        public int ProductIdNumber { get; }
        public string ProductName { get; }
        public decimal ProductPrice { get; }
        public string ProductPriceType { get; }

        public string ProductFullName => $"{ProductIdNumber} {ProductName} {ProductPrice} {ProductPriceType}";

        public ProductModel(int productIdNumber, string productName, decimal productPrice, string productPriceType)
        {
            ProductIdNumber = productIdNumber;
            ProductName = productName;
            ProductPrice = productPrice;
            ProductPriceType = productPriceType;
        }
    }
}
