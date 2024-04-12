using System.Collections.Immutable;
using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetSemesterActiveOnDateHandler : IRequestHandler<GetSemesterActiveOnDateQuery, Semester>
{
    private readonly AcademiaDbContext _context;

    public GetSemesterActiveOnDateHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Semester> Handle(GetSemesterActiveOnDateQuery request, CancellationToken cancellationToken)
    {
        var semester = await _context.Semesters
            .Where(c => c.Start <= request.ActiveOn && request.ActiveOn <= c.End).FirstOrDefaultAsync();
        return semester;
    }
}
