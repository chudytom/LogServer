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
            var foundEvents = logs.GroupBy(log => log.Id)
                .Select(groupedLogs =>
                    {
                        if (groupedLogs.Count() != 2)
                            return null;

                        var sortedLogs = groupedLogs.OrderBy(log => log.Timestamp);
                        var startLog = sortedLogs.First();
                        var finishLog = sortedLogs.Last();

                        if (startLog.State != "STARTED" || finishLog.State != "FINISHED")
                        {
                            return null;
                        }

                        var foundEvent = new Event
                        {
                            EventId = groupedLogs.Key,
                            Duration = finishLog.Timestamp - startLog.Timestamp,
                            Host = startLog.Host ?? finishLog.Host,
                            Type = startLog.Type ?? finishLog.Type,
                        };
                        foundEvent.Alert = foundEvent.Duration > 4;

                        return foundEvent;
                    })
                .Where(foundEvent => foundEvent != null);

            return foundEvents;
        }
    }
}
