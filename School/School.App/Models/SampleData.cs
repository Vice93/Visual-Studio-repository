using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.App.Models
{ 
    public class SampleData
    {
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<Student> Students { get; set; }

        public SampleData()
        {
            Courses = new ObservableCollection<Course>()
            {
                new Course()
                {
                    CourseId = 1030,
                    CourseName = "Sample Course 1",
                    Points = 10
                },
                new Course()
                {
                    CourseId = 1031,
                    CourseName = "Sample Course 2",
                    Points = 10
                },
                new Course()
                {
                    CourseId = 1032,
                    CourseName = "Sample Course 3",
                    Points = 10
                },
                new Course()
                {
                    CourseId = 1033,
                    CourseName = "Sample Course 4",
                    Points = 10
                }
            };
            Students = new ObservableCollection<Student>()
            {
                new Student()
                {
                    StudentId = 5560,
                    FirstName = "fName1",
                    LastName = "lName1",
                    StartedOnDateTime = DateTime.Now
                },
                new Student()
                {
                    StudentId = 5561,
                    FirstName = "fName2",
                    LastName = "lName2",
                    StartedOnDateTime = DateTime.Now
                },
                new Student()
                {
                    StudentId = 5562,
                    FirstName = "fName3",
                    LastName = "lName3",
                    StartedOnDateTime = DateTime.Now
                },
                new Student()
                {
                    StudentId = 5564,
                    FirstName = "fName4",
                    LastName = "lName4",
                    StartedOnDateTime = DateTime.Now
                }
            };
        }
    }
}
