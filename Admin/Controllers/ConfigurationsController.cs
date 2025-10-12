using DataAccess.IRepository;
using DataAccess.Services;
using Domain.Models;
using Domain.Models.NotificationHandlerVM;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Admin.Controllers
{
    public class ConfigurationsController : Controller
    {
        public IConfigurationRepository _ConfigurationRepository;
        private readonly IHttpContextAccessor _httpContext;
        private UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public ConfigurationsController(IConfigurationRepository ConfigurationService, IMediator mediator, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContext)
        {
            _ConfigurationRepository = ConfigurationService;
            _mediator = mediator;
            _userManager = userManager;
            _httpContext = httpContext;
        }

        // GET: ConfigurationController
        public async Task<ActionResult> Index()
        {
            var configuration = await _ConfigurationRepository.GetConfigurationsAsync();
            var id = configuration.Select(e => e.Id).FirstOrDefault();
            if (id != 0)
            {
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return RedirectToAction(nameof(Create));
        }


        // GET: ConfigurationController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: ConfigurationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Configuration Configuration)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Force UTC Kind before mapping
                    Configuration.CreatedOnUtc = DateTime.SpecifyKind(Configuration.CreatedOnUtc, DateTimeKind.Utc);
                    Configuration.UpdatedOnUtc = DateTime.SpecifyKind(Configuration.UpdatedOnUtc, DateTimeKind.Utc);

                    var result = await _ConfigurationRepository.AddConfigurationAsync(Configuration);
                    if (result)
                    {

                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Create Configuration {Configuration.EnName} with Id {Configuration.Id}",
                        });
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                }

            }


            return View(Configuration);

        }

        // GET: ConfigurationController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var Configuration = await _ConfigurationRepository.GetConfigurationAsync(id);
            if (Configuration == null)
            {
                return NotFound();

            }
            return View(Configuration);
        }

        // POST: ConfigurationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Configuration Configuration)
        {
            if (id != Configuration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var result = await _ConfigurationRepository.UpdateConfigurationAsync(Configuration);
                    if (result)
                    {

                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Update Configuration {Configuration.EnName} with Id {Configuration.Id}",
                        });
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsConfigurationExist(Configuration.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(Configuration);
        }


        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _ConfigurationRepository.DeleteConfigurationAsync((int)id);
            if (result)
            {

                _mediator.Publish(new LogAddViewModel()
                {
                    ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                    IpAddress = Config.GetIpAddress(_httpContext),
                    Table = ControllerContext.ActionDescriptor.ControllerName,
                    Action = ControllerContext.ActionDescriptor.ActionName,
                    Details = $"Delete Configuration with Id {id}",
                });
                return Json("Removed");
            }
            return Json("Failed");
        }

        public async Task<bool> IsConfigurationExist(int id)
        {
            return await _ConfigurationRepository.GetConfigurationApiAsync().Where(e => e.Id == id).AnyAsync();
        }
    }
}
