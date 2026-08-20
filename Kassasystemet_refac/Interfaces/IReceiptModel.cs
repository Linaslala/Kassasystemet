namespace Kassasystemet_refac
{
    public interface IReceiptModel
    {
        int ReceiptNumber { get; }
        int MemberIdNumber { get; }
        DateTime ReceiptCreatedAt { get; }
        IReadOnlyList<ReceiptRowModel> ReceiptRows { get; }
        int TotalItems { get; }
        decimal TotalAmount { get; }
    }
}
