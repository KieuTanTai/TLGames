namespace TLGames.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        System.Data.IDbConnection CreateConnection();
        void ExecuteQuery(string query);
    }
}
