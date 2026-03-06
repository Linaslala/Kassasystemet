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
        public decimal ReceiptProductAmount { get; }

        public ReceiptRowModel(string receiptProductText, decimal receiptProductAmount)
        {
            ReceiptProductText = receiptProductText ?? "";
            ReceiptProductAmount = receiptProductAmount;
        }
    }
}