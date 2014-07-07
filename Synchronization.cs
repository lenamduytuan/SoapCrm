using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernSoapApp.Helper.Entities;

namespace ModernSoapApp
{
    public static class Synchronization
    {

        private static bool _canGoOffline;
        /// <summary>
        /// Accounts ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public static bool CanGoOffline
        {
            get
            {
                return _canGoOffline;
            }
            set
            {
                if (value != null && value != _canGoOffline)
                {
                    _canGoOffline = value;
                }
            }
        }

        public static void Synchronize(Configuration conf)
        {
            if (conf.IsFirstRunSynchronized())
            {
                //create tables
            }
            
                //synchronize accounts(lastSync)
            

            //accounty 
            //contqacty

            //set last synced
            conf.LastUpdateTicks = DateTime.Now.Ticks;
        }

    }
}
