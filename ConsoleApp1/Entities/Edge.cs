using NodaTime;
using System;
using ConsoleApp1.Enums;

namespace ConsoleApp1.Entities
{
    public class Edge
    {
        public Guid FromId { get; set; }
        public Node From { get; set; }
        public Guid ToId { get; set; }
        public Node To { get; set; }
        public EdgeType Type { get; set; }
        public Instant CreatedDate { get; set; }

        public Edge CreateInverse() => new()
        {
            FromId = ToId,
            ToId = FromId,
            Type = (EdgeType)((int)Type * -1)
        };
    }
}
