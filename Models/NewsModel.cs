using System;

namespace TLGames.Models
{
    internal class NewsModel
    {
        public int NewsId { get; private set; }
        public int UserId { get; private set; }
        public int Category { get; private set; }
        public int RelatedProductId { get; private set; }
        public string Title { get; private set; }
        public DateTime PublishedDate { get; private set; }
        public string Content { get; private set; }
        public bool Status { get; private set; }

        public NewsModel() { }

        public NewsModel(int newsId, int userId, int category, int relatedProductId, string title, DateTime publishedDate, string content, bool status)
        {
            NewsId = newsId;
            UserId = userId;
            Category = category;
            RelatedProductId = relatedProductId;
            Title = title;
            PublishedDate = publishedDate;
            Content = content;
            Status = status;
        }

        public void SetNewsId(int id) { NewsId = id; }
        public void SetUserId(int id) { UserId = id; }
        public void SetCategory(int category) { Category = category; }
        public void SetRelatedProductId(int id) { RelatedProductId = id; }
        public void SetTitle(string title) { Title = title; }
        public void SetPublishedDate(DateTime date) { PublishedDate = date; }
        public void SetContent(string content) { Content = content; }
        public void SetStatus(bool status) { Status = status; }
    }
}
