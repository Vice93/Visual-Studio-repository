using System.Collections.Generic;

namespace Oblig4.Model
{
    public class Course
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public List<Student> Students { get; set; }
    }
}

