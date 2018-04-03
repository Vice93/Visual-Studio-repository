using School.App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StudentHasCourse : Page
    {
        AppViewModel a;
        public StudentHasCourse()
        {
            this.InitializeComponent();

            a = new AppViewModel();

            a.RunOnce();
        }

        public void GetStudentHasCourse()
        {
            int n;
            bool result = Int32.TryParse(InputCourse.Text, out n);
            if (result)
            {
                CourseList.ItemsSource = a.GetStudentHasCourses(n);
                if (CourseList.Items.Count == 0)
                {
                    InputError.Text = "Fant ingen data på student med id " + n;
                }
            }
            else
            {
                InputError.Text = "Skriv et gyldig tall";
            }
            
        }
    }
}
