using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace BackendService.RequestHelpers;

public class PropExtractor
{
    public static string CollectTextProperties(string mdString)
    {
        try
        {
            String[] textValues = mdString.Split("\n");

            for (int i = 0; i < textValues.Length; i++)
            {
                textValues[i] = Regex.Replace(textValues[i], "[^a-zA-Z0-9\\s\\-@%&]", "");
            }
            return string.Join(" ", textValues);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return string.Empty;
        }
    }
}
