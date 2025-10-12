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
    public interface IVendorRepository
    {


        public Task<bool> Addasync(Vendor vendor);

        public Task<Vendor> Getasync(int id, bool hidden = false);
        public Task<IEnumerable<Vendor>> GetAllasync(bool hidden = false);


        public Task<bool> Updateasync(Vendor vendor);
        public Task<bool> VendorAnyAsync(int id);
        public Task<bool> Deleteasync(int id);

        public Task<bool> Duplicateasync(int id);
        public Task<bool> AnyAsync(int Id);

        public Task<JqueryDataTablesPagedResults<VendorDataTable>> GetVendorDataTableAsync(JqueryDataTablesParameters table);






    }
}
