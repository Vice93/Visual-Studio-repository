using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MovieLibrary.ApiSearch;
using MovieLibrary.Models.Model;
using Template10.Mvvm;

namespace MovieLibraryApp.ViewModels
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        private readonly LookupSearch _ls = new LookupSearch();

        public Movie MovieObject { get; set; }

        public async Task<ObservableCollection<Movie>> Lookup(string id)
        {
            if (new Connection().IsInternetConnected) return await _ls.GetMovieInfoAsync(id);
            return new ObservableCollection<Movie>();
        }
    }
}
