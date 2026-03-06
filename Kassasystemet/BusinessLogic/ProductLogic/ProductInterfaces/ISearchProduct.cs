using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces
{
    public interface ISearchProduct
    {
        List<IProductModel> Search(string searchProductText);
    }
}
