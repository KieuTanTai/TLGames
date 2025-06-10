namespace TLGames.Models
{
    internal class SocialMediaProductModel
    {
        public int ProductId { get; private set; }
        public string SocialMediaName { get; private set; }
        public string SocialMediaURL { get; private set; }

        public SocialMediaProductModel() { }

        public SocialMediaProductModel(int productId, string socialMediaName, string socialMediaURL)
        {
            ProductId = productId;
            SocialMediaName = socialMediaName;
            SocialMediaURL = socialMediaURL;
        }

        public void SetProductId(int id) { ProductId = id; }
        public void SetSocialMediaName(string name) { SocialMediaName = name; }
        public void SetSocialMediaURL(string url) { SocialMediaURL = url; }
    }
}
