//using Microsoft.EntityFrameworkCore;
//using Project.DataAccess.Repository.IRepository;
//using Project.DataAccesss.Data;
//using Project.Models.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection.Metadata;
//using System.Text;
//using System.Threading.Tasks;

//namespace Project.DataAccess.Repository
//{
//    public class HPCarouselRepositore : IHPCarouselRepositore
//    {
//        private readonly ApplicationDbContext _context;

//        public HPCarouselRepositore(ApplicationDbContext context)
//        {
//            _context = context;

//        }



//        public async Task<bool> Addasync(HPCarousel hpcarousel)
//        {
//            try
//            {
//                _context.hPCarousels.Add(hpcarousel);
//                await _context.SaveChangesAsync();
//                return true;

//            }
//            catch (Exception)
//            {

//                return false;
//            }

//        }
//        public async Task<bool> Deleteasync(int id)
//        {
//            try
//            {
//                var featuretest = await _context.hPCarousels.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
//                if (featuretest != null)
//                {
//                    featuretest.Deleted = true;
//                    await _context.SaveChangesAsync();
//                    return true;

//                }

//            }
//            catch (Exception)
//            {


//            }
//            return false;
//        }




//        public async Task<IEnumerable<HPCarousel>> GetAllasync(bool hidden = false)
//        {
//            return await _context.hPCarousels.Where(s => !s.Deleted  && (hidden == false || s.Hidden != hidden) ).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
//        }

//        public async Task<HPCarousel> Getasync(int id, bool hidden = false)
//        {
//            return await _context.hPCarousels.Where(e => e.Id == id && e.Deleted != true && (hidden == false || e.Hidden != hidden)).FirstOrDefaultAsync();
//        }

//        public async Task<bool> HPcarsouselAnyAsync(int id)
//        {
//            return await _context.hPCarousels.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
//        }

//        public async Task<bool> Updateasync(HPCarousel hpcarousel)
//        {
//            try
//            {
//                _context.hPCarousels.Update(hpcarousel);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception)
//            {

//                return false;
//            }
//        }

//        public async Task<bool> Duplicateasync(int id)
//        {
//            try
//            {
//                var featuretest = await _context.hPCarousels.FirstOrDefaultAsync(e => e.Id == id);
//                if (featuretest != null)
//                {
//                    var item2 = new HPCarousel
//                    {

//                        Image = featuretest.Image,
//                        DisplayOrder = featuretest.DisplayOrder,
//                        Hidden = featuretest.Hidden,
//                        UpdatedOnUtc = DateTime.Now.ToUniversalTime(),
//                        CreatedOnUtc = DateTime.Now.ToUniversalTime(),
                        

//                    };

//                    await _context.AddAsync(item2);
//                    await _context.SaveChangesAsync();
//                    return true;

//                }
//            }
//            catch (Exception)
//            {


//            }

//            return false;
//        }




//    }
//}
