namespace TLGames.Core.Interfaces.IData
{
    public interface IDbConnectionFactory
    {
        System.Data.IDbConnection CreateConnection();
        void ExecuteQuery(string query);
    }
}
