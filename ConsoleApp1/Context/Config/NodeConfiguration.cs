using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConsoleApp1.Entities;

namespace ConsoleApp1.Context.Config
{
    public class NodeConfiguration : IEntityTypeConfiguration<Node>
    {
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            builder.HasKey(x => x.OwnerId);
            builder.HasMany(x => x.Edges).WithOne(x => x.From).HasForeignKey(x => x.FromId);
            builder.HasDiscriminator(x => x.OwnerType)
                .HasValue<WorkitemNode>(nameof(Workitem));
        }
    }
}
