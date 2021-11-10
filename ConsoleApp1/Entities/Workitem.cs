using System;
using System.Collections.Generic;

namespace ConsoleApp1.Entities
{
    public class Workitem : BaseModel
    {
        public Workitem() => Node = new WorkitemNode();
        public WorkitemNode Node { get; set; }
    }

    public class WorkitemNode : Node
    {
        public Workitem Owner { get; set; }
    }
}
