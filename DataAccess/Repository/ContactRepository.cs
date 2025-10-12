using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using DataAccess.IRepository;
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

namespace DataAccess.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDBContext _read;
        private readonly IMapper _mapper;

        public ContactRepository(WriteDbContext context,ReadDBContext read ,IMapper mapper)
        {
            _context = context;
            _read = read;
            _mapper = mapper;
        }



        public async Task<bool> Addasync(Contact contact)
        {
            try
            {
                _context.Contacts.Add(contact);
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
                var featuretest = await _context.Contacts.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
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




        public async Task<IEnumerable<Contact>> GetAllasync(bool hidden = false)
        {
            return await _context.Contacts.Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden) ).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<Contact> Getasync(int id, bool hidden = false)
        {
            return await _context.Contacts.Where(e => e.Id == id && e.Deleted != true && (hidden == false || e.Hidden != hidden)).FirstOrDefaultAsync();
        }

        public async Task<bool> ContactAnyAsync(int id)
        {
            return await _context.Contacts.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
        }

        public async Task<bool> Updateasync(Contact contact)
        {
            try
            {
                _context.Contacts.Update(contact);
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
                var featuretest = await _context.Contacts.FirstOrDefaultAsync(e => e.Id == id);
                if (featuretest != null)
                {
                    var item2 = new Contact
                    {

                        Tel = featuretest.Tel,
                        Title = featuretest.Title,
                        Fax = featuretest.Fax,
                        Address = featuretest.Address,
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

        public async Task<JqueryDataTablesPagedResults<ContactDataTable>> GetContactDataTableAsync(JqueryDataTablesParameters table)
        {
            try
            {



                var query = _read.Contacts.Where(e => !e.Deleted).OrderByDescending(t => t.Id).AsNoTracking();



                query = query.Where(
                   e =>
                   string.IsNullOrEmpty(table.Search.Value) ||
                   (
                       e.Id.ToString().Contains(table.Search.Value)



                   )
               );


                query = SearchOptionsProcessor<ContactDataTable, Contact>.Apply(query, table.Columns);
                query = SortOptionsProcessor<ContactDataTable, Contact>.Apply(query, table);

                var size = await query.CountAsync();

                var items = await query
                    .AsNoTracking()
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length)
                    .ProjectTo<ContactDataTable>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();


                return new JqueryDataTablesPagedResults<ContactDataTable>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

            }
            return new JqueryDataTablesPagedResults<ContactDataTable>
            {
                TotalSize = 0
            };
        }

   
    }
}
