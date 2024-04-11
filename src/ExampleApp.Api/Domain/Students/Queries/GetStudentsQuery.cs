using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record GetStudentsQuery() : IRequest<ICollection<Student>>;
