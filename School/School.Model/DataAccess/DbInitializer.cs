    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Data.SqlClient;
    using System.Linq;
    using School.Model;

namespace School.Model.DataAccess
{
    public class DbInitializer
    {
        public void GetCourses(string connectionString)
        {
            const string getCoursesQuery = "select CourseId, CourseName, Points from dbo.Course";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State != System.Data.ConnectionState.Open) return;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = getCoursesQuery;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var course = new Course
                                {
                                    CourseId = reader.GetInt32(0),
                                    CourseName = reader.GetString(1),
                                    Points = reader.GetInt32(2)
                                };
                                ApiModel.CreateCourse(course);
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }

        public void GetStudents(string connectionString)
        {
            const string getStudentsQuery = "select StudentId, FirstName, LastName, StartedOn from dbo.Student;";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State != System.Data.ConnectionState.Open) return;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = getStudentsQuery;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var student = new Student
                                {
                                    StudentId = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    StartedOnDateTime = reader.GetDateTime(3)
                                };
                                ApiModel.CreateStudent(student);
                            }
                                
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }

        public void StudentHasCourse(string connectionString)
        {
            const string getStudentCoursesQuery = "select * from dbo.StudentHasCourse";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State != System.Data.ConnectionState.Open) return;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = getStudentCoursesQuery;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var studhasCourse = new StudentCourse
                                {
                                    StudentId = reader.GetInt32(0),
                                    CourseId = reader.GetInt32(1),
                                };
                                ApiModel.CreateStudentCourse(studhasCourse);
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
        }

    }
}
