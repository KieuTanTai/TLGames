namespace TLGames.Services.Interfaces
{
    internal interface IDBConnection
    {
        void Connect();
        void ExecuteQuery(string query);
    }
}
