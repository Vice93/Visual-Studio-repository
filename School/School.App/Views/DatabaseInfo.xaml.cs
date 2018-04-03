﻿using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class DatabaseInfo : Page
    {
        AppViewModel a;
        public DatabaseInfo()
        {
            InitializeComponent();

            a = new AppViewModel();
            
        }

        public void GetCourses()
        {
            CourseList.ItemsSource = a.GetCourses();
        }

        public void GetStudents()
        {
            StudentList.ItemsSource = a.GetStudents();
        }

        public void Update()
        {
            a.Update();
        }

    }
}
