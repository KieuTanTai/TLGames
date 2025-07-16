using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Enums;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IServices;
using TLGames.Infrastructure.Data;

namespace TLGames.Application.Services.Product
{
    internal class GetProductService(IDbConnectionFactory connectionFactory,
                                        string tableName,
                                        string columnIdName) : ValidateService, IGetDataService<ProductModel>
    {
        public async Task<ProductModel> GetByIdAsync(string id)
        {
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetByIdAsync(id);
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetAllAsync();
        }

        public async Task<List<ProductModel>> GetAllByNameAsync(string name, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<ProductModel>();
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetRelativeAsync(name, colName);
        }

        public async Task<List<ProductModel>> GetAllByDeveloperIdAsync(int developerId, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<ProductModel>();
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetAllByIdAsync(developerId.ToString(), colName);
        }

        public async Task<List<ProductModel>> GetAllByReleaseDateAsync(DateTime releaseDate, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<ProductModel>();
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetAllByTime(releaseDate.ToString(), colName, EDataTimeType.DATETIME);
        }

        public async Task<List<ProductModel>> GetAllByReleaseDateAsync(string time, string colName, EDataTimeType eDataTimeType = EDataTimeType.MONTH)
        {
            if (!IsValidStringInputDB(colName))
                return new List<ProductModel>();
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetAllByTime(time.ToString(), colName, eDataTimeType);
        }

        public async Task<List<ProductModel>> GetAllByGameModeAsync(EProductGameMode gameMode, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<ProductModel>();
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetAllByEnum(gameMode, colName);
        }

        public async Task<List<ProductModel>> GetAllByRatingAgeAsync(int ratingAge, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<ProductModel>();
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetAllByIdAsync(ratingAge.ToString(), colName);
        }

        public async Task<List<ProductModel>> GetAllByStatusAsync(EActiveStatus status, string colName)
        {
            if (!IsValidStringInputDB(colName))
                return new List<ProductModel>();
            ProductDAO product = new ProductDAO(connectionFactory, tableName, columnIdName);
            return await product.GetAllByEnum(status, colName);
        }
    }
}