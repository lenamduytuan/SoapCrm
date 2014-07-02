using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSoapApp.Helper.Entities
{
    public class Configuration
    {
        public Configuration(SerializedConfiguration serializedConfiguration)
        {
            SerializedConfiguration = serializedConfiguration;
        }

        public SerializedConfiguration SerializedConfiguration { get; private set; }

        public String ConfigTitle
        {
            get { return SerializedConfiguration.ConfigTitle; }
            set { SerializedConfiguration.ConfigTitle = value; }
        }

        public DateTime LastUpdate
        {
            get
            {
                return new DateTime(SerializedConfiguration.LastUpdateTicks, DateTimeKind.Utc);
            }
            set { SerializedConfiguration.LastUpdateTicks = value.Ticks; }
        }

        public Uri CrmDataServiceUri
        {
            get { return new Uri(SerializedConfiguration.CrmDataServiceUrl); }
            set { SerializedConfiguration.CrmDataServiceUrl = value.ToString(); }
        }

        public Uri CrmUri
        {
            get { return new Uri(SerializedConfiguration.CrmUrl); }
            set { SerializedConfiguration.CrmUrl = value.ToString(); }
        }

        public string CrmClientId
        {
            get { return SerializedConfiguration.CrmClientId; }
            set { SerializedConfiguration.CrmClientId = value; }
        }

        public string LocalSharePointFolderPath
        {
            get { return SerializedConfiguration.LocalSharePointFolderPath; }
            set { SerializedConfiguration.LocalSharePointFolderPath = value.ToString(); }
        }

        public Guid CrmUserId
        {
            get { return new Guid(SerializedConfiguration.CrmUserId); }
            set { SerializedConfiguration.CrmUserId = value.ToString(); }
        }

        public Guid UserBusinessUnitId
        {
            get { return new Guid(SerializedConfiguration.UserBusinessUnitId); }
            set { SerializedConfiguration.UserBusinessUnitId = value.ToString(); }
        }

        public Uri OAuthUrl
        {
            get { return new Uri(SerializedConfiguration.OAuthUrl); }
            set { SerializedConfiguration.OAuthUrl = value.ToString(); }
        }

        public Uri CrmSoapServiceUrl
        {
            get { return new Uri(SerializedConfiguration.CrmSoapServiceUrl); }
            set { SerializedConfiguration.CrmSoapServiceUrl = value.ToString(); }
        }

        public Uri CrmApiUrl
        {
            get { return new Uri(SerializedConfiguration.CrmApiUrl); }
            set { SerializedConfiguration.CrmApiUrl = value.ToString(); }
        }

    }
}
