using ModernSoapApp.Helper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSoapApp.Service.Interfaces
{
   public interface IConfigurationService
    {
        Task<Configuration> GetCurrentConfiguration();
        void SaveConfiguration(Configuration configuration);
        List<Configuration> GetAllConfigurations();
    }
}
