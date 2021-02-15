using System.Text.Json;

namespace LogServer.Domain
{
    public class JsonParser
    {
        public Log DeserializeLog(string logString)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var log = JsonSerializer.Deserialize<Log>(logString, jsonSerializerOptions);
            return log;
        }
    }
}
