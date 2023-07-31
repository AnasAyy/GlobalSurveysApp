using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Models;
using static Azure.Core.HttpHeader;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IPublicList
    {
        public void Create(PublicList publicList);
        public void Update(PublicList publicList);
        public List<PublicList> GetMainItem();
        public IQueryable<GetAllItemsResponseDto> GetAll();
        public PublicList? GetByNameAR(string nameAR);
        public PublicList? GetByNameEN(string nameEN);
        public bool CheckType(int type);
        public bool GetITemByName (string nameAR , string nameEN);
        public PublicList? GetById (int id);
        public bool SaveChanges();
        
    }

    public class Public : IPublicList
    {
        private readonly DataContext _context;

        public Public(DataContext context)
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

        public IQueryable<GetAllItemsResponseDto> GetAll()
        {
            return _context.PublicLists.Select(p => new GetAllItemsResponseDto
            {
                Id = p.Id,
                NameAR = p.NameAR,
                NameEN = p.NameEN,
                Type = p.Type,
            });
        }

        public PublicList? GetById(int id)
        {
            var x = _context.PublicLists.SingleOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return x;
        }

        public PublicList? GetByNameAR(string nameAR)
        {
             return _context.PublicLists.FirstOrDefault(x => x.NameAR == nameAR);

        }
        public PublicList? GetByNameEN(string nameEN)
        {
            return _context.PublicLists.FirstOrDefault(x => x.NameEN == nameEN);

        }

        public bool GetITemByName(string nameAR, string nameEN )
        {
            return _context.PublicLists.Any(x => x.NameAR == nameAR || x.NameEN == nameEN);
        }

        public bool SaveChanges()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }catch(Exception ex)
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
    }
}
