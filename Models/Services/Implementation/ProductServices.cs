using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using WebApplication2.Data;
using WebApplication2.Models.Services.InterFace;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Models.Services.Implementation
{
    public class ProductServices : IProductServices
    {

        #region Fields
        private readonly AppDbContext _db;
        private readonly IFileServices _fileServices;
        private readonly IImagesServices _imagesServices;
        #endregion

        #region Constractors
        public ProductServices(AppDbContext db, IImagesServices imagesServices,IFileServices fileServices)
        {
            _db = db;
            _imagesServices = imagesServices;   
            _fileServices= fileServices;
        }
        #endregion

        #region Implement Function

        #region AddProductAsync
        public async Task<string> AddProductAsync(Product product, List<string>? images)
        {
            if (product == null)
            {
                return "Fail";
            }

            try
            {
                // Add the product to the database
                await _db.Product.AddAsync(product);
                await _db.SaveChangesAsync(); // Save changes to get the generated product ID

                // Add images if provided
                if (images != null && images.Count > 0)
                {
                    string imageResult = await _imagesServices.AddImageAsync(product, images);

                    if (imageResult != "Success")
                    {
                        return imageResult;
                    }
                }

                return "Success";
            }
            catch (Exception e)
            {
                return $"Error adding product: {e.Message}";
            }
        }
        #endregion


        #region DeleteProductAsync
        public async Task<string> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await GetProductByIdAsync(productId);
                if (product != null)
                {
                    _db.Product.Remove(product);
                    await _db.SaveChangesAsync();
                    return "Product deleted successfully.";
                }
                else
                {
                    return "Product not found.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error deleting product: {e.Message}");
                return $"Error deleting product: {e.Message}";
            }
        }
        #endregion

        #region GetProductByIdAsync
        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {

                var product = await _db.Product
                  .Include(p => p.Images) // Eagerly load Images
                  .FirstOrDefaultAsync(p => p.id == id);

                if (product == null)
                {
                    throw new Exception($"Product with ID {id} not found.");
                }

                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving product by ID: {e.Message}");
                return null;
            }
        }
        #endregion


        #region GetProductByNameAsync
        public async Task<Product> GetProductByNameAsync(string productName)
        {
            try
            {
                return await _db.Product.FirstOrDefaultAsync(e => e.Name == productName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving product by name: {e.Message}");
                return null;
            }
        }
        #endregion


        #region GetProductsAsync
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                return await _db.Product.Include(c => c.Categorys).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving products: {e.Message}");
                return Enumerable.Empty<Product>();
            }
        }
        #endregion

        #region IsProductNameExitsAsync
        public async Task<bool> IsProductNameExitsAsync(string Name)
        {
            return await _db.Product.AnyAsync(e => e.Name == Name);
        }
        #endregion

        #region UpdateProduct
        public string UpdateProduct(Product newProduct)
        {
            if (newProduct == null)
            {
                return "Product data is not provided.";
            }

            try
            {
                // Set other properties as needed
                _db.Product.Update(newProduct);
                _db.SaveChanges();
                return "Success";

            }
            catch (Exception e)
            {

                return $"Error updating product: {e.Message}";
            }
        }
        #endregion






        #endregion

    }
}
