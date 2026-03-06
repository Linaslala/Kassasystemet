using System.Collections.Generic;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces
{
    public interface IReadAllReceiptsFromFile
    {
        List<IReceiptModel> ReadAll();
    }
}