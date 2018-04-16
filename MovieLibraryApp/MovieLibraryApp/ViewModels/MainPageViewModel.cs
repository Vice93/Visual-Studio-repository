using Template10.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;
using MovieLibrary.ApiSearch;

namespace MovieLibraryApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly Search _search = new Search();

        public Movie MovieObject { get; set; }

        public void GotoDetailsPage() =>
            NavigationService.Navigate(typeof(Views.DetailPage));

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        public void GoToMovieDetailsPage() =>
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);


        public ObservableCollection<Movie> NormalSearch(string searchInput)
        {
            return _search.SearchForMovie(searchInput);
        }

    }
}
