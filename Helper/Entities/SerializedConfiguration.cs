using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSoapApp.Helper.Entities
{
    public class SerializedConfiguration
    {
        public SerializedConfiguration()
        {

              //    private const string _clientID = "f14c1144-914b-4302-9df3-462d6ebdb332";
      // public const string CrmServiceUrl = "https://ODataCrm.crm4.dynamics.com";    
            ConfigTitle = "ModernOData";
            LastUpdateTicks = new DateTime(1900,1,1).Ticks;
            CrmUrl = "https://ODataCrm.crm4.dynamics.com"; 
            CrmApiUrl = "https://ODataCrm.crm4.dynamics.com"; 
            CrmDataServiceUrl = CrmUrl + "/XRMServices/2011/OrganizationData.svc";
            CrmSoapServiceUrl = CrmApiUrl + "/XRMServices/2011/Organization.svc/web";
            OAuthUrl = "https://login.windows.net/common/wsfed";
            CrmClientId = "f14c1144-914b-4302-9df3-462d6ebdb332"; 
            LocalSharePointFolderPath = string.Empty;
            CrmUserId = Guid.Empty.ToString();
        }

        public string ConfigTitle { get; set; }

        public long LastUpdateTicks { get; set; }

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
