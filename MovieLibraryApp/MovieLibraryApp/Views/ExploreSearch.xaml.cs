using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExploreSearch : Page
    {
        private readonly ExploreSearchViewModel _esv;
        public ExploreSearch()
        {
            this.InitializeComponent();
            _esv = new ExploreSearchViewModel();
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
            if (GenreCombo.SelectionBoxItem == null || YearCombo.SelectionBoxItem == null) return;
            var checkedButton = TypeRadioGroup.Children.OfType<RadioButton>()
                .FirstOrDefault(r => (bool)r.IsChecked);
            MainGrid.ItemsSource = _esv.NormalSearch((string)GenreCombo.SelectionBoxItem, (string)YearCombo.SelectionBoxItem, (string)checkedButton.Content);
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
