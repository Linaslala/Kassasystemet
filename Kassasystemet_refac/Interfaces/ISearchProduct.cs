namespace Kassasystemet_refac
{
    public interface ISearchProduct
    {
        List<IProductModel> Search(string searchProductText);
    }
}
