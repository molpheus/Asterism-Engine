using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

namespace Asterism
{
    public class ExtensionString
    {
        static public string ToPascal(string text)
        {
            return Regex.Replace(
                text.Replace("_", " "),
                @"\b[a-z]",
                match => match.Value.ToUpper()).Replace(" ", "");
        }

        static public string ToCamel(string text)
        {
            return Regex.Replace(
                text.Replace("_", " "),
                @"\b[A-Z]",
                match => match.Value.ToLower()).Replace(" ", "");
        }
    }
}
