using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface ICategoryRepository
    {


        public Task<bool> AddAsync(Category category);
        public Task<bool> DeleteAsync(int id);
        public Task<bool> DuplicateCategoryAsync(int id);
        public Task<Category?> GetAsync(int id);
        public Task<IEnumerable<Category>> GetAllCategoryAsync( bool hidden = false);
        public Task<IEnumerable<Category>> GetAllparentCategorywithsub(int itemId, bool hidden = false);
        public Task<IEnumerable<Category>> GetAllParentCategoryAsync(int? id = null, bool hidden = false);
        public Task<bool> UpdateAsync(Category category);
        public Task<bool> AnyAsync(int id);

        public Task<JqueryDataTablesPagedResults<CategoryTableData>> GetCategoryDataTableAsync(JqueryDataTablesParameters table);

        public Task<SelectList?> GetCategorySelectListAsync(int? selected = null);



    }
}
