using System;

namespace TLGames.Core.Entities
{
    public class ReviewModel
    {
        public int ReviewId { get; private set; }
        public int ProductId { get; private set; }
        public int UserId { get; private set; }
        public string Content { get; private set; }
        public DateTime UploadDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public double ContentRate { get; private set; }
        public double ReviewRate { get; private set; }

        public ReviewModel() { }

        public ReviewModel(int reviewId, int productId, int userId, string content, DateTime uploadDate, DateTime lastUpdateDate, double contentRate, double reviewRate)
        {
            ReviewId = reviewId;
            ProductId = productId;
            UserId = userId;
            Content = content;
            UploadDate = uploadDate;
            LastUpdateDate = lastUpdateDate;
            ContentRate = contentRate;
            ReviewRate = reviewRate;
        }

        public void SetReviewId(int id) { ReviewId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetContent(string content) { Content = content; }
        public void SetUploadDate(DateTime date) { UploadDate = date; }
        public void SetLastUpdateDate(DateTime date) { LastUpdateDate = date; }
        public void SetContentRate(double rate) { ContentRate = rate; }
        public void SetReviewRate(double rate) { ReviewRate = rate; }
    }
}
