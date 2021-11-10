using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConsoleApp1.Entities;

namespace ConsoleApp1.Context.Config
{
    public class WorkitemConfiguration : BaseModelConfiguration<Workitem>
    {
        public override void Configure(EntityTypeBuilder<Workitem> builder)
        {
            builder.HasOne(x => x.Node)
                .WithOne(x => x.Owner)
                .HasForeignKey<Workitem>(x => x.Id);
        }
    }
}
