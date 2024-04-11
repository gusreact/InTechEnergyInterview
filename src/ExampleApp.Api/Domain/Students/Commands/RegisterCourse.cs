using MediatR;

namespace ExampleApp.Api.Domain.Students.Commands;

internal record RegisterCourse(int StudentId, string CourseId) : IRequest<Unit>;
