using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernSoapApp.Service.Interfaces;
using ModernSoapApp.Helper.Entities;
using System.IO;
using System.Runtime.Serialization;
using Windows.Storage;
using System.Xml.Serialization;
using Windows.Storage.Streams;
using Windows.Storage;

namespace ModernSoapApp.Service
{
    public class ConfigurationService : IConfigurationService
    {
        private const string ConfigurationFile = "configuration.json";
        private Configuration _configuration;

      public Configuration Configuration
        {
             get { return _configuration; }
        }
     

        public async Task<bool> SaveConfiguration(Configuration configuration=null)
      {
          _configuration = configuration;
            if (configuration == null)
            {
                _configuration = new Configuration
                {
                    ConfigTitle = "Kapor",
                    CrmApiUrl = "https://ODataCrm.api.crm4.dynamics.com",
                    CrmClientId = "f14c1144-914b-4302-9df3-462d6ebdb332",
                    CrmDataServiceUrl = "https://ODataCrm.crm4.dynamics.com/XRMServices/2011/OrganizationData.svc",
                    CrmSoapServiceUrl = "https://ODataCrm.crm4.dynamics.comm/XRMServices/2011/Organization.svc/web",
                    CrmUrl = "https://ODataCrm.crm4.dynamics.com",
                    OAuthUrl = "https://login.windows.net/semdev.onmicrosoft.com",
                    LastUpdateTicks = DateTime.MinValue.Ticks,
                    CrmUserId = Guid.Empty.ToString()
                };
                configuration = _configuration;
            }
           
            string configurationJson = configuration.Serialize();
             var applicationData = Windows.Storage.ApplicationData.Current;
    var localFolder = applicationData.LocalFolder;
            StorageFile storeFile = null;
            try
            {
                storeFile = await localFolder.CreateFileAsync(ConfigurationFile,
                Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            catch
            {
                return false;
            }
           
            await Windows.Storage.FileIO.WriteTextAsync(storeFile,
                            configurationJson);
        
           return true;
        }

        public async Task<Configuration> RestoreConfiguration()
        {
            _configuration = null;
            string configurationJson = String.Empty;
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localFolder = applicationData.LocalFolder;
            StorageFile storeFile = null;
            try
            {
                storeFile = await localFolder.GetFileAsync(ConfigurationFile);
               configurationJson =  await Windows.Storage.FileIO.ReadTextAsync(storeFile);
                _configuration = configurationJson.Deserialize<Configuration>();
            }
            catch(Exception e)
            {
                //_configuration = new Configuration
                //{
                //    ConfigTitle = "Kapor",
                //    CrmApiUrl = "https://ODataCrm.api.crm4.dynamics.com",
                //    CrmClientId = "f14c1144-914b-4302-9df3-462d6ebdb332",
                //    CrmDataServiceUrl = "https://ODataCrm.crm4.dynamics.com/XRMServices/2011/OrganizationData.svc",
                //    CrmSoapServiceUrl = "https://ODataCrm.crm4.dynamics.comm/XRMServices/2011/Organization.svc/web",
                //    CrmUrl = "https://ODataCrm.crm4.dynamics.com",
                //    OAuthUrl = "https://login.windows.net/semdev.onmicrosoft.com",
                //    LastUpdateTicks = DateTime.MinValue.Ticks,
                //    CrmUserId = Guid.Empty.ToString()
                //};

            }
            return _configuration;
        }
    }
}
