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
    /// <summary>
    /// The search class
    /// </summary>
    public class Search
    {
        /// <summary>
        /// The movie list
        /// </summary>
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        /// <summary>
        /// Searches the mediahound /search endpoint for movies based on the search input.
        /// </summary>
        /// <param name="searchInput">The search input.</param>
        /// <returns name="_movieList">The ObservableCollection containing Movie objects</returns>
        public async Task<ObservableCollection<Movie>> SearchForMovie(string searchInput)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/search/all/");
            var param = searchInput + "?types=movie&types=showseries";
            _movieList.Clear();
            using (var client = new HttpClient())
            {

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
                    result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, baseUri + param));
                }
                else if (!result.IsSuccessStatusCode) return _movieList;

                var content = await result.Content.ReadAsStringAsync();

                JObject jobject = JObject.Parse(content);
                JToken movies = jobject["content"];

                if (movies.Any()) await AddMoviesToList(movies);

                await NextResults(jobject, client);

                return _movieList;
            }
        }
        
        /// <summary>
        /// Gets the next page URI and performs a new search if it exists.
        /// Because of API limiations, it performs this search 4 extra times for a total of 50 results, however you can increase it to as many as you'd like.
        /// </summary>
        /// <param name="jobject">The jobject.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        private async Task NextResults(JObject jobject, HttpClient client)
        {
            for (var i = 0; i < 4; i++)
            {
                var next = jobject["pagingInfo"]["next"].ToString();
                if (!next.Any()) return;

                var request = new HttpRequestMessage(HttpMethod.Get, next);

                var result = await client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();

                jobject = JObject.Parse(content);
                var movies = jobject["content"];

                await AddMoviesToList(movies);

            }
            return;
        }

        /// <summary>
        /// Adds the movies to the Observable Collection.
        /// </summary>
        /// <param name="movies">The movies.</param>
        /// <returns></returns>
        private async Task AddMoviesToList(JToken movies)
        {
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
            await Task.CompletedTask;
        }
    }
}
