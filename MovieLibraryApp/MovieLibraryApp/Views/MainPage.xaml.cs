using System;
using MovieLibraryApp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml.Input;
using Template10.Utils;

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

        private void MovieBox_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var id = (sender as StackPanel).Tag as string;
                Debug.WriteLine(id);
                Frame.Navigate(typeof(MovieDetailsPage),id);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}