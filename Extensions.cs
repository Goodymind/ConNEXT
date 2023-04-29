using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
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


}