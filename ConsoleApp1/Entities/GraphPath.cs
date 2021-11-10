using System;
using System.Collections.Generic;
using ConsoleApp1.Enums;

namespace ConsoleApp1.Entities
{
    public class GraphPath
    {
        public Guid[] Path { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public EdgeType Type { get; set; }
        public int Depth { get; set; }
    }
}
