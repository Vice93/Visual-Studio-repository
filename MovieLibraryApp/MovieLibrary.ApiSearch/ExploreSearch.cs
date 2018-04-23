using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;

namespace MovieLibrary.ApiSearch
{
    public class ExploreSearch
    {
        private readonly OAuth2 _oAuth2 = new OAuth2();
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public async Task<ObservableCollection<Movie>> ExploreMovies(string genre, string year, string type)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/graph/explore?params=");



            _movieList.Clear();
            using (var client = new HttpClient())
            {
                var removeSpaceFromType = type.Replace(" ", string.Empty);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _oAuth2.Token);


                var param = "{\r\n \"filters\": \r\n{\"returnType\": \r\n{\"$eq\": \"" + removeSpaceFromType + "\"\r\n},\"traits\": \r\n{\"$eq\": \"mhgnr-" + genre.ToLower() + "\"\r\n},\"year\": \r\n{\"$gte\":" + year + "\r\n}\r\n}}";

                Debug.WriteLine(baseUri + param);
                var res = await client.GetAsync(baseUri + param);
                

                if (!res.IsSuccessStatusCode)
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

                        //Even though we search for movies/shows with the type/genre/releasedate etc. that info is not always available on the API. But we already know this info anyway because we searched specifically for it.
                        var mov = new Movie
                        {
                            MovieId = (string)movie["object"]["mhid"],
                            MovieName = (string)movie["object"]["name"],
                            Description = desc,
                            ReleaseDate = ConvertDateTime.ConvertFromUnixTimestamp(Double.Parse(year)),
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
