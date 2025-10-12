using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface ILastestNewsRepository
    {


        public Task<bool> Addasync(LastestNews news);

        public Task<LastestNews> Getasync(int id, bool hidden = false, bool ShowInHomePage = false);
        public Task<IEnumerable<LastestNews>> GetAllasync(bool hidden = false, bool ShowInHomePage = false);


        public Task<bool> Updateasync(LastestNews news);
        public Task<bool> lastestnewsAnyAsync(int id);
        public Task<bool> Deleteasync(int id);

        public Task<bool> Duplicateasync(int id);

        public Task<JqueryDataTablesPagedResults<LastestNewsDataTable>> GetDataTableAsync(JqueryDataTablesParameters table);




    }
}
