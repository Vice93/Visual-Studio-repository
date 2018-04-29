using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.ApiSearch;
using MovieLibrary.Models.Model;
using Newtonsoft.Json;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace MovieLibraryApp.ViewModels
{
    public class FavoritesViewModel : ViewModelBase
    {
        private readonly Uri _baseUri = new Uri("http://localhost:50226/api");

        public Movie MovieObject { get; set; }

        public void GoToMovieDetailsPage()
        {
            if (MovieObject == null) return;
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);
        }

        public async Task<ObservableCollection<Movie>> GetFavoriteMovies()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    LookupSearch ls = new LookupSearch();

                    var res = await client.GetStringAsync(_baseUri + "/favorites?userId=" + User.UserId); 
                    var movies = JsonConvert.DeserializeObject<ObservableCollection<Movie>>(res);

                    var idString = "";
                    bool cont = false;
                    for (var i=0; i<movies.Count; i++)
                    {
                        if (!User.FavoriteMoviesIds.Contains(movies[i].MovieId) || User.FavoriteMoviesIds.Count != movies.Count)
                        {
                            User.FavoriteMoviesIds.Add(movies[i].MovieId);

                            idString += movies[i].MovieId;
                            if (i != movies.Count - 1) idString += "\",\"";
                            cont = true;
                        }
                    }
                    if (cont)
                    {
                        var result = await ls.GetMovieInfoAsync(idString);

                        return result;
                    }
                    if (movies.Count == 0)
                    {
                        User.FavoriteMoviesIds.Clear();
                        return new ObservableCollection<Movie>();
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> DeleteMovieFromDb(string movieId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var res = await client.DeleteAsync(_baseUri + "/favorites?userId=" + User.UserId + "&movieId=" + movieId);

                    if (res.IsSuccessStatusCode) return true;
                    return false;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> InsertMovieInDb(string movieId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("userId", User.UserId.ToString()),
                        new KeyValuePair<string, string>("movieId",movieId)
                    });

                    var res = await client.PostAsync(_baseUri + "/favorites", content);

                    if(res.IsSuccessStatusCode) return true;
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        
    }
}
