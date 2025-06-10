using System;

namespace TLGames.Models
{
    internal class ReplyReviewModel
    {
        public int ReplyId { get; private set; }
        public int ReviewId { get; private set; }
        public int UserId { get; private set; }
        public string Content { get; private set; }
        public DateTime UploadDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public double ContentRate { get; private set; }

        public ReplyReviewModel() { }

        public ReplyReviewModel(int replyId, int reviewId, int userId, string content, DateTime uploadDate, DateTime lastUpdateDate, double contentRate)
        {
            ReplyId = replyId;
            ReviewId = reviewId;
            UserId = userId;
            Content = content;
            UploadDate = uploadDate;
            LastUpdateDate = lastUpdateDate;
            ContentRate = contentRate;
        }

        public void SetReplyId(int id) { ReplyId = id; }
        public void SetReviewId(int id) { ReviewId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetContent(string content) { Content = content; }
        public void SetUploadDate(DateTime date) { UploadDate = date; }
        public void SetLastUpdateDate(DateTime date) { LastUpdateDate = date; }
        public void SetContentRate(double rate) { ContentRate = rate; }
    }
}
