using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using School.App.Models;
using School.App.Views;
using Template10.Mvvm;

namespace School.App.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        public static Uri BaseUri = new Uri("http://localhost:56921/api"); // Base api url

        //I need to figure out how to check for both studId and course Id in one loop when I create the objects, but for now run it when user opens the view.
        public void RunOnce()
        {
            var client = new HttpClient();
            {
                Debug.WriteLine("Student Has course");
                Task.Run(async () => await client.GetStringAsync(BaseUri + "/studenthascourse"));
            }
        }

        public void Update()
        {
            var client = new HttpClient();
            {
                Debug.WriteLine("Fetched the data from the database");
                Task.Run(async () => await client.GetStringAsync(BaseUri + "/update"));
            }
        }


        public List<Course> GetCourses()
        {
            try
            {

                using (var client = new HttpClient())
                {
                    var response = "";

                    Task task = Task.Run(async () => { response = await client.GetStringAsync(BaseUri + "/courses"); });

                    task.Wait();

                    return JsonConvert.DeserializeObject<List<Course>>(response);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            return new List<Course>();
        }

        public List<Student> GetStudents()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = "";

                    Task task = Task.Run(async () => { response = await client.GetStringAsync(BaseUri + "/students"); });

                    task.Wait();

                    return JsonConvert.DeserializeObject<List<Student>>(response);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            return new List<Student>();
        }

        public List<StudentCourse> GetStudentHasCourses(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = "";

                    Task task = Task.Run(async () => { response = await client.GetStringAsync(BaseUri + "/studenthas/" + id); });
                    
                    task.Wait();
                    
                    return JsonConvert.DeserializeObject<List<StudentCourse>>(response);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            return new List<StudentCourse>();
        }
    }
}
