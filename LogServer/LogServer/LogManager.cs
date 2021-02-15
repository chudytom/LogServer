using LogServer.Domain;
using LogServer.Infrastructure.Files;
using System.Collections.Generic;
using System.Linq;

namespace LogServer.ConsoleApp
{
    public class LogManager
    {
        public IEnumerable<Log> ReadLogsFromFile(string filePath)
        {
            var fileReader = new FileReader();
            var allLines = fileReader.ReadAllLines(filePath);
            var jsonParser = new JsonParser();
            var logs = allLines.Select(line => jsonParser.DeserializeLog(line));
            return logs;
        }
    }
}
