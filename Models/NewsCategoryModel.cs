namespace TLGames.Models
{
    internal class NewsCategoryModel
    {
        public int NewsCategory { get; private set; }
        public string CategoryName { get; private set; }

        public NewsCategoryModel() { }

        public NewsCategoryModel(int newsCategory, string categoryName)
        {
            NewsCategory = newsCategory;
            CategoryName = categoryName;
        }

        public void SetNewsCategory(int id) { NewsCategory = id; }
        public void SetCategoryName(string name) { CategoryName = name; }
    }
}
