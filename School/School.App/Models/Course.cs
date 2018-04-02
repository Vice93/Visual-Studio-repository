using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using School.App.Annotations;

namespace School.App.Models
{
    public class Course : INotifyPropertyChanged
    {
		public int CourseId { get; set; }

		public string CourseName { get; set; }

		public int Points { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
