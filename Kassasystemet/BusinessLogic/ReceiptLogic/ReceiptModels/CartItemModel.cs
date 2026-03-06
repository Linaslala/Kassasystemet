namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptModels
{
    /// <summary>
    /// Representerar en rad i varukorgen under ett pågående köp.
    /// </summary>
    public class CartItemModel
    {
        public int ProductIdNumber { get; }
        public string ProductName { get; }
        public decimal UnitPrice { get; }
        public string PriceType { get; }
        public int ProductQuantity { get; }

        public CartItemModel(int productIdNumber, string productName, decimal unitPrice, string priceType, int productQuantity)
        {
            ProductIdNumber = productIdNumber;
            ProductName = productName ?? "";
            UnitPrice = unitPrice;
            PriceType = priceType ?? "";
            ProductQuantity = productQuantity;
        }

        public decimal LineTotal => UnitPrice * ProductQuantity;

        public CartItemModel WithQuantity(int newQuantity)
            => new CartItemModel(ProductIdNumber, ProductName, UnitPrice, PriceType, newQuantity);
    }
}