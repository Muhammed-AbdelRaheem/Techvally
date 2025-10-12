using Domain.Models;
using Domain.Models.NotificationHandlerVM;
using Domain.Models.ViewModel;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IRepository
{
    public interface IUserRepository
    {
        public Task<JqueryDataTablesPagedResults<UserTableViewModel>> GetUsersAsync(JqueryDataTablesParameters table, UserType userType);

        public Task<DashboardEditViewModel?> GetDashboardEditAsync(string id);
        public Task<UserEditViewModel?> GetUserEditAsync(string id);
        //public Task<RegistrationInfoVm?> GetUserRegistrationInfoAsync(string id);
   
        public Task<List<string>> GetUserRoles(string id);
        public Task<IdentityResultViewModel> UpdateUserAsync(UserEditViewModel userEditViewModel, bool isMobile = false);


      
        public Task<IdentityResultViewModel> UpdateDashboardAsync(DashboardEditViewModel userEditViewModel, string? currentUserId = null);
        public Task<IdentityResultViewModel> AddUserAsync(UserViewModel userViewModel, string? currentUserId = null, DeviceType? deviceType = DeviceType.WEB);
        public Task<IdentityResultViewModel> AddDashboardAsync(UserViewModel userViewModel, string? currentUserId = null, DeviceType? deviceType = DeviceType.WEB);
   
        public Task<IdentityResultViewModel> DeleteUserAsync(string id);
        public Task<IdentityResultViewModel> LockUserAsync(string id);

        public Task<MultiSelectList> GetRolesList(IEnumerable<string> role = null);

        Task UpdateUserInfoFromLogin(UpdateUserHandlerVM model);

     
    }

}
