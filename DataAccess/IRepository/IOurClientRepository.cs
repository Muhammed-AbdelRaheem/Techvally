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
    public interface IOurClientRepository
    {


        public Task<bool> Addasync(OurClient ourclient);

        public Task<OurClient> Getasync(int id, bool hidden = false);
        public Task<IEnumerable<OurClient>> GetAllasync(bool hidden = false);


        public Task<bool> Updateasync(OurClient ourclient);
        public Task<bool> ourclientAnyAsync(int id);
        public Task<bool> Deleteasync(int id);

        public Task<bool> Duplicateasync(int id);
        public Task<bool> AnyAsync(int Id);

        public Task<JqueryDataTablesPagedResults<ClientDataTable>> GetClientDataTableAsync(JqueryDataTablesParameters table);






    }
}
