using System;
using System.Collections.Generic;
using System.Text;

namespace Kassasystemet_refac
{
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
