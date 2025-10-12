using MimeKit;

using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataAccess.IRepository;
using Data.Context;
using Domain.Models;

//using MailKit.Security;

namespace DataAccess.Repository
{
    public class MessageFormRepository : IMessageFormRepository
    {
        private readonly WriteDbContext _context;

        public MessageFormRepository(WriteDbContext context)
        {
            _context = context;

        }



        public async Task<bool> AddMessageFormAsync(MessageForm messageForm)
        {
            try
            {
                _context.messageForms.Add(messageForm);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }





        public async Task<bool> DeleteMessageFormAsync(int id)
        {
            try
            {
                var ServiceRequest = await _context.messageForms.FirstOrDefaultAsync(e => e.Id == id);
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

        public async Task<IEnumerable<MessageForm>> GetMessageFormAsync()
        {
            return await _context.messageForms.Where(e => e.Deleted != true).OrderByDescending(e => e.CreatedOnUtc).ThenByDescending(e => e.Id) .ToListAsync();
        }






    }
}
