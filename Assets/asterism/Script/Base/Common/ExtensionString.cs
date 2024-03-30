using System.Text.RegularExpressions;

namespace Asterism
{
    static public class ExtensionString
    {
        static public string ToPascal(this string text)
        {
            return Regex.Replace(
                text.Replace("_", " "),
                @"\b[a-z]",
                match => match.Value.ToUpper()).Replace(" ", "");
        }

        static public string ToCamel(this string text)
        {
            return Regex.Replace(
                text.Replace("_", " "),
                @"\b[A-Z]",
                match => match.Value.ToLower()).Replace(" ", "");
        }
    }
}
