using LogServer.Application.Services;
using LogServer.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace LogServer.Tests
{
    public class LogServiceTests
    {
        [Fact]
        public void GivenLogsCollectionWithOnlyCompletedEvents_WhenFindEventsInLogs_ThenReturnEventsCollection()
        {
            // Given
            var logs = new List<Log>
            {
                new Log
                {
                    Id = "SampleId1",
                    Timestamp = 10,
                    State = "STARTED",
                    Host = "12345"
                },
                new Log
                {
                    Id = "SampleId2",
                    Timestamp = 30,
                    State = "FINISHED",
                },
                new Log
                {
                    Id = "SampleId2",
                    Timestamp = 20,
                    State = "STARTED",
                },
                new Log
                {
                    Id = "SampleId1",
                    Timestamp = 50,
                    State = "FINISHED",
                    Host = "12345"
                }
            };
            var logService = new LogService();

            var expectedEvents = new List<Event>
            {
                new Event
                {
                    EventId = "SampleId1",
                    Duration = 40,
                    Host = "12345"
                },
                new Event
                {
                    EventId = "SampleId2",
                    Duration = 10
                }
            };

            // When
            var events = logService.FindEventsInLogs(logs);

            // Then
            var eventEqualityComparer = new EventEqualityComparer();
            Assert.Equal(expectedEvents, events, eventEqualityComparer);

        }

        [Fact]
        public void GivenLogsCollectionWithNoCompletedEvents_WhenFindEventsInLogs_ThenReturnEmptyEventsCollection()
        {
            // Given
            var logs = new List<Log>
            {
                new Log
                {
                    Id = "SampleId1",
                    Timestamp = 10,
                    State = "STARTED",
                    Host = "12345"
                },
                new Log
                {
                    Id = "SampleId2",
                    Timestamp = 30,
                    State = "FINISHED",
                },
            };
            var logService = new LogService();

            // When
            var events = logService.FindEventsInLogs(logs);

            // Then
            Assert.Empty(events);
        }

        [Fact]
        public void GivenLogsWithOneLongEvent_WhenFindEventsInLogs_ThenAlertFlagForThisEventsIsTrue()
        {
            // Given
            var logs = new List<Log>
            {
                new Log
                {
                    Id = "SampleId1",
                    Timestamp = 1,
                    State = "STARTED",
                    Host = "12345"
                },
                new Log
                {
                    Id = "SampleId2",
                    Timestamp = 7,
                    State = "FINISHED",
                },
                new Log
                {
                    Id = "SampleId2",
                    Timestamp = 2,
                    State = "STARTED",
                },
                new Log
                {
                    Id = "SampleId1",
                    Timestamp = 5,
                    State = "FINISHED",
                    Host = "12345"
                }
            };
            var logService = new LogService();

            // When
            var events = logService.FindEventsInLogs(logs);

            // Then
            var expectedAlertEvent = events.SingleOrDefault(e => e.EventId == "SampleId2" && e.Alert);
            Assert.NotNull(expectedAlertEvent);

        }

        private class EventEqualityComparer : IEqualityComparer<Event>
        {
            public bool Equals(Event x, Event y)
            {
                return x.EventId == y.EventId
                    && x.Duration == y.Duration
                    && x.Host == y.Host
                    && x.Type == y.Type;
            }

            public int GetHashCode([DisallowNull] Event obj)
            {
                // Adding 1 to the HashCodes in case it gets zero and results in 0 for the whole product.
                var product = (obj.EventId.GetHashCode() + 1)
                    * (obj.Duration.GetHashCode() + 1);

                if (obj.Host != null)
                {
                    product *= obj.Host.GetHashCode() + 1;
                }
                if (obj.Type != null)
                {
                    product *= obj.Type.GetHashCode() + 1;
                }

                return product;
            }
        }
    }
}
