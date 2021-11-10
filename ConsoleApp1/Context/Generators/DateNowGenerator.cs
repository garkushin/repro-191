using Microsoft.EntityFrameworkCore.ChangeTracking;
using NodaTime;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context.Generators
{
    public class DateNowGenerator : Microsoft.EntityFrameworkCore.ValueGeneration.ValueGenerator<Instant>
    {
        public override bool GeneratesTemporaryValues => false;

        public override Instant Next([NotNull] EntityEntry entry) =>
            SystemClock.Instance.GetCurrentInstant();
    }
}
