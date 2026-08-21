namespace Kassasystemet_refac
{
    public class ProductSearch : ISearchProduct
    {
        private readonly IReadAllProductsFromFile _productReader;

        public ProductSearch(IReadAllProductsFromFile productReader)
        {
            _productReader = productReader;
        }

        public List<IProductModel> Search(string searchProductText)
        {
            var all = _productReader.ReadAll();

            if (string.IsNullOrWhiteSpace(searchProductText))
                return all;

            string userProductQuery = searchProductText.Trim().ToLowerInvariant();

            return all
                .Where(p =>
                {
                    string productName = (p.ProductName ?? "").ToLowerInvariant();
                    string productType = (p.ProductPriceType ?? "").ToLowerInvariant();
                    string fullProductName = (p.ProductFullName ?? "").ToLowerInvariant();

                    return p.ProductIdNumber.ToString().Contains(userProductQuery)
                           || productName.Contains(userProductQuery)
                           || productType.Contains(userProductQuery)
                           || fullProductName.Contains(userProductQuery)
                           || p.ProductPrice.ToString().Contains(userProductQuery);
                })
                .ToList();
        }
    }
}
