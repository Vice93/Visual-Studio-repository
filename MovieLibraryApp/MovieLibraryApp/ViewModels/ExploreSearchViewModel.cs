using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.ApiSearch;
using MovieLibrary.Models.Model;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace MovieLibraryApp.ViewModels
{
    public class ExploreSearchViewModel : ViewModelBase
    {
        private readonly ExploreSearch _explore = new ExploreSearch();

        public Movie MovieObject { get; set; }

        public void GoToMovieDetailsPage()
        {
            if(MovieObject != null)
            {
                NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);
            }
        }
            

        public async Task<ObservableCollection<Movie>>Explore(string genre, string year, string type)
        {
            Connection _status = new Connection();
            if (_status.isInternetConnected)
            {
                return await _explore.ExploreMovies(genre, year, type);
            }
            return new ObservableCollection<Movie>();
        }
    }
}
