using LogServer.Domain;
using LogServer.Infrastructure.Files;
using System.Collections.Generic;
using System.Linq;

namespace LogServer.Application.Services
{
    public class LogService
    {
        public IEnumerable<Log> ReadLogsFromFile(string filePath)
        {
            var fileReader = new FileReader();
            var allLines = fileReader.ReadAllLines(filePath);
            var jsonParser = new JsonParser();
            var logs = allLines.Select(line => jsonParser.DeserializeLog(line));
            return logs;
        }

        public IEnumerable<Event> FindEventsInLogs(IEnumerable<Log> logs)
        {
            return new List<Event>();
        }
    }
}
