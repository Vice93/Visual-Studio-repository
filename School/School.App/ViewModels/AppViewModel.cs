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
using School.App.Views;
using School.Model;
using Template10.Mvvm;

namespace School.App.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        public static Uri BaseUri = new Uri("http://localhost:56921/api"); // Base api url

        public static Frame RootFrame { get; set; }
        public static User ActiveUser { get; set; }

        public void Update()
        {
            using (var client = new HttpClient())
            {
                Task.Run(async () => await client.GetStringAsync(BaseUri + "/update"));
            }
        }


        public void GetCourses()
        {
            try
            {

                var client = new HttpClient();
                {
                    var response = "";

                    Task task = Task.Run(async () => { response = await client.GetStringAsync(BaseUri + "/courses"); });

                    task.Wait();

                    DatabaseInfo d = new DatabaseInfo();
                    d.CourseList.Items.Clear();
                    foreach (var course in JsonConvert.DeserializeObject<List<Course>>(response))
                    {
                        d.CourseList.Items.Add(course); //WHY THE FUCK WONT IT SHOW....
                       
                        Debug.WriteLine("Added object with id: " + course.CourseId);
                    }

                    //Listview.Items.AddRange(d.courselist.items);
                    Debug.WriteLine(d.CourseList.Items.Count);

                    
                }
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Exception: " + e);
            }
 
        }

    }
}
