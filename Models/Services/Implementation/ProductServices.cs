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


            // hna ana bolh htbd2h 3mlyt ale  transaction 
            var trans= await _db.Database.BeginTransactionAsync();
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
                // hna ana bolh lw awy 7aga f4let fehm mt7fz 7aga fe ale data base
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception e)
            {
                // hna ana bolh arg3ly b2h 3n kol ale 3mlyata fe ale data base
                await trans.RollbackAsync();
                // hna bolh ht7zfly kol ale ale swer ale anta 7ttha
                _fileServices.RemoveImageAsync(images);
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
                return await _db.Product
                       .Include(p => p.Categorys)
                       .Include(p => p.Images).ToListAsync();
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
        public async Task <string> UpdateProduct(EditProductViewMoldes NewProduct)
        {
            if (NewProduct == null)
            {
                return "Product data is not provided.";
            }
            var OldProduct = await GetProductByIdAsync(NewProduct.id);

            if (OldProduct == null)
            {
                return "Product not found";
            }
            var taran = await _db.Database.BeginTransactionAsync();//begin transcation comment
            try
            {
                
                // check to user need update phote
                if (NewProduct.Files != null)
                {
                    if (OldProduct.Images != null)
                    {
                        // hna a7na 3mla update fe file ale server
                        var NewImagePath = await _fileServices.UpdateImageAsync(OldProduct.Images.Select(img => img.Images).ToList(), NewProduct.Files, "Product");
                        // update in image table
                        var r = await _imagesServices.UpdateImageAsync(OldProduct, NewImagePath);
                    }
                }

                OldProduct.Name = NewProduct.Name;
                OldProduct.Price = NewProduct.Price;
                OldProduct.CategoryId = NewProduct.CategoryId;

                // Set other properties as needed
                _db.Product.Update(OldProduct);
                _db.SaveChanges();

                await taran.CommitAsync();// end transcation comment;
                return "Success";

            }
            catch (Exception e)
            {
                await taran.RollbackAsync();//Block All Transaction In DataBase
                return $"Error updating product: {e.Message}";
            }
        }
        #endregion



        public string GetTitel()
        {
            return "Home Page";
        }


        #endregion

    }
}
