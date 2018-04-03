using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using School.App.Annotations;

namespace School.App.Models
{
    public class Course
    {
		public int CourseId { get; set; }

		public string CourseName { get; set; }

		public int Points { get; set; }
        
    }
}
