using System.Linq;

namespace TLGames.Core.Interfaces
{
    public interface IStringConverter
    {
        string SnakeCaseToPascalCase(string snakeCase);
    }
}
