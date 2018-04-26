using MovieLibraryApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml.Input;
using Template10.Services.NavigationService;
using System.Collections.ObjectModel;
using MovieLibrary.Models.Model;
using MovieLibrary.ApiSearch;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System;
using System.Linq;

namespace MovieLibraryApp.Views
{
    public sealed partial class MainPage : Page
    {
        //private readonly OAuth2 oAuth2;
        private readonly MainPageViewModel _mvm;
        //private ApplicationDataCompositeValue composite;
        public MainPage()
        {
            InitializeComponent();
            //oAuth2 = new OAuth2();
            _mvm = new MainPageViewModel();
            NavigationCacheMode = NavigationCacheMode.Enabled;
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


        /* Not sure how I can compare two composite values. Idea here was to only generate the OAuth2 token when it has expired, for instance if you leave the program on overnight and it expires in the meantime.
        private async Task CreateOAuth2Token()
        {
            await oAuth2.GenerateAuth2TokenAsync();
        }
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            composite = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["expires_in"];

            if (composite == null)
            {
                await CreateOAuth2Token();
                composite["expires_in"] = oAuth2.Expires_in;
                composite["saved_at_datetime"] = ConvertDateTime.ConvertToUnixTimestamp(DateTime.Now);
                ApplicationData.Current.LocalSettings.Values["expires_in"] = composite;
            } 
            else if(composite["expires_in"] <= (ConvertDateTime.ConvertToUnixTimestamp(DateTime.Now) - composite["saved_at_datetime"]))
            {
                await CreateOAuth2Token();
                composite["expires_in"] = oAuth2.Expires_in;
                composite["saved_at_datetime"] = ConvertDateTime.ConvertToUnixTimestamp(DateTime.Now);
                ApplicationData.Current.LocalSettings.Values["expires_in"] = composite;
            }
        }
        */
    }


}