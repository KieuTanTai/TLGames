namespace TLGames.Core.Entities
{
    public class ProductImageModel
    {
        public int ProductImageId { get; private set; }
        public int ProductId { get; private set; }
        public string ImageUrl { get; private set; }

        public ProductImageModel() { }

        public ProductImageModel(int productImageId, int productId, string imageUrl)
        {
            ProductImageId = productImageId;
            ProductId = productId;
            ImageUrl = imageUrl;
        }

        public void SetProductImageId(int id) { ProductImageId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetImageUrl(string url) { ImageUrl = url; }
    }
}
