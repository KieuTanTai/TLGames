using MySql.Data.MySqlClient;
using System;
using System.Data;
using TLGames.Core.Interfaces.IData;

namespace TLGames.Infrastructure.Persistence.Repositories
{
    internal class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        private MySqlConnectionFactory()
        {
        }

        public MySqlConnectionFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("Invalid Connection String!");
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void ExecuteQuery(string query) { }

    }
}
