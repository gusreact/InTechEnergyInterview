using ExampleApp.Api.Domain.Students.Commands;
using MediatR;

namespace ExampleApp.Api.Domain.Students.CommandHandlers;

internal class RegisterCourseCommandHandler : IRequestHandler<RegisterCourse, Unit>
{
    private readonly StudentsDbContext _context;
    private readonly ILogger<RegisterCourseCommandHandler> _logger;

    public RegisterCourseCommandHandler(
        StudentsDbContext context,
        ILogger<RegisterCourseCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(RegisterCourse request, CancellationToken cancellationToken)
    {
        await _context.StudentsCourses.AddAsync(new StudentsCourses { CourseId = request.CourseId, StudentId = request.StudentId});

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
