namespace McDermott.Web.Components.Pages
{
    public partial class SplitStock
    {
        private List<ProductSplit> splitproducts = new List<ProductSplit>
    {
        new ProductSplit { Name = "Paracetamol A", Stock = 5, Batch = "A123B", ExpiryDate = new DateTime(2024, 6, 14) },
        new ProductSplit { Name = "Paracetamol B", Stock = 20, Batch = "C5678D", ExpiryDate = new DateTime(2024, 7, 1) }
    };

        private int requestedStock;
        private bool? isRequestFulfilled;
        private string? StatusAlert = "";

        private async Task ProcessRequest()
        {
            int totalStock = splitproducts.Sum(p => p.Stock);

            if (requestedStock > totalStock)
            {
                // Menampilkan alert jika permintaan stok melebihi total stok yang tersedia
                StatusAlert = "Ndak Ada";
                isRequestFulfilled = false;
            }
            else
            {
                int remainingRequest = requestedStock;

                foreach (var product in splitproducts)
                {
                    if (remainingRequest <= 0)
                    {
                        break;
                    }

                    if (product.Stock >= remainingRequest)
                    {
                        product.Stock -= remainingRequest;
                        remainingRequest = 0;
                    }
                    else
                    {
                        remainingRequest -= product.Stock;
                        product.Stock = 0;
                    }
                }

                isRequestFulfilled = remainingRequest == 0;
            }
        }
    }

    public class ProductSplit
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Batch { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}