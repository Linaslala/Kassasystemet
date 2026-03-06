using System;
using System.Collections.Generic;
using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptModels;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces
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