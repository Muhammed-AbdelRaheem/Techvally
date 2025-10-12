using Data.IRepository;
using DataAccess.IRepository;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Admin.Controllers;

[Authorize(Roles = "Admin")]
public class LogsController : Controller {
    private readonly IUnitofWork _unitofWork;
    public ILogRepository _LogRepository;

    public LogsController(IUnitofWork unitofWork)
    {
        _unitofWork = unitofWork;
    }
    // GET: Admin/Logs
    public IActionResult Index()
    {
        return View(new LogViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param)
    {
        try
        {
            HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonConvert.SerializeObject(param));
            var results = await _unitofWork.Log.GetLogs(param);

            return new JsonResult(new JqueryDataTablesResult<LogViewModel>
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
    public async Task<IActionResult> LogsExcel()
    {

        var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

        var results = await _unitofWork.Log.GetLogs(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));

        return new JqueryDataTablesExcelResult<LogViewModel>(results.Items, "Logs", "Logs");
    }
    public async Task<IActionResult> LogsPrintTable()
    {
        var param = HttpContext.Session.GetString(nameof(JqueryDataTablesParameters));

        var results = await _unitofWork.Log.GetLogs(JsonConvert.DeserializeObject<JqueryDataTablesParameters>(param));


        return PartialView("_LogsPrintTable", results.Items);
    }
  

}