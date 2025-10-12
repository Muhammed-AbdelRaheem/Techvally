using Hangfire;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Serialization;
using Utility;

namespace Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // keep session alive
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // allow both http/https

                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            });
            // Add services to the container.
            builder.Services.AddControllersWithViews(option => option.EnableEndpointRouting = false)
                              .AddSessionStateTempDataProvider()
                              .AddJsonOptions(options =>
                              {
                                  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                  options.JsonSerializerOptions.PropertyNamingPolicy = null;
                              })
                              .AddCookieTempDataProvider()
                              .AddXmlSerializerFormatters()
                              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                              opts => { opts.ResourcesPath = "Resources"; })
                              .AddDataAnnotationsLocalization(options =>
                              {
                                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                                  {
                                      var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                                      return factory.Create("SharedResource", assemblyName.Name);
                                  };

                              });
            DependencyContainer.RegisterServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 7;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();


            //use hangfire
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "dashboard",
                pattern: "{*url}",
                defaults: new { controller = "Home", action = "Index" });

            app.Run();
        }
    }
}
