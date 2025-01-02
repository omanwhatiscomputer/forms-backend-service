using System;
using Newtonsoft.Json.Linq;

namespace BackendService.RequestHelpers;

public class PropExtractor
{
    public static string CollectTextProperties(string jsonString)
    {
        try
        {
            // Parse the JSON input
            JToken jsonToken = JToken.Parse(jsonString);
            List<string> textValues = new List<string>();

            // Recursively find all "text" properties
            FindTextProperties(jsonToken, textValues);

            // Combine all text values into one string
            return string.Join(" ", textValues);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return string.Empty;
        }
    }

    public static void FindTextProperties(JToken token, List<string> textValues)
    {
        if (token == null) return;

        if (token.Type == JTokenType.Object)
        {
            foreach (var property in (JObject)token)
            {
                if (property.Key == "text" && property.Value.Type == JTokenType.String)
                {
                    textValues.Add(property.Value.ToString());
                }
                FindTextProperties(property.Value, textValues);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            foreach (var item in (JArray)token)
            {
                FindTextProperties(item, textValues);
            }
        }
    }
}
