using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using DataAccess.IRepository;
using Domain.Models;
using Domain.Models.NotificationHandlerVM;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDBContext _read;

        private readonly UserManager<ApplicationUser> _useManager;
        private readonly IMapper _autoMapper;


        public LogRepository(WriteDbContext context,
             IMapper autoMapper, ReadDBContext read)
        {
            _context = context;
            _autoMapper = autoMapper;
            _read = read;
        }


        public async Task AddLogAsync(LogAddViewModel logAddViewModel)
        {
            try
            {

                // Get the city from the IP Address
                var remoteAddress = string.IsNullOrWhiteSpace(logAddViewModel.IpAddress) ? "" : logAddViewModel.IpAddress.Split(",")[0];

                var log = new Log()
                {
                    IpAddress = remoteAddress,
                    ApplicationUserId = logAddViewModel.ApplicationUserId,
                    Action = logAddViewModel.Action,
                    Details = logAddViewModel.Details,
                    Table = logAddViewModel.Table,
                    CreatedOnUtc = Extantion.AddUtcTime(3),
                    UpdatedOnUtc = DateTime.UtcNow,
                    DisplayOrder = 10,
                    Hidden = false
                };
                await _context.Log.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }


        public async Task CleanLogAsync(string actionName, string controllerName)
        {
            try
            {
                var logs = await _context.Log.Where(e => e.CreatedOnUtc <= DateTime.UtcNow.AddDays(-7)).ToListAsync();

                if (logs.Count > 0)
                {
                    _context.Log.RemoveRange(logs);
                    await _context.SaveChangesAsync();
                }
                await AddLogAsync(new LogAddViewModel()
                {
                    Action = actionName,
                    Table = controllerName,
                    Details = $"Cleaned Log with count {logs.Count} from {DateTime.UtcNow.AddDays(-7).Date.ToShortDateString()}",
                });
            }
            catch
            { }
        }

        public async Task<JqueryDataTablesPagedResults<LogViewModel>> GetLogs(JqueryDataTablesParameters table)
        {
            IQueryable<Log> query = _read.Log.AsNoTracking();

            query = SearchOptionsProcessor<LogViewModel, Log>.Apply(query, table.Columns);
            query = SortOptionsProcessor<LogViewModel, Log>.Apply(query, table);

            var size = await query.CountAsync();
            try
            {
                if (table.Search != null && !string.IsNullOrWhiteSpace(table.Search.Value))
                {
                    var q = table.Search.Value.ToLower();
                    query = query.Where(e =>
                        e.Table!.ToLower().Contains(q) ||
                        e.Details!.ToLower().Contains(q) ||
                        e.IpAddress!.ToLower().Contains(q) ||
                        e.User.FullName!.ToLower().Contains(q) ||
                        e.User.Email!.ToLower().Contains(q)
                    );
                }

                var items = await query
               .AsNoTracking()
               .OrderByDescending(e => e.CreatedOnUtc)
               .Skip(table.Start / table.Length * table.Length)
               .Take(table.Length)
               .ProjectTo<LogViewModel>(_autoMapper.ConfigurationProvider)
               .ToArrayAsync();

                return new JqueryDataTablesPagedResults<LogViewModel>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }

}
