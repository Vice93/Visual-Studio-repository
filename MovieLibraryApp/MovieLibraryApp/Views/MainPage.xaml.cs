using MovieLibraryApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml.Input;
using MovieLibrary.ApiSearch;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace MovieLibraryApp.Views
{
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel _mvm;
        public MainPage()
        {
            InitializeComponent();
            _mvm = new MainPageViewModel();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            if (OAuth2.Token == null) Task.Run(GenerateToken); //Generate token when the app launches
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
                LoadingIndicator.IsActive = true;

                MainGrid.ItemsSource = await _mvm.NormalSearch(SearchInput.Text);
                EmptyList.Text = MainGrid.Items.Any() ? "" : "Couldn't find any movies. Try a different search.";
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