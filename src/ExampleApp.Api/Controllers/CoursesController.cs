using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(IMediator mediator, ILogger<CoursesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet(Name = "GetCurrentCourses")]
    public async Task<IEnumerable<SemesterModel>> GetCurrent()
    {
        DateOnly today = new(2023, 9, 1);
        ICollection<Course> courses = await _mediator.Send(new GetCoursesActiveOnDateQuery(today));
        _logger.LogInformation("Retrieved {Count} current courses", courses.Count);

        Semester semester = await _mediator.Send(new GetSemesterActiveOnDateQuery(today));
        _logger.LogInformation("The current semester is ", semester.Id);

        List <SemesterModel> models = new();

        var coursesBySemester = courses.GroupBy(c => c.Semester.Id).ToList();

        foreach (var group in coursesBySemester)
        {
            List<CourseModel> coursesModel = new();
            foreach (Course course in group)
            {
                var professorModel = new KeyNameModel(course.Professor.Id.ToString(), course.Professor.FullName);
                var courseModel = new CourseModel(course.Id, course.Description, professorModel);
                coursesModel.Add(courseModel);
            }
            models.Add(new SemesterModel(semester.Id, semester.Description, semester.Start, semester.End, coursesModel));
        }

        return models;
    }

    [HttpPatch(Name = "UpdatesProfessor")]
    public async Task<ActionResult> UpdateProfessor([FromBody] ProfessorUpdateModel model)
    {
        var existingCourse = await _mediator.Send(new FindCourseByIdQuery(model.CourseId));
        if (existingCourse is null)
        {
            return NotFound($"Invalid course {model.CourseId}");
        }

        var professor = await _mediator.Send(new FindProfessorByNamedQuery(model.NewProfessorName));
        if (professor is null)
        {
            return NotFound($"Cannot file a professor named {model.NewProfessorName}");
        }

        _ = await _mediator.Send(new UpdateCourseProfessor(existingCourse.Id, professor.Id));
        return Accepted();
    }
}
