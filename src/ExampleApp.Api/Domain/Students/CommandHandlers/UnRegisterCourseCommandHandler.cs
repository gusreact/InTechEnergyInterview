using ExampleApp.Api.Domain.Students.Commands;
using MediatR;

namespace ExampleApp.Api.Domain.Students.CommandHandlers;

internal class UnRegisterCourseCommandHandler : IRequestHandler<UnRegisterCourse, Unit>
{
    private readonly StudentsDbContext _context;
    private readonly ILogger<UnRegisterCourseCommandHandler> _logger;

    public UnRegisterCourseCommandHandler(
        StudentsDbContext context,
        ILogger<UnRegisterCourseCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(UnRegisterCourse request, CancellationToken cancellationToken)
    {
        _context.StudentsCourses.Remove(new StudentsCourses { StudentId = request.StudentId, CourseId = request.CourseId});
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
