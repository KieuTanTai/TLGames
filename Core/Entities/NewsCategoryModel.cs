using TLGames.Core.Enums;
using Windows.System.UserProfile;

namespace TLGames.Core.Entities
{
    internal class NewsCategoryModel
    {
        public int NewsCategory { get; private set; }
        public string CategoryName { get; private set; }
        
        public EActiveStatus Status { get; private set; }

        public NewsCategoryModel() { }

        public NewsCategoryModel(int newsCategory, string categoryName, EActiveStatus status)
        {
            NewsCategory = newsCategory;
            CategoryName = categoryName;
            Status = status;
        }

        public void SetNewsCategory(int id) { NewsCategory = id; }
        public void SetCategoryName(string name) { CategoryName = name; }
        public void SetStatus(EActiveStatus status) { Status = status; }
    }
}
