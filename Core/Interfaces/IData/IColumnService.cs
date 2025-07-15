using System.Collections.Generic;

namespace TLGames.Core.Interfaces.IData
{
    public interface IColumnService
    {
        List<string> GetValidColumns(string tableName);
        bool IsValidColumn(string tableName, string columnName);
    }
}
