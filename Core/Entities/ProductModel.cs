using System;
using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class ProductModel
    {
        public int ProductId { get; private set; }
        public int DeveloperId { get; private set; }
        public string ProductName { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public decimal BasePrice { get; private set; }
        public string TrailerUrl { get; private set; }
        public EProductGameMode GameMode { get; private set; }
        public int RatingAge { get; private set; }
        public EActiveStatus Status { get; private set; }
        public int DownloadedCount { get; private set; }
        public ProductModel() { }

        public ProductModel(int productId, int developerId, string productName, DateTime releaseDate, decimal basePrice,
                            string trailerUrl, EProductGameMode gameMode, int ratingAge, EActiveStatus status, int downloadedCount)
        {
            ProductId = productId;
            DeveloperId = developerId;
            ProductName = productName;
            ReleaseDate = releaseDate;
            BasePrice = basePrice;
            TrailerUrl = trailerUrl;
            GameMode = gameMode;
            RatingAge = ratingAge;
            Status = status;
            DownloadedCount = downloadedCount;
        }

        public void SetProductId(int id) { ProductId = id; }
        public void SetDeveloperId(int id) { DeveloperId = id; }
        public void SetProductName(string name) { ProductName = name; }
        public void SetReleaseDate(DateTime date) { ReleaseDate = date; }
        public void SetBasePrice(decimal price) { BasePrice = price; }
        public void SetTrailerUrl(string url) { TrailerUrl = url; }
        public void SetGameMode(EProductGameMode mode) { GameMode = mode; }
        public void SetRatingAge(int age) { RatingAge = age; }
        public void SetDownloadedCount(int count) { DownloadedCount = count; }
        public void SetStatus(EActiveStatus status) { Status = status; }
    }
}
