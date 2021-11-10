using System;
using NodaTime;

namespace ConsoleApp1.Entities
{
    public abstract class BaseModel
    {
        public virtual Guid Id { get; set; }
        public virtual string Caption { get; set; }
        public virtual string Description { get; set; }
        public virtual Instant CreatedDate { get; set; }
        public virtual Instant UpdatedDate { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
