using System.Text.RegularExpressions;
using TLGames.Core.Interfaces.IValidate;

namespace TLGames.Infrastructure.Services
{
    public class StringChecker : IStringChecker
    {
        public bool IsSnakeCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return Regex.IsMatch(str, @"^[a-z]+(_[a-z0-9]+)*$");
        }

        public bool IsPascalCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            if (!char.IsUpper(str[0]))
            {
                return false;
            }
            return Regex.IsMatch(str, @"^[A-Z][a-zA-Z0-9]*$") || Regex.IsMatch(str, @"^[A-Z][a-z0-9]+([A-Z][a-z0-9]+)*$");
        }

        public bool IsCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return !char.IsUpper(str[0]) && Regex.IsMatch(str, @"^[a-z][a-zA-Z0-9]*$") || Regex.IsMatch(str, @"^[a-z][a-z0-9]+([A-Z][a-z0-9]+)*$");
        }

        public bool IsConstantCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return Regex.IsMatch(str, @"^[A-Z]+(_[A-Z0-9]+)*$");
        }

        public bool ContainsProblematicDbChars(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            string problematicCharsPattern = @"['"";`#&\|\*\/\\<>=%_!\x00-\x1F\x7F]";
            return Regex.IsMatch(str, problematicCharsPattern);
        }

        public bool IsSafeDbString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            string safePattern = @"^[a-zA-Z0-9\s_-]*$";
            return Regex.IsMatch(str, safePattern);
        }
    }
}
