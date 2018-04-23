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

                var param = @"{""ids"": [""mhmov-gladiator""],""components"": [""primaryImage"",""keyTraits""]}";

                var test = "{%22ids%22:[%22mhsss7qo1dwcAQid4ETZ2oJqq4yzrzG3uL1b0VklxMhU%22],%22components%22:%20[%22primaryImage%22,%22keyTraits%22]}";
                
                client.DefaultRequestHeaders.Add("Postman-Token", "af427dd0-b266-4652-a569-6ed7cb6977d6");
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
                Debug.WriteLine(baseUri + test);

                var res = await client.GetAsync(baseUri + param);
                if(!res.IsSuccessStatusCode)
                {
                    throw new Exception("HttpClient Error: " + res.StatusCode);
                }
                var content = await res.Content.ReadAsStringAsync();
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
