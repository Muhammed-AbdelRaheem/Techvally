using AutoMapper;
using DataAccess.IRepository;
using DataAccess.Services;
using Domain.Models;
using Domain.Models.NotificationHandlerVM;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class VendorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitofWork _unitofWork;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public VendorController(IMediator mediator, IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager, IMapper mapper, IUnitofWork unitofWork)
        {
            _mediator = mediator;
            _httpContext = httpContext;
            _userManager = userManager;
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        // GET: CityController
        public async Task<ActionResult> Index()
        {
            return View(new VendorDataTable());
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _unitofWork.Vendor.GetVendorDataTableAsync(param);


                return new JsonResult(new JqueryDataTablesResult<VendorDataTable>
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
        public async Task<IActionResult> Excel()
        {

            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Vendor.GetVendorDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

            return new JqueryDataTablesExcelResult<VendorDataTable>(results.Items, "Desire", "Desire");
        }
        public async Task<IActionResult> PrintTable()
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Vendor.GetVendorDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


            return PartialView("_PrintTable", results.Items);
        }


        // GET: CityController/Create
        public async Task<ActionResult> Create()
        {

            return View();
        }

        // POST: CityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendorVM masterVM)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var result2 = _mapper.Map<Vendor>(masterVM);

                    var result = await _unitofWork.Vendor.Addasync(result2);
                    if (result)
                    {


                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Created Desire  {masterVM.Title} with Id {masterVM.Id}",

                        });
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                }

            }


            return View(masterVM);

        }

        // GET: CityController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var Master = await _unitofWork.Vendor.Getasync(id);
            var result2 = _mapper.Map<VendorVM>(Master);

            if (result2 == null)
            {
                return NotFound();

            }
            return View(result2);
        }

        // POST: CityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VendorVM master)
        {
            if (id != master.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result2 = _mapper.Map<Vendor>(master);

                    var result = await _unitofWork.Vendor.Updateasync(result2);
                    if (result)
                    {
                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Edit Vendor  {master.Title} with Id {master.Id}",

                        });
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsCityExist(master.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(master);
        }


        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.Vendor.Deleteasync((int)id);
            if (result)
            {

                _mediator.Publish(new LogAddViewModel()
                {
                    ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                    IpAddress = Config.GetIpAddress(_httpContext),
                    Table = ControllerContext.ActionDescriptor.ControllerName,
                    Action = ControllerContext.ActionDescriptor.ActionName,
                    Details = $"Delete Vendor with Id {id}",
                });
                return Json("Removed");
            }
            return Json("Failed");
        }

        public async Task<bool> IsCityExist(int id)
        {
            return await _unitofWork.Vendor.AnyAsync(id);
        }
    }

}
