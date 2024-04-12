using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Students.Queries;
using ExampleApp.Api.Domain.Students;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students.Commands;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDataReader;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
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
            return NotFound($"Inactive course {model.CourseId}");
        }

        _ = await _mediator.Send(new RegisterCourse(model.StudentId, model.CourseId));
        return Accepted();
    }

    [HttpPatch(Name = "UnRegisterCourse")]
    public async Task<ActionResult> UnRegister([FromBody] CourseRegisterModel model)
    {
        DateOnly today = new(2023, 9, 1);
        var existingCourse = await _mediator.Send(new GetCourseActiveOnDateQuery(today, model.CourseId));
        if (existingCourse is null)
        {
            return NotFound($"Inactive course {model.CourseId}");
        }

        _ = await _mediator.Send(new UnRegisterCourse(model.StudentId, model.CourseId));
        return Accepted();
    }

    [HttpPost("BulkUpdateRegistration")]
    public async Task<ActionResult> BulkUpdate(IFormFile formFile)
    {
        List<StudentsCourses> studentsCourses = new List<StudentsCourses>();
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        using (var stream = new  MemoryStream())
        {
            formFile.CopyTo(stream);
            stream.Position = 0;
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Read()) //Each row of the file
                {
                    _ = await _mediator.Send(new RegisterCourse(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString()));
                }
            }
        }

        return Accepted();
    }
}
