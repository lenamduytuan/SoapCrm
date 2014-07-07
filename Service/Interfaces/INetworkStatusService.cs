using ModernSoapApp.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Interfaces
{
    public interface INetworkStatusService
    {
        bool IsOnline();
        bool IsOnlineWifi();
        string GetStatusString();

        event EventHandler<NetworkChangedEventArgs> NetworkStatusChanged;
    }
}
