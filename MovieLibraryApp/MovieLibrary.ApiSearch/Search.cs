using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;

namespace MovieLibrary.ApiSearch
{
    public class Search
    {
        private readonly OAuth2 _oAuth2 = new OAuth2();
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public ObservableCollection<Movie> SearchForMovie(string searchInput)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/search/all/");

            _movieList.Clear();
            using (var client = new HttpClient())
            {
                var res = "";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _oAuth2.Token);

                Task task = Task.Run(async () => { res = await client.GetStringAsync(baseUri + searchInput + "?type=movie"); });

                task.Wait();

                JObject jobject = JObject.Parse(res);

                JToken movies = jobject["content"];

                for (var i = 0; i < movies.Count(); i++)
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
