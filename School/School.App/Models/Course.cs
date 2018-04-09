using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using School.App.Annotations;

namespace School.App.Models
{
    public class Course : INotifyPropertyChanged
    {
        private int _courseId;
        private string _courseName;
        private int _points;

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
            get => _courseId;
            set => SetField(ref _courseId, value, nameof(_courseId)); 
        }


        public string CourseName {
            get => _courseName;
            set => SetField(ref _courseName, value, nameof(_courseName));
        }

        public int Points
        {
            get => _points;
            set => SetField(ref _points, value, nameof(_points));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
