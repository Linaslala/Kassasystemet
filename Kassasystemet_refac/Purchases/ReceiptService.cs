using System.Globalization;

namespace Kassasystemet_refac
{
    internal class ReceiptService
    {
        public static bool TryPayAndShowReceipt(ref int memberIdNumber, List<CartItemModel> cart)
        {
            if (cart.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Du kan inte betala ett tomt köp.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                return false;
            }

            if (memberIdNumber <= 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow("Du måste ange kundnummer innan betalning.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();
                memberIdNumber = CreateNewPurchase.ReadMemberIdNumber();
            }

            var memberReader = new ReadAllMembersFromFile();
            var members = memberReader.ReadAll();

            int memberIdSnapshot = memberIdNumber;

            bool customerExists = members.Any(m => m.MemberIdNumber == memberIdSnapshot);

            if (!customerExists)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                CenterConsoleOutput.CenterTextToWindow(
                    $"Ingen kund hittades med kundnummer {memberIdNumber}.");
                Console.ResetColor();
                ValidatedConsoleInput.PauseCentered();

                memberIdNumber = 0;
                return false;
            }

            ReceiptModel receipt = CompletePayment(memberIdNumber, cart);
            DraftPurchaseService.ClearPurchaseDraft();

            Console.Clear();
            CenterConsoleOutput.CenterTextToWindow("== KVITTO ==");
            Console.WriteLine();
            ReceiptPrinter.PrintDetailed(receipt);
            ValidatedConsoleInput.PauseCentered("Tryck valfri tangent för att gå tillbaka...");
            return true;
        }

        public static ReceiptModel CompletePayment(int memberIdNumber, List<CartItemModel> cart)
        {
            var campaigns = new ReadAllCampaignsFromFile()
                .ReadAll()
                .OfType<PercentOffCampaign>()
                .Where(c => c.IsActive(DateTime.Now))
                .ToList();

            var receiptRows = new List<ReceiptRowModel>();

            foreach (var item in cart)
            {
                decimal lineTotal = item.LineTotal;
                receiptRows.Add(new ReceiptRowModel(item.ProductName, item.ProductQuantity, lineTotal));

                var bestCampaign = campaigns
                    .Where(c => c.ProductIdNumbers != null && c.ProductIdNumbers.Contains(item.ProductIdNumber))
                    .OrderByDescending(c => c.PercentOff)
                    .FirstOrDefault();

                if (bestCampaign != null && bestCampaign.PercentOff > 0m)
                {
                    decimal discount = Math.Round(lineTotal * (bestCampaign.PercentOff / 100m), 2);
                    if (discount > 0m)
                    {

                        receiptRows.Add(new ReceiptRowModel(
                            $"Rabatt: {bestCampaign.PercentOff.ToString("0.0", CultureInfo.InvariantCulture)}%", 0,
                            -discount));
                    }
                }
            }

            decimal totalAmount = receiptRows.Sum(r => r.ReceiptProductAmount);
            int totalItems = cart.Sum(x => x.ProductQuantity);

            var receiptReader = new ReadAllReceiptsFromFile();
            var receiptWriter = new SaveReceiptToFile();
            var receipts = receiptReader.ReadAll();

            int nextReceiptNumber = receipts.Any() ? receipts.Max(r => r.ReceiptNumber) + 1 : 1;

            var receipt = new ReceiptModel(
                nextReceiptNumber,
                memberIdNumber,
                DateTime.Now,
                receiptRows,
                totalItems,
                totalAmount);

            receipts.Add(receipt);
            receiptWriter.SaveAll(receipts);
            return receipt;
        }
    }
}
