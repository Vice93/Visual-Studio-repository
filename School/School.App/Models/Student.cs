using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace School.App.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;

        public DateTime StartedOnDateTime { get; set; }
        
    }
}
