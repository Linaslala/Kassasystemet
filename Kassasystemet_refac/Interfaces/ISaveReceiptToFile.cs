namespace Kassasystemet_refac
{
    public interface ISaveReceiptToFile
    {
        void SaveAll(List<IReceiptModel> receipts);
    }
}
