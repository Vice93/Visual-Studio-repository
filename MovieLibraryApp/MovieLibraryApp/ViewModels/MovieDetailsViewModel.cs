using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http.Filters;
using MovieLibrary.ApiSearch;
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
            var baseUri = new Uri("https://api.mediahound.com/1.3/graph/lookup?params=");
            
            _movieList.Clear();

            using (var client = new HttpClient())
            {

                var param = "{\"ids\":[\"" + id +  "\"],\"components\":[\"primaryImage\",\"keyTraits\"]}";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
                Debug.WriteLine(baseUri + param);

                
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + param);
                request.Content = new StringContent("",
                                    Encoding.UTF8,
                                    "application/json");//CONTENT-TYPE header
                
                var result = await client.SendAsync(request);

                /*
                var res = await client.GetAsync(baseUri + param);
                if(!res.IsSuccessStatusCode)
                {
                    throw new Exception("HttpClient Error: " + res.StatusCode);
                }
                */
                var content = await result.Content.ReadAsStringAsync();
                
                JObject jobject = JObject.Parse(content);

                JToken movies = jobject["content"];

                for (var i = 0; i < movies.Count(); i++)
                {
                    try
                    {
                        var movie = movies[i];

                        var desc = (string)movie["object"]["description"] ?? "No description available";
                        var imgRef = (string)movie["object"]["primaryImage"]["object"]["small"]["url"] ?? "/Assets/noImageAvailable.png";
                        var genre = (string) movie["object"]["keyTraits"]["content"]["object"]["name"] ?? "No genre available";
                        var releaseDate = movie["object"]["releaseDate"] ?? 0;

                        var mov = new Movie
                        {
                            MovieId = (string)movie["object"]["mhid"],
                            MovieName = (string)movie["object"]["name"],
                            Description = desc,
                            ReleaseDate = ConvertDateTime.ConvertFromUnixTimestamp((double)releaseDate),
                            Genre = genre,
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
