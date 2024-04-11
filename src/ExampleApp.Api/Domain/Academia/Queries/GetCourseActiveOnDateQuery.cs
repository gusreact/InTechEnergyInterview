using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record GetCourseActiveOnDateQuery(DateOnly ActiveOn, string courseId) : IRequest<Course>;
