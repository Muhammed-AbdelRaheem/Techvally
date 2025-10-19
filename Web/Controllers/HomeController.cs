using DataAccess.IRepository;
using DataAccess.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using System.Diagnostics;
using System.Reflection;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUnitofWork _UnitofWork;

        public HomeController( IUnitofWork UnitofWork)
        {
          
            _UnitofWork = UnitofWork;


        }
        public async Task<IActionResult> Index()
        {


            var vendor = await _UnitofWork.Vendor.GetAllasync();
            if (vendor is null)
            {
                return NotFound();
            }

            var homepage = await _UnitofWork.Block.GetBlockshomepage(BlockType.HomePage);
            if (homepage is null)
            {
                return NotFound();
            }
            var ourclient = await _UnitofWork.OurClient.GetAllasync();
            if (ourclient is null)
            {
                return NotFound();
            }

            var lastestNews = await _UnitofWork.LastestNews.GetAllasync();
            if (lastestNews is null)
            {
                return NotFound();
            }

            var Profile = await _UnitofWork.Profile.GetAllasync();
            if (lastestNews is null)
            {
                return NotFound();
            }
            //var testimonial = await _UnitofWork.Feed.GetAllasync();
            //if (testimonial is null)
            //{
            //    return NotFound();
            //}

            var contact = await _UnitofWork.Contact.GetAllasync();
            if (contact is null)
            {
                return NotFound();
            }

            var category = await _UnitofWork.Category.GetAllCategoryAsync();
            if (category is null)
            {
                return NotFound();
            }

            HomePageVM headerVM = new()
            {
                profiles = Profile.ToList(),
                vendors = vendor.ToList(),
                ourClients = ourclient.ToList(),
                LastestNews = lastestNews.ToList(),
                //feeds = testimonial.ToList(),
                contacts = contact.ToList(),
                categories = category.ToList(),
                Blocks = homepage.ToList()
            };

            return View(headerVM);
        }

        //Category
        public async Task<IActionResult> CategoryPage(int page = 1)
        {
            int pageSize = 3;
            IEnumerable<Category> allitems = await _UnitofWork.Category.GetAllCategoryAsync();


            allitems = allitems.Where(e => !e.Hidden);

            int totalProducts = allitems.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            List<Category> category = allitems.Where(e => !e.Hidden).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new HomePageVM
            {
                categories = category,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }


        //Product
        public async Task<IActionResult> ViewProduct(int itemId)
        {
            var product = await _UnitofWork.Product.GetAllProductAsync(itemId);
            if (product is null)
            {
                return NotFound();
            }

            var category = await _UnitofWork.Category.GetAllparentCategorywithsub(itemId);
            if (category is null)
            {
                return NotFound();
            }

            HomePageVM headerVM = new()
            {

                categories = category.ToList(),
                products = product.ToList(),

            };


            ViewData["ItemId"] = itemId;
            return View(headerVM);
        }

        public async Task<IActionResult> DetailsViewProduct(int id, int itemId)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var banner = await _UnitofWork.Product.GetAsync(id);
            if (banner == null)
            {
                return NotFound();

            }

            //IEnumerable<SelectListItem> ProductList = _productRepository.GetAll().Select(i => new SelectListItem
            //{
            //    Text = i.Title,
            //    Value = i.Id.ToString()
            //});

            //ViewData["ProductList"] = ProductList;

            ViewData["ItemId"] = itemId;

            HomePageVM headerVM = new()
            {
                getproduct = banner,


            };


            return View(headerVM);
        }
        [HttpPost]
        public async Task<IActionResult> DetailsViewProduct(ProductRequest homePageVM)
        {



            //Validation for sliver side
            if (ModelState.IsValid)
            {
                await _UnitofWork.ProductRequest.AddServiceRequestAsync(homePageVM);

                TempData["success"] = " Create successfully";
                return RedirectToAction(nameof(DetailsViewProduct));
            }

            return View();
        }


        //Profile
        public async Task<IActionResult> Profile()
        {


            var z = await _UnitofWork.Profile.GetAllasync();

            //var profile = new Profile    
            //{
            //    HPTitle = z.HPTitle,
            //    HPImage = z.HPImage,
            //    DImage = z.DImage,
            //    Description = z.Description,
            //    hiddenImage1 = z.hiddenImage1,
            //    hiddenImage2 = z.hiddenImage2,
            //    Hidden = z.Hidden,
            //    Deleted = z.Deleted,
            //    DisplayOrder = z.DisplayOrder,
            //    CreatedOnUtc = z.CreatedOnUtc,
            //    UpdatedOnUtc = z.UpdatedOnUtc,
            //    Id = z.Id,
            //};

            var vendor2 = await _UnitofWork.Vendor.GetAllasync();
            if (vendor2 is null)
            {
                return NotFound();
            }



            HomePageVM headerVM = new()
            {
                profiles = z.ToList(),
                vendors = vendor2.ToList(),
            };

            return View(headerVM);

        }
        //Vendor
        public async Task<IActionResult> Vendor(int page = 1)
        {
            int pageSize = 3;
            IEnumerable<Vendor> allVendros = await _UnitofWork.Vendor.GetAllasync();

            allVendros = allVendros.Where(e => !e.Hidden);

            int totalVendors = allVendros.Count();
            int totalPages = (int)Math.Ceiling((double)totalVendors / pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            List<Vendor> vendors = allVendros.Where(e => !e.Hidden).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PaginationVenderVM
            {
                vendors = vendors,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
        //Client
        public async Task<IActionResult> Client(int page = 1)
        {
            int pageSize = 8;
            IEnumerable<OurClient> allitems = await _UnitofWork.OurClient.GetAllasync();

            // Filter out hidden items
            allitems = allitems.Where(e => !e.Hidden);

            // Calculate the total number of pages
            int totalProducts = allitems.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            List<OurClient> client = allitems.Where(e => !e.Hidden).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new HomePageVM
            {
                ourClients = client,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }



        //LastestNews
        public async Task<IActionResult> LatestNews(int page = 1)
        {
            int pageSize = 3;
            IEnumerable<LastestNews> allitems = await _UnitofWork.LastestNews.GetAllasync();


            allitems = allitems.Where(e => !e.Hidden);

            int totalProducts = allitems.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            page = Math.Max(1, Math.Min(page, totalPages));


            List<LastestNews> news = allitems.Where(e => !e.Hidden).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new HomePageVM
            {
                LastestNews = news,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public async Task<IActionResult> LastestNewsDetails(int id)
        {
            // how to create edit btton
            if (id == 0)
            {
                return NotFound();
            }

            LastestNews lenstypefromDb = await _UnitofWork.LastestNews.Getasync(id); ;
            if (lenstypefromDb == null)
            {
                return NotFound();
            }


            return View(lenstypefromDb);
        }


        //ContactUs
        public async Task<IActionResult> ContactUs()
        {


            var contactt = await _UnitofWork.Contact.GetAllasync();
            if (contactt is null)
            {
                return NotFound();
            }

            HomePageVM headerVM = new()
            {

                contacts = contactt.ToList(),

            };

            return View(headerVM);
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs(MessageForm homePageVM)
        {

            //Validation for sliver side
            if (ModelState.IsValid)
            {
                await _UnitofWork.MessageForm.AddMessageFormAsync(homePageVM);

                TempData["success"] = " Create successfully";
                return RedirectToAction(nameof(ContactUs));
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // TODO: Load state from previously suspended application2

            return View(new Domain.Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

        #region Not Use
        //public async Task<IActionResult> Solution(int page = 1)
        //{
        //    int pageSize = 3; // Number of products to display per page
        //    IEnumerable<Solution> allitems = await _UnitofWork.Solution.GetAllasync(); // Replace with your data retrieval logic

        //    // Filter out hidden items
        //    allitems = allitems.Where(e => !e.Hidden);

        //    // Calculate the total number of pages
        //    int totalProducts = allitems.Count();
        //    int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
        //    // Adjust the page number if it exceeds the total number of pages
        //    page = Math.Max(1, Math.Min(page, totalPages));

        //    // Paginate the products using Skip and Take
        //    List<Solution> solution = allitems.Where(e => !e.Hidden).Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    var viewModel = new HomePageVM
        //    {
        //        solutions = solution,
        //        CurrentPage = page,
        //        TotalPages = totalPages
        //    };

        //    return View(viewModel);
        //}



        //public async Task<IActionResult> SolutionDetails(int id)
        //{
        //    // how to create edit btton
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Solution lenstypefromDb = await _UnitofWork.Solution.Getasync(id); ;
        //    if (lenstypefromDb == null)
        //    {
        //        return NotFound();
        //    }


        //    return View(lenstypefromDb);
        //}

        //public async Task<IActionResult> Promotion()
        //{


        //    var promotion = await _UnitofWork.Promotion.GetAllasync();
        //    if (promotion is null)
        //    {
        //        return NotFound();
        //    }


        //    HomePageVM headerVM = new()
        //    {

        //        promotions = promotion.ToList(),
        //    };
        //    return View(headerVM);
        //}



        //public async Task<IActionResult> PromotionDetails(int id)
        //{
        //    // how to create edit btton
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Promotion lenstypefromDb = await _UnitofWork.Promotion.Getasync(id); ;
        //    if (lenstypefromDb == null)
        //    {
        //        return NotFound();
        //    }


        //    return View(lenstypefromDb);
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ContactForm(string language, Feedback feedback)
        //{

        //    var recaptcha = await _recaptcha.Validate(Request);
        //    if (!recaptcha.success || recaptcha.score < 0.9)
        //        //return Json(new { status = "failed", message = language == "ar" ? "هناك خطأ ما!" : "Something went wrong!" });
        //        return RedirectToAction("Index", new { language });


        //    if (ModelState.IsValid)
        //    {
        //        //var host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
        //        var result = await _feedbackRepository.SendEmail(feedback, language);

        //        if (result)
        //        {
        //            //return Json(new { status = "success" });
        //            TempData["success"] = "successfully";
        //            return RedirectToAction("Index", new { language, Sent = true });

        //        }
        //    }

        //    //return Json(new { status = "failed", message = language == "ar" ? "هناك خطأ ما!" : "Something went wrong!" });
        //    return RedirectToAction("Index", new { language });
        //}

        #endregion