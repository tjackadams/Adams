using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Infrastructure
{
    public sealed class SmokingContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "smoker";

        public SmokingContext(DbContextOptions<SmokingContext> options)
            : base(options)
        {
            Debug.WriteLine("OrderingContext::ctor ->" + GetHashCode());
        }
    }
}