using MimeKit;


using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Data.Context;
using Domain.Models;
using DataAccess.IRepository;

//using MailKit.Security;

namespace DataAccess.Repository
{
    public class ProductRequestRepository : IProductRequestRepository
    {
        private readonly WriteDbContext _context;

        public ProductRequestRepository(WriteDbContext context)
        {
            _context = context;

        }


        public async Task<bool> AddServiceRequestAsync(ProductRequest AboutUs)
        {
            try
            {
                AboutUs.Id = 0;
                _context.ProductRequests.Add(AboutUs);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }






        public async Task<bool> DeleteServiceRequestAsync(int id)
        {
            try
            {
                var ServiceRequest = await _context.ProductRequests.FirstOrDefaultAsync(e => e.Id == id);
                if (ServiceRequest == null)
                    return false;

                ServiceRequest.Deleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public async Task<IEnumerable<ProductRequest>> GetServiceRequestsAsync()
        {
            return await _context.ProductRequests.Where(e => e.Deleted != true).OrderByDescending(e => e.CreatedOnUtc).ThenByDescending(e => e.Id) .ToListAsync();
        }






    }
}
