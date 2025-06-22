using System;

namespace TLGames.Core.Entities
{
    internal class ProductDescriptionModel
    {
        public int DescriptionId { get; private set; }
        public int ProductId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime LastUpdatedDate { get; private set; }

        public ProductDescriptionModel() { }

        public ProductDescriptionModel(int descriptionId, int productId, string title, string content, DateTime lastUpdatedDate)
        {
            DescriptionId = descriptionId;
            ProductId = productId;
            Title = title;
            Content = content;
            LastUpdatedDate = lastUpdatedDate;
        }

        public void SetDescriptionId(int id) { DescriptionId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetTitle(string title) { Title = title; }
        public void SetContent(string content) { Content = content; }
        public void SetLastUpdatedDate(DateTime date) { LastUpdatedDate = date; }
    }
}
