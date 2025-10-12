using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IConfigurationRepository
    {
        public Task<int> GetPointsPerOrderAsync();
        public Task<IEnumerable<Configuration>> GetConfigurationsAsync();
        public Task<Configuration?> GetConfigurationAsync(int id);
        public Task<Configuration?> GetFirstConfigurationAsync();
     

        public Task<bool> AddConfigurationAsync(Configuration Configuration);

        public Task<bool> UpdateConfigurationAsync(Configuration Configuration);

        public Task<bool> DeleteConfigurationAsync(int id);
        public IQueryable<Configuration> GetConfigurationApiAsync();
    }
}
