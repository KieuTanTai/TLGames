using System;

namespace TLGames.Models
{
    internal class ProductModel
    {
        public int ProductId { get; private set; }
        public int DeveloperId { get; private set; }
        public string ProductName { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public decimal BasePrice { get; private set; }
        public string TrailerURL { get; private set; }
        public string GameMode { get; private set; }
        public int RatingAge { get; private set; }
        public bool Status { get; private set; }

        public ProductModel() { }

        public ProductModel(int productId, int developerId, string productName, DateTime releaseDate, decimal basePrice,
                            string trailerURL, string gameMode, int ratingAge, bool status)
        {
            ProductId = productId;
            DeveloperId = developerId;
            ProductName = productName;
            ReleaseDate = releaseDate;
            BasePrice = basePrice;
            TrailerURL = trailerURL;
            GameMode = gameMode;
            RatingAge = ratingAge;
            Status = status;
        }

        public void SetProductId(int id) { ProductId = id; }
        public void SetDeveloperId(int id) { DeveloperId = id; }
        public void SetProductName(string name) { ProductName = name; }
        public void SetReleaseDate(DateTime date) { ReleaseDate = date; }
        public void SetBasePrice(decimal price) { BasePrice = price; }
        public void SetTrailerURL(string url) { TrailerURL = url; }
        public void SetGameMode(string mode) { GameMode = mode; }
        public void SetRatingAge(int age) { RatingAge = age; }
        public void SetStatus(bool status) { Status = status; }
    }
}
