using System.Collections.Generic;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces
{
    public interface IReceiptSearch
    {
        List<IReceiptModel> Search(string searchReceiptText);
    }
}