namespace Kassasystemet_refac
{
    public class ReceiptSearch : IReceiptSearch
    {
        private readonly IReadAllReceiptsFromFile _reader;

        public ReceiptSearch(IReadAllReceiptsFromFile reader)
        {
            _reader = reader;
        }

        public List<IReceiptModel> Search(string searchText)
        {
            var all = _reader.ReadAll();

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
