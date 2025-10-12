using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IProductRequestRepository
    {

        public Task<IEnumerable<ProductRequest>> GetServiceRequestsAsync();


        public Task<bool> AddServiceRequestAsync(ProductRequest serviceRequest);


        public Task<bool> DeleteServiceRequestAsync(int id);




    }
}
