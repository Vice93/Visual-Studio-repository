using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;
using Template10.Services.SerializationService;
using MovieLibrary.Models.Model;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieDetailsPage
    {
        private readonly MovieDetailsViewModel _mdvm;
        private readonly ISerializationService _serializationService;
        private string movieId = "";
        public MovieDetailsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            _mdvm = new MovieDetailsViewModel();
            _serializationService = SerializationService.Json;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null) return;

            try
            {
                LoadingIndicator.IsActive = true;
                AddToFavorites.Visibility = Visibility.Collapsed;

                movieId = _serializationService.Deserialize(e.Parameter?.ToString()).ToString();
                var res = await _mdvm.Lookup(movieId);
                MainGrid.ItemsSource = res;

                CheckIfMovieIsFavorite(movieId);
            }
            finally
            {
                LoadingIndicator.IsActive = false;
                AddToFavorites.Visibility = Visibility.Visible;
            }
        }

        private async void AddToFavorites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FavoritesViewModel fvm = new FavoritesViewModel();
            if (CheckIfMovieIsFavorite(movieId))
            {
                var response = await fvm.DeleteMovieFromDb(movieId);
                Debug.WriteLine(response);

                if (response)
                {
                    //User.FavoriteMoviesIds.Remove(movieId);
                }
                //Delete from DB
            }
            else
            {
                var response = await fvm.InsertMovieInDb(movieId);
                Debug.WriteLine(response);

                if (response) User.FavoriteMoviesIds.Add(movieId);
                //Add to DB
            }
            CheckIfMovieIsFavorite(movieId);
        }

        private bool CheckIfMovieIsFavorite(string id)
        {
            if (User.FavoriteMoviesIds.Contains(id)){
                AddToFavorites.Background = new SolidColorBrush(Colors.Red);
                AddToFavorites.Content = "Remove from favorites";
                return true;
            }
            else
            {
                AddToFavorites.Background = new SolidColorBrush(Colors.DarkGray);
                AddToFavorites.Content = "Add to favorites";
                return false;
            }
        }

        //Copied from http://joeljoseph.net/converting-hex-to-color-in-universal-windows-platform-uwp/
        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }
}
