using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using Data.IRepository;
using Dataaccess.IRepository;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess.Repository
{
    public class RoleRepository : IRoleRepository

    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly WriteDbContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(RoleManager<ApplicationRole> roleManager, WriteDbContext context, IMapper mapper)
        {
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddRoleAsync(RoleViewModel Role)
        {
            try
            {
                var role = _mapper.Map<RoleViewModel, ApplicationRole>(Role);
                role.Id = Guid.NewGuid().ToString();
                role.NormalizedName = role.Name.ToUpper();
                var result = await _roleManager.CreateAsync(role);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            try
            {
                var role = await _roleManager.Roles.Where(e => e.Id == id).FirstOrDefaultAsync();
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    return result.Succeeded;
                }

            }
            catch (Exception ex)
            {
            }
            return false;

        }

        public async Task<RoleViewModel?> GetRoleAsync(string id)
        {
            return await _roleManager.Roles.Where(e => e.Id == id)
                .ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.OrderByDescending(e => e.Id).ToListAsync();
        }

        public async Task<bool> UpdateRoleAsync(RoleViewModel Role)
        {
            try
            {
                var result = await _context.Roles.Where(e => e.Id == Role.Id).FirstOrDefaultAsync();

                if (result != null)
                {
                    result.Name = Role.Name;
                    result.NormalizedName = Role.Name.ToUpper();
                    await _context.SaveChangesAsync();

                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return false;

        }
    }

}
