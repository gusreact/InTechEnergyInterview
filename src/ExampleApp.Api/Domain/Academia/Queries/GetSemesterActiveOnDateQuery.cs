using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record GetSemesterActiveOnDateQuery(DateOnly ActiveOn) : IRequest<Semester>;
