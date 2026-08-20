namespace Kassasystemet_refac
{
    public interface IReceiptSearch
    {
        List<IReceiptModel> Search(string searchReceiptText);
    }
}
