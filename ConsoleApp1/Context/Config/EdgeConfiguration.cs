using Infrastructure.Context.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConsoleApp1.Entities;

namespace ConsoleApp1.Context.Config
{
    public class EdgeConfiguration : IEntityTypeConfiguration<Edge>
    {
        public void Configure(EntityTypeBuilder<Edge> builder)
        {
            builder.HasKey(x => new { x.FromId, x.ToId, x.Type });
            builder.Property(x => x.CreatedDate).ValueGeneratedOnAdd().HasValueGenerator<DateNowGenerator>();
            builder.HasOne(x => x.To).WithMany().HasForeignKey(x => x.ToId);
        }
    }
}
