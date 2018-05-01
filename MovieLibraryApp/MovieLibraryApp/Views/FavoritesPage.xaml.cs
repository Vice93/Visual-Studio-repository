using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;
using MovieLibrary.ApiSearch;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                if (!new Connection().IsInternetConnected)
                {
                    NoConnection.Text = "You need a working internet connection to see your favorite movies.";
                    return;
                }
                NoConnection.Text = "";

                LoadingIndicator.IsActive = true;

                FavoritesViewModel fvm = new FavoritesViewModel();

                var res = await fvm.GetFavoriteMovies();

                if (res != null) MainGrid.ItemsSource = res;
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }
    }
}
