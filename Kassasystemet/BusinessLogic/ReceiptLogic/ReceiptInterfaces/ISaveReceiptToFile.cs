using System.Collections.Generic;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces
{
    public interface ISaveReceiptToFile
    {
        void SaveAll(List<IReceiptModel> receipts);
    }
}