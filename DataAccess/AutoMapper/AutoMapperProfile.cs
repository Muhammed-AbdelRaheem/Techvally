using DataAccess.Services;
using Domain.Models;
using Domain.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Reflection;
using Profile = AutoMapper.Profile;

namespace Dataaccess.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            string? language = null;
            string? userId = null;
            bool? apiAssets = false;
            int parentFilterValueId = 0;
            List<int>? selectedInterests = new List<int>();
            int? eventId = null;
            int allLevelsCount = 0;

            CreateMap<Log, LogViewModel>().ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc.ToUniversalTime())).ReverseMap();
            CreateMap<Log, LogDataTable>().ReverseMap();
            CreateMap<ApplicationUser, LogUserViewModel>();
            CreateMap<ApplicationUser, UserEditInfoViewModel>();
            CreateMap<ApplicationUser, UserTableViewModel>()
                .ForMember(e => e.Mobile, s => s.MapFrom(s => s.CountryCode + s.Mobile))
                .ForMember(e => e.Gender, s => s.MapFrom(s => s.Gender != null ? s.Gender.DisplayName() : null))
                 .ForMember(e => e.Picture, s => s.MapFrom(s => (s.Picture != null ? Config.Imageflow + s.Picture + Config.ImageResizerAdmin : null)))
                 .ForMember(e => e.BirthDay, s => s.MapFrom(s => s.BirthDay != null ? s.BirthDay : null))

                .ForMember(e => e.Roles, s => s.MapFrom(s => string.Join(" / ", s.UserRoles.Select(c => c.Role.Name))));

            CreateMap<ApplicationUser, UserTableFrontViewModel>();

            CreateMap<RoleViewModel, ApplicationRole>().ReverseMap();

            CreateMap<ApplicationUser, UserEditViewModel>()
             .ForMember(e => e.Mobile, s => s.MapFrom(s => s.Mobile))
             .ForMember(e => e.Gender, s => s.MapFrom(s => s.Gender))
             .ForMember(e => e.MobilePicture, s => s.MapFrom(s => (s.Picture != null ? Config.PictureBaseURL + s.Picture : null)))
              .ReverseMap();

            CreateMap<ApplicationUser, UserScoreBoard>()
                .ForMember(e => e.Points, s => s.MapFrom(s => s.TotalScorePoint))
             .ForMember(e => e.Name, s => s.MapFrom(s => s.FullName))
             .ForMember(e => e.Picture, s => s.MapFrom(s => (s.Picture != null ? Config.PictureBaseURL + s.Picture : null)));


            CreateMap<ApplicationUser, DashboardEditViewModel>()
                .ReverseMap();
            CreateMap<UserEditInfoViewModel, UserEditViewModel>()

                .ReverseMap();



            CreateMap<UserInfoModel, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserLightVM>().ReverseMap();

            CreateMap<IdentityResult, IdentityResultViewModel>().ReverseMap();
            CreateMap<IdentityError, ErrorRequestViewModel>();

            CreateMap<Block, BlockVM>().ReverseMap();
            CreateMap<Block, BlockDataTable>().ReverseMap();
            

         




            CreateMap<Configuration, ConfigurationLightVM>()
               .ForMember(e => e.FooterBrief, s => s.MapFrom(s => language == "ar" ? s.ArFooterBrief : s.EnFooterBrief))

                ;

            CreateMap<Configuration, ConfigurationVM>()
               .ForMember(e => e.Name, s => s.MapFrom(s => language == "ar" ? s.ArName : s.EnName))
               .ForMember(e => e.MetaDescription, s => s.MapFrom(s => language == "ar" ? s.ArMetaDescription : s.EnMetaDescription))
               .ForMember(e => e.Keywords, s => s.MapFrom(s => language == "ar" ? s.ArKeywords : s.EnKeywords))
               .ForMember(e => e.SocialPicture, s => s.MapFrom(s => s.SocialPicture != null ? $"{Config.Schema}{Config.BaseURL}/{s.SocialPicture}{Config.ImageResizerBox}" : null));





            CreateMap<Block, BlockLightVM>().ReverseMap();
                     
                     ;



            CreateMap<Block, BlockLightVM>()
              .ReverseMap();





            CreateMap<ApplicationUser, UserEditInfoViewModel>();

            CreateMap<UserEditInfoViewModel, RegisterApiViewModel>().ReverseMap();

            CreateMap<RegisterApiViewModel, ApplicationUser>()
                 .ForMember(e => e.PhoneNumber, s => s.MapFrom(s => s.Mobile))
                 .ForMember(e => e.UserName, s => s.MapFrom(s => s.Email + Guid.NewGuid().ToString()))
                 .ForMember(e => e.CreatedOnUtc, s => s.MapFrom(s => Extantion.AddUtcTime()))
                 .ForMember(e => e.UpdatedOnUtc, s => s.MapFrom(s => Extantion.AddUtcTime()))
                 .ForMember(e => e.Active, s => s.MapFrom(s => true))
                 .ForMember(e => e.EnableNotification, s => s.MapFrom(s => true))
                 .ForMember(e => e.EmailConfirmed, s => s.MapFrom(s => true));



            CreateMap<IdentityResult, IdentityResultViewModel>().ReverseMap();




            CreateMap<UserNotifictionInfo, ApplicationUser>()
             .ReverseMap();


            CreateMap<Product, ProductDataTable>()
                 .ForMember(e => e.Category, s => s.MapFrom(s => s.Category.Name))
                 .ReverseMap();
            CreateMap<ProductVM, Product>()
                .ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc.ToUniversalTime()))
            .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => src.UpdatedOnUtc.ToUniversalTime()))
                 .ReverseMap();


            CreateMap<Category, CategoryTableData>()
            .ForMember(dest => dest.ParentCategoryName,opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
            .ReverseMap();
            CreateMap<CategoryVM, Category>()
            .ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc.ToUniversalTime()))
            .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => src.UpdatedOnUtc.ToUniversalTime()))
            .ReverseMap();




            CreateMap<OurClient, ClientDataTable>().ReverseMap();
            CreateMap<ClientVM, OurClient>()
                .ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc.ToUniversalTime()))
                .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => src.UpdatedOnUtc.ToUniversalTime())).ReverseMap();


            CreateMap<Vendor, VendorDataTable>().ReverseMap();
            CreateMap<VendorVM, Vendor>()
                .ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc.ToUniversalTime()))
                .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => src.UpdatedOnUtc.ToUniversalTime()))
                .ReverseMap();


            CreateMap<Contact, ContactDataTable>().ReverseMap();
            CreateMap<ContactVm, Contact>()
               .ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc.ToUniversalTime()))
               .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => src.UpdatedOnUtc.ToUniversalTime())).ReverseMap();

            CreateMap<ProfileDataTable, Domain.Models.Profile>().ReverseMap();
            CreateMap<Domain.Models.Profile, ProfileVM>()
             .ForMember(dest => dest.CreatedOnUtc, opt => opt.MapFrom(src => src.CreatedOnUtc.ToUniversalTime()))
             .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => src.UpdatedOnUtc.ToUniversalTime())).ReverseMap();

            CreateMap<LastestNewsDataTable, LastestNews>().ReverseMap();

        }

    }
}
