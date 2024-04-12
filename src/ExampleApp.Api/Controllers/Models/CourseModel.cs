using ExampleApp.Api.Domain.Academia;
using System.Text.Json.Serialization;

namespace ExampleApp.Api.Controllers.Models;

public record CourseModel([property: JsonPropertyName("key")] string Id, [property: JsonPropertyName("name")] string Description, KeyNameModel Professor);
