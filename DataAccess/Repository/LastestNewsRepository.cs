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
    public class LastestNewsRepository : ILastestNewsRepository
    {
        private readonly AppDbContext _context;
        private readonly ReadDBContext _readDB;
        private readonly IMapper _mapper;

        public LastestNewsRepository(WriteDbContext context, ReadDBContext read ,IMapper mapper)
        {
            _context = context;
            _readDB = read;
            _mapper = mapper;
        }



        public async Task<bool> Addasync(LastestNews news)
        {
            try
            {
                _context.lastestNews.Add(news);
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
                var featuretest = await _context.lastestNews.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
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




        public async Task<IEnumerable<LastestNews>> GetAllasync(bool hidden = false, bool ShowInHomePage = false)
        {
            return await _context.lastestNews.Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden) && (ShowInHomePage == false || s.ShowInHomePage == ShowInHomePage)).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<LastestNews> Getasync(int id, bool hidden = false, bool ShowInHomePage = false)
        {
            return await _context.lastestNews.Where(e => e.Id == id && e.Deleted != true && (hidden == false || e.Hidden != hidden) && (ShowInHomePage == false || e.ShowInHomePage == ShowInHomePage)).FirstOrDefaultAsync();
        }

        public async Task<bool> lastestnewsAnyAsync(int id)
        {
            return await _context.lastestNews.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
        }

        public async Task<bool> Updateasync(LastestNews news)
        {
            try
            {
                _context.lastestNews.Update(news);
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
                var featuretest = await _context.lastestNews.FirstOrDefaultAsync(e => e.Id == id);
                if (featuretest != null)
                {
                    var item2 = new LastestNews
                    {

                        Image = featuretest.Image,
                        Title = featuretest.Title,
                        PublishDate = featuretest.PublishDate,
                        Description = featuretest.Description,
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

        public async Task<JqueryDataTablesPagedResults<LastestNewsDataTable>> GetDataTableAsync(JqueryDataTablesParameters table)
        {
            try
            {



                var query = _readDB.lastestNews.Where(e => !e.Deleted).OrderByDescending(t => t.Id).AsNoTracking();



                query = query.Where(
                   e =>
                   string.IsNullOrEmpty(table.Search.Value) ||
                   (
                       e.Id.ToString().Contains(table.Search.Value)



                   )
               );


                query = SearchOptionsProcessor<LastestNewsDataTable, LastestNews>.Apply(query, table.Columns);
                query = SortOptionsProcessor<LastestNewsDataTable, LastestNews>.Apply(query, table);

                var size = await query.CountAsync();

                var items = await query
                    .AsNoTracking()
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length)
                    .ProjectTo<LastestNewsDataTable>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();


                return new JqueryDataTablesPagedResults<LastestNewsDataTable>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

            }
            return new JqueryDataTablesPagedResults<LastestNewsDataTable>
            {
                TotalSize = 0
            };
        }


    }
}
