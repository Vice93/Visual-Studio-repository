﻿using Template10.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;
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

        public void GoToMovieDetailsPage()
        {
            if (MovieObject == null) return;
            NavigationService.Navigate(typeof(Views.MovieDetailsPage), MovieObject.MovieId);
        }

        public async Task<ObservableCollection<Movie>> NormalSearch(string searchInput)
        {
            if (new Connection().IsInternetConnected) return await _search.SearchForMovie(searchInput);
            return new ObservableCollection<Movie>();
        }
    }
}
