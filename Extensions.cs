using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Godot;
public static class Extensions
{
    public static int LeadingSpaces(this string word) => word.TakeWhile(c => char.IsWhiteSpace(c)).Count();
    public static string ExtractFunctionName(this string line)
    {
        // Define the regex pattern to match a Python function definition
        string pattern = @"def\s+([\w_]+)\s*\(";

        // Use regex to extract the function name from the line of code
        Match match = Regex.Match(line, pattern);

        // If a match is found, return the captured group containing the function name
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        // Otherwise, return null to indicate no function name was found
        return null;
    }
    public static string ExtractClassName(this string line)
    {
        // Define the regex pattern to match a Python class definition
        string pattern = @"class\s+([\w_]+)";

        // Use regex to extract the class name from the line of code
        Match match = Regex.Match(line, pattern);

        // If a match is found, return the captured group containing the class name
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        // Otherwise, return null to indicate no class name was found
        return null;
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
       => self.Select((item, index) => (item, index));

    public static bool Open(this string line)
    {
        Dictionary<char, char> pairs = new Dictionary<char, char>()
        {
            {'}', '{'},
            {']', '['},
            {')', '('}
        };
        List<char> brackets = new List<char>();
        foreach (char c in line)
        {
            if (pairs.Values.Contains(c))
            {
                brackets.Add(c);
            }
            else if (pairs.Keys.Contains(c))
                if (brackets.Last() != pairs[c])
                    return true;
                else
                    brackets.RemoveAt(brackets.Count - 1);
        }
        if (brackets.Count > 0)
            return true;
        return false;
    }
    public static bool ContainsBreakers(this string line)
    {
        string[] words = { "break", "continue", "return" };
        foreach (var word in words)
        {
            if (line.Contains(word)) return true;
        }
        return false;
    }
    public static List<string> RemoveComments(this List<string> pythonCodes)
    {
        List<string> result = new List<string>();
        bool inQuotation = false;
        foreach (string codeLine in pythonCodes)
        {
            string uncommentedLine = "";
            for (int i = 0; i < codeLine.Length; i++)
            {
                char c = codeLine[i];
                if (c == '"' || c == '\'')
                {
                    inQuotation = !inQuotation;
                }
                if (c == '#' && !inQuotation)
                {
                    break;
                }
                uncommentedLine += c;
            }
            result.Add(uncommentedLine);
        }
        return result;
    }
    public static List<string> RemoveWhitespaces(this List<string> list) => list.Where(x => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrEmpty(x)).ToList();
}