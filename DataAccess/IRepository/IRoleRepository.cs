using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess.IRepository
{
    public interface IRoleRepository
    {
        public Task<IEnumerable<ApplicationRole>> GetRolesAsync();
        public Task<RoleViewModel?> GetRoleAsync(string id);
        public Task<bool> AddRoleAsync(RoleViewModel Role);
        public Task<bool> UpdateRoleAsync(RoleViewModel Role);
        public Task<bool> DeleteRoleAsync(string id);
    }
}
