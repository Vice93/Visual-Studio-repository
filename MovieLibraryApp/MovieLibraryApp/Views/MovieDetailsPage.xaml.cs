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
using MovieLibrary.ApiSearch;
using Windows.UI.Notifications;

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

            if (!new Connection().IsInternetConnected) ShowToastNotification("No internet", "You need an internet connection to add or remove a movie from favorites.");

            if (CheckIfMovieIsFavorite(movieId)) // Delete from db
            {
                var response = await fvm.DeleteMovieFromDb(movieId);

                if (response)
                {
                    User.FavoriteMoviesIds.Remove(movieId);
                }
            }
            else //Add to db
            {
                var response = await fvm.InsertMovieInDb(movieId);

                if (response) User.FavoriteMoviesIds.Add(movieId);
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

            AddToFavorites.Background = new SolidColorBrush(Colors.DarkGray);
            AddToFavorites.Content = "Add to favorites";
            return false;
        }

        // Copied from https://stackoverflow.com/a/37542911/5309584
        private void ShowToastNotification(string title, string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }
    }
}
