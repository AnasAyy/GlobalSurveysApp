using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IPublicListRepo
    {
        public void Create(PublicList publicList);
        public void Update(PublicList publicList);
        public List<PublicList> GetMainItem();
        public IQueryable<GetAllItemsResponseDto> GetAll(int type);
        public bool CheckType(int type);
        public bool GetITemByNameForAdd(string nameAR, string nameEN);
        public bool GetITemByNameForUpdate(string nameAR, string nameEN, int type, int id);
        public PublicList? GetById(int id);
        
        public bool SaveChanges();


        public Task<int> GetPhoneNumber();

    }

    public class PublicRepo : IPublicListRepo
    {
        private readonly DataContext _context;

        public PublicRepo(DataContext context)
        {
            _context = context;
        }

        public void Create(PublicList publicList)
        {
            _context.PublicLists.Add(publicList);
        }

        public List<PublicList> GetMainItem()
        {
            return _context.PublicLists.Where(x => x.Type == 0).ToList();
        }


        public IQueryable<GetAllItemsResponseDto> GetAll(int type)
        {
            return from p in _context.PublicLists
                   where p.Type == type && p.Type != 0
                   select new GetAllItemsResponseDto
                   { 
                       Id = p.Id,
                      NameAR = p.NameAR,
                      NameEN = p.NameEN
                   };

        }

        public PublicList? GetById(int id)
        {
            var x = _context.PublicLists.SingleOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return x;
        }

        public bool GetITemByNameForAdd(string nameAR, string nameEN)
        {

            return _context.PublicLists.Any(x => x.NameAR == nameAR || x.NameEN == nameEN);

        }

        public bool SaveChanges()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _context.ChangeTracker.Clear();
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public void Update(PublicList publicList)
        {
            _context.PublicLists.Update(publicList);
        }

        public bool CheckType(int type)
        {
            return _context.PublicLists.Any(x => x.Id == type && x.Type == 0);
        }

        public bool GetITemByNameForUpdate(string nameAR, string nameEN, int type, int id)
        {
            return _context.PublicLists.Any(x => x.NameAR == nameAR && x.NameEN == nameEN && x.Type == type && x.Id != id);
        }


        public async Task<int> GetPhoneNumber()
        {
            var x = await _context.PublicLists.SingleOrDefaultAsync(x => x.Id == 1091);
            _context.ChangeTracker.Clear();
            if(x != null)
            {
                int number = Convert.ToInt32(x.NameAR.ToString());
                return number;
            }
            return 0;
            
        }

       


    }
}
