using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogServer.Domain
{
    public class Event
    {
        public string EventId { get; set; } = "";
        public long Duration { get; set; }
        public bool Alert { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }


    }
}
