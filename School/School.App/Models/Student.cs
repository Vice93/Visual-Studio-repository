using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace School.App.Models
{
    public class Student : INotifyPropertyChanged
    {
        private int _studentId;
        private string _firstName;
        private string _lastName;
        private DateTime _startedOnDateTime;

        protected bool SetField<Student>(ref Student field, Student value,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<Student>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string FullName => FirstName + " " + LastName;

        public int StudentId
        {
            get => _studentId;
            set => SetField(ref _studentId, value, nameof(_studentId));
        }

        public string FirstName
        {
            get => _firstName;
            set => SetField(ref _firstName, value, nameof(_firstName));
        }

        public string LastName
        {
            get => _lastName;
            set => SetField(ref _lastName, value, nameof(_lastName));
        }



        public DateTime StartedOnDateTime
        {
            get => _startedOnDateTime;
            set => SetField(ref _startedOnDateTime, value, nameof(_startedOnDateTime));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
