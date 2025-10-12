using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IProductRepository
    {


        public Task<bool> AddAsync(Product product);
        public Task<bool> DeleteAsync(int id);
        public Task<bool> DuplicateProductAsync(int id);
        public Task<IEnumerable<Product>> GetAllProductListasync(bool hidden = false);

        public Task<IEnumerable<Product>> GetAllProductAsync(int itemId, bool hidden = false);
        public Task<Product?> GetAsync(int id, bool hidden = false);
        public Task<bool> AnyAsync(int id);
        public Task<bool> UpdateAsync(Product product);
        public Task<bool> DublicateProductForItem(int itemId, int duplicatedItemId);


        IEnumerable<Product> GetAll(Expression<Func<Product, bool>>? filter = null, string? includeproperties = null);

        public Task<JqueryDataTablesPagedResults<ProductDataTable>> GetProductDataTableAsync(JqueryDataTablesParameters table);

    }
}
