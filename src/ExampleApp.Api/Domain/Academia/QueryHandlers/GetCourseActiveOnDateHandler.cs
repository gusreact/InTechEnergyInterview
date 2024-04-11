using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetCourseActiveOnDateHandler : IRequestHandler<GetCourseActiveOnDateQuery, Course>
{
    private readonly AcademiaDbContext _context;

    public GetCourseActiveOnDateHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Course> Handle(GetCourseActiveOnDateQuery request, CancellationToken cancellationToken)
    {
        var courses = await _context.Courses
            .Where(c => c.Semester.Start <= request.ActiveOn && request.ActiveOn <= c.Semester.End && c.Id == request.courseId)
            .Include(c => c.Semester)
            .Include(c => c.Professor)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return courses;
    }
}
