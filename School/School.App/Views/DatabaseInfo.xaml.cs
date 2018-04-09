using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using School.App.ViewModels;
using School.App.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class DatabaseInfo : Page
    {
        private readonly AppViewModel _a;

        public DatabaseInfo()
        {
            InitializeComponent();

            _a = new AppViewModel();
        }

        

        public void GetCourses()
        {
            CourseList.ItemsSource = _a.GetCourses();
        }

        public void GetStudents()
        {
            StudentList.ItemsSource = _a.GetStudents();
        }

        public void Update()
        {
            _a.Update();
        }

    }
}
