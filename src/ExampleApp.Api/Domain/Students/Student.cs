using ExampleApp.Api.Domain.Academia;

namespace ExampleApp.Api.Domain.Students;

internal class Student : AggregateRoot<int>
{
    public Student(
        int id,
        string fullName,
        string? badge,
        string residenceStatus,
        DateTimeOffset createdOn,
        DateTimeOffset lastModifiedOn)
    {
        Id = id;
        FullName = fullName;
        Badge = badge;
        ResidenceStatus = residenceStatus;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    public string FullName { get; init; } = "TBD";
    public string? Badge { get; init; }

    public string ResidenceStatus { get; init; }

    public List<Course> CoursesList { get; protected set; }
}
