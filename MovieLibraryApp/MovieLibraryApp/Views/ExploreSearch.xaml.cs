using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            PopulateYearCombo();
        }

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
                if (MainGrid.Items.Count == 0)
                {
                    EmptyList.Text = "Couldn't find any movies. Try a different search.";
                }
                else
                {
                    EmptyList.Text = "";
                }
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            Search();
        }

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
