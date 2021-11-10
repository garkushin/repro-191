using System;
using System.Collections.Generic;

namespace ConsoleApp1.Entities
{
    public abstract class Node
    {
        public string OwnerType { get; set; }
        public Guid OwnerId { get; set; }
        public ICollection<Edge> Edges { get; set; }
    }
}
