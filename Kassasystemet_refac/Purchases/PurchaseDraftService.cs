using Kassasystemet_refac.Data;
using System.Globalization;

namespace Kassasystemet_refac
{
    public class PurchaseDraftService
    {
        //Spara köp-utkast
        public static void SavePurchaseDraft(int memberIdNumber, List<CartItemModel> cart)
        {
            string items = string.Join("\n", cart.Select(c => $"{c.ProductIdNumber},{c.ProductQuantity}"));
            string line = $"{memberIdNumber};{items}";
            File.WriteAllText(ReceiptFilePath.ReceiptDraftPath, line);
        }

        //Ladda köp-utkast
        public static List<CartItemModel> LoadCartFromSavedItems(List<(int productIdNumber, int productQuantity)> savedItems)
        {
            IReadAllProductsFromFile productReader = new ReadAllProductsFromFile();
            var products = productReader.ReadAll();

            var cart = new List<CartItemModel>();

            foreach (var (productIdNumber, productQuantity) in savedItems)
            {
                var product = products.FirstOrDefault(p => p.ProductIdNumber == productIdNumber);
                if (product == null) continue;

                cart.Add(new CartItemModel(
                    product.ProductIdNumber,
                    product.ProductName,
                    product.ProductPrice,
                    product.ProductPriceType,
                    productQuantity));
            }

            return cart;
        }

        //Rensa köp-utkast
        public static void ClearPurchaseDraft()
        {
            if (File.Exists(ReceiptFilePath.ReceiptDraftPath))
                File.Delete(ReceiptFilePath.ReceiptDraftPath);
        }

        //Bygga varukorg från sparat utkast (Frågan om jag gör detta... finns inget med cart här, bara receipt) Kolla upp!
        public static bool TryLoadReceiptDraft(out int memberIdNumber, out List<(int productIdNumber, int productQuantity)> items)
        {
            memberIdNumber = 0;
            items = new List<(int, int)>();

            string content = File.ReadAllText(ReceiptFilePath.ReceiptDraftPath);
            if (string.IsNullOrWhiteSpace(content))
                return false;

            var receiptParts = content.Split(';');
            if (receiptParts.Length < 2)
                return false;

            int.TryParse(receiptParts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out memberIdNumber);

            var receiptRows = receiptParts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in receiptRows)
            {
                var two = row.Split(',');
                if (two.Length != 2) continue;

                if (int.TryParse(two[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productIdNumber) &&
                    int.TryParse(two[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int productQuantity) &&
                    productIdNumber > 0 && productQuantity > 0)
                {
                    items.Add((productIdNumber, productQuantity));
                }
            }

            return true;
        }


    }
}
