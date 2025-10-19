using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public static class Config
    {


        //Local
        public static string Write_DefaultConnection { get; set; } = "Server=.;DataBase=Techvally;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
        public static string Read_DefaultConnection { get; set; } = "Server=.;DataBase=Techvally;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
        public static string AssetsDomain { get; set; } = "http://localhost:5248";


        ////Live
        //public static string Write_DefaultConnection { get; set; } = "Server=db29218.public.databaseasp.net; Database=db29218; User Id=db29218; Password=Pg3-y9?X+sK6; Encrypt=False; MultipleActiveResultSets=True;";
        //public static string Read_DefaultConnection { get; set; } = "Server=db29218.public.databaseasp.net; Database=db29218; User Id=db29218; Password=Pg3-y9?X+sK6; Encrypt=False; MultipleActiveResultSets=True;";
        //public static string AssetsDomain { get; set; } = "http://techvalley-admin.runasp.net";
      
        



        public static string Placeholder { get; set; } = "images/front/placeholder.png";
        public static string Imageflow { get; set; } = "/uploads/images/";
        public static string AdminUserId { get; set; } = "d21e21ed-c2d7-4c1f-a903-9dcfe4f9b3db";
        public static string ImageResizerBox { get; set; } = "?w=600&h=600&scale=both&mode=pad";
        public static string Schema { get; set; } = "https://";
        public static string BaseURL { get; set; } = "https://.com";
        public static string PictureBaseURL { get; set; }
        public static string ImageResizerAdmin { get; set; } = "?w=100&h=100&scale=both&mode=pad";



        public static List<string> HiddenUsers()
        {
            return new List<string>() { "494b2239-5bd6-45e0-a2e4-b1b36ffb18ef", "74e5ca29-0135-4d98-8160-2a51394c8cec", "1b2d7150-805c-4863-885d-eb19093979e4", "fa4973c0-54a9-4322-9f01-b19ea83cd530" };
        }

        public static string? GetIpAddress(IHttpContextAccessor _httpContextAccessor)
        {
            var ipAddress = string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"]) ? "0:0:0:1" :
                _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            return ipAddress;
        }

        public static string? GetUserId(IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager)
        {
            var updatedByUserId = _httpContextAccessor?.HttpContext?.User != null ? _userManager.GetUserId(_httpContextAccessor?.HttpContext?.User) ??
                                                                                    AdminUserId : AdminUserId;
            return updatedByUserId;
        }

        public static MemoryCacheEntryOptions GetCacheEntryOption(int second = 30)
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(second),
                SlidingExpiration = TimeSpan.FromSeconds(second),
                Size = 1024,
            };
        }
    }
}
