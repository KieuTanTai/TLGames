namespace TLGames.Core.Entities
{
    public class ProductPublisherModel
    {
        public int ProductId { get; private set; }
        public int PublisherId { get; private set; }

        public ProductPublisherModel() { }

        public ProductPublisherModel(int productId, int publisherId)
        {
            ProductId = productId;
            PublisherId = publisherId;
        }

        public void SetProductId(int id) { ProductId = id; }
        public void SetPublisherId(int id) { PublisherId = id; }
    }
}
