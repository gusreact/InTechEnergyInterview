using MediatR;

namespace ExampleApp.Api.Domain.Students.Commands;

internal record UnRegisterCourse(int StudentId, string CourseId) : IRequest<Unit>;
