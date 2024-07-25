using WebApplication2.Models.ViewModels;

namespace WebApplication2.Models.Services.InterFace
{
    public interface IProductServices
    {
        public Task<IEnumerable<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> GetProductByNameAsync(string productName);
        public Task<string> DeleteProductAsync(int productId);
        public Task<string> AddProductAsync(Product product,List<string>? Images);
        public Task<bool>IsProductNameExitsAsync(string Name);
        public Task<string> UpdateProduct(EditProductViewMoldes NewProduct);
        public string GetTitel();

    }
}
