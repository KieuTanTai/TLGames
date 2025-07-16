using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TLGames.Application.Services;
using TLGames.Core.Interfaces.IData;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Infrastructure.Data
{
    public abstract class BaseDAO<T>(
        IDbConnectionFactory connectionFactory,
        IColumnService colService,
        IStringConverter converter,
        IStringChecker checker,
        string tableName,
        string columnIdName,
        string secondColumnIdName = null) :IDAO<T>, ICrudOperationsAsync<T>, IQueryOperationsAsync, IExecuteOperationsAsync where T : class
    {
        protected IDbConnectionFactory ConnectionFactory { get; } = connectionFactory;
        protected IColumnService ColService { get; } = colService;
        protected IStringConverter Converter { get; } = converter;
        protected IStringChecker Checker { get; } = checker;

        protected string TableName { get; } = tableName;
        protected string ColumnIdName { get; } = columnIdName;
        protected string SecondColumnIdName { get; } = secondColumnIdName ?? string.Empty;

        // GET ALL
        public virtual async Task<List<T>> GetAllAsync()
        {
            try
            {
                string query = $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}";
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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

        public virtual async Task<int> InsertManyAsync(IEnumerable<T> entities)
        {
            try
            {
                using IDbConnection connection = ConnectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int result = 0;
                    foreach (T entity in entities)
                        result += await connection.ExecuteAsync(GetInsertQuery(), entity, transaction);
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
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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

        public virtual async Task<bool> UpdateManyAsync(IEnumerable<T> entities)
        {
            try
            {
                using IDbConnection connection = ConnectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int affectRow = 0;
                    foreach (T entity in entities)
                        affectRow += await connection.ExecuteAsync(GetUpdateQuery(), entity, transaction);
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
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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

        public virtual async Task<bool> DeleteManyAsync(IEnumerable<string> ids)
        {
            try
            {
                string query = DeleteByIdQuery(ColumnIdName);
                using IDbConnection connection = ConnectionFactory.CreateConnection();
                using IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int affectRow = 0;
                    foreach (string id in ids)
                        affectRow += await connection.ExecuteAsync(query, new { Id = id }, transaction);
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
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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
                using IDbConnection connection = ConnectionFactory.CreateConnection();
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
            if (!ColService.IsValidColumn(TableName, colIdName))
                return "";
            return $"SELECT * FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colIdName} = @Id";
        }

        protected virtual string DeleteByIdQuery(string colIdName)
        {
            if (!ColService.IsValidColumn(TableName, colIdName))
                return "";
            return $"DELETE FROM {(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))} WHERE {colIdName} = @Id";
        }

        protected bool IsValidStringInputDB(string input)
        {
            if (Checker.ContainsProblematicDbChars(input) || !Checker.IsSafeDbString(input))
                return false;
            return true;
        }

#nullable disable
        protected abstract string GetInsertQuery();
        protected abstract string GetUpdateQuery();
    }
}
