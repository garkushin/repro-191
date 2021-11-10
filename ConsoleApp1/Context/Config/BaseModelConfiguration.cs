using Infrastructure.Context.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConsoleApp1.Entities;

namespace ConsoleApp1.Context.Config
{
    public class BaseModelConfiguration<TBaseModel> : IEntityTypeConfiguration<TBaseModel> where TBaseModel : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<TBaseModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Caption).HasMaxLength(150);
            builder.Property(x => x.CreatedDate).ValueGeneratedOnAdd().HasValueGenerator<DateNowGenerator>();
            builder.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate().HasValueGenerator<DateNowGenerator>();
            builder.Property(x => x.Deleted).HasDefaultValue(false);
            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
