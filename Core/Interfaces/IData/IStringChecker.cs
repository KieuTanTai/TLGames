namespace TLGames.Core.Interfaces.IData
{
    public interface IStringChecker
    {
        bool IsSnakeCase(string input);
        bool IsPascalCase(string input);
        bool IsCamelCase(string input);
        bool IsConstantCase(string input);
        bool ContainsProblematicDbChars(string str);
        bool IsSafeDbString(string str);
    }
}
