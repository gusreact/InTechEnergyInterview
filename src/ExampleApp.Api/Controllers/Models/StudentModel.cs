using ExampleApp.Api.Domain.Students;

namespace ExampleApp.Api.Controllers.Models;

public record StudentModel(int Id, string FullName, string Badge, string ResidenceStatus);
