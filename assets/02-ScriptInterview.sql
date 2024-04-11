SELECT TOP (1000) [Id]
      ,[Description]
      ,[ProfessorId]
      ,[SemesterId]
      ,[CreatedOn]
      ,[LastModifiedOn]
  FROM [example-db].[dbo].[Courses]
SELECT TOP (1000) [Id]
      ,[FullName]
      ,[Extension]
      ,[CreatedOn]
      ,[LastModifiedOn]
  FROM [example-db].[dbo].[Professors]
  SELECT TOP (1000) [Id]
      ,[Description]
      ,[StartDate]
      ,[EndDate]
      ,[CreatedOn]
      ,[LastModifiedOn]
  FROM [example-db].[dbo].[Semesters]

  SELECT TOP (1000) [Id]
      ,[FullName]
      ,[Badge]
      ,[ResidenceStatus]
      ,[CreatedOn]
      ,[LastModifiedOn]
  FROM [example-db].[dbo].[Students]

  CREATE TABLE [example-db].[dbo].StudentsCourses(
  StudentId int not null,
  CourseId varchar(20) not null,
  primary key(StudentId,CourseId),
  constraint FK_StudentsCourses_StudentsId foreign key(StudentId) references [example-db].[dbo].Students(Id),
  constraint FK_StudentsCourses_CoursesId foreign key(CourseId) references [example-db].[dbo].Courses(Id)
  )

INSERT [dbo].[StudentsCourses] ([StudentId], [CourseId]) VALUES (1, N'POT-101-23')
GO
INSERT [dbo].[StudentsCourses] ([StudentId], [CourseId]) VALUES (2, N'POT-101-23')
GO
INSERT [dbo].[StudentsCourses] ([StudentId], [CourseId]) VALUES (2, N'TLP-201-23')
GO
INSERT [dbo].[StudentsCourses] ([StudentId], [CourseId]) VALUES (3, N'POT-101-23')
GO


ALTER TABLE [dbo].[StudentsCourses]  
DROP CONSTRAINT PK__Students__E5DB116311CA56CE;   
GO 
ALTER TABLE [dbo].[StudentsCourses]  
DROP CONSTRAINT FK_StudentsCourses_StudentsId;   
GO  
ALTER TABLE [dbo].[StudentsCourses]  
DROP CONSTRAINT FK_StudentsCourses_CoursesId;   
GO

DROP TABLE [dbo].[StudentsCourses]
