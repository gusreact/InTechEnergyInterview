using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Students.Queries;
using ExampleApp.Api.Domain.Students;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students.Commands;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IMediator mediator, ILogger<StudentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet(Name = "GetStudentsAndTheirCourses")]
    public async Task<IEnumerable<StudentModel>> GetTheirCourses()
    {
        ICollection<Student> students = await _mediator.Send(new GetStudentsQuery());
        _logger.LogInformation("Retrieved {Count} students", students.Count);

        List<StudentModel> models = new();
        foreach (var student in students)
        {
            //var semesterModel = new KeyNameModel(course.Semester.Id, course.Semester.Description);
            //var professorModel = new KeyNameModel(course.Professor.Id.ToString(), course.Professor.FullName);
            StudentModel studentModel = new(student.Id, student.FullName, student.Badge, student.ResidenceStatus);
            models.Add(studentModel);
        }

        return models;
    }

    [HttpPatch(Name = "RegisterCourse")]
    public async Task<ActionResult> Register([FromBody] CourseRegisterModel model)
    {
        DateOnly today = new(2023, 9, 1);
        var existingCourse = await _mediator.Send(new GetCourseActiveOnDateQuery(today, model.CourseId));
        if (existingCourse is null)
        {
            return NotFound($"Invalid course {model.CourseId}");
        }

        _ = await _mediator.Send(new RegisterCourse(model.StudentId, model.CourseId));
        return Accepted();
    }
}
