using static Kassasystemet_refac.SearchMember;

namespace Kassasystemet_refac
{
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
