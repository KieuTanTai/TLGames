namespace TLGames.Core.Entities
{
    public class ProductCategoryModel
    {
        public int CategoryId { get; private set; }
        public int ProductId { get; private set; }

        public ProductCategoryModel() { }

        public ProductCategoryModel(int categoryId, int productId)
        {
            CategoryId = categoryId;
            ProductId = productId;
        }

        public void SetCategoryId(int id) { CategoryId = id; }
        public void SetProductId(int id) { ProductId = id; }
    }
}
