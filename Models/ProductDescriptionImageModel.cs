namespace TLGames.Models
{
    internal class ProductDescriptionImageModel
    {
        public int DescriptionId { get; private set; }
        public string ImageURL { get; private set; }

        public ProductDescriptionImageModel() { }

        public ProductDescriptionImageModel(int descriptionId, string imageURL)
        {
            DescriptionId = descriptionId;
            ImageURL = imageURL;
        }

        public void SetDescriptionId(int id) { DescriptionId = id; }
        public void SetImageURL(string url) { ImageURL = url; }
    }
}
