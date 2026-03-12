namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptModels
{
    /// <summary>
    /// Representerar en enskild rad på ett kvitto.
    /// 
    /// Kan vara en produkt eller en rabatt.
    /// </summary>
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