using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;


namespace ModernSoapApp.Helper.Entities
{
   
    public class Configuration
    {
        public bool IsFirstRunSynchronized { get; set; }
     
        public string ConfigTitle { get; set; }

        public long LastUpdateTicks { get; set; }

        public string AccesToken { get; set; }

        public string CrmDataServiceUrl { get; set; }

        public string CrmUrl { get; set; }

        public string CrmClientId { get; set; }

        public string LocalSharePointFolderPath { get; set; }

        public string CrmUserId { get; set; }

        public string UserBusinessUnitId { get; set; }

        public string OAuthUrl { get; set; }

        public string CrmSoapServiceUrl { get; set; }

        public string CrmApiUrl { get; set; }

    }
}
