using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.App.Models;

namespace School.App.ViewModels
{
    public class SampleModel
    {
        public ObservableCollection<Course> SampleListCourse { get; set; }
        public ObservableCollection<Student> SampleListStudent { get; set; }
        private SampleData s;

        public SampleModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                s = new SampleData();

                SampleListCourse = new ObservableCollection<Course>();
                SampleListCourse = s.Courses;
                SampleListStudent = new ObservableCollection<Student>();
                SampleListStudent = s.Students;
            }
        }
    }
}
