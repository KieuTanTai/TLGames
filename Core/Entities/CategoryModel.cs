using TLGames.Core.Enums;

namespace TLGames.Core.Entities
{
    public class CategoryModel
    {
        private CategoryModel entity;

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

        public CategoryModel(CategoryModel entity)
        {
            this.entity = entity;
        }

        public void SetCategoryId(int id) { CategoryId = id; }
        public void SetCategoryName(string name) { CategoryName = name; }
        public void SetStatus(EActiveStatus status) { Status = status; }
    }
}
