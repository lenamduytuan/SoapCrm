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

namespace ModernSoapApp.Service
{
    public class ConfigurationService :IConfigurationService
    {
        private const string _ConfigurationFileName = "configuration.json";
        private Configuration _configuration;
        public async Task<Configuration> GetCurrentConfiguration()
        {
            if (_configuration != null)
            {
                return _configuration;
            }
            _configuration = new Configuration(new SerializedConfiguration());
            string configurationJson;
            StorageFile sampleFile = await ApplicationData.Current.LocalFolder.GetFileAsync(_ConfigurationFileName);
            return _configuration;
        }

        public async void SaveConfiguration(Configuration configuration)
        {
            MemoryStream sessionData = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(Configuration));
            serializer.WriteObject(sessionData, configuration);

            StorageFile file = await ApplicationData.Current.LocalFolder
                                     .CreateFileAsync(_ConfigurationFileName);
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                sessionData.Seek(0, SeekOrigin.Begin);
                await sessionData.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }

        public List<Configuration> GetAllConfigurations()
        {
            var configurations = new List<Configuration>();

            var semdev = new Configuration(new SerializedConfiguration
            {
            ConfigTitle = "ModernOData",
            LastUpdateTicks = new DateTime(1900,1,1).Ticks,
            CrmUrl = "https://ODataCrm.crm4.dynamics.com",
            CrmApiUrl = "https://ODataCrm.crm4.dynamics.com",
            CrmDataServiceUrl =  "https://ODataCrm.crm4.dynamics.com/XRMServices/2011/OrganizationData.svc",
            CrmSoapServiceUrl = "https://ODataCrm.crm4.dynamics.com/XRMServices/2011/Organization.svc/web",
            OAuthUrl = "https://login.windows.net/common/wsfed",
            CrmClientId = "f14c1144-914b-4302-9df3-462d6ebdb332", 
            LocalSharePointFolderPath = string.Empty,
            CrmUserId = Guid.Empty.ToString()
            });
            configurations.Add(semdev);
            return configurations;
        }
    }
}
