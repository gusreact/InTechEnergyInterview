using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students;

[PrimaryKey(nameof(StudentId), nameof(CourseId))]
internal class StudentsCourses
{
    public int StudentId { get; init; }
    public string CourseId { get; init; }
}
