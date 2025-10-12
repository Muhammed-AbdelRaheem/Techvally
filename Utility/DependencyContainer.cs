using Data.Context;
using DataAccess.Services;
using Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Services;
using Dataaccess.AutoMapper;
using Hangfire;


namespace Utility
{
    public class DependencyContainer
    {

       

        public static void RegisterServices(IServiceCollection services)
        {


            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<DependencyContainer>();
            });

            services.AddDbContext<WriteDbContext>(options =>
     options.UseSqlServer(Config.Write_DefaultConnection));

            services.AddDbContext<ReadDBContext>(options =>
                options.UseSqlServer(Config.Read_DefaultConnection));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
            })
           .AddEntityFrameworkStores<WriteDbContext>()
           .AddDefaultTokenProviders();


            RegisterModelsServices.RegisterModelServices(services);

            // Configure supported cultures and localization options
            services.Configure<RequestLocalizationOptions>(options => {
                var supportedCultures = new[]
                {
                    new CultureInfo("ar-EG"),
                    new CultureInfo("ar"),
                    new CultureInfo("en-GB"),
                };
                var cultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider()
                };
                options.DefaultRequestCulture = new RequestCulture("en-GB", "en-GB");

                options.SupportedCultures = supportedCultures;

                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = cultureProviders;

                //options.RequestCultureProviders.Insert(0, new RouteCultureProvider(options.DefaultRequestCulture));
            });

            services.AddDistributedMemoryCache();




            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddHttpContextAccessor();

            //hangfire setting
            services.AddHangfire(config =>

           
                    config.UseSqlServerStorage(Config.Write_DefaultConnection));
        }


    }
}
