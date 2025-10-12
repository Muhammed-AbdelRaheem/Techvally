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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using Project.DataAccess.Repository;
using Project.DataAccess.Repository.IRepository;
using Profile = Domain.Models.Profile;


namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]


    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IMapper _mapper;

        public ProfileController(IMediator mediator, IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager, IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment ,IMapper mapper)
        {
            _mediator = mediator;
            _httpContext = httpContext;
            _userManager = userManager;
            _unitofWork = unitofWork;
            _WebHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            return View(new ProfileDataTable());
        }

        [HttpPost]
        public async Task<IActionResult> ProfileLoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _unitofWork.Profile.GetProfileDataTableAsync(param);


                return new JsonResult(new JqueryDataTablesResult<ProfileDataTable>
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
        public async Task<IActionResult> ProfileExcel()
        {

            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Profile.GetProfileDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

            return new JqueryDataTablesExcelResult<ProfileDataTable>(results.Items, "Masters", "Masters");
        }
        public async Task<IActionResult> ProfilePrintTable()
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Profile.GetProfileDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


            return PartialView("_ProductPrintTable", results.Items);
        }
        // get create
        public IActionResult Create()
        {

            return View();
        }

        // post create
        [HttpPost]
        public async Task<IActionResult> Create(Profile aboutus, IFormFile? file, IFormFile? file2)  //  (obj== anta btstlm el value el fe post method input fe el create view )  when we have the obj here that will have the value of category that needs to be add
        {


            //Validation for sliver side
            if (ModelState.IsValid)
            {

                // Upload the first image
                if (file != null)
                {
                    aboutus.HPImage = await UploadImageAsync(file, aboutus.HPImage);
                }

                // Upload the second image
                if (file2 != null)
                {
                    aboutus.DImage = await UploadImageAsync(file2, aboutus.DImage);
                }



                await _unitofWork.Profile.Addasync(aboutus);
                _unitofWork.Save();
                TempData["success"] = "  Create successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<string> UploadImageAsync(IFormFile file, string existingImagePath)
        {
            string wwwRootPath = _WebHostEnvironment.WebRootPath;

            // Delete the old image if it exists
            if (!string.IsNullOrEmpty(existingImagePath))
            {
                var oldImagePath = Path.Combine(wwwRootPath, existingImagePath.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images\product");

            // Save the new image
            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return @"/images/product/" + fileName;
        }

        // get Edit
        public async Task<IActionResult> Edit(int id)
        {


            // how to create edit btton
            if ( id == 0)
            {
                return NotFound();
            }

            Profile? lenstypefromDb = await _unitofWork.Profile.Getasync();
            var profilVM = _mapper.Map<ProfileVM>(lenstypefromDb);

            if (profilVM == null)
            {
                return NotFound();
            }

    
            return View(profilVM);
        }

        // post Edit
        [HttpPost]
        public async Task<IActionResult> Edit(ProfileVM profileVM, IFormFile? file , int id, IFormFile? file2)
        {

            if (id != profileVM.Id)
            {
                return NotFound();
            }

            //Validation for sliver side
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    profileVM.HPImage = await UploadImageAsync(file, profileVM.HPImage);
                }

                // Upload the second image
                if (file2 != null)
                {
                    profileVM.DImage = await UploadImageAsync(file2, profileVM.DImage);
                }


                ////if (aboutus.HPImage == null && aboutus.hiddenImage1 == null )
                ////{
                ////    TempData["error"] = "You Need choose Image ";
                ////    return RedirectToAction("Edit");
                ////}

                ////if (aboutus.HPImage == null && aboutus.hiddenImage1 != null )
                ////{
                ////    aboutus.HPImage = aboutus.hiddenImage1;

                ////} 

                ////if (aboutus.DImage == null && aboutus.hiddenImage2 == null)
                ////{
                ////    TempData["error"] = "You Need choose Image ";
                ////    return RedirectToAction("Edit");
                ////}

                ////if (aboutus.DImage == null && aboutus.hiddenImage2 != null)
                ////{
                ////    aboutus.DImage = aboutus.hiddenImage2;
                ////}


                try

                {

                    var result2 = _mapper.Map<Profile>(profileVM);

                    var result = await _unitofWork.Profile.Updateasync(result2);
                    if (result)
                    {
                        _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Update Category  {profileVM.HPTitle} with Id {profileVM.Id}",
                        });
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsHPcarsouselExist(profileVM.Id)))
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


        public async Task<bool> IsHPcarsouselExist(int id)
        {
            return await _unitofWork.Profile.HPcarsouselAnyAsync(id);
        }



        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.Profile.Deleteasync(id.Value);
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
            var result = await _unitofWork.Profile.Duplicateasync(id.Value);
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







//if ((header.HPImage == null && header.hiddenImage1 == null) || (header.DImage == null && header.hiddenImage2 == null))
//{
//    TempData["error"] = "You Need to choose Image Again";
//    return RedirectToAction("Edit");
//}