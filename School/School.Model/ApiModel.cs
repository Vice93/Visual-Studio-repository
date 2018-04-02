using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.App.DataAccess;

namespace School.Model
{
    public class ApiModel
    {
        private static string _connectionString = @"Data Source=donau.hiof.no;Initial Catalog=jonasv;Persist Security Info=True;User ID=jonasv;Password=Sp58y2";

        public static string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        private static List<Student> students = new List<Student>()
        {
            //Sample Data
            new Student
            {
                StudentId = 5540,
                FirstName = "Afzaal Ahmad",
                LastName = "Zeeshan",
                StartedOnDateTime = DateTime.Now
            },
            new Student
            {
                StudentId = 5541,
                FirstName = "Shani",
                LastName = "Kebar",
                StartedOnDateTime = DateTime.Now
            },
            new Student
            {
                StudentId = 5542,
                FirstName = "Daniel",
                LastName = "Ringby",
                StartedOnDateTime = DateTime.Now
            },
            new Student
            {
                StudentId = 5543,
                FirstName = "Henrik",
                LastName = "Smal",
                StartedOnDateTime = DateTime.Now
            }
        };

        private static List<Course> courses = new List<Course>()
        {
            new Course
            {
                CourseId = 1040,
                CourseName = "Bedriftsøkonomi",
                Points = 10
            },

            new Course
            {
                CourseId = 1041,
                CourseName = "Java",
                Points = 10
            },

            new Course
            {
                CourseId = 1042,
                CourseName = "Prosjektledelse",
                Points = 10
            },

            new Course
            {
                CourseId = 1043,
                CourseName = "Software Engineering",
                Points = 10
            }
        };

        // C part of CRUD
        public static void CreateStudent(Student student)
        {
            if (students.Any(s => s.StudentId == student.StudentId))
            {
                //If the student from the DB already exists in the system, do not create it
            }
            else
            {
                students.Add(student);
            }
        }

        // R part of CRUD
        public static List<Student> GetAllStudents()
        {
            return students;
        }

        public static Student GetStudent(int id)
        {
            return students.Find(x => x.StudentId == id); // Find one student and return him
        }

        // U part of CRUD
        public static void UpdateStudent(int id, Student student)
        {
            students.Remove(students.Find(x => x.StudentId == id)); // Remove the previous student
            students.Add(student);
        }

        // D part of CRUD 
        public static void DeleteStudent(int id)
        {
            students.Remove(students.Find(x => x.StudentId == id)); // Find and remove the student
        }


        // C part of CRUD
        public static void CreateCourse(Course course)
        {
            if (courses.Any(c => c.CourseId == course.CourseId))
            {
                //If the course from the DB already exists in the system, do not create it
            }
            else
            {
                courses.Add(course);
            }
        }

        // R part of CRUD
        public static List<Course> GetAllCourses()
        {
            return courses;
        }

        public static Course GetCourse(int id)
        {
            return courses.Find(x => x.CourseId == id); // Find one course and return it
        }

        // U part of CRUD
        public static void UpdateCourse(int id, Course course)
        {
            courses.Remove(courses.Find(x => x.CourseId == id)); // Remove the previous course
            courses.Add(course);
        }

        // D part of CRUD 
        public static void DeleteCourse(int id)
        {
            courses.Remove(courses.Find(x => x.CourseId == id)); // Find and remove the course
        }

        public static void UpdateStudents()
        {
            DbInitializer d = new DbInitializer();
            d.GetStudents(ConnectionString);
        }

        public static void UpdateCourses()
        {
            DbInitializer d = new DbInitializer();
            d.GetCourses(ConnectionString);
        }
    }
}
