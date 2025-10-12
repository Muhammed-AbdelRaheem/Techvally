using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDBContext _read;
        private readonly IMapper _mapper;

        public VendorRepository(WriteDbContext context , ReadDBContext readDB , IMapper mapper)
        {
            _context = context;
            _read = readDB;
            _mapper = mapper;
        }



        public async Task<bool> Addasync(Vendor vendor)
        {
            try
            {
                _context.Vendor.Add(vendor);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }
        public async Task<bool> Deleteasync(int id)
        {
            try
            {
                var featuretest = await _context.Vendor.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
                if (featuretest != null)
                {
                    featuretest.Deleted = true;
                    await _context.SaveChangesAsync();
                    return true;

                }

            }
            catch (Exception)
            {


            }
            return false;
        }


        public async Task<IEnumerable<Vendor>> GetAllasync(bool hidden = false)
        {
            return await _context.Vendor.Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden)).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<Vendor> Getasync(int id, bool hidden = false)
        {
            return await _context.Vendor.Where(e => e.Id == id && e.Deleted != true && (hidden == false || e.Hidden != hidden)).FirstOrDefaultAsync();
        }

        public async Task<bool> VendorAnyAsync(int id)
        {
            return await _context.Vendor.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
        }

        public async Task<bool> Updateasync(Vendor vendor)
        {
            try
            {
                _context.Vendor.Update(vendor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Duplicateasync(int id)
        {
            try
            {
                var featuretest = await _context.Vendor.FirstOrDefaultAsync(e => e.Id == id);
                if (featuretest != null)
                {
                    var item2 = new Vendor
                    {

                        Image = featuretest.Image,
                        Url = featuretest.Url,
                        DisplayOrder = featuretest.DisplayOrder,
                        Hidden = featuretest.Hidden,
                        UpdatedOnUtc = DateTime.Now.ToUniversalTime(),
                        CreatedOnUtc = DateTime.Now.ToUniversalTime(),
                        

                    };

                    await _context.AddAsync(item2);
                    await _context.SaveChangesAsync();
                    return true;

                }
            }
            catch (Exception)
            {


            }

            return false;
        }


        public async Task<JqueryDataTablesPagedResults<VendorDataTable>> GetVendorDataTableAsync(JqueryDataTablesParameters table)
        {
            try
            {



                var query = _read.Vendor.Where(e => !e.Deleted).OrderByDescending(t => t.Id).AsNoTracking();



                query = query.Where(
                   e =>
                   string.IsNullOrEmpty(table.Search.Value) ||
                   (
                       e.Id.ToString().Contains(table.Search.Value)



                   )
               );

                query = SearchOptionsProcessor<VendorDataTable, Vendor>.Apply(query, table.Columns);
                query = SortOptionsProcessor<VendorDataTable, Vendor>.Apply(query, table);

                var size = await query.CountAsync();

                var items = await query
                    .AsNoTracking()
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length)
                    .ProjectTo<VendorDataTable>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();

                return new JqueryDataTablesPagedResults<VendorDataTable>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

            }
            return new JqueryDataTablesPagedResults<VendorDataTable>
            {
                TotalSize = 0
            };
        }

        public async Task<bool> AnyAsync(int Id)
        {
            return await _read.Vendor.AnyAsync(e => e.Id == Id);
        }

    }
}
