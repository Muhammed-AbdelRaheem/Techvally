using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using Data.IRepository;
using Dataaccess.IRepository;
using DataAccess.IRepository;
using DataAccess.Services;
using Domain.Models;
using Domain.Models.ManageViewModels;
using Domain.Models.NotificationHandlerVM;
using Domain.Models.ViewModel;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Dataaccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMediator _mediator;
        private readonly IMapper _autoMapper;
        private readonly WriteDbContext _context;
        private readonly ReadDBContext _read;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoleRepository _roleRepository;
        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationRepository _configuration;
        private readonly IBlockRepository _blockRepository;
        //private readonly IEmailSender _emailSender;


        public UserRepository(IMapper autoMapper, WriteDbContext context, RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
             IRoleRepository roleRepository, 
            IConfigurationRepository configuration, IBlockRepository blockRepository,  IMediator mediator,
            IHttpContextAccessor httpContext, ReadDBContext read)
        {
            _autoMapper = autoMapper;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _blockRepository = blockRepository;
            _mediator = mediator;
            _httpContext = httpContext;

            _read = read;
        }




        public Task UpdateUserInfoFromLogin(UpdateUserHandlerVM model)
        {
            _mediator.Publish(model);
            return Task.CompletedTask;
        }

         public async Task<IdentityResultViewModel> AddDashboardAsync(UserViewModel userViewModel, string? currentUserId = null, DeviceType? deviceType = DeviceType.WEB)
        {
            var identityResultViewModel = new IdentityResultViewModel();
            var errorList = new List<ErrorRequestViewModel>();
            ApplicationUser userModel = new ApplicationUser
            {
                UserName = userViewModel.Email + Guid.NewGuid().ToString(),
                FullName = userViewModel.FullName,
                Email = userViewModel.Email,
                EnableNotification = userViewModel.EnableNotification,
                CountryCode = userViewModel.CountryCode ?? "",
                Mobile = userViewModel.Mobile ?? "",
                Active = true,
                EmailConfirmed = true,
                BirthDay = userViewModel.BirthDay,
                Gender = userViewModel.Gender ?? Gender.Male,
                UserType = UserType.Dashboard,
                DeviceType = userViewModel.DeviceType,
                CreatedOnUtc = Extantion.AddUtcTime(),
                UpdatedOnUtc = Extantion.AddUtcTime(),
                //Age = userViewModel.Age,
                //Weight = userViewModel.Weight,
                //LevelId = userViewModel.LevelId,
            };





            var userIsExist = await _userManager.Users.Where(e => e.Email == userModel.Email).FirstOrDefaultAsync();
            var role = userViewModel.Role;

            if (userIsExist != null)
            {
                identityResultViewModel.Succeeded = false;
                errorList.Add(new ErrorRequestViewModel
                {
                    Code = "Email",
                    Description = "This email already exist!"
                });
                identityResultViewModel.Errors = errorList;
                return identityResultViewModel;
            }


            IdentityResult result = await _userManager.CreateAsync(userModel, userViewModel.Password);

            if (result.Succeeded)
            {
                var selectedRoles = await _roleManager.Roles.Where(e => role != null && role.Count() > 0 && (role.Contains(e.Id) || role.Contains(e.Name))).Select(e => e.Name).ToListAsync();
                if (selectedRoles != null && selectedRoles.Any())
                {
                    var results = await _userManager.AddToRolesAsync(userModel, selectedRoles.ToArray<string>());
                }
                else if (role != null && role.Where(e => !string.IsNullOrWhiteSpace(e)).Any())
                {
                    foreach (var roleName in role)
                    {
                        var createRole = new ApplicationRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = roleName,
                            NormalizedName = roleName.ToUpper()
                        };
                        var resultRole = await _roleManager.CreateAsync(createRole);
                        if (resultRole.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(userModel, createRole.Name);
                        }
                    }

                }
                else
                {
                    var defualtRole = await _roleManager.Roles.Where(e => e.Name == "User").Select(e => e.Name).FirstOrDefaultAsync();
                    if (defualtRole != null)
                    {
                        await _userManager.AddToRoleAsync(userModel, defualtRole);
                    }
                    else
                    {
                        var createRole = new ApplicationRole
                        {
                            Id = Guid.NewGuid().ToString(),

                            Name = "User",
                            NormalizedName = "USER"
                        };
                        var resultRole = await _roleManager.CreateAsync(createRole);
                        if (resultRole.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(userModel, createRole.Name);
                        }
                    }

                }

            }

            var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
            resultModel.UserId = userModel.Id;
            return resultModel;
        }

        public async Task<IdentityResultViewModel> AddUserAsync(UserViewModel userViewModel, string? currentUserId = null, DeviceType? deviceType = DeviceType.WEB)
        {
            var identityResultViewModel = new IdentityResultViewModel();

            var userEmailIsExist = await UserEmailExist(userViewModel.Email, identityResultViewModel);
            if (!userEmailIsExist.Succeeded)
                return userEmailIsExist;



            var userModel = new ApplicationUser
            {
                UserName = userViewModel.Email + Guid.NewGuid().ToString(),
                FullName = userViewModel.FullName,
                Email = userViewModel.Email,
                Picture = userViewModel.Picture,
                EnableNotification = true,
                CountryCode = userViewModel.CountryCode ?? "",
                Mobile = userViewModel.Mobile ?? "",
                Active = true,
                EmailConfirmed = true,
                BirthDay = userViewModel.BirthDay,
                Gender = userViewModel.Gender ?? Gender.Male,
                UserType = UserType.User,
                DeviceType = userViewModel.DeviceType,
                CreatedOnUtc = Extantion.AddUtcTime(),
                UpdatedOnUtc = Extantion.AddUtcTime(),
                Age = userViewModel.Age,
                Weight = userViewModel.Weight,
                LevelId = userViewModel.LevelId,
            };


            var result = await _userManager.CreateAsync(userModel, userViewModel.Password);

            if (result.Succeeded)
                _mediator.Publish(new AddUserRoleHandlerVM() { UserId = userModel.Id });


            var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
            resultModel.UserId = userModel.Id;
            return resultModel;
        }

        public ErrorRequestViewModel AddError(string? code, string message)
        {
            return new ErrorRequestViewModel
            {
                Code = code,
                Description = message
            };
        }


        public async Task<MultiSelectList> GetRolesList(IEnumerable<string>? role = null)
        {
            var roles = await _roleRepository.GetRolesAsync();
            var listOfRoles = new MultiSelectList(roles.Select(e => new
            {
                e.Id,
                e.Name
            }), "Id", "Name", role);
            return listOfRoles;
        }

        public async Task<IdentityResultViewModel> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Deleted = true;
                user.Active = false;


                IdentityResult result = await _userManager.UpdateAsync(user);
                var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
                return resultModel;
            }
            var resultModelError = new IdentityResultViewModel() { Succeeded = false };
            return resultModelError;
        }
        public async Task<IdentityResultViewModel> LockUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Active = !user.Active;
                user.UpdatedOnUtc = Extantion.AddUtcTime();

                IdentityResult result = await _userManager.UpdateAsync(user);
                var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
                if (resultModel.Data != null)
                {
                    resultModel.Data.Active = user.Active;
                    resultModel.Data.UserType = user.UserType;

                }
                else
                {
                    resultModel.Data = new TokenApiViewModel()
                    {
                        Active = user.Active,
                        UserType = user.UserType
                    };

                }

                return resultModel;
            }
            var resultModelError = new IdentityResultViewModel() { Succeeded = false };
            return resultModelError;
        }

        public async Task<UserEditViewModel?> GetUserEditAsync(string id)
        {
            return await _read.Users.Where(e => e.Id == id)
                .ProjectTo<UserEditViewModel>
                    (_autoMapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<DashboardEditViewModel?> GetDashboardEditAsync(string id)
        {
            return await _read.Users.Where(e => e.Id == id)
                .ProjectTo<DashboardEditViewModel>
                    (_autoMapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetUserRoles(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                var userRoles = await _read.UserRoles.Where(e => e.UserId == id).Select(e => e.RoleId).ToListAsync();
                return userRoles;
            }
            return new List<string>();
        }

        public async Task<JqueryDataTablesPagedResults<UserTableViewModel>> GetUsersAsync(JqueryDataTablesParameters table, UserType userType)
        {

            try
            {
                IQueryable<ApplicationUser> query = _read.Users.Where(e => !Config.HiddenUsers().Contains(e.Id) && e.UserType == userType).AsNoTracking();

                query = SearchOptionsProcessor<UserTableViewModel, ApplicationUser>.Apply(query, table.Columns);
                query = SortOptionsProcessor<UserTableViewModel, ApplicationUser>.Apply(query, table);

                if (table.Search != null && !string.IsNullOrWhiteSpace(table.Search.Value))
                {
                    var q = table.Search.Value;
                    query = query.Where(e =>
                        e.Email!.ToLower().Contains(q) ||
                        e.FullName.ToLower().Contains(q)
                    );
                }
                var size = await query.CountAsync();

                var items = await query
                    .AsNoTracking()
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length)
                    .ProjectTo<UserTableViewModel>(_autoMapper.ConfigurationProvider)
                    .ToArrayAsync();

                return new JqueryDataTablesPagedResults<UserTableViewModel>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

            }
            return new JqueryDataTablesPagedResults<UserTableViewModel>
            {
                TotalSize = 0
            };
        }
        public async Task<IdentityResultViewModel> UpdateDashboardAsync(DashboardEditViewModel userEditViewModel, string? currentUserId = null)
        {
            var identityResultViewModel = new IdentityResultViewModel();
            var errorList = new List<ErrorRequestViewModel>();
            var applicationUser = await _context.Users.Where(e => e.Id == userEditViewModel.Id)
                .FirstOrDefaultAsync();
            if (applicationUser != null)
            {
                var userIsExist = await _userManager.Users.Where(e => e.Id != userEditViewModel.Id && e.Email == userEditViewModel.Email && e.Deleted != true && e.UserType == UserType.Dashboard)
                    .FirstOrDefaultAsync();

                if (userIsExist != null)
                {
                    identityResultViewModel.Succeeded = false;
                    errorList.Add(AddError("Email", "email_exist"));
                    identityResultViewModel.Errors = errorList;
                    return identityResultViewModel;
                }

                if (!string.IsNullOrWhiteSpace(userEditViewModel.Email))
                {
                    applicationUser.Email = userEditViewModel.Email;
                }

                if (!string.IsNullOrWhiteSpace(userEditViewModel.FullName))
                {
                    applicationUser.FullName = userEditViewModel.FullName;
                }

                applicationUser.Age = userEditViewModel.Age;
                applicationUser.Weight = userEditViewModel.Weight;
                applicationUser.LevelId = userEditViewModel.LevelId;
                applicationUser.LevelId = userEditViewModel.LevelId;
                applicationUser.CountryCode = userEditViewModel.CountryCode ?? "+966";
                applicationUser.Mobile = userEditViewModel.Mobile;
                applicationUser.EmailConfirmed = true;
                applicationUser.Gender = userEditViewModel.Gender ?? Gender.Male;
                applicationUser.NationalityId = userEditViewModel.NationalityId;



                applicationUser.UpdatedOnUtc = Extantion.AddUtcTime();

                IdentityResult result = await _userManager.UpdateAsync(applicationUser);

                if (result.Succeeded)
                {

                    if (!String.IsNullOrWhiteSpace(userEditViewModel.NewPassword))
                    {
                        var userToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
                        if (!String.IsNullOrWhiteSpace(userToken))
                        {
                            await _userManager.ResetPasswordAsync(applicationUser, userToken, userEditViewModel.NewPassword);
                        }
                    }

                    var roles = userEditViewModel.Roles;
                    var userRoles = await _userManager.GetRolesAsync(applicationUser);
                    var results = await _userManager.RemoveFromRolesAsync(applicationUser, userRoles.ToArray<string>());

                    if (!results.Succeeded)
                    {
                        identityResultViewModel.Succeeded = false;
                        errorList.Add(new ErrorRequestViewModel
                        {
                            Code = "Role",
                            Description = results.Errors.First().ToString()
                        });
                        identityResultViewModel.Errors = errorList;
                        return identityResultViewModel;
                    }
                    var selectedRoles = await _roleManager.Roles.Where(e => roles != null && roles.Count() > 0 && (roles.Contains(e.Id) || roles.Contains(e.Name))).Select(e => e.Name).ToListAsync();

                    if (selectedRoles.Any())
                    {
                        await _userManager.AddToRolesAsync(applicationUser, selectedRoles.ToArray<string>());
                    }
                    else if (roles != null && roles.Count() > 0 && roles.Any(s => !string.IsNullOrWhiteSpace(s)))
                    {

                        foreach (var role in roles)
                        {
                            var createRole = new ApplicationRole
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = role,
                                NormalizedName = role.ToUpper()
                            };

                            var resultRole = await _roleManager.CreateAsync(createRole);
                            if (resultRole.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(applicationUser, createRole.Name);
                            }
                        }

                    }
                    else
                    {
                        var defualtRole = await _roleManager.Roles.Where(e => e.Name == "User").Select(e => e.Name).FirstOrDefaultAsync();
                        if (defualtRole != null)
                        {
                            await _userManager.AddToRoleAsync(applicationUser, defualtRole);
                        }
                        else
                        {
                            var createRole = new ApplicationRole
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "User",
                                NormalizedName = "USER"
                            };
                            var resultRole = await _roleManager.CreateAsync(createRole);
                            if (resultRole.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(applicationUser, createRole.Name);
                            }
                        }

                    }


                }
                var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
                return resultModel;
            }
            identityResultViewModel.Succeeded = false;
            errorList.Add(AddError("Id", "user_not_exist"));
            identityResultViewModel.Errors = errorList;
            return identityResultViewModel;
        }

        public async Task<IdentityResultViewModel> UpdateUserAsync(UserEditViewModel userEditViewModel, bool isMobile = false)
        {
            var identityResultViewModel = new IdentityResultViewModel();
            var errorList = new List<ErrorRequestViewModel>();
            var applicationUser = await _context.Users.Where(e => e.Id == userEditViewModel.Id)
                .FirstOrDefaultAsync();
            if (applicationUser != null)
            {
                var userIsExist = await _userManager.Users.Where(e => e.Id != userEditViewModel.Id && e.Email == userEditViewModel.Email && e.Deleted != true && e.UserType == UserType.User)
                    .FirstOrDefaultAsync();

                if (userIsExist != null)
                {
                    identityResultViewModel.Succeeded = false;
                    errorList.Add(AddError("Email", "email_exist"));
                    identityResultViewModel.Errors = errorList;
                    return identityResultViewModel;
                }


                applicationUser.Email = userEditViewModel.Email;
                applicationUser.FullName = userEditViewModel.FullName;
                applicationUser.CountryCode = userEditViewModel.CountryCode;
                applicationUser.Mobile = userEditViewModel.Mobile;

                applicationUser.LevelId = userEditViewModel.LevelId;
                applicationUser.Age = userEditViewModel.Age;
                applicationUser.Weight = userEditViewModel.Weight;

                if (!isMobile)
                    applicationUser.Picture = userEditViewModel.Picture;


                //if (userEditViewModel is { ImageFile: { } })
                //{
                //    applicationUser.Picture = await AddImageAsync(userEditViewModel.ImageFile);

                //}



                applicationUser.UpdatedOnUtc = Extantion.AddUtcTime();

                var result = await _userManager.UpdateAsync(applicationUser);

                if (result.Succeeded)
                {

                    if (!String.IsNullOrWhiteSpace(userEditViewModel.NewPassword))
                    {
                        var userToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
                        if (!String.IsNullOrWhiteSpace(userToken))
                        {
                            await _userManager.ResetPasswordAsync(applicationUser, userToken, userEditViewModel.NewPassword);
                        }
                    }

                    var userRoles = await _userManager.GetRolesAsync(applicationUser);
                    var results = await _userManager.RemoveFromRolesAsync(applicationUser, userRoles.ToArray<string>());
                    if (!results.Succeeded)
                    {
                        identityResultViewModel.Succeeded = false;
                        errorList.Add(new ErrorRequestViewModel
                        {
                            Code = "Role",
                            Description = results.Errors.First().ToString()
                        });
                        identityResultViewModel.Errors = errorList;
                        return identityResultViewModel;
                    }

                    if (userEditViewModel.FromAdmin)
                    {
                        _mediator.Publish(new AddUserRoleHandlerVM() { UserId = applicationUser.Id });
                        _mediator.Publish(new AddUserInterestsHandlerVM() { UserId = applicationUser.Id });
                    }

                }
                var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
                return resultModel;
            }
            identityResultViewModel.Succeeded = false;
            errorList.Add(AddError("Id", "user_not_exist"));
            identityResultViewModel.Errors = errorList;
            return identityResultViewModel;
        }


        private async Task<IdentityResultViewModel> UserEmailExist(string email, IdentityResultViewModel identityResultViewModel, string? userId = null)
        {
            var errorList = new List<ErrorRequestViewModel>();
            var userEmailIsExist = await _read.Users.AsNoTracking().AnyAsync(e => (userId == null || e.Id != userId) && e.Email == email);
            if (userEmailIsExist)
            {
                identityResultViewModel.Succeeded = false;
                errorList.Add(new ErrorRequestViewModel
                {
                    Code = "Email",
                    Description = "This email already exist!"
                });
                identityResultViewModel.Errors = errorList;
                return identityResultViewModel;
            }
            return new IdentityResultViewModel() { Succeeded = true };
        }

        //private async Task<bool> CompleteRegisterStep(string id)
        //{
        //    return await _read.Users.Where(e => e.Id == id && e.UserType == UserType.User && e.Age != null && e.Weight != null && e.LevelId != null && e.Active).AnyAsync();

        //}
        //public async Task<IdentityResultViewModel> GetUserToken(ApplicationUser userModel, ApiClient client, IdentityResultViewModel identityResultViewModel, bool? addRole = false)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, userModel.Id),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim("UserId", userModel.Id),
        //        new Claim("FullName", userModel.FullName?? ""),
        //        new Claim("DeviceType", userModel.DeviceType.ToString()?? "0"),
        //        new Claim("Language", userModel.Language ?? "en"),
        //    };

        //    var accessToken = _tokenRepository.GenerateAccessToken(claims, client.AccessTokenExpiry);
        //    var refreshToken = _tokenRepository.GenerateRefreshToken();

        //    Task.Run(async () =>
        //    {
        //        userModel.DeviceType = userModel.DeviceType;
        //        userModel.MobileAppId = userModel.MobileAppId;
        //        userModel.RefreshToken = refreshToken;
        //        userModel.RefreshTokenExpiryUTC = DateTime.UtcNow.AddDays(client.RefreshTokenExpiry).ToSaudiDate();
        //        await _getHeaders.UserHeaders(userModel, true);
        //        var user = new UpdateUserHandlerVM { User = userModel, AddRole = addRole ?? false };
        //        BackgroundJob.Enqueue(() => UpdateUserInfoFromLogin(user));
        //    });

        //    var _result = new TokenApiViewModel()
        //    {
        //        AccessToken = accessToken,
        //        RefreshToken = refreshToken,
        //        Id = userModel.Id,
        //        RegistrationId = userModel.RegistrationId,
        //        CountryCode = userModel.CountryCode,
        //        Mobile = userModel.Mobile,
        //        UserType = userModel.UserType,
        //        FullName = userModel.FullName,
        //        Picture = userModel.Picture != null ? Config.PictureBaseURL + userModel.Picture : null,
        //        Email = userModel.Email,
        //        Gender = userModel.Gender,
        //        Language = userModel.Language,
        //        EmailVerified = userModel.EmailConfirmed,
        //        NotificationEnabled = userModel.EnableNotification,
        //        CompleteRegistration = await CompleteRegisterStep(userModel.Id),
        //        Age = userModel.Age,
        //        Weight = userModel.Weight,
        //    };
        //    identityResultViewModel.Succeeded = true;
        //    identityResultViewModel.Data = _result;
        //    return identityResultViewModel;
        //}

        //public async Task<bool> ForgotPassword(ForgotPasswordHandlerVM model)
        //{
        //    try
        //    {
        //        var user = await _userManager.Users.Where(e => e.Email == model.Email).FirstOrDefaultAsync();
        //        if (user != null)
        //        {
        //            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            var callbackUrl = $"https://{Config.Website}/{model.language}/Account/ResetPassword?userId={user.Id}&code={code}";
        //            var email = user.Email;
        //            var link = HtmlEncoder.Default.Encode(callbackUrl);

        //            var config = await _configuration.GetFirstConfigurationAsync();
        //            if (
        //                config != null &&
        //                !string.IsNullOrWhiteSpace(config.EmailSender) &&
        //                !string.IsNullOrWhiteSpace(config.PasswordEmailSender) &&
        //                !string.IsNullOrWhiteSpace(config.Host) &&
        //                config.Port != null
        //            )
        //            {
        //                var block = await _blockRepository.GetAPIBlockVMAsync(BlockType.ForgotPassword, model.language);

        //                if (block == null)
        //                {
        //                    return false;
        //                }
        //                //await _emailSender.SendEmailAsync(
        //                //    config.Host,
        //                //    config.Port,
        //                //    config.UseSSL,
        //                //    config.EmailSender,
        //                //    config.PasswordEmailSender,
        //                //    email,
        //                //    block.Name,
        //                //    message: block.Content
        //                //        .Replace("%name%", model.User.FullName)
        //                //        .Replace("%link%", link),
        //                //    config.DefaultEmailName,
        //                //    config.DefaultEmail);

        //                return true;

        //            }
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        _mediator.Publish(new LogAddViewModel()
        //        {
        //            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
        //            IpAddress = Config.GetIpAddress(_httpContext),
        //            Table = "ForgotPassword",
        //            Action = "ForgotPassword",
        //            Details = $"Failed ForgotPassword ===\n{e.Message}\n{e.InnerException}",
        //        });
        //    }
        //    return false;

        //}

        //public async Task<IdentityResultViewModel> AddUserAPIAsync(RegisterApiViewModel userViewModel, string language, ApiClient client)
        //{
        //    var identityResultViewModel = new IdentityResultViewModel();

        //    var mobileExist = await UserMobileExist(userViewModel.CountryCode, userViewModel.Mobile, identityResultViewModel);
        //    if (!mobileExist.Succeeded)
        //        return mobileExist;

        //    var emailExist = await UserEmailExist(userViewModel.Email, identityResultViewModel);
        //    if (!emailExist.Succeeded)
        //        return emailExist;


        //    var userModel = _autoMapper.Map<ApplicationUser>(userViewModel);
        //    userModel.Language = language;


        //    IdentityResult result = await _userManager.CreateAsync(userModel, userViewModel.Password);
        //    if (result.Succeeded)
        //        return await GetUserToken(userModel, client, identityResultViewModel, addRole: true);


        //    var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
        //    resultModel.UserId = userModel.Id;
        //    return resultModel;
        //}

        //public async Task<APIResponse> CompleteUserRegisterationAsync(CompleteRegister model)
        //{
        //    var userModel = await _context.Users.Where(e => e.Id == model.Id && e.Active).FirstOrDefaultAsync();

        //    if (userModel == null)
        //        return new APIResponse
        //        {
        //            Result = false,
        //            Msg = "user_not_exist"
        //        };

        //    userModel.Age = model.Age;
        //    userModel.Weight = model.Weight;
        //    userModel.LevelId = model.LevelId;

        //    _context.Update(userModel);
        //    return await _context.SaveChangesAsync() > 0 ? new APIResponse { Result = true } : new APIResponse { Result = false, Msg = "Something went wrong" };
        //}


        //public async Task<IdentityResultViewModel> AddUsersInFirstApprovalAsync(UserViewModel userViewModel, string? currentUserId = null, DeviceType? deviceType = DeviceType.WEB)
        //{
        //    var password = Extantion.RNGCharacterMask();

        //    var identityResultViewModel = new IdentityResultViewModel();
        //    var errorList = new List<ErrorRequestViewModel>();



        //    ApplicationUser userModel = new ApplicationUser
        //    {
        //        UserName = userViewModel.Email + Guid.NewGuid().ToString(),
        //        FullName = userViewModel.FullName,
        //        Email = userViewModel.Email,
        //        NationalityId = userViewModel.NationalityId,
        //        RegistrationId = userViewModel.RegistrationId,

        //        EnableNotification = userViewModel.EnableNotification,
        //        Mobile = userViewModel.Mobile ?? "",
        //        Active = true,
        //        EmailConfirmed = true,
        //        BirthDay = userViewModel.BirthDay,
        //        Gender = userViewModel.Gender ?? Gender.Male,
        //        UserType = UserType.User,
        //        DeviceType = userViewModel.DeviceType,
        //        CreatedOnUtc = Extantion.AddUtcTime(),
        //        UpdatedOnUtc = Extantion.AddUtcTime(),
        //    };


        //    var userIsExist = await _userManager.Users.Where(e => e.NationalityId == userModel.NationalityId).FirstOrDefaultAsync();
        //    var role = userViewModel.Role;

        //    if (userIsExist != null)
        //    {
        //        identityResultViewModel.Succeeded = false;
        //        errorList.Add(new ErrorRequestViewModel
        //        {
        //            Code = "Email",
        //            Description = "This email already exist!"
        //        });
        //        identityResultViewModel.Errors = errorList;
        //        return identityResultViewModel;
        //    }


        //    IdentityResult result = await _userManager.CreateAsync(userModel, userViewModel.Password);

        //    if (result.Succeeded)
        //    {
        //        await _getHeaders.UserHeaders(userModel, false);
        //        var selectedRoles = await _roleManager.Roles.Where(e => role != null && role.Count() > 0 && (role.Contains(e.Id) || role.Contains(e.Name))).Select(e => e.Name).ToListAsync();
        //        if (selectedRoles != null && selectedRoles.Any())
        //        {
        //            var results = await _userManager.AddToRolesAsync(userModel, selectedRoles.ToArray<string>());
        //        }
        //        else if (role != null && role.Where(e => !string.IsNullOrWhiteSpace(e)).Any())
        //        {
        //            foreach (var roleName in role)
        //            {
        //                var createRole = new ApplicationRole
        //                {
        //                    Id = Guid.NewGuid().ToString(),
        //                    Name = roleName,
        //                    NormalizedName = roleName.ToUpper()
        //                };
        //                var resultRole = await _roleManager.CreateAsync(createRole);
        //                if (resultRole.Succeeded)
        //                {
        //                    await _userManager.AddToRoleAsync(userModel, createRole.Name);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            var defualtRole = await _roleManager.Roles.Where(e => e.Name == "User").Select(e => e.Name).FirstOrDefaultAsync();
        //            if (defualtRole != null)
        //            {
        //                await _userManager.AddToRoleAsync(userModel, defualtRole);
        //            }
        //            else
        //            {
        //                var createRole = new ApplicationRole
        //                {
        //                    Id = Guid.NewGuid().ToString(),

        //                    Name = "User",
        //                    NormalizedName = "USER"
        //                };
        //                var resultRole = await _roleManager.CreateAsync(createRole);
        //                if (resultRole.Succeeded)
        //                {
        //                    await _userManager.AddToRoleAsync(userModel, createRole.Name);
        //                }
        //            }

        //        }

        //    }

        //    var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
        //    resultModel.UserId = userModel.Id;
        //    return resultModel;
        //}

        //public async Task<IdentityResultViewModel> AddUserFirstApprovel(UserViewModel userViewModel, string? currentUserId = null, DeviceType? deviceType = DeviceType.WEB)
        //{
        //    var identityResultViewModel = new IdentityResultViewModel();

        //    var userEmailIsExist = await UserEmailExist(userViewModel.NationalityId, identityResultViewModel);
        //    if (!userEmailIsExist.Succeeded)
        //        return userEmailIsExist;



        //    var userModel = new ApplicationUser
        //    {
        //        UserName = userViewModel.Email,
        //        FullName = userViewModel.FullName,
        //        Email = userViewModel.Email,
        //        Picture = userViewModel.Picture,
        //        NationalityId = userViewModel.NationalityId,
        //        RegistrationId = userViewModel.RegistrationId,
        //        EnableNotification = true,
        //        CountryCode = userViewModel.CountryCode ?? "",
        //        Mobile = userViewModel.Mobile ?? "",
        //        Active = true,
        //        EmailConfirmed = true,
        //        BirthDay = userViewModel.BirthDay,
        //        Gender = userViewModel.Gender ?? Gender.Male,
        //        UserType = UserType.User,
        //        DeviceType = userViewModel.DeviceType,
        //        CreatedOnUtc = Extantion.AddUtcTime(),
        //        UpdatedOnUtc = Extantion.AddUtcTime(),
        //        Age = userViewModel.Age,
        //        Weight = userViewModel.Weight,
        //        LevelId = userViewModel.LevelId,
        //    };


        //    var result = await _userManager.CreateAsync(userModel, userViewModel.Password);

        //    if (result.Succeeded)
        //        _mediator.Publish(new AddUserRoleHandlerVM() { UserId = userModel.Id });


        //    var resultModel = _autoMapper.Map<IdentityResult, IdentityResultViewModel>(result);
        //    resultModel.UserId = userModel.Id;
        //    return resultModel;
        //}





        //public async Task<bool> UpdateUserPoints(string id)
        //{
        //    try
        //    {

        //        var user = await _context.Users.Where(e => e.Id == id && e.Active).FirstOrDefaultAsync();
        //        if (user != null)
        //        {
        //            var points = await _configuration.GetPointsPerOrderAsync();

        //            user.TotalScorePoint = user.TotalScorePoint != null ? user.TotalScorePoint + points : 0.0;
        //            _context.Users.Update(user);
        //            await _context.SaveChangesAsync();

        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    return false;
        //}


        //public async Task<IdentityResultViewModel> LoginUserAPIAsync(LoginApiViewModel model, ApiClient client)
        //{
        //    var identityResultViewModel = new IdentityResultViewModel();
        //    var errorList = new List<ErrorRequestViewModel>();
        //    if (!string.IsNullOrEmpty(model.NationalityId))
        //    {

        //        ApplicationUser? user = await _userManager.Users
        //            .Where(e => e.Active && e.NationalityId == model.NationalityId && e.UserType == UserType.User)
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync()
        //            .ConfigureAwait(false);

        //        if (user == null)
        //        {
        //            var invalidLogin = await _errorMessageRepository.GetErrorMessageByKeyAsync("user_not_exist", model.Language) ?? "user_not_exist";
        //            identityResultViewModel.Succeeded = false;
        //            errorList.Add(new ErrorRequestViewModel
        //            {
        //                Code = null,
        //                Description = invalidLogin
        //            });
        //            identityResultViewModel.Errors = errorList;
        //            return identityResultViewModel;
        //        }

        //        if (!user.Active)
        //        {
        //            var invalidLogin = await _errorMessageRepository.GetErrorMessageByKeyAsync("user_not_exist", model.Language) ?? "user_not_exist";
        //            identityResultViewModel.Succeeded = false;
        //            errorList.Add(new ErrorRequestViewModel
        //            {
        //                Code = "",
        //                Description = invalidLogin
        //            });
        //            identityResultViewModel.Errors = errorList;
        //            return identityResultViewModel;
        //        }


        //        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false).ConfigureAwait(false);
        //        if (!result.Succeeded)
        //        {
        //            var invalidLogin = await _errorMessageRepository.GetErrorMessageByKeyAsync("user_not_exist", model.Language) ?? "user_not_exist";
        //            identityResultViewModel.Succeeded = false;
        //            errorList.Add(new ErrorRequestViewModel
        //            {
        //                Code = "",
        //                Description = invalidLogin
        //            });
        //            identityResultViewModel.Errors = errorList;
        //            return identityResultViewModel;
        //        }

        //        user.Language = model.Language;
        //        return await GetUserToken(user, client, identityResultViewModel).ConfigureAwait(false);
        //    }
        //    else
        //    {
        //        var invalidLogin = await _errorMessageRepository.GetErrorMessageByKeyAsync("user_not_exist", model.Language) ?? "user_not_exist";
        //        identityResultViewModel.Succeeded = false;
        //        errorList.Add(new ErrorRequestViewModel
        //        {
        //            Code = "",
        //            Description = invalidLogin
        //        });
        //        identityResultViewModel.Errors = errorList;
        //        return identityResultViewModel;
        //    }
        //}

        //public async Task<UserViewModel?> GetUserAsync(string id)
        //{
        //    return await _read.Users.Where(e => e.Id == id).ProjectTo<UserViewModel>(_autoMapper.ConfigurationProvider).FirstOrDefaultAsync();
        //}

        //public async Task<string?> GetUserNameAsync(string id)
        //{
        //    return await _read.Users.Where(e => e.Id == id).Select(s => s.FullName).FirstOrDefaultAsync();
        //}

        //public async Task<IEnumerable<UserScoreBoard>?> GetScoreBoardAsync(int? take = null, int? skip = null)
        //{
        //    return await _read.Users.Where(e => e.Active)
        //        .ProjectTo<UserScoreBoard>(_autoMapper.ConfigurationProvider)
        //        .OrderByDescending(e => e.Points)
        //        .ThenBy(e => e.Id)
        //        .Skip(skip ?? 0)
        //        .Take(take ?? int.MaxValue)
        //        .ToListAsync();
        //}



        //public async Task<int?> GetRegistrationId(string Id)
        //{
        //    return await _read.Users
        //                           .Where(e => e.Id == Id)
        //                           .Select(e => e.RegistrationId)
        //                           .FirstOrDefaultAsync();
        //}


        //public string? GetFullNameAsync(string id)
        //{
        //    var username = _read.Users.Where(e => e.Id == id)
        //        .Select(s => s.FullName).FirstOrDefault();
        //    return username;
        //}

        //public async Task<UserNotifictionInfo?> GetEmailAndDeviceAsync(string id)
        //{
        //    return await _read.Users.Where(e => e.Id == id && e.Active && !e.Deleted).ProjectTo<UserNotifictionInfo>(_autoMapper.ConfigurationProvider).FirstOrDefaultAsync();
        //}


        //public async Task<JqueryDataTablesPagedResults<UserTableViewModel>> GetUsersByTypeAsync(JqueryDataTablesParameters table, UserType userType)
        //{
        //    try
        //    {
        //        var hiddenUsers = new List<string>() { "4a2c3d8f-160b-49b2-880d-7aa35ca09a4f", "77f8e2e5-6034-4fb1-ad5f-2c689e967c35", "8815a67f-bf8d-40bd-a964-6f20210dc44e", "d21e21ed-c2d7-4c1f-a903-9dcfe4f9b3db" };
        //        IQueryable<ApplicationUser> query = _read.Users.Where(e => !hiddenUsers.Contains(e.Id) && e.UserType == userType).AsNoTracking();

        //        query = SearchOptionsProcessor<UserTableViewModel, ApplicationUser>.Apply(query, table.Columns);
        //        query = SortOptionsProcessor<UserTableViewModel, ApplicationUser>.Apply(query, table);

        //        var size = await query.CountAsync();


        //        var items = await query
        //            .AsNoTracking()
        //            .Skip(table.Start / table.Length * table.Length)
        //            .Take(table.Length)
        //            .ProjectTo<UserTableViewModel>(_autoMapper.ConfigurationProvider)
        //            .ToArrayAsync();



        //        return new JqueryDataTablesPagedResults<UserTableViewModel>
        //        {
        //            Items = items,
        //            TotalSize = size
        //        };
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return new JqueryDataTablesPagedResults<UserTableViewModel>
        //    {
        //        TotalSize = 0
        //    };
        //}


        //public async Task<IEnumerable<UserTableFrontViewModel>> GetUsersAsync()
        //{
        //    IEnumerable<UserTableFrontViewModel> users = await _read.Users
        //        .Where(e => e.Deleted != true)
        //        .ProjectTo<UserTableFrontViewModel>(_autoMapper.ConfigurationProvider)
        //        .ToListAsync();
        //    return users;
        //}

        //public async Task<bool> UpdateLoginUserAsync(ApplicationUser user)
        //{
        //    try
        //    {
        //        _context.Update(user);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return false;

        //}


        //public async Task AddUserToRole(string userId)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByIdAsync(userId);
        //        if (user != null)
        //        {
        //            var roleName = user.UserType.DisplayName();
        //            var selectedRoles = await _roleManager.Roles.Where(e => e.Name == roleName).Select(e => e.Name)
        //                .AsNoTracking()
        //                .ToListAsync();

        //            if (selectedRoles.Any())
        //            {
        //                await _userManager.AddToRolesAsync(user, selectedRoles.ToArray<string>());
        //            }
        //            else
        //            {
        //                var createRole = new ApplicationRole
        //                {
        //                    Id = Guid.NewGuid().ToString(),
        //                    Name = roleName,
        //                    NormalizedName = roleName.ToUpper()
        //                };
        //                var resultRole = await _roleManager.CreateAsync(createRole);
        //                if (resultRole.Succeeded)
        //                {
        //                    await _userManager.AddToRoleAsync(user, createRole.Name);
        //                }

        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        _mediator.Publish(new LogAddViewModel()
        //        {
        //            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
        //            IpAddress = Config.GetIpAddress(_httpContext),
        //            Table = "Role",
        //            Action = "Update user role",
        //            Details = $"Failed Update user role ===\n{e.Message}\n{e.InnerException}",
        //        });
        //    }

        //}
        //public async Task<ApplicationUser?> GetUserBranch(string id)
        //{
        //    return await _context.Users.Where(e => e.Id == id && e.Active && !e.Deleted).FirstOrDefaultAsync();
        //}



        //public async Task<IEnumerable<UserNotifactionVm>> GetUsersDataNotificationAsync(UserType userType, List<int>? notificationInterests)
        //{
        //    var allUser = await _read.Users.Where(e => e.Active && e.UserType == userType && e.EndpointArn != null && e.EndpointArn != "")
        //        .Select(e => new UserNotifactionVm
        //        {
        //            Id = e.Id,
        //            EndpointArn = e.EndpointArn,
        //            DeviceType = e.DeviceType,
        //            Language = e.Language ?? "en"
        //        })
        //        .ToListAsync();


        //    return allUser;
        //}

        //public async Task<UserNotifactionVm> GetUserDataNotificationAsync(string userId)
        //{
        //    var allUser = await _read.Users.Where(e => e.Id == userId && e.Active && e.EndpointArn != null && e.EndpointArn != "")
        //        .Select(e => new UserNotifactionVm
        //        {
        //            Id = e.Id,
        //            EndpointArn = e.EndpointArn,
        //            DeviceType = e.DeviceType,
        //            Language = e.Language ?? "en"
        //        })
        //        .FirstOrDefaultAsync();


        //    return allUser;
        //}

        //private async Task<IdentityResultViewModel> UserMobileExist(string countryCode, string mobile, IdentityResultViewModel identityResultViewModel, string? userId = null)
        //{
        //    var errorList = new List<ErrorRequestViewModel>();
        //    var userMobileIsExist = await _read.Users.AsNoTracking().AnyAsync(e => (userId == null || e.Id != userId) && e.CountryCode == countryCode && e.Mobile == mobile);
        //    if (userMobileIsExist)
        //    {
        //        identityResultViewModel.Succeeded = false;
        //        errorList.Add(new ErrorRequestViewModel
        //        {
        //            Code = "UserProfile.Mobile",
        //            Description = "This mobile already exist!"
        //        });
        //        identityResultViewModel.Errors = errorList;
        //        return identityResultViewModel;
        //    }
        //    return new IdentityResultViewModel() { Succeeded = true };
        //}

    }

}
