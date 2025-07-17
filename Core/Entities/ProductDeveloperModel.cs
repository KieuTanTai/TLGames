namespace TLGames.Core.Entities
{
    public class ProductDeveloperModel
    {
        public int ProductId { get; private set; }
        public int DeveloperId { get; private set; }

        public ProductDeveloperModel() { }

        public ProductDeveloperModel(int productId, int developerId)
        {
            ProductId = productId;
            DeveloperId = developerId;
        }

        public void SetProductId(int id) { ProductId = id; }
        public void SetDeveloperId(int id) { DeveloperId = id; }
    }
}
