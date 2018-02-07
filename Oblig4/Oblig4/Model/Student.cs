using System;
using System.Collections.Generic;


namespace Oblig4.Model
{
    public class Student
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get => DateTime.Today.Year - DateOfBirth.Year; }

        public virtual List<Course> Courses { get; set; }

    }
}
