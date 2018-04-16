using MovieLibraryApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml.Input;
using Template10.Services.NavigationService;

namespace MovieLibraryApp.Views
{
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel _mvm;
        public MainPage()
        {
            InitializeComponent();
            _mvm = new MainPageViewModel();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
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

        private void Search()
        {
            if (SearchInput.Text != "")
            {
                MainGrid.ItemsSource = _mvm.NormalSearch(SearchInput.Text);
            }
        }
    }
}