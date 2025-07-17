namespace TLGames.Core.Entities
{
    public class ProductDescriptionImageModel
    {
        public int DescriptionImageId { get; private set; }
        public int DescriptionId { get; private set; }
        public string ImageUrl { get; private set; }

        public ProductDescriptionImageModel() { }

        public ProductDescriptionImageModel(int descriptionImageId, int descriptionId, string imageUrl)
        {
            DescriptionImageId = descriptionImageId;
            DescriptionId = descriptionId;
            ImageUrl = imageUrl;
        }

        public void SetDescriptionImageId(int id) { DescriptionImageId = id; }
        public void SetDescriptionId(int id) { DescriptionId = id; }
        public void SetImageUrl(string url) { ImageUrl = url; }
    }
}
