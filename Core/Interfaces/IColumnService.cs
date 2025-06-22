using System.Collections.Generic;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces
{
    public interface IColumnService
    {
        List<string> GetValidColumns(string tableName);
        bool IsValidColumn(string tableName, string columnName);
    }
}
