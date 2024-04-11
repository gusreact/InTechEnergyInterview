using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students.QueryHandlers;

internal class GetStudentsHandler : IRequestHandler<GetStudentsQuery, ICollection<Student>>
{
    private readonly StudentsDbContext _context;

    public GetStudentsHandler(StudentsDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Student>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _context.Students
            //.Include(c => c.CoursesList)
            .ToListAsync(cancellationToken: cancellationToken);
        return students;
    }
}
