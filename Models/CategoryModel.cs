namespace TLGames.Models
{
    internal class CategoryModel
    {
        public int CategoryId { get; private set; }
        public string CategoryName { get; private set; }

        public CategoryModel() { }

        public CategoryModel(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }

        public void SetCategoryId(int id) { CategoryId = id; }
        public void SetCategoryName(string name) { CategoryName = name; }
    }
}
