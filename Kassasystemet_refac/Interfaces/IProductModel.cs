namespace Kassasystemet_refac
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
