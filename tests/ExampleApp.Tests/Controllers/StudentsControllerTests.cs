using ExampleApp.Api.Controllers;
using ExampleApp.Api.Domain.Students;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace ExampleApp.Tests.Controllers;
public class StudentsControllerTests
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger = Utils.CreateLogger<StudentsController>();

    public StudentsControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [Fact]
    public async Task MapsStudentsAndTheirCourses()
    {
        // Arrange
        List<Student> students = new()
        {
            new Student(
                1,
                "Player One",
                "player-one",
                "InState",
                DateTimeOffset.Now,
                DateTimeOffset.Now
            ),
            new Student(
                2,
                "Santa I Claus",
                "santa-i-claus",
                "Foreign",
                DateTimeOffset.Now,
                DateTimeOffset.Now
            ),
            new Student(
                3,
                "Alf",
                "alf",
                "OutOfState",
                DateTimeOffset.Now,
                DateTimeOffset.Now
            )
        };
        _mediator.Send(Arg.Any<IRequest<ICollection<Student>>>())
            .Returns(students);

        // Act
        var response = await new StudentsController(_mediator, _logger).GetTheirCourses();

        // Assert
        response.Should().HaveCount(3);
        response.Should()
            .BeEquivalentTo(
                new[]
                {
                    new
                    {
                        Id = 1,
                        FullName = "Player One",
                        Badge = "player-one",
                        ResidenceStatus = "InState"
                    },
                    new
                    {
                        Id = 2,
                        FullName = "Santa I Claus",
                        Badge = "santa-i-claus",
                        ResidenceStatus = "Foreign"
                    },
                    new
                    {
                        Id = 3,
                        FullName = "Alf",
                        Badge = "alf",
                        ResidenceStatus = "OutOfState"
                    }
                });
    }
}
