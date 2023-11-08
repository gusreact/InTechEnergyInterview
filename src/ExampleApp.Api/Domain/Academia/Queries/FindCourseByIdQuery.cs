using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record FindCourseByIdQuery(string Name) : IRequest<Course?>;
