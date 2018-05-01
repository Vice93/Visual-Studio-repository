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
    public class Search
    {
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

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

        //I realise this is a silly solution and a lot of redundant code (DRY), however since the API only give me 10 results per page and requires me to make an additional request per 10 more, I don't see a way around it.
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
