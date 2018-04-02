using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace School.Model
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
