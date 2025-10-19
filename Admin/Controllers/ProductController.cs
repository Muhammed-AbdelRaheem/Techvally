using Data;
using Data.IRepository;
using Domain.Models.NotificationHandlerVM;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Newtonsoft.Json;
using AutoMapper;
using DataAccess.IRepository;
using DataAccess.Services;

namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitofWork _unitofWork;

        public ProductController(IMediator mediator, IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager, IUnitofWork unitofWork ,IMapper mapper)
        {
            _mediator = mediator;
            _httpContext = httpContext;
            _userManager = userManager;
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            return View(new ProductDataTable());
        }

        [HttpPost]
        public async Task<IActionResult> ProductLoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _unitofWork.Product.GetProductDataTableAsync(param);


                return new JsonResult(new JqueryDataTablesResult<ProductDataTable>
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
        public async Task<IActionResult> ProductExcel()
        {

            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Product.GetProductDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

            return new JqueryDataTablesExcelResult<ProductDataTable>(results.Items, "Masters", "Masters");
        }
        public async Task<IActionResult> ProductPrintTable()
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Product.GetProductDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


            return PartialView("_ProductPrintTable", results.Items);
        }


       // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {

            ViewBag.category = await _unitofWork.Category.GetCategorySelectListAsync();

            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductVM   masterVM)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    

                    var result2 = _mapper.Map<Product>(masterVM);

                    var result = await _unitofWork.Product.AddAsync(result2);
                    if (result)
                    {


                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Created Product  {masterVM.Title} with Id {masterVM.Id}",
                        });
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                }

            }

            ViewBag.mastercategory = await _unitofWork.Category.GetCategorySelectListAsync(masterVM.CategoryId);

            return View(masterVM);

        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var product = await _unitofWork.Product.GetAsync(id);
            var productVm = _mapper.Map<ProductVM>(product);

            if (productVm == null)
            {
                return NotFound();

            }
            ViewBag.category = await _unitofWork.Category.GetCategorySelectListAsync(product.CategoryId);
            return View(productVm);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductVM  master)
        {
            if (id != master.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result2 = _mapper.Map<Product>(master);

                    var result = await _unitofWork.Product.UpdateAsync(result2);
                    if (result)
                    {
                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Update Product  {master.Title} with Id {master.Id}",
                        });
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsCategoryExist(master.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.mastercategory = await _unitofWork.Category.GetCategorySelectListAsync(master.CategoryId);

            return View(master);
        }


        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.Product.DeleteAsync((int)id);
            if (result)
            {

                _mediator.Publish(new LogAddViewModel()
                {
                    ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                    IpAddress = Config.GetIpAddress(_httpContext),
                    Table = ControllerContext.ActionDescriptor.ControllerName,
                    Action = ControllerContext.ActionDescriptor.ActionName,
                    Details = $"Delete Master with Id {id}",
                });
                return Json("Removed");
            }
            return Json("Failed");
        }

        public async Task<bool> IsCategoryExist(int id)
        {
            return await _unitofWork.Product.AnyAsync(id);
        }
    }
}
