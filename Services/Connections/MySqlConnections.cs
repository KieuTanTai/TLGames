using System;
using TLGames.Services.Interfaces;

namespace TLGames.Services.Connections
{
    internal class MySqlConnections : IDBConnection
    {
        private readonly string _connectionString;
        public MySqlConnections(string connecionString)
        {
            if (string.IsNullOrEmpty(connecionString))
                throw new ArgumentNullException("Invalid Connection String!");
            _connectionString = connecionString;

        }

        public void Connect() { }

        public void ExecuteQuery(string query) { }
    }
}
