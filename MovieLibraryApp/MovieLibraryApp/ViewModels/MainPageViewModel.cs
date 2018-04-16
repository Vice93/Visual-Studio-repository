using Template10.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;

namespace MovieLibraryApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private const string AuthToken = "310a11e6-a408-4367-869f-6307e49ded06";

        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public Movie MovieObject { get; set; }

        public void GotoDetailsPage() =>
            NavigationService.Navigate(typeof(Views.DetailPage));

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        public void GoToMovieDetailsPage() =>
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);


        //Temporary for testing
        public ObservableCollection<Movie> NormalSearch(string searchInput)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/search/all/");

            _movieList.Clear();
            using (var client = new HttpClient())
            {
                var res = "";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",AuthToken);

                Task task = Task.Run(async () => { res = await client.GetStringAsync(baseUri + searchInput + "?type=movie"); });

                task.Wait();

                JObject jobject = JObject.Parse(res);

                JToken movies = jobject["content"];
                
                for(var i = 0; i<movies.Count(); i++)
                {
                    try
                    {
                        var movie = movies[i];
                        
                        var imgRef = (string)movie["object"]["primaryImage"]["object"]["small"]["url"] ?? "/Assets/noImageAvailable.png";
                        

                        var mov = new Movie
                        {
                            MovieId = (string)movie["object"]["mhid"],
                            MovieName = (string)movie["object"]["name"],
                            ImageReference = imgRef
                        };
                        _movieList.Add(mov);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        break;
                    }
                }
                return _movieList;
            }
        }

    }
}
