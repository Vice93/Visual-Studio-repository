using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// The explore search page.
    /// </summary>
    public sealed partial class ExploreSearch : Page
    {
        /// <summary>
        /// The explore search viewmodel.
        /// </summary>
        private readonly ExploreSearchViewModel _esv;
        /// <summary>
        /// Initializes a new instance of the <see cref="ExploreSearch"/> class.
        /// </summary>
        public ExploreSearch()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            _esv = new ExploreSearchViewModel();
            PopulateYearCombo();
        }

        /// <summary>
        /// Performs a search based on the selected parameters.
        /// </summary>
        private async void Search()
        {
            if (GenreCombo.SelectionBoxItem == null || YearCombo.SelectionBoxItem == null) return;

            try
            {
                LoadingIndicator.IsActive = true;

                var checkedButton = TypeRadioGroup.Children.OfType<RadioButton>()
                .FirstOrDefault(r => (bool)r.IsChecked);
                var res = await _esv.Explore((string)GenreCombo.SelectionBoxItem, YearCombo.SelectionBoxItem.ToString(),
                    (string)checkedButton.Content);

                MainGrid.ItemsSource = res;
                EmptyList.Text = MainGrid.Items.Any() ? "" : "Nothing was found. Try some other parameters!";
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        /// <summary>
        /// Handles the OnClick event of the SearchButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Populates the year combo with the last 50 years.
        /// </summary>
        private void PopulateYearCombo()
        {
            for(var i=0; i<50; i++)
            {
                var year = DateTime.Now.Year - i;
                var item = new ComboBoxItem
                {
                    Content = year
                };
                YearCombo.Items.Add(item);
            }
        }
    }
}
