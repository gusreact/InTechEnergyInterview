using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

internal record UpdateCourseProfessor(string CourseId, int NewProfessorId) : IRequest<Unit>;
