using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using DataAccess.IRepository;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CategoryRepository  : ICategoryRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDBContext _read;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CategoryRepository(WriteDbContext context, ReadDBContext read, IProductRepository productRepository ,IMapper mapper)
        {
            _context = context;
            _read = read;
            _productRepository = productRepository;
            _mapper = mapper;
        }



        public async Task<bool> AddAsync(Category category)
        {
            try
            {
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var service = await _context.Categories.Include(c => c.Subcategories).Include(c => c.Products).Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
                if (service != null)
                {
                    service.Deleted = true;

                    // Set the ParentCategoryId to null for associated subcategories
                    foreach (var subcategory in service.Subcategories)
                    {
                        subcategory.ParentCategoryId = null;
                    }


                


                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception)
            {
            }
            return false;
        }

        public async Task<bool> DuplicateCategoryAsync(int id)
        {
            try
            {
                var service = await _context.Categories.Include(c => c.Subcategories).FirstOrDefaultAsync(e => e.Id == id);
                if (service != null)
                {
                    var item2 = new Category
                    {
                        Name = service.Name,
                        Description = service.Description,
                        Image = service.Image,
                        DisplayOrder = service.DisplayOrder,
                        Hidden = service.Hidden,
                        UpdatedOnUtc = DateTime.UtcNow.ToUniversalTime(),
                        CreatedOnUtc = DateTime.UtcNow.ToUniversalTime(),
                        ParentCategoryId = service.ParentCategoryId,



                    };

                    await _context.AddAsync(item2);
                    await _context.SaveChangesAsync();


                



                    await _productRepository.DublicateProductForItem(id, item2.Id);
                    return true;
                }

            }
            catch (Exception)
            {

            }
            return false;
        }


        public async Task<Category?> GetAsync(int id)
        {
            return await _context.Categories.Where(e => e.Id == id && e.Deleted != true).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Category>> GetAllCategoryAsync( bool hidden = false)
        {
            return await _context.Categories.Include(s => s.ParentCategory).Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden)).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllparentCategorywithsub(int itemId, bool hidden = false)
        {
            return await _context.Categories.Where(s => !s.Deleted && s.ParentCategoryId == itemId && (hidden == false || s.Hidden != hidden)).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllParentCategoryAsync(int? id = null, bool hidden = false)
        {
            var parentList = await _context.Products.Select(p => p.CategoryId).ToListAsync();

            return await _context.Categories.Where(s => (id == null || s.Id != id) && (hidden == false || s.Hidden != hidden)  /*&& s.ParentCategoryId == null */&&  !parentList.Contains(s.Id)   && !s.Deleted).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Categories.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }



        public async Task<JqueryDataTablesPagedResults<CategoryTableData>> GetCategoryDataTableAsync(JqueryDataTablesParameters table)
        {
            try
            {



                var query = _read.Categories.Where(e => !e.Deleted).OrderByDescending(t => t.Id).AsNoTracking();



                query = query.Where(
                   e =>
                   string.IsNullOrEmpty(table.Search.Value) ||
                   (
                       e.Id.ToString().Contains(table.Search.Value)
                   


                   )
               );


                query = SearchOptionsProcessor<CategoryTableData, Category>.Apply(query, table.Columns);
                query = SortOptionsProcessor<CategoryTableData, Category>.Apply(query, table);

                var size = await query.CountAsync();

                var items = await query
                    .AsNoTracking()
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length)
                    .ProjectTo<CategoryTableData>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();


                return new JqueryDataTablesPagedResults<CategoryTableData>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

            }
            return new JqueryDataTablesPagedResults<CategoryTableData>
            {
                TotalSize = 0
            };
        }





        public async Task<SelectList?> GetCategorySelectListAsync(int? selected = null)
        {
            var zones = await _read.Categories.Where(e => !e.Hidden && !e.Deleted).Select(e => new { e.Id, e.Name }).ToListAsync();

            return new SelectList(zones.Select(e => new
            {
                e.Id,
                e.Name,

            }), "Id", "Name", selected);
        }







    }
}
