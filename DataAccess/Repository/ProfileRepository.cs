using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using DataAccess.IRepository;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Profile = Domain.Models.Profile;

namespace DataAccess.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDBContext _readDb;
        private readonly IMapper _mapper;

        public ProfileRepository(WriteDbContext context, ReadDBContext readDb, IMapper mapper)
        {
            _context = context;
            _readDb = readDb;
            _mapper = mapper;
        }



        public async Task<bool> Addasync(Profile AboutUs)
        {
            try
            {
                _context.profiles.Add(AboutUs);
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
                var featuretest = await _context.profiles.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
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



        public async Task<IEnumerable<Profile>> GetAllasync(bool hidden = false)
        {
            return await _context.profiles.Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden)).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
        }

        public async Task<Profile?> Getasync(bool hidden = false)
        {
            return await _context.profiles.Where(e => e.Deleted != true && (hidden == false || e.Hidden != hidden)).FirstOrDefaultAsync();
        }

        public async Task<bool> HPcarsouselAnyAsync(int id)
        {
            return await _context.profiles.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
        }

        public async Task<bool> Updateasync(Profile AboutUs)
        {
            try
            {
                _context.profiles.Update(AboutUs);
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
                var featuretest = await _context.profiles.FirstOrDefaultAsync(e => e.Id == id);
                if (featuretest != null)
                {
                    var item2 = new Profile
                    {

                        HPTitle = featuretest.HPTitle,
                        HPImage = featuretest.HPImage,
                        DImage = featuretest.DImage,
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

        public async Task<JqueryDataTablesPagedResults<ProfileDataTable>> GetProfileDataTableAsync(JqueryDataTablesParameters table)
        {
            try
            {



                var query = _readDb.profiles.Where(e => !e.Deleted).OrderByDescending(t => t.Id).AsNoTracking();



                query = query.Where(
                   e =>
                   string.IsNullOrEmpty(table.Search.Value) ||
                   (
                       e.Id.ToString().Contains(table.Search.Value)



                   )
               );


                query = SearchOptionsProcessor<ProfileDataTable, Profile>.Apply(query, table.Columns);
                query = SortOptionsProcessor<ProfileDataTable, Profile>.Apply(query, table);

                var size = await query.CountAsync();

                var items = await query
                    .AsNoTracking()
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length)
                    .ProjectTo<ProfileDataTable>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();


                return new JqueryDataTablesPagedResults<ProfileDataTable>
                {
                    Items = items,
                    TotalSize = size
                };
            }
            catch (Exception ex)
            {

            }
            return new JqueryDataTablesPagedResults<ProfileDataTable>
            {
                TotalSize = 0
            };
        }



    }
}
