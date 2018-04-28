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
                    var res = await client.GetStringAsync(_baseUri + "/favorites?userId=" + User.UserId);
                    
                    Debug.WriteLine(res);

                    var ids = res.Replace("[", string.Empty).Replace("]", string.Empty);

                    LookupSearch ls = new LookupSearch();
                    var movies = await ls.GetMovieInfoAsync(ids);

                    return movies;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        
    }
}
