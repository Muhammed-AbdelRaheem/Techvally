using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using DataAccess.IRepository;
using DataAccess.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess.Repository
{
    public class ConfigurationRepository : IConfigurationRepository

    {
        private readonly WriteDbContext _context;
        private readonly ReadDBContext _read;
        private readonly IMapper _mapper;
        private IMemoryCache _cache;

        public ConfigurationRepository(WriteDbContext context,  ReadDBContext read, IMemoryCache cache)
        {
            _context = context;
            _read = read;
            _cache = cache;
        }
        public ConfigurationRepository(WriteDbContext context,  IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> AddConfigurationAsync(Configuration Configuration)
        {
            try
            {

                await _context.AddAsync(Configuration);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;

        }
        public async Task<bool> UpdateConfigurationAsync(Configuration Configuration)
        {
            try
            {
                _context.Update(Configuration);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public async Task<bool> DeleteConfigurationAsync(int id)
        {
            try
            {
                var Configuration = await _context.Configurations.FirstOrDefaultAsync(e => e.Id == id);
                if (Configuration == null)
                    return false;

                Configuration.Deleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        public async Task<Configuration?> GetConfigurationAsync(int id)
        {
            return await _read.Configurations.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Configuration?> GetFirstConfigurationAsync()
        {
            return await _read.Configurations.FirstOrDefaultAsync();
        }


        public async Task<int> GetPointsPerOrderAsync()
        {

            if (_cache.TryGetValue($"Configuration_points", out int tax))
                return tax;
            try
            {
                tax = await _read.Configurations
                    .Select(e => e.PointsPerOrder).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
            }

            _cache.Set("Configuration_points", tax, Config.GetCacheEntryOption(60));
            return tax;
        }
        public async Task<IEnumerable<Configuration>> GetConfigurationsAsync()
        {
            return await _read.Configurations.ToListAsync();
        }

        public IQueryable<Configuration> GetConfigurationApiAsync()
        {
            return _read.Configurations;

        }

       

    }

}
