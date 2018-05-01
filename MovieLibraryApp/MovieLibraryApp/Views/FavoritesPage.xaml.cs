using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;
using MovieLibrary.ApiSearch;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// The favorites page
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class FavoritesPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoritesPage"/> class.
        /// </summary>
        public FavoritesPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
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
