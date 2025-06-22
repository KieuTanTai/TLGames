namespace TLGames.Core.Interfaces
{
    internal interface IStringChecker
    {
        public bool IsSnakeCase(string input);
        public bool IsPascalCase(string input);
        public bool IsCamelCase(string input);
        public bool IsConstantCase(string input);
    }
}
