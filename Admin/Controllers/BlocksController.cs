using DataAccess.IRepository;
using DataAccess.Services;
using Domain.Models;
using Domain.Models.NotificationHandlerVM;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Utility;

namespace Admin.Controllers
{
    public class BlocksController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private IStringLocalizer<SharedResource> _localizer;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlocksController(IUnitofWork unitofWork, IStringLocalizer<SharedResource> localizer, IMediator mediator, IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager)
        {
            _unitofWork = unitofWork;
            _localizer = localizer;
            _mediator = mediator;
            _httpContext = httpContext;
            _userManager = userManager;
        }




        // GET: BlockController
        public async Task<ActionResult> Index(BlockType blockType)
        {
            ViewData["Title"] = _localizer[blockType.DisplayName()];
            ViewData["BlockType"] = blockType;

            var Blocks = await _unitofWork.Block.GetBlocksAsync(blockType);


            //if (Blocks.Any())
            //{

            //    return RedirectToAction("Edit", new { blockType, id = Blocks.Select(e => e.Id).FirstOrDefault() });

            //}
            if (blockType == BlockType.HomePage)
            {
                return View(new BlockDataTable());
            }

            if (blockType == BlockType.Profile)
            {
                return RedirectToAction("Index","Profile");

            }
            if (blockType == BlockType.Contactus)
            {
                return RedirectToAction("Index", "Contact");

            }
              if (blockType == BlockType.LastestNews)
            {
                return RedirectToAction("Index", "LastestNews");

            }
           

            return RedirectToAction("Create", new { blockType });



            //return View(Blocks);

        }

        [HttpPost]
        public async Task<IActionResult> BlockLoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
                var results = await _unitofWork.Block.GetBlockDataTableAsync(param);


                return new JsonResult(new JqueryDataTablesResult<BlockDataTable>
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


        public async Task<IActionResult> BlockExcel()
        {

            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Block.GetBlockDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

            return new JqueryDataTablesExcelResult<BlockDataTable>(results.Items, "Masters", "Masters");
        }
        public async Task<IActionResult> BlockPrintTable()
        {
            var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

            var results = await _unitofWork.Block.GetBlockDataTableAsync(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


            return PartialView("_ProductPrintTable", results.Items);
        }


        // GET: BlockController/Create
        public ActionResult Create(BlockType blockType)
        {
            ViewData["Main"] = _localizer[blockType.DisplayName()];
            ViewData["BlockType"] = blockType;

            return View();
        }

        // POST: BlockController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BlockType blockType, Block Block)
        {
            ViewData["Main"] = _localizer[blockType.DisplayName()];
            ViewData["BlockType"] = blockType;

            if (ModelState.IsValid)
            {
                try
                {
                    Block.CreatedOnUtc = DateTime.SpecifyKind(Block.CreatedOnUtc, DateTimeKind.Utc);
                    Block.UpdatedOnUtc = DateTime.SpecifyKind(Block.UpdatedOnUtc, DateTimeKind.Utc);


                    var result = await _unitofWork.Block.AddBlockAsync(Block);
                    if (result)
                    {
                        if (blockType == BlockType.Contactus)
                        {
                            return RedirectToAction("Edit", new { blockType, id = Block.Id });
                        }

                        await _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Create {blockType} with Id {(Block.Id)}",
                        });

                        return RedirectToAction(nameof(Index), new { blockType });


                    }
                }
                catch
                {
                }

            }


            return View(Block);

        }

        // GET: BlockController/Edit/5
        public async Task<ActionResult> Edit(BlockType blockType, int id)
        {
            ViewData["BlockType"] = blockType;
            ViewData["Main"] = _localizer[blockType.DisplayName()];

            if (id == 0)
            {
                return NotFound();
            }
            var Block = await _unitofWork.Block.GetBlockAsync(blockType, id);

            if (Block == null)
            {
                return NotFound();

            }
            return View(Block);
        }

        // POST: BlockController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BlockType blockType, int id, Block Block)
        {
            ViewData["Main"] = _localizer[blockType.DisplayName()];
            ViewData["BlockType"] = blockType;

            if (id != Block.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var result = await _unitofWork.Block.UpdateBlockAsync(Block);
                    if (result)
                    {

                        await _mediator.Publish(new LogAddViewModel()
                        {
                            ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                            IpAddress = Config.GetIpAddress(_httpContext),
                            Table = ControllerContext.ActionDescriptor.ControllerName,
                            Action = ControllerContext.ActionDescriptor.ActionName,
                            Details = $"Edit {blockType} with Id {(Block.Id)}",
                        });
                        return RedirectToAction(nameof(Index), new { blockType });
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await IsBlockExist(Block.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }


            return View(Block);
        }

        public async Task<JsonResult> JsonDuplicate(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.Block.DuplicateBlockAsync((int)id);
            if (result)
            {

                await _mediator.Publish(new LogAddViewModel()
                {
                    ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                    IpAddress = Config.GetIpAddress(_httpContext),
                    Table = ControllerContext.ActionDescriptor.ControllerName,
                    Action = ControllerContext.ActionDescriptor.ActionName,
                    Details = $"Duplicated with Id {(id)}",
                });
                return Json("Duplicated");
            }
            return Json("Failed");

        }

        public async Task<JsonResult> JsonDelete(int? id)
        {
            if (id == null)
            {
                return Json("Failed");

            }
            var result = await _unitofWork.Block.DeleteBlockAsync((int)id);
            if (result)
            {
                await _mediator.Publish(new LogAddViewModel()
                {
                    ApplicationUserId = Config.GetUserId(_httpContext, _userManager),
                    IpAddress = Config.GetIpAddress(_httpContext),
                    Table = ControllerContext.ActionDescriptor.ControllerName,
                    Action = ControllerContext.ActionDescriptor.ActionName,
                    Details = $"Delete with Id {(id)}",
                });

                return Json("Removed");
            }
            return Json("Failed");
        }

        public async Task<bool> IsBlockExist(int id)
        {
            return await _unitofWork.Block.BlockAnyAsync(id);
        }
        public IActionResult BookingOverview()
        {
            return View();
        }
    }

}
