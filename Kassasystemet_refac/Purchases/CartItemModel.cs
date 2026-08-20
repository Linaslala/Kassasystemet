namespace Kassasystemet_refac
{
    public class CartItemModel
    {
        public int ProductIdNumber { get; }
        public string ProductName { get; }
        public decimal ProductPrice { get; }
        public string PriceType { get; }
        public int ProductQuantity { get; }

        public CartItemModel(int productIdNumber, string productName, decimal productPrice, string priceType, int productQuantity)
        {
            ProductIdNumber = productIdNumber;
            ProductName = productName ?? "";
            ProductPrice = productPrice;
            PriceType = priceType ?? "";
            ProductQuantity = productQuantity;
        }

        public decimal LineTotal => ProductPrice * ProductQuantity;

        public CartItemModel WithQuantity(int newQuantity)
            => new CartItemModel(ProductIdNumber, ProductName, ProductPrice, PriceType, newQuantity);
    }
}
