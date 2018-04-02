select Student.StudentId, FirstName, LastName, StartedOn, Course.CourseId, CourseName, Points from dbo.Student inner join dbo.StudentHasCourse on dbo.Student.StudentId = StudentHasCourse.StudentId
inner join dbo.Course on dbo.StudentHasCourse.CourseId = Course.CourseId;

select StudentId, FirstName, LastName, StartedOn from dbo.Student;

select CourseId, CourseName, Points from dbo.Course