using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    internal class CategoryModel
    {
        public int CategoryId { get; private set; }
        public string CategoryName { get; private set; }
        public EActiveStatus Status { get; private set; }
        public CategoryModel() { }

        public CategoryModel(int categoryId, string categoryName, EActiveStatus status)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Status = status;
        }

        public void SetCategoryId(int id) { CategoryId = id; }
        public void SetCategoryName(string name) { CategoryName = name; }
        public void SetStatus(EActiveStatus status) { Status = status; }
    }
}
