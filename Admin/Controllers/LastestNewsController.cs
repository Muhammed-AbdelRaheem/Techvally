using DataAccess.IRepository;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;


namespace Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class LastestNewsController : Controller
    {

        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public LastestNewsController(IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork;
            _WebHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {

            return View(new LastestNewsDataTable());
        }



        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _unitofWork.LastestNews.GetDataTableAsync(param);


                return new JsonResult(new JqueryDataTablesResult<LastestNewsDataTable>
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

            var results = await _unitofWork.LastestNews.GetDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

            return new JqueryDataTablesExcelResult<LastestNewsDataTable>(results.Items, "Masters", "Masters");
        }
        public async Task<IActionResult> PrintTable()
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.LastestNews.GetDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


            return PartialView("_ProductPrintTable", results.Items);
        }


        // get create
        public IActionResult Create()
        {

            return View();
        }

        // post create
        [HttpPost]
        public async Task<IActionResult> Create(LastestNews news, IFormFile? file)  //  (obj== anta btstlm el value el fe post method input fe el create view )  when we have the obj here that will have the value of category that needs to be add
        {


            //Validation for sliver side
            if (ModelState.IsValid)
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product\");


                    // change photos and delete old photo if Exists
                    if (!string.IsNullOrEmpty(news.Image))
                    {
                        var oldimagepath = Path.Combine(wwwRootPath, news.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimagepath))
                        {
                            System.IO.File.Delete(oldimagepath);
                        }
                    }
                    // change piets to file and  (( Save the photo))
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))  // where to have save the file
                    {
                        file.CopyTo(fileStream);      // that will copy the file in the new location|| that we added  in line 76
                    }

                    news.Image = @"\images\product\" + fileName;
                }


                await _unitofWork.LastestNews.Addasync(news);
                _unitofWork.Save();
                TempData["success"] = "  Create successfully";
                return RedirectToAction("Index");
            }
            return View();
        }



        // get Edit
        public async Task<IActionResult> Edit(int id)
        {


            // how to create edit btton
            if (id == 0)
            {
                return NotFound();
            }

            LastestNews? lenstypefromDb = await _unitofWork.LastestNews.Getasync(id); ;
            if (lenstypefromDb == null)
            {
                return NotFound();
            }

            //lenstypefromDb.hiddenImage = lenstypefromDb.Image;
            return View(lenstypefromDb);
        }

        // post Edit
        [HttpPost]
        public async Task<IActionResult> Edit(LastestNews news, IFormFile? file, int id)
        {

            if (id != news.Id)
            {
                return NotFound();
            }

            //Validation for sliver side
            if (ModelState.IsValid)
            {

                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product\");


                    // change photos and delete old photo if Exists
                    if (!string.IsNullOrEmpty(news.Image))
                    {
                        var oldimagepath = Path.Combine(wwwRootPath, news.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimagepath))
                        {
                            System.IO.File.Delete(oldimagepath);
                        }
                    }

                    // change piets to file and  (( Save the photo))
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))  // where to have save the file
                    {
                        file.CopyTo(fileStream);      // that will copy the file in the new location|| that we added  in line 76
                    }
                    news.Image = @"\images\product\" + fileName;


                }


                //if (news.Image == null && news.hiddenImage == null)
                //{
                //    TempData["error"] = "You Need choose Image Again";
                //    return RedirectToAction("Edit");
                //}

                //if (news.Image == null && news.hiddenImage != null)
                //{
                //    news.Image = news.hiddenImage;
                //}

                try
                {
                    var featuretest = await _unitofWork.LastestNews.Updateasync(news);
                    if (featuretest)
                    {
                        TempData["success"] = "  Update successfully";
                        return RedirectToAction("Index");
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsourclientlExist(news.Id)))
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
            return await _unitofWork.LastestNews.lastestnewsAnyAsync(id);
        }



        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.LastestNews.Deleteasync(id.Value);
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
            var result = await _unitofWork.LastestNews.Duplicateasync(id.Value);
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
