using System.Net.NetworkInformation;

namespace MovieLibrary.ApiSearch
{
    public class Connection
    {
        public bool IsInternetConnected { get; } = NetworkInterface.GetIsNetworkAvailable();
    }
}
