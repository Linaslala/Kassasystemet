using static Kassasystemet_refac.SearchMember;

namespace Kassasystemet_refac
{
    public class ReceiptModel : IReceiptModel
    {
        public int ReceiptNumber { get; }
        public int MemberIdNumber { get; }
        public DateTime ReceiptCreatedAt { get; }
        public IReadOnlyList<ReceiptRowModel> ReceiptRows { get; }
        public int TotalItems { get; }
        public decimal TotalAmount { get; }

        public ReceiptModel(
            int receiptNumber,
            int memberIdNumber,
            DateTime receiptCreatedAt,
            IReadOnlyList<ReceiptRowModel> receiptRows,
            int totalItems,
            decimal totalAmount)
        {
            ReceiptNumber = receiptNumber;
            MemberIdNumber = memberIdNumber;
            ReceiptCreatedAt = receiptCreatedAt;
            ReceiptRows = receiptRows ?? new List<ReceiptRowModel>();
            TotalItems = totalItems;
            TotalAmount = totalAmount;
        }
    }
}
