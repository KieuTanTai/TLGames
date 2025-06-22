using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Core.Interfaces;

namespace TLGames.Applications.Services
{
    public abstract class BaseDAO<T>(IDbConnectionFactory connectionFactory) : ICrudOperationsAsync<T>, IQueryOperationsAsync, IExecuteOperationsAsync where T : class
    {
        protected abstract string TableName { get; }
        protected abstract string ColumnIdName { get; }
        protected readonly IColumnService _colService = App.SystemServices.GetService<IColumnService>();

        // GET ALL
        public virtual async Task<List<T>> GetAllAsync()
        {
            try
            {
                string query = $"SELECT * FROM {TableName}";
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<T> result = await connection.QueryAsync<T>(query);
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new();
            }
        }

        // GET SINGLE BY ID
        public virtual async Task<T> GetByIdAsync(string id)
        {
            try
            {
                string query = GetByIdQuery(ColumnIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                T result = await connection.QueryFirstOrDefaultAsync(query);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        // INSERT ENTITY
        public virtual async Task<int> InsertAsync(T entity)
        {
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int result = await connection.QueryFirstOrDefaultAsync(GetInsertQuery(), entity, transaction);
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
        }

        // 2. Usage in UpdateAsync
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int affectRow = await connection.ExecuteAsync(GetUpdateQuery(), entity, transaction);
                    transaction.Commit();
                    return affectRow > 0;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        //DELETE ENTITY
        public virtual async Task<bool> DeleteAsync(string id)
        {
            try
            {
                string query = DeleteByIdQuery(ColumnIdName);
                using IDbConnection connection = connectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int affectRow = await connection.ExecuteAsync(query, new { Id = id }, transaction);
                    transaction.Commit();
                    return affectRow > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

#nullable enable
        public virtual async Task<List<TResult>?> QueryAsync<TResult>(string query, object? parameters = null)
        {
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                IEnumerable<TResult> result = await connection.QueryAsync<TResult>(query, parameters);
                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return default;
            }
        }

        public virtual async Task<TResult?> QueryFirstOrDefaultAsync<TResult>(string query, object? parameters = null, IDbTransaction? transaction = null)
        {
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                try
                {
                    TResult? result = await connection.QueryFirstOrDefaultAsync(query, parameters, transaction);
                    transaction?.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction?.Rollback();
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return default;
            }
        }

        public virtual async Task<bool> ExecuteAsync(string query, object? parameters = null, IDbTransaction? transaction = null)
        {
            try
            {
                using IDbConnection connection = connectionFactory.CreateConnection();
                try
                {
                    int affectRow = await connection.ExecuteAsync(query, parameters, transaction);
                    transaction?.Commit();
                    return affectRow > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Commit!\n{ex.StackTrace}");
                    transaction?.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        protected virtual string GetByIdQuery(string colIdName)
        {
            if (!_colService.IsValidColumn(TableName, colIdName))
                return "";
            return $"SELECT * FROM {TableName} WHERE {colIdName} = @Id";
        }

        protected virtual string DeleteByIdQuery(string colIdName)
        {
            if (!_colService.IsValidColumn(TableName, colIdName))
                return "";
            return $"DELETE FROM {TableName} WHERE {colIdName} = @Id";
        }

#nullable disable
        protected abstract string GetInsertQuery();
        protected abstract string GetUpdateQuery();
    }
}
