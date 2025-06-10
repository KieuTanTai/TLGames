namespace TLGames.Models
{
    internal class ProductImageModel
    {
        public int ProductId { get; private set; }
        public string ImageURL { get; private set; }

        public ProductImageModel() { }

        public ProductImageModel(int productId, string imageURL)
        {
            ProductId = productId;
            ImageURL = imageURL;
        }

        public void SetProductId(int id) { ProductId = id; }
        public void SetImageURL(string url) { ImageURL = url; }
    }
}
