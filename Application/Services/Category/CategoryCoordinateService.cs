using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices.ICategory;

namespace TLGames.Application.Services.Category
{
    public class CategoryCoordinateService : ICategoryCoordinateService
    {
        private readonly IDAO<ProductCategoryModel> _productCategoryDAO;
        private readonly IDAO<CategoryModel> _categoryDao;

        public CategoryCoordinateService(IDAO<ProductCategoryModel> productCategoryDAO, IDAO<CategoryModel> categoryDao)
        {
            _productCategoryDAO = productCategoryDAO;
            _categoryDao = categoryDao;
        }

        // NOTE: JUST DELETE WHEN DON'T EXIST ANY RECORD IN PRODUCT CATEGORY
        public async Task<bool> DeleteCategoryAndProductCategory(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "Category ID cannot be null or empty.");
            if (await IsHaveConstraintProductCategory(id))
                throw new InvalidOperationException("Cannot delete category with existing products have this category.");
            // Delete product categories associated with the category
            bool result = await _categoryDao.DeleteAsync(id);
            if (!result)
                throw new InvalidOperationException("Failed to delete category.");
            return result;
        }

        public async Task<bool> IsHaveConstraintProductCategory(string id)
        {
            ProductCategoryModel existObjects = await _productCategoryDAO.GetByIdAsync(id);
            return existObjects != null;
        }
    }
}
