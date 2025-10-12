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
    public class CategoryController : Controller
    {
         readonly ICategoryRepository   _categoryRepository;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryController(IMediator mediator, IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager , IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mediator = mediator;
            _httpContext = httpContext;
            _userManager = userManager;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        // GET: CityController
        public async Task<ActionResult> Index()
        {
            return View(new CategoryTableData());
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _categoryRepository.GetCategoryDataTableAsync(param);


                return new JsonResult(new JqueryDataTablesResult<CategoryTableData>
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

            var results = await _categoryRepository.GetCategoryDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

            return new JqueryDataTablesExcelResult<CategoryTableData>(results.Items, "Category", "Category");
        }
        public async Task<IActionResult> PrintTable()
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _categoryRepository.GetCategoryDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


            return PartialView("_CategoryPrintTable", results.Items);
        }


        // GET: CategoryController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryVM   categoryVM)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var result2 = _mapper.Map<Category>(categoryVM);

                    var result = await _categoryRepository.AddAsync(result2);
                    if (result)
                    {


                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Created Category  {categoryVM.Name} with Id {categoryVM.Id}",
                        });
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                }

            }


            return View(categoryVM);

        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = await _categoryRepository.GetAsync(id);

            var categoryVM = _mapper.Map<CategoryVM>(category);

            if (categoryVM == null)
            {
                return NotFound();

            }

            return View(categoryVM);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CategoryVM categoryvm)
        {
            if (id != categoryvm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try

                {
                
                    var result2 = _mapper.Map<Category>(categoryvm);

                    var result = await _categoryRepository.UpdateAsync(result2);
                    if (result)
                    {
                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Update Category  {categoryvm.Name} with Id {categoryvm.Id}",
                        });
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsCategoryExist(categoryvm.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(categoryvm);
        }


        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _categoryRepository.DeleteAsync((int)id);
            if (result)
            {

                _mediator.Publish(new LogAddViewModel()
                {
                    ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                    IpAddress = Config.GetIpAddress(_httpContext),
                    Table = ControllerContext.ActionDescriptor.ControllerName,
                    Action = ControllerContext.ActionDescriptor.ActionName,
                    Details = $"Delete category with Id {id}",
                });
                return Json("Removed");
            }
            return Json("Failed");
        }

        public async Task<bool> IsCategoryExist(int id)
        {
            return await _categoryRepository.AnyAsync(id);
        }
    }
}
