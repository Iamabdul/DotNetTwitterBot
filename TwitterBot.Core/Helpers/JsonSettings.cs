using System.Text.Json;

namespace TwitterBot.Core.Helpers
{
    public static class TwitterJsonSettings
    {
        public static JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
