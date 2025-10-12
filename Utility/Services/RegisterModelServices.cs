using Data.Context;
using Data.IRepository;
using Dataaccess.IRepository;
using Dataaccess.Repository;
using DataAccess.IRepository;
using DataAccess.Repository;
using DataAccess.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Utility.Services
{
    public class RegisterModelsServices
    {

        public static void RegisterModelServices(IServiceCollection services)
        {
            // Register Write DB
            services.AddDbContext<WriteDbContext>(options =>
                options.UseSqlServer(Config.Write_DefaultConnection));

            // Register Read DB
            services.AddDbContext<ReadDBContext>(options =>
                options.UseSqlServer(Config.Read_DefaultConnection));




            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOurClientRepository, OurClientRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();




        }

    }
}
