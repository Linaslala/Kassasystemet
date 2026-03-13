using LinasKlubbLivs.BusinessLogic.ReceiptLogic.ReceiptInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LinasKlubbLivs.BusinessLogic.ReceiptLogic
{
    /// <summary>
    /// Sökmotor för kvitton.
    /// 
    /// Stödjer sökning på:
    /// Kvittonummer
    /// Kundnummer
    /// 
    /// Tom söksträng returnerar alla kvitton.
    /// </summary>
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