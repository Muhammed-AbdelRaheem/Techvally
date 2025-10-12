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
//    public class SolutionRepository : ISolutionRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public SolutionRepository(ApplicationDbContext context)
//        {
//            _context = context;

//        }



//        public async Task<bool> Addasync(Solution solution)
//        {
//            try
//            {
//                _context.solutions.Add(solution);
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
//                var featuretest = await _context.solutions.Where(e => !e.Deleted && e.Id == id).FirstOrDefaultAsync();
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




//        public async Task<IEnumerable<Solution>> GetAllasync(bool hidden = false)
//        {
//            return await _context.solutions.Where(s => !s.Deleted && (hidden == false || s.Hidden != hidden) ).OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.Id).ToListAsync();
//        }

//        public async Task<Solution> Getasync(int id, bool hidden = false)
//        {
//            return await _context.solutions.Where(e => e.Id == id && e.Deleted != true && (hidden == false || e.Hidden != hidden)).FirstOrDefaultAsync();
//        }

//        public async Task<bool> lastestnewsAnyAsync(int id)
//        {
//            return await _context.solutions.Where(e => e.Id == id && e.Deleted != true).AnyAsync();
//        }

//        public async Task<bool> Updateasync(Solution solution)
//        {
//            try
//            {
//                _context.solutions.Update(solution);
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
//                var featuretest = await _context.solutions.FirstOrDefaultAsync(e => e.Id == id);
//                if (featuretest != null)
//                {
//                    var item2 = new Solution
//                    {

//                        Image = featuretest.Image,
//                        Name = featuretest.Name,
//                        Description = featuretest.Description,
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
