using ExampleApp.Api.Domain.Academia;
using System.Text.Json.Serialization;
namespace ExampleApp.Api.Controllers.Models;

public record SemesterModel([property: JsonPropertyName("key")] string Id, [property: JsonPropertyName("name")] string Description, [property: JsonPropertyName("startDate")] DateOnly Start, [property: JsonPropertyName("endDate")] DateOnly End, List<CourseModel> Courses);
