using System.Text.Json;

namespace DotNetCore.Helpers
{
    public static class SerializerHelper
    {
        public static string Serialize(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T? Deserialize<T>(this string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}