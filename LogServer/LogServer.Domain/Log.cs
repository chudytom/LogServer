﻿namespace LogServer.Domain
{
    public class Log
    {
        public string Id { get; set; } = "";
        public string State { get; set; } = "";
        public long Timestamp { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }

    }
}
