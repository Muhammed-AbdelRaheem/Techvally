//using AutoMapper;
//using Data.Context;
//using DataAccess.IRepository;
//using Domain.Models;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
//using MimeKit.Text;
////using MailKit.Security;

//namespace DataAccess.Repository
//{
//    public class FeedRepository : IFeedRepository
//    {

//        private readonly AppDbContext _context;
//        private readonly ReadDBContext _read;
//        private readonly IMapper _mapper;

//        public FeedRepository(WriteDbContext context, ReadDBContext read, IMapper mapper)
//        {
//            _context = context;
//            _read = read;
//            _mapper = mapper;
//        }


//        public async Task<bool> Addasync(Feed feed)
//            {
//                try
//                {
//                    _context.feeds.Add(feed);
//                    await _context.SaveChangesAsync();
//                    return true;

//                }
//                catch (Exception)
//                {

//                    return false;
//                }

//            }
//            public async Task<bool> Deleteasync(int id)
//            {
//                try
//                {
//                    var featuretest = await _context.feeds.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
//                    if (featuretest != null)
//                    {
//                        featuretest.Deleted = true;
//                        await _context.SaveChangesAsync();
//                        return true;

//                    }

//                }
//                catch (Exception)
//                {


//                }
//                return false;
//            }




//            public async Task<IEnumerable<Feed>> GetAllasync(bool hidden = false)
//            {
//                return await _context.feeds.Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden)).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
//            }

//            public async Task<Feed> Getasync(int id, bool hidden = false)
//            {
//                return await _context.feeds.Where(e => e.Id == id && e.Deleted != true && (hidden == false || e.Hidden != hidden)).FirstOrDefaultAsync();
//            }

//            public async Task<bool> feedAnyAsync(int id)
//            {
//                return await _context.feeds.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
//            }

//            public async Task<bool> Updateasync(Feed feed)
//            {
//                try
//                {
//                    _context.feeds.Update(feed);
//                    await _context.SaveChangesAsync();
//                    return true;
//                }
//                catch (Exception)
//                {

//                    return false;
//                }
//            }

//            public async Task<bool> Duplicateasync(int id)
//            {
//                try
//                {
//                    var featuretest = await _context.feeds.FirstOrDefaultAsync(e => e.Id == id);
//                    if (featuretest != null)
//                    {
//                        var item2 = new Feed
//                        {

//                            Name = featuretest.Name,
//                            Job = featuretest.Job,
//                            Message = featuretest.Message,
//                            DisplayOrder = featuretest.DisplayOrder,
//                            Hidden = featuretest.Hidden,
//                            UpdatedOnUtc = DateTime.Now.ToUniversalTime(),
//                            CreatedOnUtc = DateTime.Now.ToUniversalTime(),


//                        };

//                        await _context.AddAsync(item2);
//                        await _context.SaveChangesAsync();
//                        return true;

//                    }
//                }
//                catch (Exception)
//                {


//                }

//                return false;
//            }




//        }



//    }
//}
