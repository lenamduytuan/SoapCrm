using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Windows.Networking.Connectivity;

namespace Sample.Service
{
    public class NetworkStatusService : INetworkStatusService
    {
        public NetworkStatusService()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        public bool IsOnline()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            if (connections == null)
            {
                return false;
            }

            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (NetworkStatusChanged != null)
            {

                NetworkChangedEventArgs e = new NetworkChangedEventArgs() { };
                e.Status = GetStatusString();
                e.NetWorkStatus = GetStatus();

                NetworkStatusChanged(this, e);
            }
        }

        public bool IsOnlineWifi()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();

            if (connections == null || connections.NetworkAdapter == null)
            {
                return false;
            }

            switch (connections.NetworkAdapter.IanaInterfaceType)
            {
                case 71:
                    return true;
                default:
                    return false;
            }

        }


        public event EventHandler<NetworkChangedEventArgs> NetworkStatusChanged;


        public string GetStatusString()
        {
            if (IsOnlineWifi())
            {
                return "Online WIFI";
            }
            else
            {
                if (IsOnline())
                {
                    return "Online";
                }
                else
                {
                    return "Offline";
                }
            }
        }

        public NetworkStatus GetStatus()
        {
            if (IsOnlineWifi())
            {
                return NetworkStatus.OnlineWifi;
            }
            else
            {
                if (IsOnline())
                {
                    return NetworkStatus.Online;
                }
                else
                {
                    return NetworkStatus.Offline;
                }
            }
        }
    }
}
