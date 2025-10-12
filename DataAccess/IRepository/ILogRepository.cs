
using Domain.Models;
using Domain.Models.NotificationHandlerVM;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;

namespace DataAccess.IRepository
{
    public interface ILogRepository
    {
        public Task<JqueryDataTablesPagedResults<LogViewModel>> GetLogs(JqueryDataTablesParameters table);
        public Task AddLogAsync(LogAddViewModel logAddViewModel);
        public Task CleanLogAsync(string actionName, string controllerName);

    }
}
