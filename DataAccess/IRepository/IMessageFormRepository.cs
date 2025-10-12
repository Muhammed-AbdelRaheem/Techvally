using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IMessageFormRepository
    {

        public Task<IEnumerable<MessageForm>> GetMessageFormAsync();


        public Task<bool> AddMessageFormAsync(MessageForm messageForm);


        public Task<bool> DeleteMessageFormAsync(int id);




    }
}
