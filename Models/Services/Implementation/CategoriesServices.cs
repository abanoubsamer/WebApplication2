using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models.Services.InterFace;

namespace WebApplication2.Models.Services.Implementation
{
    public class CategoriesServices : ICategoriesServices
    {

        private readonly AppDbContext _db;
        public CategoriesServices(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _db.Category.ToListAsync();

        }
    }
}
