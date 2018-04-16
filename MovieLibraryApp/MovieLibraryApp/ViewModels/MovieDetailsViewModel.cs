using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using MovieLibrary.Models.Model;
using MovieLibraryApp.Services;
using Newtonsoft.Json.Linq;
using Template10.Mvvm;

namespace MovieLibraryApp.ViewModels
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        private const string AuthToken = "310a11e6-a408-4367-869f-6307e49ded06";
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public async Task<ObservableCollection<Movie>> GetMoviesAsync(string id)
        {
            await Task.Run(() => LookUpMovie(id));
            return _movieList;
        }

        public void LookUpMovie(string id)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/graph/lookup?params=");

            _movieList.Clear();
            using (var client = new HttpClient())
            {
                var res = "";

                var param = "{\r\n  \"ids\": [\r\n    " + "\"" +  id + "\"" + "\r\n ],\r\n  \"components\": [\r\n    \"primaryImage\",\r\n    \"keyTraits\"\r\n  ]}";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
                Debug.WriteLine(baseUri + param);
                Task task = Task.Run(async () => { res = await client.GetStringAsync(baseUri + param); });

                task.Wait();

                JObject jobject = JObject.Parse(res);

                JToken movies = jobject["content"];

                for (var i = 0; i < movies.Count(); i++)
                {
                    try
                    {
                        var movie = movies[i];

                        var desc = (string)movie["object"]["description"] ?? "No description available";
                        var imgRef = (string)movie["object"]["primaryImage"]["object"]["small"]["url"] ?? "/Assets/noImageAvailable.png";
                        var genre = (string) movie["object"]["keyTraits"]["content"]["object"]["name"] ?? "No genre available";

                        if ((string) movie["object"]["releaseDate"] == null)
                        {
                            var mov = new Movie
                            {
                                MovieId = (string)movie["object"]["mhid"],
                                MovieName = (string)movie["object"]["name"],
                                Description = desc,
                                Genre = genre,
                                ImageReference = imgRef
                            };
                            _movieList.Add(mov);
                        }
                        else
                        {
                            var mov = new Movie
                            {
                                MovieId = (string)movie["object"]["mhid"],
                                MovieName = (string)movie["object"]["name"],
                                Description = desc,
                                ReleaseDate = ConvertDateTime.ConvertFromUnixTimestamp((double)movie["object"]["releaseDate"]),
                                Genre = genre,

                                ImageReference = imgRef
                            };
                            _movieList.Add(mov);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        break;
                    }
                }
            }
        }
    }
}
