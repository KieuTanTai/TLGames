using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using TLGames.Core.Interfaces.IData;
using TLGames.Infrastructure.Configuration;

namespace TLGames.Infrastructure.Services
{
    public class ColumnService(IDbConnectionFactory connectionFactory) : IColumnService
    {
        private readonly IDbConnectionFactory _connectionFactory = connectionFactory;
        private readonly Dictionary<string, HashSet<string>> _columnCache = new(StringComparer.OrdinalIgnoreCase);

        public List<string> GetValidColumns(string tableName)
        {
            try
            {
                if (_columnCache.TryGetValue(tableName, out var cached))
                {
                    return cached.ToList();
                }

                using IDbConnection connection = _connectionFactory.CreateConnection();

                if (connection is not DbConnection dbConnection)
                    throw new InvalidOperationException("Connection must inherit from DbConnection to use GetSchema.");

                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                var restrictions = new string[] { null, null, tableName, null };
                DataTable schema = dbConnection.GetSchema("Columns", restrictions);

                var columns = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    string columnName = row["COLUMN_NAME"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(columnName))
                        columns.Add(columnName);
                }

                var columnSet = new HashSet<string>(columns, StringComparer.OrdinalIgnoreCase);
                _columnCache[tableName] = columnSet;

                return columns;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetValidColumns] Error for table '{(IsValidStringInputDB(TableName) ? TableName : throw new ArgumentException("error Input"))}': {ex.Message}");
                return new();
            }
        }

        public bool IsValidColumn(string tableName, string columnName)
        {
            if (!_columnCache.TryGetValue(tableName, out var cols))
            {
                var colList = GetValidColumns(tableName);
                cols = new HashSet<string>(colList, StringComparer.OrdinalIgnoreCase);
                _columnCache[tableName] = cols;
            }

            return SqlWhitelist.IsSafeColumn(columnName) && cols.Contains(columnName);
        }

    }
}
