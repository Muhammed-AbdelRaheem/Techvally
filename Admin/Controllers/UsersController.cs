using Data.IRepository;
using Domain.Models;
using Domain.Models.ManageViewModels;
using Domain.Models.NotificationHandlerVM;
using Domain.Models.ViewModel;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Utility;
using DataAccess.Services;

namespace Admin.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {

        private readonly IUserRepository _userService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<SharedResource> _localizer;



        public UsersController(IUserRepository userService, UserManager<ApplicationUser> userManager, IStringLocalizer<SharedResource> localizer, IMediator mediator, IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _userManager = userManager;
            _localizer = localizer;
            _mediator = mediator;
            _httpContext = httpContext;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param, UserType userType)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _userService.GetUsersAsync(param, userType);

                return new JsonResult(new JqueryDataTablesResult<UserTableViewModel>
                {
                    Draw = param.Draw,
                    Data = results.Items,
                    RecordsFiltered = results.TotalSize,
                    RecordsTotal = results.TotalSize
                });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        public async Task<IActionResult> UsersOnGetExcel(UserType userType)
        {
            // Here we will be getting the param that we have stored in the session in server side action method/page handler
            // and deserialize it to get the required data.
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            // If you're using Json.Net, then uncomment below line else remove below line
            // var results = await _demoService.GetDataAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));
            // If you're using new System.Text.Json then use below line
            //var crmUsers = await _userService.GetUsersAsync(JsonSerializer.Deserialize<JqueryDataTablesParameters>(param), userType);
            var crmUsers = await _userService.GetUsersAsync(
    JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param),
    userType);

            var title = userType == UserType.Dashboard ? "Dashboard Users" : "Mobile Users";

