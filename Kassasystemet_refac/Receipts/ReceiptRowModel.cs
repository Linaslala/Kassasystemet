namespace Kassasystemet_refac
{
    //POCO (Plain Old CLR Object)
    //Innehåller mest sata, lite logik och få beroenden
    //Därför bra att börja med tidigt i refaktoreringen!
    public class ReceiptRowModel
    {
        public string ReceiptProductText { get; }
        public int ReceiptProductQuantity { get; }
        public decimal ReceiptProductAmount { get; }

        public ReceiptRowModel(string receiptProductText, int receiptProductQuantity, decimal receiptProductAmount)
        {
            ReceiptProductText = receiptProductText ?? "";
            ReceiptProductQuantity = receiptProductQuantity;
            ReceiptProductAmount = receiptProductAmount;
        }
    }
}
