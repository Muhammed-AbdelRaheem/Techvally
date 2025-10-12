using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using DataAccess.IRepository;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ReadDBContext _read;
        private readonly IMapper _mapper;

        public ProductRepository(WriteDbContext context ,ReadDBContext read ,IMapper mapper)
        {
            _context = context;
            _read = read;
            _mapper = mapper;
        }


        public async Task<bool> AddAsync(Product product)
        {
            try
            {

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var section = await _context.Products.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
                if (section != null)
                {
                    section.Deleted = true;
                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return false;

        }

        public async Task<Product?> GetAsync(int id, bool hidden = false)
        {
            return await _context.Products.Include(e => e.Category).Where(e => !e.Deleted && e.Id == id  && (hidden == false || e.Hidden != hidden))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync(int itemId, bool hidden = false)
        {
            return await _context.Products.Where(s => s.CategoryId == itemId && !s.Deleted && (hidden == false || s.Hidden != hidden)).OrderBy(e => e.DisplayOrder).OrderByDescending(e => e.Id).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductListasync(bool hidden = false)
        {
            return await _context.Products.Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden)).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            try
            {

                _context.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Products.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
        }
        public async Task<bool> DuplicateProductAsync(int id)
        {
            try
            {
                var section = await _context.Products.FirstOrDefaultAsync(e => e.Id == id);
                if (section != null)
                {
                    var item2 = new Product
                    {
                        Title = section.Title,
                        Description = section.Description,
                        Content = section.Content,
                        Spacification = section.Spacification,
                        Image = section.Image,
                        CategoryId = section.CategoryId,
                        DisplayOrder = section.DisplayOrder,
                        Hidden = section.Hidden,
                        UpdatedOnUtc = DateTime.Now.ToUniversalTime(),
                        CreatedOnUtc = DateTime.Now.ToUniversalTime(),


                    };

                    await _context.AddAsync(item2);
                    await _context.SaveChangesAsync();


                    return true;
                }

            }
            catch (Exception ex)
            {

            }
            return false;

        }

        public async Task<bool> DublicateProductForItem(int itemId, int duplicatedItemId)
        {
            var relatedSections = await _context.Products.Where(s => !s.Deleted && s.CategoryId == itemId).ToListAsync();
            var duplicatedSections = new List<Product>();

            if (relatedSections.Count > 0)
            {
                foreach (var relatedSec in relatedSections)
                {
                    duplicatedSections.Add(
                        new Product
                        {
                            CategoryId = duplicatedItemId,
                            Title = relatedSec.Title,
                            Description = relatedSec.Description,
                            Content = relatedSec.Content,
                            Spacification = relatedSec.Spacification,
                            Image = relatedSec.Image,
                            UpdatedOnUtc = DateTime.Now.ToUniversalTime(),
                            CreatedOnUtc = DateTime.Now.ToUniversalTime(),
                            DisplayOrder = relatedSec.DisplayOrder,
                            Hidden = relatedSec.Hidden,
                        }
                        );
                }
            }
            if (duplicatedSections.Count > 0)
            {
                try
                {
                    await _context.AddRangeAsync(duplicatedSections);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {

                }
            }
            return false;

        }

        public IEnumerable<Product> GetAll(Expression<Func<Product, bool>>? filter = null, string? includeproperties = null)
        {
            IQueryable<Product> query = _context.Products;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeproperties))
            {
                foreach (var includeprop in includeproperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }

            return query.ToList();
        }



        public async Task<JqueryDataTablesPagedResults<ProductDataTable>> GetProductDataTableAsync(JqueryDataTablesParameters table)
        {
            try
            {



                var query = _read.Products.Where(e => !e.Deleted).OrderByDescending(t => t.Id).AsNoTracking();



                query = query.Where(
                   e =>
                   string.IsNullOrEmpty(table.Search.Value) ||
                   (
                       e.Id.ToString().Contains(table.Search.Value)
              


                   )
               );


                query = SearchOptionsProcessor<ProductDataTable, Product>.Apply(query, table.Columns);
                query = SortOptionsProcessor<ProductDataTable, Product>.Apply(query, table);

                var size = await query.CountAsync();

                var items = await query
                    .AsNoTracking()
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length)
                    .ProjectTo<ProductDataTable>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();


                return new JqueryDataTablesPagedResults<ProductDataTable>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

            }
            return new JqueryDataTablesPagedResults<ProductDataTable>
            {
                TotalSize = 0
            };
        }

        
    }
}