            return new JqueryDataTablesExcelResult<UserTableViewModel>(crmUsers.Items, title, title);
        }

        public async Task<IActionResult> UsersOnGetPrint(UserType userType)
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var crmUsers = await _userService.GetUsersAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param),
    userType);

            ViewBag.UserType = userType;

            return PartialView("_UsersPrintTable", crmUsers.Items);
        }

        #region User

        public async Task<IActionResult> Users()
        {
            return View(new UserTableViewModel());
        }

        public async Task<IActionResult> CreateUser()
        {
            ViewData["Roles"] = await _userService.GetRolesList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserViewModel user)
        {

            if (ModelState.IsValid)
            {

                var result = await _userService.AddUserAsync(user);

                if (result.Succeeded)
                {
                    _mediator.Publish(new LogAddViewModel()
                    {
                        ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                        IpAddress = Config.GetIpAddress(_httpContext),
                        Table = ControllerContext.ActionDescriptor.ControllerName,
                        Action = ControllerContext.ActionDescriptor.ActionName,
                        Details = $"Create User {user.FullName} with Email {user.Email}",
                    });
                    return RedirectToAction(nameof(Users));
                }

                AddErrors(result);
            }

            ViewData["Roles"] = await _userService.GetRolesList();

            return View(user);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userService.GetUserEditAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, UserEditViewModel user)
        {

            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(user);

                if (result.Succeeded)
                {

                    _mediator.Publish(new LogAddViewModel()
                    {
                        ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                        IpAddress = Config.GetIpAddress(_httpContext),

                        Table = ControllerContext.ActionDescriptor.ControllerName,
                        Action = ControllerContext.ActionDescriptor.ActionName,
                        Details = $"Updated User {user.FullName} with Email {user.Email}",
                    });
                    return RedirectToAction(nameof(Users));
                }

                AddErrors(result);
            }

            return View(user);
        }

        #endregion

        #region Dashboard

        public async Task<IActionResult> Index()
        {
            return View(new UserTableViewModel());
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Roles"] = await _userService.GetRolesList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel user, [Required] List<string> role)
        {
            var adminUser = await _userManager.GetUserAsync(User);
            //if (adminUser?.BranchId == null)
            //   return RedirectToAction(nameof(Index));

            //user.BranchId = adminUser.BranchId;

            if (role == null || role.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                ViewData["Roles"] = await _userService.GetRolesList(role);
                ModelState.AddModelError("Role", "Can't select empty role");

                return View(user);
            }
            if (ModelState.IsValid)
            {

                var result = await _userService.AddDashboardAsync(user);

                if (result.Succeeded)
                {

                    _mediator.Publish(new LogAddViewModel()
                    {
                        ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                        IpAddress = Config.GetIpAddress(_httpContext),

                        Table = ControllerContext.ActionDescriptor.ControllerName,
                        Action = ControllerContext.ActionDescriptor.ActionName,
                        Details = $"Create Dashboard {user.FullName} with Email {user.Email}",
                    });
                    return RedirectToAction(nameof(Index));
                }

                AddErrors(result);
            }
            ViewData["Roles"] = await _userService.GetRolesList(role);

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userService.GetDashboardEditAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var selectedRoles = await _userService.GetUserRoles(id);
            ViewData["Roles"] = await _userService.GetRolesList(selectedRoles);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DashboardEditViewModel user)
        {

            if (user.Roles == null || user.Roles.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                ViewData["Roles"] = await _userService.GetRolesList(user.Roles);
                ModelState.AddModelError("Roles", "Can't select empty role");

                return View(user);
            }
            if (ModelState.IsValid)
            {
                user.UserType = UserType.Dashboard;
                user.FromAdmin = true;
                var result = await _userService.UpdateDashboardAsync(user);

                if (result.Succeeded)
                {

                    _mediator.Publish(new LogAddViewModel()
                    {
                        ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                        IpAddress = Config.GetIpAddress(_httpContext),

                        Table = ControllerContext.ActionDescriptor.ControllerName,
                        Action = ControllerContext.ActionDescriptor.ActionName,
                        Details = $"Updated Dashboard User {user.FullName} with Email {user.Email}",
                    });
                    return RedirectToAction(nameof(Index));
                }

                AddErrors(result);
            }
            var selectedRoles = await _userService.GetUserRoles(id);
            ViewData["Roles"] = await _userService.GetRolesList(selectedRoles);

            return View(user);
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> SetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(string id, SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var removePass = await _userManager.RemovePasswordAsync(user);
            if (removePass.Succeeded)
            {
                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addPasswordResult.Succeeded)
                {
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }

            StatusMessage = "Your password has been set.";

            return View(model);
        }


        public async Task<JsonResult> JsonDelete(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                var result = await _userService.DeleteUserAsync(id);

                if (result.Succeeded)
                {
                    _mediator.Publish(new LogAddViewModel()
                    {
                        ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                        IpAddress = Config.GetIpAddress(_httpContext),

                        Table = ControllerContext.ActionDescriptor.ControllerName,
                        Action = ControllerContext.ActionDescriptor.ActionName,
                        Details = $"Deleted User with Id {id}",
                    });
                    return Json("Removed");
                }
            }

            return Json("Error");
        }


        public async Task<IActionResult> SwitchActivity(string id, UserType userType)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                var result = await _userService.LockUserAsync(id);

                if (result.Succeeded)
                {
                    _mediator.Publish(new LogAddViewModel()
                    {
                        ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                        IpAddress = Config.GetIpAddress(_httpContext),

                        Table = ControllerContext.ActionDescriptor.ControllerName,
                        Action = ControllerContext.ActionDescriptor.ActionName,
                        Details = $"{(result.Data.Active ? "UnLock" : "Lock")} User with Id {id}",
                    });

                    return RedirectToAction((result.Data.UserType == UserType.Dashboard ? "Index" : "Users"));
                }
            }

            return RedirectToAction((userType == UserType.Dashboard ? "Index" : "Users"));
        }


        private void AddErrors(IdentityResultViewModel result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, _localizer[error.Description].Value);
            }
        }

    }

}
