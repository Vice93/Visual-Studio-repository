using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;

namespace MovieLibrary.ApiSearch
{
    public class ExploreSearch
    {
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public async Task<ObservableCollection<Movie>> ExploreMovies(string genre, string year, string type)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/graph/explore?params=");

            _movieList.Clear();
            using (var client = new HttpClient())
            {
                var removeDashFromType = type.Replace("/", string.Empty);
                var param = Uri.EscapeUriString("{\"filters\": {\"returnType\": {\"$eq\": \"" + removeDashFromType + "\"},\"traits\": {\"$eq\": \"mhgnr-" + genre.ToLower() + "\"},\"year\": {\"$in\":[" + year + "]}}, \"components\":[\"primaryImage\"]}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(OAuth2.TokenType, OAuth2.Token);
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + param);

                var result = await client.SendAsync(request);

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await OAuth2.GenerateAuth2TokenAsync("OAuth2 token expired. Generated a new one.");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(OAuth2.TokenType, OAuth2.Token);
                    result = await client.SendAsync(request);
                }
                else if (!result.IsSuccessStatusCode) return _movieList;

                var content = await result.Content.ReadAsStringAsync();

                JObject jobject = JObject.Parse(content);
                JToken movies = jobject["content"];

                if (movies.Any()) await AddMoviesToList(movies, genre, year);
                
                await NextResults(jobject, client, genre, year);

                return _movieList;
            }
        }

        //I realise this is a silly solution and a lot of redundant code (DRY), however since the API only give me 10 results per page and requires me to make an additional request per 10 more, I don't see a way around it.
        private async Task NextResults(JObject jobject, HttpClient client, string genre, string year)
        {
            for (var i = 0; i < 4; i++) //Because I have a very limited amount of requests to make, I only make the request 4 times (5 in total for 50 results).
            {
                var next = jobject["pagingInfo"]["next"].ToString(); //Somehow this is nesting the components part of the 'next' url causing it to fail because its nested >3 times, I have no clue why because it doesn't do it in /search.
                if (!next.Any()) return;
                
                int index = next.IndexOf("%2C%22components%22%3A%5B%7B%22name%22%3A%22metadata%22"); //Remove metadata components from the string so we can get the next pages.
                if (index >= 0)
                {
                    next = next.Remove(index);
                    next += "}]}";
                }

                var request = new HttpRequestMessage(HttpMethod.Get, next);

                var result = await client.SendAsync(request);
                if (!result.IsSuccessStatusCode) return;
                var content = await result.Content.ReadAsStringAsync();

                jobject = JObject.Parse(content);
                var movies = jobject["content"];

                await AddMoviesToList(movies,genre,year);
            }
            return;
        }

        public async Task AddMoviesToList(JToken movies, string genre, string year)
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
                catch (NullReferenceException e)
                {
                    Debug.WriteLine(e.Message);
                    break;
                }
            }
            await Task.CompletedTask;
        }
    }
}
