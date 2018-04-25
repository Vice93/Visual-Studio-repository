using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieLibrary.ApiSearch;
using MovieLibrary.Models.Model;
using Newtonsoft.Json.Linq;
using Template10.Mvvm;

namespace MovieLibraryApp.ViewModels
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        private readonly LookupSearch _ls = new LookupSearch();
        public async Task<ObservableCollection<Movie>> Lookup(string id)
        {
            Connection _status = new Connection();
            if (_status.isInternetConnected)
            {
                return await _ls.GetMovieInfoAsync(id);
            }
            return new ObservableCollection<Movie>();
        }
    }
}
