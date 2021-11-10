using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConsoleApp1.Entities;

namespace ConsoleApp1.Context.Config
{
    public class GraphPathConfiguration : IEntityTypeConfiguration<GraphPath>
    {
        public void Configure(EntityTypeBuilder<GraphPath> builder)
        {
            builder.HasNoKey();
        }
    }
}
