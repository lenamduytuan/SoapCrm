
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModernSoapApp.Service;

namespace ModernSoapApp.EventArgs
{
    public class NetworkChangedEventArgs : System.EventArgs
    {
        public string Status { get; set; }
        public NetworkStatus NetWorkStatus { get; set; }
    }
}
