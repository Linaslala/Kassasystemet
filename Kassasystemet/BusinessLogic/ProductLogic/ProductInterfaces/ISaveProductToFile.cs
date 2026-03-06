using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic.ProductInterfaces
{
    public interface ISaveProductToFile
    {
        void SaveAll(List<IProductModel> products);
    }
}
