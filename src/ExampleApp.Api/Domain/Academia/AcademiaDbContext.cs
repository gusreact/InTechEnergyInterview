using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia;

internal class AcademiaDbContext : DbContext
{
    public AcademiaDbContext(DbContextOptions<AcademiaDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    protected DbSet<Professor> Professors { get; set; }
    protected DbSet<Semester> Semesters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Semester>(
            e =>
            {
                e.Property(x => x.Start).HasColumnName("StartDate");
                e.Property(x => x.End).HasColumnName("EndDate");
            });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}
