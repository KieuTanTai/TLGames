using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Category
{
    internal class DeleteCategoryService(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : IDeleteSingleKeyDataService<CategoryModel>, ISoftDeleteService //IDeleteIntegrityService<CategoryModel>
    {
        public async Task<bool> DeleteByIdAsync(string id)
        {
            CategoryDAO categoryDAO = new(connectionFactory, tableName, columnIdName);
            return await categoryDAO.DeleteAsync(id);
        }

        public async Task<bool> DeleteByIdsAsync(IEnumerable<string> ids)
        {
            CategoryDAO categoryDAO = new(connectionFactory, tableName, columnIdName);
            if (ids == null || !ids.Any())
                return false;
            return await categoryDAO.DeleteManyAsync(ids);
        }

        public async Task<bool> SoftDeleteByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "category ID cannot be null or empty.");
            CategoryDAO categoryDAO = new(connectionFactory, tableName, columnIdName);
            CategoryModel category = await categoryDAO.GetByIdAsync(id);

            if (category == null) return false;
            category.SetStatus(EActiveStatus.INACTIVE);
            return await categoryDAO.SoftDeleteAsync(category);
        }

        public async Task<bool> SoftDeleteByIdsAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                throw new ArgumentNullException(nameof(ids), "Product IDs cannot be null or empty.");
            CategoryDAO categoryDAO = new(connectionFactory, tableName, columnIdName);
            List<CategoryModel> categories = new();
            foreach (string id in ids)
            {
                CategoryModel product = await categoryDAO.GetByIdAsync(id);
                if (product != null)
                {
                    product.SetStatus(EActiveStatus.INACTIVE);
                    categories.Add(product);
                }
            }
            if (categories == null || categories.Count == 0)
                return false;
            return await categoryDAO.UpdateManyAsync(categories);
        }

        //public async Task<bool> DeleteParentsWithChildrenAsync(IEnumerable<string> parentIds, EDeleteStrategy EDeleteStrategy = EDeleteStrategy.Restrict)
        //{
        //    if (EDeleteStrategy == EDeleteStrategy.Restrict)
        //    {
        //        foreach (string parentId in parentIds)
        //            if (!CanDeleteParentAsync(parentId).Result)
        //                return false;
        //        CategoryDAO categoryDAO = new(connectionFactory);
        //        return await categoryDAO.DeleteManyAsync(parentIds);
        //    }
        //    else if (EDeleteStrategy == EDeleteStrategy.Cascade)
        //    {
        //        foreach (string parentId in parentIds)
        //            if (!await DeleteParentWithChildrenAsync(parentId, EDeleteStrategy))
        //                return false;
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        //public async Task<bool> DeleteParentWithChildrenAsync(string parentId, EDeleteStrategy deleteStrategy = EDeleteStrategy.Restrict)
        //{
        //    if (deleteStrategy == EDeleteStrategy.Restrict)
        //    {
        //        if (!CanDeleteParentAsync(parentId).Result)
        //            return false;
        //        return await DeleteByIdAsync(parentId);
        //    }
        //    else if (deleteStrategy == EDeleteStrategy.Cascade)
        //    {
        //        ProductCategoryDAO productCategoryDAO = new(connectionFactory);
        //        await productCategoryDAO.DeleteAsync(parentId);
        //        CategoryDAO categoryDAO = new(connectionFactory);
        //        return await categoryDAO.DeleteAsync(parentId);
        //    }
        //    else
        //        return false;
        //}
    }
}
