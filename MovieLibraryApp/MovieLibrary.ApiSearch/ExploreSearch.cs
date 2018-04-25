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

            if (_oAuth2.Expires_in <= 5)
            {
                await _oAuth2.GenerateAuth2TokenAsync();
            }

            _movieList.Clear();
            using (var client = new HttpClient())
            {
                var removeDashFromType = type.Replace("/", string.Empty);
                var param = Uri.EscapeUriString("{\"filters\": {\"returnType\": {\"$eq\": \"" + removeDashFromType + "\"},\"traits\": {\"$eq\": \"mhgnr-" + genre.ToLower() + "\"},\"year\": {\"$in\":[" + year + "]}}, \"components\":[\"primaryImage\"]}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_oAuth2.Token_type, _oAuth2.Token);
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + param);

                var result = await client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();

                JObject jobject = JObject.Parse(content);

                JToken movies = jobject["content"];

                AddMoviesToList(movies,genre,year);
                
                //I realise this is a silly solution and a lot of redundant code (DRY), however since the API only give me 10 results per page and requires me to make an additional request for 10 more, I don't see a way around it.
                JToken next = jobject["pagingInfo"];
                var nextPage = next["next"];
                if (nextPage != null)
                {
                    request = new HttpRequestMessage(HttpMethod.Get, (string)nextPage);

                    result = await client.SendAsync(request);
                    content = await result.Content.ReadAsStringAsync();

                    jobject = JObject.Parse(content);
                    movies = jobject["content"];

                    AddMoviesToList(movies,genre,year);
                }
                return _movieList;
            }
        }

        public void AddMoviesToList(JToken movies, string genre, string year)
        {
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
                        ReleaseDate = DateTime.ParseExact(year, "yyyy", null),
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
        }
    }
}
