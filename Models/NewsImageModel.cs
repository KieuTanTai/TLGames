namespace TLGames.Models
{
    internal class NewsImageModel
    {
        public int NewsId { get; private set; }
        public string ImageURL { get; private set; }

        public NewsImageModel() { }
        public NewsImageModel(int id, string imageURL)
        {
            NewsId = id;
            ImageURL = imageURL;
        }

        public void SetNewsId(int id) { NewsId = id; }
        public void SetImageURL(string imageURL) { ImageURL = imageURL; }
    }
}
