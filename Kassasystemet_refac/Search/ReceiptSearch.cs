namespace Kassasystemet_refac
{
    public class ReceiptSearch : IReceiptSearch
    {
        private readonly IReadAllReceiptsFromFile _receiptReader;

        public ReceiptSearch(IReadAllReceiptsFromFile receiptReader)
        {
            _receiptReader = receiptReader;
        }

        public List<IReceiptModel> Search(string searchText)
        {
            var all = _receiptReader.ReadAll();

            if (string.IsNullOrWhiteSpace(searchText))
                return all;

            string userReceiptQuery = searchText.Trim();

            return all.Where(r =>
                   r.ReceiptNumber.ToString().Contains(userReceiptQuery)
                || r.MemberIdNumber.ToString().Contains(userReceiptQuery)
            ).ToList();
        }
    }
}
