using System.Net.NetworkInformation;

namespace MovieLibrary.ApiSearch
{
    public class Connection
    {
        public bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
    }
}
