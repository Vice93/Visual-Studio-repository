using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;

namespace MovieLibrary.ApiSearch
{
    public class LookupSearch
    {
        private readonly ObservableCollection<Movie> _movieList = new ObservableCollection<Movie>();

        public async Task<ObservableCollection<Movie>> GetMovieInfoAsync(string id)
        {
            var baseUri = new Uri("https://api.mediahound.com/1.3/graph/lookup?params=");

            if (OAuth2.ExpiresIn <= 5)
            {
                await OAuth2.GenerateAuth2TokenAsync();
            }

            _movieList.Clear();

            using (var client = new HttpClient())
            {

                var param = Uri.EscapeUriString("{\"ids\":[\"" + id + "\"],\"components\":[\"primaryImage\",\"keyTraits\",\"keySuitabilities\"]}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(OAuth2.TokenType, OAuth2.Token);
                Debug.WriteLine(baseUri + param);


                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUri + param);

                var result = await client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();

                JObject jobject = JObject.Parse(content);

                try
                {
                    JToken movie = jobject["content"][0];

                    var desc = (string)movie["object"]["description"] ?? "No description available";
                    var imgRef = (string)movie["object"]["primaryImage"]["object"]["small"]["url"] ?? "/Assets/noImageAvailable.png";
                    var releaseDate = movie["object"]["releaseDate"] ?? 1;
                    var genre = "";
                    var pg = "Not specified";

                    if (movie["object"]["keySuitabilities"]["content"].Any())
                    {
                        pg = (string)movie["object"]["keySuitabilities"]["content"][0]["object"]["name"];
                    }

                    var genreCount = movie["object"]["keyTraits"]["content"] ?? 0;
                    for (var i = 0; i < genreCount.Count(); i++)
                    {
                        genre += (string)movie["object"]["keyTraits"]["content"][i]["object"]["name"];
                        if(genreCount.Count()-1 != i)
                        {
                            genre += ", ";
                        }
                    }

                    if (!genreCount.Any())
                    {
                        genre = "No genre available";
                    }

                    var mov = new Movie
                    {
                        MovieId = (string)movie["object"]["mhid"],
                        MovieName = (string)movie["object"]["name"],
                        Description = desc,
                        ReleaseDate = ConvertDateTime.ConvertFromUnixTimestamp((double)releaseDate),
                        Genre = genre,
                        Pg = pg,
                        ImageReference = imgRef
                    };
                    _movieList.Add(mov);

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                return _movieList;
            }

        }
    }
}
