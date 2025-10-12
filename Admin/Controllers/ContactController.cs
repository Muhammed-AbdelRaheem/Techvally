using AutoMapper;
using DataAccess.IRepository;
using DataAccess.Repository;
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
using Project.DataAccess.Repository.IRepository;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;


namespace Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ContactController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public ContactController(IMediator mediator, IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager, IUnitofWork unitofWork ,IMapper mapper)
        {
            _mediator = mediator;
            _httpContext = httpContext;
            _userManager = userManager;
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            return View(new ContactDataTable());

        }



        [HttpPost]
        public async Task<IActionResult> ContactLoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _unitofWork.Contact.GetContactDataTableAsync(param);


                return new JsonResult(new JqueryDataTablesResult<ContactDataTable>
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
        public async Task<IActionResult> ContactExcel()
        {

            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Product.GetProductDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

            return new JqueryDataTablesExcelResult<ProductDataTable>(results.Items, "Masters", "Masters");
        }
        public async Task<IActionResult> ContactPrintTable()
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Product.GetProductDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


            return PartialView("_ProductPrintTable", results.Items);
        }



        // get create
        public IActionResult Create()
        {

            return View();
        }

        // post create
        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)  //  (obj== anta btstlm el value el fe post method input fe el create view )  when we have the obj here that will have the value of category that needs to be add
        {


            //Validation for sliver side
            if (ModelState.IsValid)
            {
                

            var result=    await _unitofWork.Contact.Addasync(contact);
                if (result)
                {


                    _mediator.Publish(new LogAddViewModel()
                    {
                        ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                        IpAddress = Config.GetIpAddress(_httpContext),
                        Table = ControllerContext.ActionDescriptor.ControllerName,
                        Action = ControllerContext.ActionDescriptor.ActionName,
                        Details = $"Created Product  {contact.Title} with Id {contact.Id}",
                    });
                    return RedirectToAction(nameof(Index));
                }
                _unitofWork.Save();
                TempData["success"] = "  Create successfully";
                return RedirectToAction("Index");

            }
                return View();
                

        }
      
        



        // get Edit
        public async Task<IActionResult> Edit(int id)
        {


            if (id == 0)
            {
                return NotFound();
            }

            Contact? lenstypefromDb = await _unitofWork.Contact.Getasync(id);
            var contactVM = _mapper.Map<ContactVm>(lenstypefromDb);

            if (contactVM == null)
            {
                return NotFound();
            }
            return View(contactVM);
        }

        // post Edit
        [HttpPost]
        public async Task<IActionResult> Edit(ContactVm contactvm, int id)
        {

            if (id != contactvm.Id)
            {
                return NotFound();
            }

            //Validation for sliver side
            if (ModelState.IsValid)
            {

                try
                {
                    var result2 = _mapper.Map<Contact>(contactvm);

                    var featuretest = await _unitofWork.Contact.Updateasync(result2);
                    if (featuretest)
                    {
                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Updated Contact  {contactvm.Title} with Id {contactvm.Id}",
                        });

                        TempData["success"] = "  Update successfully";
                        return RedirectToAction(nameof(Index));
                    }

                    return View(contactvm);


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsourclientlExist(contactvm.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }


            }
            return View();
        }


        public async Task<bool> IsourclientlExist(int id)
        {
            return await _unitofWork.Contact.ContactAnyAsync(id);
        }



        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.Contact.Deleteasync(id.Value);
            if (result)
            {

                return Json("Removed");
            }
            return Json("Failed");
        }


        public async Task<JsonResult> JsonDuplicate(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.Contact.Duplicateasync(id.Value);
            if (result)
            {
                //_UnitofWork.Save();
                TempData["success"] = "  duplicate successfully";
                return Json("Duplicated");
            }
            return Json("Failed");

        }



    }
}
