using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces
{
    public interface IProductModel
    {
        int ProductIdNumber { get; }
        string ProductName { get; }
        decimal ProductPrice { get; }
        string ProductPriceType { get; }
        string ProductFullName { get; }
    }
}
