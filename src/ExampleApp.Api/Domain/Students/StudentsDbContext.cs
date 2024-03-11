using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students;

internal class StudentsDbContext : DbContext
{
    public StudentsDbContext(DbContextOptions<AcademiaDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}
