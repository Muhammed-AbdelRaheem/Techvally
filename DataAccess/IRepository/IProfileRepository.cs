using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IProfileRepository
    {


        public Task<bool> Addasync(Profile AboutUs);

        public Task<Profile?> Getasync(bool hidden = false);
        public Task<IEnumerable<Profile>> GetAllasync(bool hidden = false);


        public Task<bool> Updateasync(Profile AboutUs);
        public Task<bool> HPcarsouselAnyAsync(int id);
        public Task<bool> Deleteasync(int id);

        public Task<bool> Duplicateasync(int id);



        public Task<JqueryDataTablesPagedResults<ProfileDataTable>> GetProfileDataTableAsync(JqueryDataTablesParameters table);


    }
}
