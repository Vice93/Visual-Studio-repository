using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using School.App.Annotations;

namespace School.App.Models
{
    public class Course : INotifyPropertyChanged
    {
        private int courseId;
        private string courseName;
        private int points;

        protected bool SetField<Course>(ref Course field, Course value,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<Course>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public int CourseId
        {
            get => courseId;
            set => SetField(ref courseId, value, nameof(courseId)); 
        }


        public string CourseName {
            get => courseName;
            set => SetField(ref courseName, value, nameof(courseName));
        }

        public int Points
        {
            get => points;
            set => SetField(ref points, value, nameof(points));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
