namespace TLGames.Core.Entities
{
    internal class NewsImageModel
    {
        public int NewsImageId { get; private set; }
        public int NewsId { get; private set; }
        public string ImageUrl { get; private set; }

        public NewsImageModel() { }
        public NewsImageModel(int newsImageId, int newsId, string imageURL)
        {
            NewsImageId = newsImageId;
            NewsId = newsId;
            ImageUrl = imageURL;
        }

        public void SetNewsImageId(int id) { NewsImageId = id; }
        public void SetNewsId(int id) { NewsId = id; }
        public void SetImageUrl(string imageUrl) { ImageUrl = imageUrl; }
    }
}
