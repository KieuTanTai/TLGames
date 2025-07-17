using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    public class SocialMediaProductModel
    {
        public int SocialMediaId { get; private set; }
        public int ProductId { get; private set; }
        public string AccountName { get; private set; }
        public ESocialMediaType? SocialMediaType;
        public string SocialMediaUrl { get; private set; }

        public SocialMediaProductModel() { }

        public SocialMediaProductModel(int socialMediaId, int productId, string accountName, ESocialMediaType socialMediaType, string socialMediaURL)
        {
            SocialMediaId = socialMediaId;
            ProductId = productId;
            AccountName = accountName;
            SocialMediaType = socialMediaType;
            SocialMediaUrl = socialMediaURL;
        }

        public void SetSocialMediaId(int id) { SocialMediaId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetSocialMediaType(ESocialMediaType type) { SocialMediaType = type; }
        public void SetSocialMediaUrl(string url) { SocialMediaUrl = url; }
        public void SetAccountName(string name) { AccountName = name; }
    }
}
