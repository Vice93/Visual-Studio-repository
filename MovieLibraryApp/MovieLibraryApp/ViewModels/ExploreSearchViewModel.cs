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
        private readonly ExploreSearch _search = new ExploreSearch();

        public Movie MovieObject { get; set; }

        public void GoToMovieDetailsPage() =>
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);

        public ObservableCollection<Movie> NormalSearch(string genre, string year, string type)
        {
            return _search.SearchForMovie(genre, year, type);
        }
    }
}
