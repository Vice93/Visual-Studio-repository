using System;
using System.Diagnostics;
using MovieLibraryApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml.Input;
using MovieLibrary.ApiSearch;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace MovieLibraryApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            //Retrieve the OAuth2 token when the app launches
            if (OAuth2.Token == null) Task.Run(GenerateToken); 

            //Get user GUID
            var eas = new EasClientDeviceInformation();
            MovieLibrary.Models.Model.User.UserId = eas.Id;
        }

        private void SearchIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Search();
        }

        private void SearchInput_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Search();
            }
        }

        private async void Search()
        {
            if (SearchInput.Text == "") return;
            try
            {
                var mpvm = new MainPageViewModel();
                LoadingIndicator.IsActive = true;
                Debug.WriteLine(MovieLibrary.Models.Model.User.UserId);
                MainGrid.ItemsSource = await mpvm.NormalSearch(SearchInput.Text);
                EmptyList.Text = MainGrid.Items.Any() ? "" : "'" + SearchInput.Text + "' didn't return any results. Try searching for something else!";
            } finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        private static async Task GenerateToken()
        {
            await OAuth2.GenerateAuth2TokenAsync("Retrieved OAuth2 token on app launch.");
        }
    }
}