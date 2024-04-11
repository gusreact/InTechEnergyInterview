using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Students;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests.Controllers;
public class StudentControllerIntegrationTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly StudentsDbContext _db;
    private readonly StudentsController _controller;

    public StudentControllerIntegrationTests(DatabaseFixture fixture)
    {
        var mediator = (IMediator)fixture.Services.GetService(typeof(IMediator))!;
        var logger = (ILogger<StudentsController>)fixture.Services.GetService(typeof(ILogger<StudentsController>))!;
        _controller = new StudentsController(mediator, logger);
        _db = (StudentsDbContext)fixture.Services.GetService(typeof(StudentsDbContext))!;

        // remove previous values, in case there were left by an aborted debug session
        Dispose();

        // create records available for all tests
        _db.Database.ExecuteSql(
            $@"
INSERT [dbo].[Semesters] ([Id], [Description], [StartDate], [EndDate], [CreatedOn], [LastModifiedOn]) 
VALUES (N'2023-1', N'Spring ''23', CAST(N'2023-01-09' AS Date), CAST(N'2023-06-30' AS Date), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset))
INSERT [dbo].[Semesters] ([Id], [Description], [StartDate], [EndDate], [CreatedOn], [LastModifiedOn]) 
VALUES (N'2023-2', N'Fall ''23', CAST(N'2023-08-14' AS Date), CAST(N'2023-12-15' AS Date), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset))
INSERT [dbo].[Semesters] ([Id], [Description], [StartDate], [EndDate], [CreatedOn], [LastModifiedOn]) 
VALUES (N'2024-1', N'Spring ''24', CAST(N'2024-01-08' AS Date), CAST(N'2024-06-28' AS Date), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset))
SET IDENTITY_INSERT [dbo].[Professors] ON 
INSERT [dbo].[Professors] ([Id], [FullName], [Extension], [CreatedOn], [LastModifiedOn]) 
VALUES (1, N'Severus Snape', N'4851', CAST(N'2024-04-10T12:51:21.0411810-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0411810-03:00' AS DateTimeOffset))
INSERT [dbo].[Professors] ([Id], [FullName], [Extension], [CreatedOn], [LastModifiedOn]) 
VALUES (2, N'Charles Xavier', NULL, CAST(N'2024-04-10T12:51:21.0411810-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0411810-03:00' AS DateTimeOffset))
INSERT [dbo].[Professors] ([Id], [FullName], [Extension], [CreatedOn], [LastModifiedOn]) 
VALUES (3, N'Horace Slughorn', N'4850', CAST(N'2024-04-10T12:51:21.0411810-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0411810-03:00' AS DateTimeOffset))
SET IDENTITY_INSERT [dbo].[Professors] OFF
INSERT [dbo].[Courses] ([Id], [Description], [ProfessorId], [SemesterId], [CreatedOn], [LastModifiedOn]) 
VALUES (N'POT-101-23', N'Potions - 101; ''23', 1, N'2023-2', CAST(N'2024-04-10T12:51:21.0573942-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0573942-03:00' AS DateTimeOffset))
INSERT [dbo].[Courses] ([Id], [Description], [ProfessorId], [SemesterId], [CreatedOn], [LastModifiedOn]) 
VALUES (N'TLP-201-23', N'Telepathy, Advanced; ''23', 2, N'2023-2', CAST(N'2024-04-10T12:51:21.0573942-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0573942-03:00' AS DateTimeOffset))
SET IDENTITY_INSERT [dbo].[Students] ON 
INSERT [dbo].[Students] ([Id], [FullName], [Badge], [ResidenceStatus], [CreatedOn], [LastModifiedOn]) 
VALUES (1, N'Player One', N'player-one', N'InState', CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset))
INSERT [dbo].[Students] ([Id], [FullName], [Badge], [ResidenceStatus], [CreatedOn], [LastModifiedOn]) 
VALUES (2, N'Santa I Claus', N'santa-i-claus', N'Foreign', CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset))
INSERT [dbo].[Students] ([Id], [FullName], [Badge], [ResidenceStatus], [CreatedOn], [LastModifiedOn]) 
VALUES (3, N'Alf', N'alf', N'OutOfState', CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset), CAST(N'2024-04-10T12:51:21.0246989-03:00' AS DateTimeOffset))
SET IDENTITY_INSERT [dbo].[Students] OFF
INSERT [dbo].[StudentsCourses] ([StudentId], [CourseId]) 
VALUES (2, N'TLP-201-23');");
    }

    public void Dispose()
    {
        // clean up after tests
        _db.Database.ExecuteSql(
            $@"
DELETE FROM [example-db].dbo.StudentsCourses
DELETE FROM [example-db].dbo.Students WHERE Id = 1;
DELETE FROM [example-db].dbo.Students WHERE Id = 2;
DELETE FROM [example-db].dbo.Students WHERE Id = 3;
DELETE FROM [example-db].dbo.Courses WHERE Id = 'POT-101-23';
DELETE FROM [example-db].dbo.Courses WHERE Id = 'TLP-201-23';
DELETE FROM [example-db].dbo.Professors WHERE Id = 1;
DELETE FROM [example-db].dbo.Professors WHERE Id = 2;
DELETE FROM [example-db].dbo.Professors WHERE Id = 3;
DELETE FROM [example-db].dbo.Semesters WHERE Id = '2023-1';
DELETE FROM [example-db].dbo.Semesters WHERE Id = '2023-2';
DELETE FROM [example-db].dbo.Semesters WHERE Id = '2024-1';
            ");
    }

    [Fact]
    public async Task RegisterCourse()
    {
        // Arrange
        CourseRegisterModel payload = new(1, "POT-101-23");

        // Act
        var response = await _controller.Register(payload);

        // Assert
        response.Should().BeOfType<AcceptedResult>();

        StudentsCourses? studentCourse = await _db.StudentsCourses
            .FirstOrDefaultAsync(c => c.StudentId == payload.StudentId && c.CourseId == payload.CourseId);

        studentCourse.Should().NotBeNull();
        studentCourse.CourseId.Should().Be(payload.CourseId);
        studentCourse.StudentId.Should().Be(1);
    }

    [Fact]
    public async Task UnRegisterCourse()
    {
        // Arrange
        CourseRegisterModel payload = new(2, "TLP-201-23");

        // Act
        var response = await _controller.UnRegister(payload);

        // Assert
        response.Should().BeOfType<AcceptedResult>();

        StudentsCourses? studentCourse = await _db.StudentsCourses
            .FirstOrDefaultAsync(c => c.StudentId == payload.StudentId && c.CourseId == payload.CourseId);

        studentCourse.Should().BeNull();
    }

    [Fact]
    public async Task ReturnsNotFoundIfCourseIsNotFound()
    {
        // Arrange
        CourseRegisterModel payload = new(1, "TEST-00");

        // Act
        var response = await _controller.Register(payload);

        // Assert
        response.Should()
            .NotBeNull()
            .And.BeOfType<NotFoundObjectResult>();

        string? message = ((NotFoundObjectResult)response).Value as string;

        message.Should()
            .NotBeNull()
            .And.Be("Inactive course TEST-00");
    }
}
