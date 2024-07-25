namespace WebApplication2.Models.Services.InterFace
{
    public interface ICategoriesServices
    {
        public Task<IEnumerable<Category>> GetCategoriesAsync();


    }
}
