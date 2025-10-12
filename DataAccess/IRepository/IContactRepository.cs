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
    public interface IContactRepository
    {


        public Task<bool> Addasync(Contact contact);

        public Task<Contact> Getasync(int id, bool hidden = false);
        public Task<IEnumerable<Contact>> GetAllasync(bool hidden = false);


        public Task<bool> Updateasync(Contact contact);
        public Task<bool> ContactAnyAsync(int id);
        public Task<bool> Deleteasync(int id);

        public Task<bool> Duplicateasync(int id);



        public Task<JqueryDataTablesPagedResults<ContactDataTable>> GetContactDataTableAsync(JqueryDataTablesParameters table);


    }
}
