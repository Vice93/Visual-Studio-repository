using System.Net.NetworkInformation;

namespace MovieLibrary.ApiSearch
{
    public class Connection
    {
        private bool IsInternetConnected = NetworkInterface.GetIsNetworkAvailable();

        public bool IsInternetConnected { get => IsInternetConnected; }
    }
}
