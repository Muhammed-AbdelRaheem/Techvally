

using Data.IRepository;
using Domain.Models;
using Domain.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using Utility;

namespace Admin.Controllers
{
    //[Authorize(Roles = "Admin,Editor,Manage Users")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IStringLocalizer<SharedResource> localizer,
            IUserRepository userRepository,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var model = new LoginViewModel
            {
                //ReturnUrl = returnUrl,
                //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            ViewData["ReturnUrl"] = returnUrl;


            return View(model);
        }

         [HttpGet]
         [AllowAnonymous]
         public async Task<IActionResult> CreateUser()
         {
             await _roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid().ToString(),Name = "User" });
             await _roleManager.CreateAsync(new ApplicationRole {Id = Guid.NewGuid().ToString(),Name = "Admin" });
             await _roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Editor" });
             await _userRepository.AddDashboardAsync(new UserViewModel
             {
                 Email = "MuhammedAbdelRaheem93@gmail.com",
                 Mobile = "0102268605",
                 Deleted = false,
                 Active = true,
                 Password = "Admin@123",
                 ConfirmPassword = "Admin@123",
                 FullName = "Muhammed AbdelRaheem",
                 BirthDay = DateTime.UtcNow,
                 Role = new List<string> { "Admin" }
             });
                 return RedirectToAction("Login");
             }
           



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.Where(e => e.Active && e.Deleted != true && e.Email.ToLower() == model.Email.ToLower()).Include(s => s.UserRoles).ThenInclude(e=>e.Role).FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Invalid login attempt.");
                        return RedirectToAction("Index", "Home");

                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, _localizer["Invalid login attempt."]);
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer["Invalid login attempt."]);
                    return View(model);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirecturl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirecturl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {

            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel
            {
                //ReturnUrl = returnUrl,
                //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from External Provider:{remoteError}");
                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error Loading external Login information.");
                return View("Login", loginViewModel);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                ViewBag.ErrorTitle = $"Email Claim Not received from {info.LoginProvider}";
                ViewBag.ErrorMessage = $"Please Contact Support";

                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account");

        }

    }
}
