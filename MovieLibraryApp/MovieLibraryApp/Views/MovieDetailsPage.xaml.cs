using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MovieLibraryApp.ViewModels;
using Template10.Services.SerializationService;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieLibraryApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieDetailsPage
    {
        private readonly MovieDetailsViewModel _mdvm;
        private readonly ISerializationService _serializationService;
        public MovieDetailsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            _mdvm = new MovieDetailsViewModel();
            _serializationService = SerializationService.Json;
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null) return;

            try
            {
                LoadingIndicator.IsActive = true;
                AddToFavorites.Visibility = Visibility.Collapsed;

                var id = _serializationService.Deserialize(e.Parameter?.ToString()).ToString();
                var res = await _mdvm.Lookup(id);
                MainGrid.ItemsSource = res;
            }
            finally
            {
                LoadingIndicator.IsActive = false;
                AddToFavorites.Visibility = Visibility.Visible;
            }
        }

        private void AddToFavorites_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
